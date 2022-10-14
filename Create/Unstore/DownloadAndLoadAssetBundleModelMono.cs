using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DownloadAndLoadAssetBundleModelMono : MonoBehaviour
{
    public string m_path = "https://github.com/EloiStree/Test_DeployKey-A/blob/main/drone%20square%20version?raw=true";
    public string m_readMe;
    public string m_idKey;
    public bool m_overridePrevious;

    public Transform m_parent;
    public GameObject m_prefab;
    public GameObject m_intance;
    public UnityEvent       m_startLoading;
    public UnityEvent       m_failToLoad;
    public ModelLoadEvent   m_modelLoaded;
    public string m_prefabNameToLoad= "BundlePrefab";

    [System.Serializable]
    public class ModelLoadEvent : UnityEvent<GameObject> { }


    [ContextMenu("Generate Key")]
    public void GenerateKey() {

        m_idKey = ComputeSha256Hash(m_path);
    }

    [ContextMenu("OpenFolder")]
    public void OpenFolder()
    {
        Application.OpenURL(StoragePath());
    }

    private static string StoragePath()
    {
        return Path.Combine(Application.persistentDataPath , "Bundles");
    }

    public void Start()
    {
        LoadInspectorModel();
    }

    [ContextMenu("Load Inspector Model")]
    private void LoadInspectorModel()
    {
        StartCoroutine(InstantiateObject(m_path, m_overridePrevious));
    }
    public Dictionary<string, AssetBundle> m_bundles = new Dictionary<string, AssetBundle>();
    IEnumerator InstantiateObject(string url, bool overridePrevious=true)
    {
        m_startLoading.Invoke();
        m_path = url;
        m_overridePrevious= overridePrevious;

        m_idKey = ComputeSha256Hash(url);
        Directory.CreateDirectory(StoragePath());
        string storageFilePath = Path.Combine(StoragePath() , m_idKey);

        if (overridePrevious || ! File.Exists(storageFilePath)  ) {
            if (Application.isPlaying)
            {
                UnityWebRequest www = new UnityWebRequest(url);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    // Show results as text
                    Debug.Log(www.downloadHandler.text);

                    // Or retrieve results as binary data
                    byte[] results = www.downloadHandler.data;
                    File.WriteAllBytes(storageFilePath, results);
                }
            }
            else {
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(url, storageFilePath);
                    }
                }
                catch (Exception exce) {
                    Debug.Log("E:" + exce.StackTrace);
                    m_failToLoad.Invoke();
                    yield break;
                }

            }
        }
        url = storageFilePath;

        if (File.Exists(url))
            url = "file:///" + url;
        var request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = null;
        if (!m_bundles.ContainsKey(m_idKey))
        {
            bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
            m_bundles.Add(m_idKey, bundle);
        }
        else {
            bundle= m_bundles[m_idKey] ;
        }

        GameObject model = bundle.LoadAsset<GameObject>(m_prefabNameToLoad);
        m_prefab = model;
        TextAsset charDataFile = bundle.LoadAsset("ReadMe.txt") as TextAsset;
        if (charDataFile != null) m_readMe = charDataFile.text;
        else m_readMe = "";

        if (model == null)
        {
            m_failToLoad.Invoke();
            yield break;
        }
        GameObject created= Instantiate(model);
        m_intance = created;
        created.transform.parent = m_parent;
        created.transform.localPosition = Vector3.zero;
        created.transform.localRotation = Quaternion.identity;
        m_created.Add(created);
        m_modelLoaded.Invoke(model);
    }
    public List<GameObject> m_created = new List<GameObject>();

    [ContextMenu("Flush")]
    public void Flush() {

        for (int i = 0; i < m_created.Count; i++)
        {
            if (m_created[i] != null)
                Destroy(m_created[i]);
        }
        m_created.Clear();
    }

    public void LoadAndDownload(string url, bool overridePrevious) {
        StartCoroutine(InstantiateObject(url, overridePrevious));
    }
    static string ComputeSha256Hash(string rawData)
    {
        //source: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/#:~:text=The%20HashAlgorithm%20class%20is%20the,byte%20array%20of%20256%20bits.
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
