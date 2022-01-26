using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

public class TcpProtobuf : Singleton<TcpProtobuf>
{
    int uid = 10000;//先随便定义一个
    int index = 1;
    /// <summary>
    /// 解析数据
    /// </summary>
    public void ParseProtobuf(PBCommon.Csmsgid protocolId,byte [] bodyBuffer, Client _client) {
        switch (protocolId) {
            case PBCommon.Csmsgid.TCPREQUESTConnect:
                PBLogin.TcpRequestConnect requestConnect = MessageData.GetDeSerializeData<PBLogin.TcpRequestConnect>(bodyBuffer);
                Debug.Log("Receive msg:" + requestConnect.Token);
                break;
            case PBCommon.Csmsgid.TcpRequestLogin:
                PBLogin.TcpRequestLogin tcpRequestLogin = MessageData.GetDeSerializeData<PBLogin.TcpRequestLogin>(bodyBuffer);
                //check login data
                if (TcpManager.Instance.Check(tcpRequestLogin.Account, tcpRequestLogin.Password))
                {
                  
                    PBLogin.TcpResponseLogin tcpResponseLogin = new PBLogin.TcpResponseLogin();
                    tcpResponseLogin.Result = true;
                    tcpResponseLogin.Error = "No Error";
                    if (_client.ClientIpIsLocal())
                    {
                        tcpResponseLogin.udpPort = NetConfig.port_udp+ index;
                        _client.SetPortUdp(tcpResponseLogin.udpPort);
                        index++;
                    }
                    else {
                       
                        tcpResponseLogin.udpPort = NetConfig.port_udp;
                        _client.SetPortUdp(tcpResponseLogin.udpPort);
                    }
                    tcpResponseLogin.Uid = uid++;
                    _client.SendMessage(MessageData.GetSendMessage<PBLogin.TcpResponseLogin>(tcpResponseLogin, PBCommon.Scmsgid.TcpResponseLogin));
                    TcpManager.Instance.AddUser(tcpRequestLogin.Account, tcpResponseLogin.Uid.ToString(), _client);
                    Debug.Log("User :" + tcpRequestLogin.Account+" Login Success...");
                }
                else {
                    PBLogin.TcpResponseLogin tcpResponseLogin = new PBLogin.TcpResponseLogin();
                    tcpResponseLogin.Result = false;
                    tcpResponseLogin.Error = "Login Failed : Already Login";
                    _client.SendMessage(MessageData.GetSendMessage<PBLogin.TcpResponseLogin>(tcpResponseLogin,PBCommon.Scmsgid.TcpResponseLogin));
                }
                break;
            case PBCommon.Csmsgid.TcpRequestMatch:
                PBHall.TcpRequestMatch tcpRequestMatch = MessageData.GetDeSerializeData<PBHall.TcpRequestMatch>(bodyBuffer);
                int totalPeopleNum = tcpRequestMatch.peopleNum;
                int game_type = tcpRequestMatch.matchType;
                //if totalPeoplenum==1
                MatchManager.Instance.JoinMatch(game_type, totalPeopleNum, _client);
                break;

            case PBCommon.Csmsgid.TcpCancelMatch:
                PBHall.TcpCancelRequestMatch tcpCancelRequestMatch = MessageData.GetDeSerializeData<PBHall.TcpCancelRequestMatch>(bodyBuffer);
                MatchManager.Instance.CancelMatch(_client);
                break;
        }
    }
}
