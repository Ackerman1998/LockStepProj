using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.Text;

public class TestScripts : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://127.0.0.1:1003/geturl");
        //yield return unityWebRequest.SendWebRequest();
        //if (unityWebRequest.isDone&& !unityWebRequest.isHttpError) {
        //    Debug.Log(unityWebRequest.downloadHandler.text);
        //}
        LoginData loginData = new LoginData();
        loginData.userId = "1";
        loginData.password = "abcdefg";
        string text = JsonMapper.ToJson(loginData);
        byte [] bb = Encoding.Default.GetBytes(text);
        string newText = Encoding.Default.GetString(bb);
        Debug.Log(newText);
        UnityWebRequest unityWebRequest = UnityWebRequest.Post("http://127.0.0.1:1003/requestlogin", "POST");
        unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bb);
        unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone && !unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.downloadHandler.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
class LoginData {
    public string userId;
    public string password;
}