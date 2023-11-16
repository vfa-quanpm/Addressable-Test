using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionNameUtility
{
    public static string GetMinorVersion(string currentVersion)
    {
        string[] numbers = currentVersion.Split('.');

        if (numbers.Length < 3)
        {
            throw new System.Exception("Bundle version must include minor version number.");
        }

        //string minorVersion = numbers[0] + "." + numbers[1];

        return currentVersion;
    }
}
