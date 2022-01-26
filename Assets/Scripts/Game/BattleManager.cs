using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static GameStart;

public class BattleManager : MonoSingleton<BattleManager>
{
    public CallBack readyBattle_callback;
    public BattleData battleData;
    bool isStart = false;
    private void Awake()
    {
        
    }

    private void Start()
    {
        battleData = BattleData.Instance;
        if (GlobalConfig.Instance.gameType==GameType.Playback) {
            battleData.Init();
            PlayBack();
            return;
        }
        if (SceneManager.GetActiveScene().name == "StandaloneScene") {
            readyBattle_callback = ReadyBattleComplete_Standalone;
            readyBattle_callback();
        } else if (SceneManager.GetActiveScene().name == "GameScene") {
            UdpManager.Instance.Create();
            readyBattle_callback = ReadyBattleComplete;
            StartCoroutine(DelayInitFinish());
        }
    }

    public void LoadFile()
    {
        string path = Application.streamingAssetsPath + "/Desktopspeed.txt";
        StartCoroutine(Load(path));
    }

    IEnumerator Load(string path) {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone)
        {
            string text = unityWebRequest.downloadHandler.text;
            battleData.InitSpeedInfo(text);
        }
        else { 
            
        }
    }

    IEnumerator DelayInitFinish() {
        yield return new WaitUntil(()=> {
            return UdpManager.Instance.isInit;
        });
        PBBattle.UdpReadyBattle udpReadyBattle = new PBBattle.UdpReadyBattle();
        udpReadyBattle.Roomid = TcpManager.Instance.userData.roomId;
        udpReadyBattle.Uid = TcpManager.Instance.userData.uid;
        UdpManager.Instance.SendMessage(MessageData.GetSendMessage<PBBattle.UdpReadyBattle>(udpReadyBattle,PBCommon.Csmsgid.UdpBattleReady));
    }

    public void ReadyBattleComplete() {
        if (isStart) {
            return;
        }
        isStart = true;
        InvokeRepeating("RepeatSendFrameOperation",0,0.033f);
        StartCoroutine(StartLogicUpdate());
        RoleManager.Instance.Initialized();
    }

    private void RepeatSendFrameOperation() {
        battleData.SendCurrentOperationData();
    }

    IEnumerator StartLogicUpdate() {
        yield return new WaitUntil(() => {
            return battleData.GameCurrentFrame>0;
        });
        if (GlobalConfig.Instance.gameType == GameType.Playback)
        {
            InvokeRepeating("LateUpdateOperation", 0, 0.033f);
        }
        else {
            InvokeRepeating("LateUpdateOperation", 0, 0.02f);
        }
   
    }

    void LateUpdateOperation() {
        PBBattle.AllPlayerOperation allPlayerOperation = battleData.GetOperationData();
        if (allPlayerOperation != null)
        {
            RoleManager.Instance.AddAllRoleOperations(allPlayerOperation);
            RoleManager.Instance.Logic_Move();
            battleData.AddLogicFrameNum();
        }
        else {
          
        }
    }

    public void HandleAllPlayerOperations(PBBattle.UdpAllPlayerOperations allPlayerOperation) {
        battleData.AddFrameOperationData(allPlayerOperation);
    }

    private void OnDestroy()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            UdpManager.Instance.Dispose();
        }
    }



    #region Standalone Mode
    public void ReadyBattleComplete_Standalone()
    {
        if (isStart)
        {
            return;
        }
        isStart = true;
        InvokeRepeating("RepeatSendFrameOperation_Standalone", 0, 0.033f);
        StartCoroutine(StartLogicUpdate());
        RoleManager.Instance.Initialized_Standalone();
    }

    private void RepeatSendFrameOperation_Standalone()
    {
        battleData.SendCurrentOperationData_Standalone();
    }
    #endregion

    #region Play back Mode
    public void PlayBack() {
        if (isStart)
        {
            return;
        }
        isStart = true;
        RecordManager.Instance.Read((obj)=> {
            RecordFile recordFile = obj;
            RecordData [] recordData = recordFile.recordDatas.ToArray();
            foreach (RecordData recordData1 in recordData) {
                battleData.AddFrameOperationData(recordData1.udpPlayerOperations);
            }
            RoleManager.Instance.Initialized_Standalone();
            StartCoroutine(StartLogicUpdate());
            
        });
       
    }
    #endregion

}
