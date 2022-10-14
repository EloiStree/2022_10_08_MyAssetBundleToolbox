using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleListUrlMono : MonoBehaviour
{
    public BundleListUrl m_bundleList;
}
[System.Serializable]
public class BundleListUrl
{
    public List<string> m_bunbleUrl = new List<string>();
    public void GetUrlFromIndex(int index, out string url) => url = m_bunbleUrl[index];
    public string GetUrlFromIndex(int index) { return m_bunbleUrl[index]; }
    public int GetCount() { return m_bunbleUrl.Count; }

    public void Set(in string[] m_bundleUrls)
    {
        m_bunbleUrl.Clear();
        m_bunbleUrl.AddRange(m_bundleUrls);
    }
    public void Add(in string[] m_bundleUrls)
    {
        m_bunbleUrl.AddRange(m_bundleUrls);
    }
    public void GetList(out List<string> url) { url = m_bunbleUrl; }
}