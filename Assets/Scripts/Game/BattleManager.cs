using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        UdpManager.Instance.Create();
        battleData = BattleData.Instance;
        readyBattle_callback = ReadyBattleComplete;
        StartCoroutine(DelayInitFinish());
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


        InvokeRepeating("LateUpdateOperation", 0, 0.02f);
    }

    void LateUpdateOperation() {
        PBBattle.AllPlayerOperation allPlayerOperation = battleData.GetOperationData();
        if (allPlayerOperation!=null) {
            RoleManager.Instance.AddAllRoleOperations(allPlayerOperation);
            RoleManager.Instance.Logic_Move();
            battleData.AddLogicFrameNum();
        }
    }

    public void HandleAllPlayerOperations(PBBattle.UdpAllPlayerOperations allPlayerOperation) {
        battleData.AddFrameOperationData(allPlayerOperation);
    }

    private void OnDestroy()
    {
        UdpManager.Instance.Dispose();
    }
}
