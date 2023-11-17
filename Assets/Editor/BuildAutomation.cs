using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class BuildAutomation
{
    public static string APIKey = "408c54102b487c53b6f569030d31851f";
    public static string OrganizationId = "18967492849752";
    public static string ProjectId = "d4125025-0ddb-438f-993f-98c4dbef9edf";
    public static string BuildTargetId = "default-android1";

    [MenuItem("Build/Build Automation")]
    public static void Build()
    {
        string shellCommand = "curl -X POST " +
            "-d '{\"clean\": true, \"delay\": 30}' " +
            "-H \"Content - Type: application / json\" " +
            $"-H \"Authorization: Basic {APIKey}\" " +
            @"https://build-api.cloud.unity3d.com/api/v1/" +
            $"orgs/{OrganizationId}/" +
            $"projects/{ProjectId}/" +
            $"buildtargets/{BuildTargetId}/builds";

        ProcessStartInfo startInfo = new ProcessStartInfo("/bin/bash");
        startInfo.WorkingDirectory = "/";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;

        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        process.StandardInput.WriteLine(shellCommand);
        //process.StandardInput.WriteLine("exit");  // if no exit then WaitForExit will lockup your program
        process.StandardInput.Flush();

        string line = process.StandardOutput.ReadLine();
        UnityEngine.Debug.Log(line);
    }
}