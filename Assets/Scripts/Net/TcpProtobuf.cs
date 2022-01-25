using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TcpProtobuf : Singleton<TcpProtobuf>
{
    /// <summary>
    /// 解析数据
    /// </summary>
    public void ParseProtobuf(PBCommon.Scmsgid protocolId,byte [] bodyBuffer) {
        bool result;
        switch (protocolId) {
            case PBCommon.Scmsgid.TcpResponseLogin:
                PBLogin.TcpResponseLogin tcpResponseLogin = MessageData.GetDeSerializeData<PBLogin.TcpResponseLogin>(bodyBuffer);
                result = tcpResponseLogin.Result;
                if (result)
                {
                    TcpManager.Instance.userData = new UserData();
                    TcpManager.Instance.userData.uid = tcpResponseLogin.Uid;
                    TcpManager.Instance.userData.portUdp = tcpResponseLogin.udpPort;
                    Debug.Log("Login Success: uid="+ tcpResponseLogin.Uid+",udpPort:"+ tcpResponseLogin.udpPort);
                    NetGlobal.Instance.AddAction(() =>
                    {
                        GameStart.Instance.login_callback();
                    });
                }
                else {
                    //login failed 
                    NetGlobal.Instance.AddAction(() =>
                    {
                        GameStart.Instance.loginfailed_callback(tcpResponseLogin.Error);
                    });
                }
                break;

            case PBCommon.Scmsgid.TcpResponseCancelMatch:
                PBHall.TcpResponseCancelMatch tcpResponseCancelMatch = MessageData.GetDeSerializeData<PBHall.TcpResponseCancelMatch>(bodyBuffer);
                result = tcpResponseCancelMatch.Result;
                if (result)
                {
                    NetGlobal.Instance.AddAction(() =>
                    {
                        GameStart.Instance.cancelRequestMatch_callback();
                    });
                }
                else { 
                    //wtf?
                }
                break;

            case PBCommon.Scmsgid.TcpResponseUpdateMatching:
                PBHall.TcpResponseMatching tcpResponseMatching = MessageData.GetDeSerializeData<PBHall.TcpResponseMatching>(bodyBuffer);
                int currentNum = tcpResponseMatching.currentPeopleNum;
                Debug.Log("current match people number:"+currentNum);
                break;
            case PBCommon.Scmsgid.TcpResponseRequestMatch:
                PBHall.TcpResponseMatch tcpResponseMatch = MessageData.GetDeSerializeData<PBHall.TcpResponseMatch>(bodyBuffer);
                result = tcpResponseMatch.Result;
                int roomid = tcpResponseMatch.roomId;
                TcpManager.Instance.userData.roomId = roomid;
                Debug.Log("joint success,roomid:"+ roomid);
               
                break;

            case PBCommon.Scmsgid.TcpEnterBattle:

                PBBattle.TcpEnterBattle tcpEnterBattle = MessageData.GetDeSerializeData<PBBattle.TcpEnterBattle>(bodyBuffer);
                TcpManager.Instance.userData.seed = tcpEnterBattle.randSeed;
                TcpManager.Instance.userData.battleUserInfoes= new List<PBBattle.BattleUserInfo>(tcpEnterBattle.battleUserInfoes);
                NetGlobal.Instance.AddAction(() =>
                {
                    //GameStart.Instance.requestMatch_callback();
                    SceneManager.LoadScene("GameScene");
                });
               
                break;
        }
    }
}
