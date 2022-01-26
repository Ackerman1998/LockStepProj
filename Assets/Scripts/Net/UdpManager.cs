using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UdpManager : Singleton<UdpManager>
{
    private UdpClient _udpClient=null;
    public bool isInit = false;
    private IPEndPoint iPEndPoint;
    Thread thread;
    public void Create() {
        if (isInit) {
            return;
        }
        _udpClient = new UdpClient();
        iPEndPoint = new IPEndPoint(IPAddress.Parse(NetUtils.GetLocalAddress()), TcpManager.Instance.userData.portUdp);
        _udpClient.Connect(iPEndPoint);
        isInit = true;
        //StartReceiveMessage();
        thread = new Thread(Receive);
        thread.Start();
    }

    private void StartReceiveMessage() {
        _udpClient.BeginReceive(ReceiveAsync, _udpClient);
    }

    private void Receive() {
        IPEndPoint ipEp;
        ipEp = new IPEndPoint(IPAddress.Parse(NetUtils.GetLocalAddress()), TcpManager.Instance.userData.portUdp);
        while (isInit) {
            try
            {
                byte[] buffer = _udpClient.Receive(ref ipEp);
                byte[] totalLen = new byte[4];
                Array.Copy(buffer, 0, totalLen, 0, 4);
                int totalLength = BitConverter.ToInt32(totalLen, 0);
                PBCommon.Scmsgid scmsgid = (PBCommon.Scmsgid)buffer[4];
                byte[] bodyBuffer = new byte[totalLength - 1];
                Array.Copy(buffer, 5, bodyBuffer, 0, totalLength - 1);
                UdpProtobuf.Instance.ParseProtobuf(scmsgid, bodyBuffer);
            }
            catch (Exception e)
            {
                Debug.Log("Udp Receive Message Error:" + e);
            }
        }
    }

    private void ReceiveAsync(IAsyncResult ar)
    {
        try {
            if (ar.IsCompleted)
            {
                UdpClient client = ar.AsyncState as UdpClient;
                byte[] buffer = client.EndReceive(ar, ref iPEndPoint);
                byte[] totalLen = new byte[4];
                Array.Copy(buffer, 0, totalLen, 0, 4);
                int totalLength = BitConverter.ToInt32(totalLen, 0);
                PBCommon.Scmsgid scmsgid = (PBCommon.Scmsgid)buffer[4];
                byte[] bodyBuffer = new byte[totalLength - 1];
                Array.Copy(buffer, 5, bodyBuffer, 0, totalLength - 1);
                UdpProtobuf.Instance.ParseProtobuf(scmsgid, bodyBuffer);
                StartReceiveMessage();
            }
        }
        catch (Exception e) {
            Debug.Log("Udp Receive Message Error:" + e);
        }
    }

    public void SendMessage(byte []buffer) {
        if (isInit&& _udpClient!=null) {
            _udpClient.Send(buffer,buffer.Length);
        }
    }

    public void Restart() {

    }
    /// <summary>
    /// 场景销毁，释放
    /// </summary>
    public void Dispose() {
        isInit = false;
        thread.Abort();
        thread = null;
    }
}
