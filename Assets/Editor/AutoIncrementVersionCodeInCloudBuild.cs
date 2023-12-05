using UnityEngine;
using UnityEditor;
using System;

public class AutoIncrementVersionCodeInCloudBuild : MonoBehaviour
{
#if UNITY_CLOUD_BUILD
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
        string buildNumber = manifest.GetValue("buildNumber", "0");
        int buildNumberInt = int.Parse(buildNumber);
        int bundleVersion = PlayerSettings.Android.bundleVersionCode;
        if(buildNumberInt < bundleVersion)
        {
            buildNumberInt = bundleVersion + 1;
        }

        Debug.LogWarning("Setting build number to " + buildNumberInt);
        PlayerSettings.Android.bundleVersionCode = buildNumberInt;
        PlayerSettings.iOS.buildNumber = buildNumberInt.ToString();

        manifest.SetValue("buildNumber", (buildNumberInt + 1).ToString());
    }
#endif
}
