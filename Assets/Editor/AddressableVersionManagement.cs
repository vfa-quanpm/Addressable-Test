using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AddressalbeAssetManagement
{
   
    public static AddressableEnvironment ADDRESSABLE_ENVIRONMENT;

    [MenuItem("Build/Build Addressable for Current Version Develop")]
    public static void BuildAddressablesWithVersionDevelop()
    {

        var setting = Resources.Load<EnvironmentSettings>("EnvironmentSetting");
        
        // Set environment
        ADDRESSABLE_ENVIRONMENT = setting.Environment;
        
        // Get the AddressableAssetSettings
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        // Get the current version
        string currentVersion = ADDRESSABLE_ENVIRONMENT == AddressableEnvironment.Develop
            ? PlayerSettings.bundleVersion + ".b"
            : PlayerSettings.bundleVersion;
        string minorVersion = VersionNameUtility.GetMinorVersion(currentVersion);

        string defaultProfileId = settings.profileSettings.GetProfileId("Default");

        string buildTarget = EditorUserBuildSettings.activeBuildTarget.ToString();

        settings.profileSettings.SetValue(defaultProfileId, "ContentVersion", minorVersion);

        EditorUtility.SetDirty(settings);


        //string remoteBuildPath = "/Users/t-nagaoka/Workspace/unity/udl-joggle/ServerData/" + minorVersion + "/" + buildTarget;
        string remoteBuildPath = "/Users/t-nagaoka/Workspace/unity/udl-joggle/ServerData/" + minorVersion;
        ClearDirectory(remoteBuildPath);

        //UnityEngine.Debug.Log(settings.buildSettings.bundleBuildPath);

        // Build Player Content
        AddressableAssetSettings.BuildPlayerContent();
    }
    
    // [MenuItem("Build/Build Addressable for Current Version Production")]
    // public static void BuildAddressablesWithVersionProduction()
    // {
    //     // Set environment
    //     ADDRESSABLE_ENVIRONMENT = AddressableEnvironment.Production;
    //     
    //     // Get the AddressableAssetSettings
    //     var settings = AddressableAssetSettingsDefaultObject.Settings;
    //
    //     // Get the current version
    //     string currentVersion = ADDRESSABLE_ENVIRONMENT == AddressableEnvironment.Develop
    //         ? PlayerSettings.bundleVersion + ".b"
    //         : PlayerSettings.bundleVersion;
    //     string minorVersion = VersionNameUtility.GetMinorVersion(currentVersion);
    //
    //     string defaultProfileId = settings.profileSettings.GetProfileId("Default");
    //
    //     string buildTarget = EditorUserBuildSettings.activeBuildTarget.ToString();
    //
    //     settings.profileSettings.SetValue(defaultProfileId, "ContentVersion", minorVersion);
    //
    //     EditorUtility.SetDirty(settings);
    //
    //
    //     //string remoteBuildPath = "/Users/t-nagaoka/Workspace/unity/udl-joggle/ServerData/" + minorVersion + "/" + buildTarget;
    //     string remoteBuildPath = "/Users/t-nagaoka/Workspace/unity/udl-joggle/ServerData/" + minorVersion;
    //     ClearDirectory(remoteBuildPath);
    //
    //     //UnityEngine.Debug.Log(settings.buildSettings.bundleBuildPath);
    //
    //     // Build Player Content
    //     AddressableAssetSettings.BuildPlayerContent();
    // }

    /// <summary>
    /// Clears all files and directories within the specified directory.
    /// </summary>
    /// <param name="path">The directory path to clear.</param>
    private static void ClearDirectory(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);

        if (!dir.Exists)
        {
            return; // If directory does not exist, no need to clear
        }

        foreach (FileInfo file in dir.GetFiles())
        {
            file.Delete();
        }

        foreach (DirectoryInfo subDir in dir.GetDirectories())
        {
            subDir.Delete(true); // Delete recursively
        }
    }

    [MenuItem("Build/Upload to GCS")]
    public static void UploadToGCS()
    {
        //var settings = AddressableAssetSettingsDefaultObject.Settings;
        //string defaultProfileId = settings.profileSettings.GetProfileId("Default");
        string currentVersion = PlayerSettings.bundleVersion;
        string minorVersion = VersionNameUtility.GetMinorVersion(currentVersion);
        string buildTarget = EditorUserBuildSettings.activeBuildTarget.ToString();

        string localPath = "/Users/t-nagaoka/Workspace/unity/udl-joggle/ServerData/" + minorVersion + "/" +
                           buildTarget + "/";
        string remotePath = "gs://joggle-f02d1.appspot.com/assets/" + minorVersion + "/";

        string command = string.Format("cp -r {0} {1}", localPath, remotePath);

        UnityEngine.Debug.Log(command);

        var startInfo = new ProcessStartInfo
        {
            FileName = "/Users/t-nagaoka/google-cloud-sdk/bin/gsutil",
            Arguments = command,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        var process = new Process
        {
            StartInfo = startInfo
        };
        process.OutputDataReceived += (sender, data) =>
        {
            UnityEngine.Debug.Log(data.Data); // Unityのコンソールにログを表示
        };
        process.ErrorDataReceived += (sender, data) =>
        {
            UnityEngine.Debug.LogError(data.Data); // エラーメッセージをUnityのコンソールに表示
        };

        process.Start();
        process.BeginOutputReadLine();
        process.WaitForExit();

        UnityEngine.Debug.Log(startInfo.Arguments);
        //UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());
    }
}