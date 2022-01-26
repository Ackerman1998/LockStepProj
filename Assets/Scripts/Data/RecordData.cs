using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBBattle;
[System.Serializable]
public class RecordData 
{
    public int frameId;
    public PBBattle.UdpAllPlayerOperations udpPlayerOperations;
}
[System.Serializable]
public class RecordFile {
    public List<RecordData> recordDatas;
}