using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Main : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject _assetReference; 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _assetReference.LoadAssetAsync<GameObject>().Completed += OnLoaded;
        }
    }

    public void OnLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
        }
    }
    
}
