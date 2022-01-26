using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoSingleton<GameStart>
{
    private string token;
    public GameObject panel_Login;
    public GameObject panel_Hall;
    public GameObject tips;
    public Text account;
    public Text password;
    public delegate void CallBack();
    public delegate void CallBackString(string msg);
    public CallBack login_callback;
    public CallBackString loginfailed_callback;
    public CallBack cancelRequestMatch_callback;
    public CallBack requestMatch_callback;
    public int peopleNum = 1;
    private void Awake()
    {
        token = SystemInfo.deviceUniqueIdentifier;
        login_callback += ConnectSuccess;
        loginfailed_callback += LoginFail;
        cancelRequestMatch_callback += CancelRquestMatchSuccess;
        requestMatch_callback += RquestMatchSuccess;
        Debug.Log("IP:"+ NetUtils.GetLocalAddress());
        TcpManager.Instance.Connect(NetUtils.GetLocalAddress(), (result) =>
        {
            if (result)
            {
                PBLogin.TcpRequestConnect tcpRequestConnect = new PBLogin.TcpRequestConnect();
                tcpRequestConnect.Token = token;
                Debug.Log("Send:" + tcpRequestConnect.Token);
                TcpManager.Instance.SendMessage(MessageData.GetSendMessage<PBLogin.TcpRequestConnect>(tcpRequestConnect, PBCommon.Csmsgid.TCPREQUESTConnect));
            }
            else
            {
                Debug.Log("Connect Failed");
            }
        });

    }
    public void ConnectSuccess() {
        panel_Login.SetActive(false);
        panel_Hall.SetActive(true);
    } 
    public void LoginFail(string msg) {
        tips.GetComponent<Text>().text = msg;
        tips.SetActive(true);
    }
    public void OnConnected()
    {
        


    }

    public void JoinGame() { 
    
    }

    public void Login() {
        if (account.text.Length<=0|| password.text.Length <= 0 ) {
            return;
        }
        PBLogin.TcpRequestLogin tcpRequestLogin = new PBLogin.TcpRequestLogin();
        tcpRequestLogin.Account = account.text;
        tcpRequestLogin.Password = password.text;
        TcpManager.Instance.SendMessage(MessageData.GetSendMessage<PBLogin.TcpRequestLogin>(tcpRequestLogin, PBCommon.Csmsgid.TcpRequestLogin));
        
    }

    public void RequestMatch() {
        //request 1 people match
        PBHall.TcpRequestMatch tcpRequestMatch = new PBHall.TcpRequestMatch();
        tcpRequestMatch.peopleNum = 2;
        tcpRequestMatch.matchType = 1;//1-Match 2-Rank
        TcpManager.Instance.SendMessage(MessageData.GetSendMessage<PBHall.TcpRequestMatch>(tcpRequestMatch, PBCommon.Csmsgid.TcpRequestMatch));
        transform.FindAll("ReuqestButton").gameObject.SetActive(false);
        transform.FindAll("CancelReqButton").gameObject.SetActive(true);
    }

    public void CancelRequestMatch() {
        PBHall.TcpCancelRequestMatch tcpCancelRequestMatch = new PBHall.TcpCancelRequestMatch();
        tcpCancelRequestMatch.Content = "Cancel Match";
        TcpManager.Instance.SendMessage(MessageData.GetSendMessage<PBHall.TcpCancelRequestMatch>(tcpCancelRequestMatch,PBCommon.Csmsgid.TcpCancelMatch));

    }
    public void CancelRquestMatchSuccess() {
        transform.FindAll("ReuqestButton").gameObject.SetActive(true);
        transform.FindAll("CancelReqButton").gameObject.SetActive(false);
    }
    /// <summary>
    /// 请求成功
    /// </summary>
    public void RquestMatchSuccess()
    {
        //
    }
}
