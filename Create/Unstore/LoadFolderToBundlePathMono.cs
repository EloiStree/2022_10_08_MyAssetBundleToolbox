using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadFolderToBundlePathMono : MonoBehaviour
{
    public string m_directory;
    [TextArea(0,10)]
    public string m_bundlePath;

    [ContextMenu("Refresh List")]
    void RefreshList()
    {
        m_bundlePath =string.Join("\n",Directory.GetFiles(m_directory).Where(k=>k.IndexOf(".meta")<0).ToList());
    }

}
