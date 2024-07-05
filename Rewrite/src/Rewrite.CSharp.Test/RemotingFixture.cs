using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using Rewrite.Core;
using Rewrite.Remote;

namespace Rewrite.RewriteCSharp.Test;

public class RemotingFixture : IDisposable
{
    private string? _rewriteRemoteDir;
    private readonly Socket _socket;
    private Process? _process;

    public RemotingFixture()
    {
        // register recipes (try to do this dynamically)
        IRemotingContext.RegisterRecipeFactory(Recipe.Noop().GetType().FullName!, _ => Recipe.Noop());

        var socketPath = Path.Combine(Path.GetTempPath(), "rewrite-java.sock");
        var endpoint = new UnixDomainSocketEndPoint(socketPath);
        _socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

        var remotingContext = IRemotingContext.Create();

        try
        {
            _socket.Connect(endpoint);
            remotingContext.Connect(_socket);
            remotingContext.SetCurrent();
            return;
        }
        catch (SocketException)
        {
        }

        // start new server process
        StartJavaRemotingServer();

        for (var i = 0; i < 5; i++)
        {
            try
            {
                _socket.Connect(endpoint);
                remotingContext.Connect(_socket);
                remotingContext.SetCurrent();
                break;
            }
            catch (SocketException)
            {
                Thread.Sleep(1000);
            }
        }
    }

    private void StartJavaRemotingServer()
    {
        _rewriteRemoteDir = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.GetTempFileName()));
        File.Delete(_rewriteRemoteDir);
        ExtractEmbeddedResources(_rewriteRemoteDir);

        var psi = new ProcessStartInfo();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            psi.FileName = "cmd.exe";
            psi.Arguments = $@"/C start {_rewriteRemoteDir}\rewrite-remote\bin\rewrite-remote.bat";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                 || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            using (var chmod = new Process())
            {
                chmod.StartInfo.FileName = "/bin/chmod";
                chmod.StartInfo.ArgumentList.Add("+x");
                chmod.StartInfo.ArgumentList.Add($"{_rewriteRemoteDir}/rewrite-remote/bin/rewrite-remote");
                chmod.StartInfo.UseShellExecute = false;
                chmod.Start();

                chmod.WaitForExit();
            }
            
            psi.FileName = "/bin/bash";
            psi.Arguments = $"-c \"{_rewriteRemoteDir}/rewrite-remote/bin/rewrite-remote\"";
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
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using var resourceStream = assembly.GetManifestResourceStream(resourceName);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            resourceStream.CopyTo(fileStream);
        }
    }
    
    public void Dispose()
    {
        _socket.Close();
        _process?.Kill();
        if (_rewriteRemoteDir != null && Directory.Exists(_rewriteRemoteDir))
            Directory.Delete(_rewriteRemoteDir, true);
    }
    
}
