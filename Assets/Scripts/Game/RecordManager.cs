using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class RecordManager : MonoSingleton<RecordManager>
{
    bool record = false;
    private RecordFile recordFile=null;
    public RecordFile recordFileHistory;
    private string path = Application.streamingAssetsPath + "/GameRecord.rcd";
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
        using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
            binary.Serialize(fileStream, recordFile);
        }
    }
    private void OnDestroy()
    {
        if (GlobalConfig.Instance.gameType == GameType.Playback)
        {
            return;
        }
        //Save();
    }
    public void Read(Action<RecordFile> callback)
    {
        StartCoroutine(Read_Cor(path, callback));
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
