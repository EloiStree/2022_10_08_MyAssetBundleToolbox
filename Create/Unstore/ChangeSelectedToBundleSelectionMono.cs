#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ChangeSelectedToBundleSelectionMono : MonoBehaviour
{

    public string m_bundleName;
    public List<string> m_path=new List<string>();
    [ContextMenu("ChangeBundleName with inspector")]
    public void ChangeBundleNameInspectorName()
    {
        var assets = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();

        m_path.Clear();
        foreach (var a in assets)
        {
            string assetPath = AssetDatabase.GetAssetPath(a);
            m_path.Add(assetPath);
            AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(m_bundleName, "");
        }

    }
    [ContextMenu("RemoveAllBundle ")]
    public void RemoveAllBundle() {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (string name in names) { 
            Debug.Log("Asset Bundle: " + name);
            AssetDatabase.RemoveAssetBundleName(name, true);
        }
    }


    [ContextMenu("ChangeBundleName wiht H256 ID")]
    public void ChangeBundleNameWithH256()
    {
        var assets = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();

        foreach (var a in assets)
        {
            string assetPath = AssetDatabase.GetAssetPath(a);
            int slashIndex = assetPath.LastIndexOf("/");
            if (slashIndex < 0) slashIndex = assetPath.LastIndexOf("\\");
            string assetPathWithCut = assetPath;
            if (slashIndex > -1) assetPathWithCut = assetPath.Substring(0, slashIndex);
            string id = ComputeSha256Hash(assetPathWithCut);
            AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(id, "");
        }
    }


    public List<string> m_test;
    [ContextMenu("ChangeBundle Explore Folder With H256")]
    public void ChangeBundleExploreFolderWithH256()
    {
        Dictionary<string,string> allPath = new Dictionary<string, string>();
        var assets = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();
        m_test = Selection.objects.Select(k=> AssetDatabase.GetAssetPath(k)).ToList();
       

        foreach (var a in assets)
        {
            string assetPath = AssetDatabase.GetAssetPath(a);

            if (File.Exists(assetPath))
            {
                if (!allPath.ContainsKey(assetPath))
                    allPath.Add(assetPath, null);
            }
            if( Directory.Exists(assetPath)){
                string [] ps =  Directory.GetFiles(assetPath,"*", SearchOption.AllDirectories);
                foreach (var item in ps)
                {
                    if (!allPath.ContainsKey(item))
                        allPath.Add(item, null);
                }

            }
        }
        foreach (var assetPath in allPath.Keys.ToList())
        {
            int slashIndex = assetPath.LastIndexOf("/");
            if (slashIndex < 0) slashIndex = assetPath.LastIndexOf("\\");
            string assetPathWithCut = assetPath;
            if (slashIndex > -1) assetPathWithCut = assetPath.Substring(0, slashIndex);
            string id = ComputeSha256Hash(assetPathWithCut);
            AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(id, "");
        }
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



    public string m_selection="";
    public List<string> m_selections = new List<string>();


    [ContextMenu("Refresh")]
    public  string GetSelectedPathOrFallback()
    {
        m_selection = "";
        m_selections.Clear();

        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(path))
                    m_selection = path;
                m_selections.Add(path);
            }
        }
        return path;
    }
}
#endif