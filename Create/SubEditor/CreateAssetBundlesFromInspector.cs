
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundlesFromInspector : MonoBehaviour
{
    public bool m_useRelativePath;
    public string m_whereToStoreUnityRelativePath= "Assets/AssetBundles";

    public bool m_useAbsolutePath;
    public string m_whereToStoreBundleAbsolutePath;


    public BuildAssetBundleOptions m_buildOption = BuildAssetBundleOptions.None;
    public BuildTarget m_buildTarget = BuildTarget.StandaloneWindows;

    [ContextMenu("Build Asset Bundles")]
    public void BuildAllAssetBundles()
    {
        if (m_whereToStoreUnityRelativePath.Length > 0)
        {
            string assetBundleDirectory = m_whereToStoreUnityRelativePath;
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                           m_buildOption,
                                            m_buildTarget);
        }
        if (m_whereToStoreBundleAbsolutePath.Length > 0)
        {
            string assetBundleDirectory = m_whereToStoreBundleAbsolutePath;
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                           m_buildOption,
                                            m_buildTarget);
        }
    }
}
#endif
