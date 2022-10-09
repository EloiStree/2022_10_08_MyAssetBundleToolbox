using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAssetFromURLDemo : MonoBehaviour
{
    public string m_path= "https://github.com/EloiStree/Test_DeployKey-A/blob/main/drone%20square%20version?raw=true";

    [TextArea(0, 5)]
    public string m_textReadMeMd;
    [TextArea(0, 5)]
    public string m_textReadMeTxt;
    [TextArea(0, 5)]
    public byte[] m_textDefaultBytes;
    void Start() {
        StartCoroutine(InstantiateObject());
    }

    IEnumerator InstantiateObject()
    {
        
        string url = m_path;
        if (File.Exists(url))
            url = "file:///" + url;
        var request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        GameObject drone = bundle.LoadAsset<GameObject>("Drone");
        TextAsset charDataFile = bundle.LoadAsset("ReadMe.txt") as TextAsset;
        if (charDataFile != null) m_textReadMeTxt = charDataFile.text;
        if (charDataFile != null) m_textDefaultBytes = charDataFile.bytes;
        //charDataFile = bundle.LoadAsset("ReadMe.bytes") as TextAsset;
        //if (charDataFile != null) m_textDefaultBytes = charDataFile.bytes;
        drone.transform.parent = this.transform;
        Instantiate(drone);
    }
}
