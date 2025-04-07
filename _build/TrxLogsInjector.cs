
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Nuke.Common.IO;

public static class TrxLogsInjector
{

    public static void InjectLogs(AbsolutePath trxPath, AbsolutePath logsPath)
    {
        var testOutputs = ParseTestLog(logsPath);
        InjectOutputsIntoTrx(trxPath, testOutputs);
    }

    public class TestResult
    {
        public string Name { get; set; }
        public string StandardOutput { get; set; } = "";
        public string ErrorOutput { get; set; } = "";
    }

    public static void InjectOutputsIntoTrx(string trxPath, List<TestResult> testResults)
    {
        var doc = XDocument.Load(trxPath);
        XNamespace ns = doc.Root.Name.Namespace;

        var unitTestResults = doc.Descendants(ns + "UnitTestResult").ToList();

        foreach (var result in testResults)
        {
            var matching = unitTestResults
                .FirstOrDefault(x => (string)x.Attribute("testName") == result.Name);

            if (matching == null)
                continue;

            bool hasStdOut = !string.IsNullOrWhiteSpace(result.StandardOutput);
            bool hasErrOut = !string.IsNullOrWhiteSpace(result.ErrorOutput);

            // Don't add anything if there's no output
            if (!hasStdOut && !hasErrOut)
                continue;

            // Always overwrite existing output
            matching.Element(ns + "Output")?.Remove();

            var outputNode = new XElement(ns + "Output");

            if (hasStdOut)
            {
                outputNode.Add(new XElement(ns + "StdOut", new XCData(result.StandardOutput.Trim())));
            }

            if (hasErrOut)
            {
                outputNode.Add(
                    new XElement(ns + "ErrorInfo",
                        new XElement(ns + "Message", new XCData(result.ErrorOutput.Trim()))
                    )
                );
            }

            matching.Add(outputNode);
        }

        doc.Save(trxPath);
    }

    private static List<TestResult> ParseTestLog(string filePath)
    {
        var results = new List<TestResult>();
        var lines = File.ReadAllLines(filePath);

        var testLineRegex = new Regex(@"^(passed|failed|skipped)\s+(.*?)\s+\((.*?)\)", RegexOptions.Compiled);
        var stdOutStartRegex = new Regex(@"^\s+Standard output", RegexOptions.Compiled);
        var errOutStartRegex = new Regex(@"^\s+Error output", RegexOptions.Compiled);

        TestResult currentTest = null;
        string currentSection = null;

        foreach (var line in lines)
        {
            var testMatch = testLineRegex.Match(line);
            if (testMatch.Success)
            {
                // Save the previous test result, if any
                if (currentTest != null)
                {
                    results.Add(currentTest);
                }

                currentTest = new TestResult
                {
                    Name = testMatch.Groups[2].Value
                };

                currentSection = null;
                continue;
            }

            if (currentTest != null)
            {
                if (stdOutStartRegex.IsMatch(line))
                {
                    currentSection = "stdout";
                    continue;
                }
                else if (errOutStartRegex.IsMatch(line))
                {
                    currentSection = "stderr";
                    continue;
                }
                else if (line.TrimStart().StartsWith("-")) // next section (e.g., summary or artifacts)
                {
                    currentSection = null;
                    continue;
                }

                if (currentSection == "stdout")
                {
                    currentTest.StandardOutput += line.TrimStart() + Environment.NewLine;
                }
                else if (currentSection == "stderr")
                {
                    currentTest.ErrorOutput += line.TrimStart() + Environment.NewLine;
                }
            }
        }

        // Add the last test result if the file ends without another test line
        if (currentTest != null)
        {
            results.Add(currentTest);
        }

        return results;
    }

}
