using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class BuildAutomation : MonoBehaviour
{
    public static string APIKey = "408c54102b487c53b6f569030d31851f";
    public static string OrganizationId = "18967492849752";
    public static string ProjectId = "d4125025-0ddb-438f-993f-98c4dbef9edf";
    public static string BuildTargetId = "default-android1";

    [MenuItem("Build/Build Automation")]
    public static void Build()
    {
        string url = "https://build-api.cloud.unity3d.com/api/v1/" +
                    $"orgs/{OrganizationId}/" +
                    $"projects/{ProjectId}/" +
                    $"buildtargets/{BuildTargetId}/builds";
        string jsonData = "{\"clean\": true, \"delay\": 30}";

        UnityEngine.Debug.Log("Run coroutine");

        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData, "application/json"))
        {
            www.SetRequestHeader("Authorization", "Basic 408c54102b487c53b6f569030d31851f");
            www.SendWebRequest();

            while(!www.isDone)
            {

            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                UnityEngine.Debug.Log(www.downloadHandler.text);
            }
        }
    }

}