using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAssetFromClassicFilePath : MonoBehaviour
{
    public string m_bundleName;
    public string m_prefabName;

    void Start()
    {
        var myLoadedAssetBundle 
            = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, m_bundleName));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(m_prefabName);
        Instantiate(prefab);
    }
}