using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Build;
using UnityEngine;

public class PreBuildProcessor : IPreprocessBuild
{
    public int callbackOrder { get; }
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        var setting = Resources.Load<EnvironmentSettings>("EnvironmentSetting");
        
        // Set environment
        var ADDRESSABLE_ENVIRONMENT = setting.Environment;
        
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
    }
}
