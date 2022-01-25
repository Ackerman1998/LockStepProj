using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


class Udp_Client
{
    private UdpClient _udpClient = null;
    private IPEndPoint iPEndPoint;
    private bool ready = false;

    public bool Ready { get => ready; set => ready = value; }

    public Udp_Client() {
        iPEndPoint = new IPEndPoint(IPAddress.Parse(NetUtils.GetLocalAddress()), NetConfig.port_udp);
        _udpClient = new UdpClient(iPEndPoint);
      
        
        StartReceiveMessage();
    }

    private void StartReceiveMessage()
    {
        _udpClient.BeginReceive(ReceiveAsync, _udpClient);
    }

    private void ReceiveAsync(IAsyncResult ar)
    {
        try
        {
            UdpClient client = ar.AsyncState as UdpClient;
            byte[] buffer = client.EndReceive(ar, ref iPEndPoint);
            byte[] totalLen = new byte[4];
            Array.Copy(buffer, 0, totalLen, 0, 4);
       
            int totalLength = BitConverter.ToInt32(totalLen, 0);
            PBCommon.Csmsgid scmsgid = (PBCommon.Csmsgid)buffer[4];
            byte[] bodyBuffer = new byte[totalLength - 1];
            Array.Copy(buffer, 5, bodyBuffer, 0, totalLength - 1);
            UdpProtobuf.Instance.ParseProtobuf(scmsgid, bodyBuffer);
            StartReceiveMessage();
        }
        catch (Exception e)
        {
            Debug.Log("Udp Receive Message Error:" + e);
        }
    }

    public void SendMessage(byte[] buffer)
    {
        if (_udpClient != null)
        {
            _udpClient.Send(buffer, buffer.Length, iPEndPoint);
        }
    }

    public void Restart()
    {

    }
    /// <summary>
    /// 场景销毁，释放
    /// </summary>
    public void Dispose()
    {

    }
}

