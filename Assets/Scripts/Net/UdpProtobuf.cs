using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UdpProtobuf : Singleton<UdpProtobuf>
{
    public void ParseProtobuf(PBCommon.Scmsgid protocolId, byte[] bodyBuffer)
    {
        bool result;
        switch (protocolId) {
            case PBCommon.Scmsgid.UdpBattleStart:
                PBBattle.UdpReadyBattleResponse udpReadyBattleResponse = MessageData.GetDeSerializeData<PBBattle.UdpReadyBattleResponse>(bodyBuffer);
                result = udpReadyBattleResponse.Result;
                Debug.Log("UdpBattleStart");
                if (result) {
                    NetGlobal.Instance.AddAction(()=> {
                        BattleManager.Instance.readyBattle_callback();
                    });
                }
                break;
            case PBCommon.Scmsgid.UdpDownFrameOperations:
                PBBattle.UdpAllPlayerOperations udpAllPlayerOperations = MessageData.GetDeSerializeData<PBBattle.UdpAllPlayerOperations>(bodyBuffer);
                NetGlobal.Instance.AddAction(() => {
                    BattleManager.Instance.HandleAllPlayerOperations(udpAllPlayerOperations);
                });
                //Debug.Log("Receive Game Frame Data [ Frame Num : "+ udpAllPlayerOperations .frameID+" ] Success! move = "+ udpAllPlayerOperations.Operations.Operations.Count+",[0]=" + udpAllPlayerOperations.Operations.Operations[0].Move 
                //    + ",[1]=" + udpAllPlayerOperations.Operations.Operations[1].Move);
                break;
            
        }
    }
}
