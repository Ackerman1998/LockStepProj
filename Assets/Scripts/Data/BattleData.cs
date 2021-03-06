using PBBattle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleData:Singleton<BattleData>
{
    public int mapWidth=50000;
    public int mapHeigh= 50000;
    private int gameCurrentFrame = 0;

    private int logicCurrentFrame = 0;

    private int messageId = 0;
    PlayerOperation selfOperation = null;
    private Dictionary<int, GameVector2> dic_speed = new Dictionary<int, GameVector2>();
    private List<AllPlayerOperationContainer> allPlayerOperations = new List<AllPlayerOperationContainer>();
    public int GameCurrentFrame {
        get { return allPlayerOperations.Count; }
    }  
    public int GameCurrentFramePlayBack {
        get { return logicCurrentFrame; }
        set { logicCurrentFrame = value; }
    }

    public List<AllPlayerOperationContainer> AllPlayerOperations { get => allPlayerOperations; set => allPlayerOperations = value; }

    public void Init() {
        BattleManager.Instance.LoadFile();
        //string _fileStr = File.ReadAllText(Application.streamingAssetsPath+ "/Desktopspeed.txt") ;
        //InitSpeedInfo(_fileStr);
    }

    public void InitSpeedInfo(string _fileStr)
    {
        dic_speed.Clear();
        
        
        string[] lineArray = _fileStr.Split("\n"[0]);

        int dir;
        for (int i = 0; i < lineArray.Length; i++)
        {
            if (lineArray[i] != "")
            {
                GameVector2 date = new GameVector2();
                string[] line = lineArray[i].Split(new char[1] { ',' }, 3);
                dir = System.Int32.Parse(line[0]);
                date.x = System.Int32.Parse(line[1]);
                date.y = System.Int32.Parse(line[2]);
                //dic_speed[dir] = date;
                dic_speed.Add(dir,date);
            }
        }
    }

    public void SendCurrentOperationData() {
        if (selfOperation == null) {
            InitselfOperation();
            Init();
        }
        messageId++;
        PBBattle.UdpPlayerOperations udpPlayerOperations = new PBBattle.UdpPlayerOperations();
        udpPlayerOperations.frameNum = messageId;
        udpPlayerOperations.Roomid = TcpManager.Instance.userData.roomId;
        udpPlayerOperations.Operation = selfOperation;
        udpPlayerOperations.Operation.Uid = TcpManager.Instance.userData.uid;
        UdpManager.Instance.SendMessage(MessageData.GetSendMessage<PBBattle.UdpPlayerOperations>(udpPlayerOperations, PBCommon.Csmsgid.UdpUpPlayerOperations));
    }

    public void SendCurrentOperationData_Standalone()
    {
        if (selfOperation == null)
        {
            InitselfOperation();
            Init();
        }
        messageId++;
        PBBattle.UdpAllPlayerOperations udpAllPlayerOperations = new PBBattle.UdpAllPlayerOperations();
        PBBattle.AllPlayerOperation allPlayerOperation = new PBBattle.AllPlayerOperation();
        PlayerOperation sop=new PlayerOperation();
        sop.Move = selfOperation.Move;
        sop.operationID = selfOperation.operationID;
        sop.operationValue1 = selfOperation.operationValue1;
        sop.operationValue2 = selfOperation.operationValue2;
        sop.rightOperation = selfOperation.rightOperation;
        sop.Uid = selfOperation.Uid;
        allPlayerOperation.Operations.Add(sop);
        udpAllPlayerOperations.frameID = messageId;
        udpAllPlayerOperations.Operations = allPlayerOperation;
        AddFrameOperationData(udpAllPlayerOperations);
        RecordManager.Instance.AddFrameDataToFile(messageId, udpAllPlayerOperations);
    }

    private void InitselfOperation()
    {
        selfOperation = new PlayerOperation();
    }

    public void AddFrameOperationData(PBBattle.UdpAllPlayerOperations allPlayerOperation) {
        PBBattle.AllPlayerOperation all = allPlayerOperations.Find((x) => (x.frameNum == allPlayerOperation.frameID)).playerOperation;
        if (all != null)
        {

        }
        else {
         
            allPlayerOperations.Add(new AllPlayerOperationContainer(allPlayerOperation.Operations, allPlayerOperation.frameID));
        }
    }

    public PBBattle.AllPlayerOperation GetOperationData() {
        PBBattle.AllPlayerOperation allPlayerOperation = allPlayerOperations.Find((x)=>(x.frameNum== logicCurrentFrame+1)).playerOperation;
      
        return allPlayerOperation;
    }

    public void AddLogicFrameNum() {
        logicCurrentFrame++;
    }

    public void UpdateMoveDir(int upDir)
    {
        if (GlobalConfig.Instance.gameType == GameType.Playback) {
            return;
        }
        selfOperation.Move = upDir;
    }

    public GameVector2 GetSpeed(int _dir)
    {
        return dic_speed[_dir];
    }

    //坐标不超出地图
    public GameVector2 GetMapLogicPosition(GameVector2 _pos)
    {
        return new GameVector2(Mathf.Clamp(_pos.x, -mapWidth, mapWidth), Mathf.Clamp(_pos.y,-mapHeigh, mapHeigh));
    }
}
public struct AllPlayerOperationContainer {
    public PBBattle.AllPlayerOperation playerOperation;
    public int frameNum;

    public AllPlayerOperationContainer(AllPlayerOperation playerOperation, int frameNum)
    {
        this.playerOperation = playerOperation;
        this.frameNum = frameNum;
    }
}