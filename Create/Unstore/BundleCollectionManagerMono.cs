using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleCollectionManagerMono : MonoBehaviour
{
    public DownloadAndLoadAssetBundleModelMono m_bundleDownloader;
    public BundleListUrlMono m_bundleList;
    [TextArea(0,10)]
    public string m_bundlePerLine;
    public string[] m_bundleUrls;
    public int m_index;
    void Start()
    {
        LoadBundleListFromText();
    }

    [ContextMenu("Load Bundle List From Text")]
    private void LoadBundleListFromText()
    {
        m_bundleUrls = m_bundlePerLine.Split('\n');
        m_bundleList.m_bundleList.Set(in m_bundleUrls);
    }

    [ContextMenu("Next")]
    public void Next()
    {
        m_index++;
        if (m_index >= m_bundleList.m_bundleList.GetCount())
            m_index = 0;
        string link = m_bundleList.m_bundleList.GetUrlFromIndex(m_index);
        m_bundleDownloader.LoadAndDownload(link, true);

    }
    [ContextMenu("Random")]
    public void Random()
    {
        m_index = UnityEngine.Random.Range(0, m_bundleList.m_bundleList.GetCount());
        string link = m_bundleList.m_bundleList.GetUrlFromIndex(m_index);
        m_bundleDownloader.LoadAndDownload(link, true);

    }
    [ContextMenu("Previous")]
    public void Previous() {
        m_index--;
        if (m_index <0)
            m_index = m_bundleList.m_bundleList.GetCount()-1;
        string link = m_bundleList.m_bundleList.GetUrlFromIndex(m_index);
        m_bundleDownloader.LoadAndDownload(link, true);

    }

}
