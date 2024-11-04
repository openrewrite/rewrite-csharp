using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using Rewrite.Core;
using Rewrite.Remote;
using Rewrite.Test;
using Xunit;

namespace Rewrite.Test.Engine.Remote;

public class RemotingFixture : IDisposable
{
    private string? _rewriteRemoteDir;
    private Socket _socket = null!;
    private Process? _process;


    private void StartJavaRemotingServer()
    {
        _rewriteRemoteDir = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.GetTempFileName()));
        File.Delete(_rewriteRemoteDir);
        ExtractEmbeddedResources(_rewriteRemoteDir);

        var psi = new ProcessStartInfo();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            psi.FileName = "cmd.exe";
            psi.Arguments = $@"/C start {_rewriteRemoteDir}\rewrite-test-engine-remote\bin\rewrite-test-engine-remote.bat";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                 || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            using (var chmod = new Process())
            {
                chmod.StartInfo.FileName = "/bin/chmod";
                chmod.StartInfo.ArgumentList.Add("+x");
                chmod.StartInfo.ArgumentList.Add($"{_rewriteRemoteDir}/rewrite-test-engine-remote/bin/rewrite-test-engine-remote");
                chmod.StartInfo.UseShellExecute = false;
                chmod.Start();

                chmod.WaitForExit();
            }

            psi.FileName = "/bin/bash";
            psi.Arguments = $"-c \"{_rewriteRemoteDir}/rewrite-test-engine-remote/bin/rewrite-test-engine-remote\"";
        }

        var process = new Process
        {
            StartInfo = psi
        };

        _process = process;
        _process.Start();
    }

    public void ExtractEmbeddedResources(string outputDirectory)
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var resourceName in assembly.GetManifestResourceNames())
        {
            var filePath = Path.Combine(outputDirectory, resourceName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceName.StartsWith("rewrite-test-engine-remote"))
            {
                ZipFile.ExtractToDirectory(resourceStream!, outputDirectory);
                var dir = Directory.EnumerateDirectories(outputDirectory, "rewrite-test-engine-remote*").First();
                Directory.Move(dir, Path.Combine(outputDirectory, "rewrite-test-engine-remote"));
            }
            else
            {
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                resourceStream!.CopyTo(fileStream);
            }
        }
    }



    public RemotingFixture()
    {
        // register recipes (try to do this dynamically)
        IRemotingContext.RegisterRecipeFactory(Recipe.Noop().GetType().FullName!, _ => Recipe.Noop());
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

        var remotingContext = IRemotingContext.Create();

        try
        {
            _socket.Connect(IPAddress.Loopback, 65432);
            remotingContext.Connect(_socket);
            remotingContext.SetCurrent();
            ITestExecutionContext.SetCurrent(new RemotingTestExecutionContext(remotingContext));
            IPrinterFactory.Set(new RemotePrinterFactory(remotingContext.Client!));
        }
        catch (SocketException)
        {
            // start new server process
            StartJavaRemotingServer();
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    _socket.Connect(IPAddress.Loopback, 65432);
                    remotingContext.Connect(_socket);
                    remotingContext.SetCurrent();
                    break;
                }
                catch (SocketException)
                {
                    AsyncHelper.RunSync(() => Task.Delay(1000));
                }
            }
        }

        ITestExecutionContext.SetCurrent(new RemotingTestExecutionContext(remotingContext));
        IPrinterFactory.Set(new RemotePrinterFactory(remotingContext.Client!));
    }

    public void Dispose()
    {
        _socket.Close();

        var cancellationToken = new CancellationTokenSource(5000).Token;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {

                try
                {
                    _process?.Kill();
                    var closed = _process?.CloseMainWindow(); // on windows this is necessary to properly shut it down
                }
                catch (Exception)
                {
                    // ignore
                }
                if (_rewriteRemoteDir != null && Directory.Exists(_rewriteRemoteDir))
                    Directory.Delete(_rewriteRemoteDir, true);
            }
            catch (Exception)
            {
                AsyncHelper.RunSync(() => Task.Delay(1000));
            }
        }
    }
}
