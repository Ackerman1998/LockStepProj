using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RecordManager : MonoSingleton<RecordManager>
{
    bool record = false;
    private RecordFile recordFile=null;
    public RecordFile recordFileHistory;
    private string path = Application.streamingAssetsPath + "/GameRecord/";
    // Start is called before the first frame update
    void Start()
    {
        record = GlobalConfig.Instance.recording;
    }
    public void StartRecordGame() {
        if (!record) {
            return;
        }
    }

    public void AddFrameDataToFile(int frame, PBBattle.UdpAllPlayerOperations udpPlayerOperations) {
        if (recordFile==null) {
            recordFile = new RecordFile();
            recordFile.recordDatas = new List<RecordData>();
        }
        RecordData recordData = new RecordData();
        recordData.frameId = frame;
        recordData.udpPlayerOperations = udpPlayerOperations;
        recordFile.recordDatas.Add(recordData);
        Debug.Log("Record Game Frame Data;::Move" + udpPlayerOperations.Operations.Operations[0].Move);
    }

    public void Save() {
        BinaryFormatter binary = new BinaryFormatter();
        DateTime dateTime = DateTime.Now;
        StringBuilder sb = new StringBuilder();
        string time = dateTime.ToString();
        string [] times = time.Split(' ');
        times[0].Replace(@"\","/");
        for (int i=0;i<times[0].Split('/').Length;i++) {
            sb.Append(times[0].Split('/')[i]);
            sb.Append("-");
        }
        for (int i = 0; i < times[1].Split(':').Length; i++)
        {
            sb.Append(times[1].Split(':')[i]);
            if (i == times[1].Split(':').Length - 1)
            {

            }
            else {
                sb.Append("-");
            }
        }
        string pathReal = Path.Combine(path, sb.ToString()+".rcd");
        using (FileStream fileStream = File.Open(pathReal, FileMode.OpenOrCreate))
        {
            binary.Serialize(fileStream, recordFile);
        }
    }
    private void OnDestroy()
    {
        if (GlobalConfig.Instance.gameType != GameType.Playback&&SceneManager.GetActiveScene().name!= "StandaloneScene"
            ||!GlobalConfig.Instance.recording
            )
        {
            return;
        }
        Save();
    }
    public void Read(Action<RecordFile> callback)
    {
        string readPath = Path.Combine(Application.streamingAssetsPath, "GameRecord", GlobalConfig.Instance.currentRecordName);
        StartCoroutine(Read_Cor(readPath, callback));
    }
    IEnumerator Read_Cor(string path,Action<RecordFile> callback) {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            RecordFile rf = ReadRecord(unityWebRequest.downloadHandler.data);
            recordFileHistory = rf;
            callback?.Invoke(rf);
        }
        else
        {

        }
    }
    private RecordFile ReadRecord(byte [] buffer) {
        BinaryFormatter binary = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(buffer))
        {
            return binary.Deserialize(stream) as RecordFile;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            Save();
            Debug.LogError("存档成功");
        }
    }
}
