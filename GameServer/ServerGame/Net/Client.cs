using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;


public class Client
{
    MessageQueue<MessageNode> messageQueue = null;
    private Socket _socket;
    private string client_ip;
    byte[] messageBuffer = new byte[1024];
    byte[] buffHead = new byte[4];//read mesage head byte (integer)
    int header = 0;
    int readIndex = 0;
    Bufferbyte bufferbyte = new Bufferbyte(1024 * 1024);
    private UserData myUser;
    public int matchingNum = -1;
    private int portUdp = 2846;
    public  Client(Socket cc,string ipAddress) {
        _socket = cc;
        client_ip = ipAddress;
        messageQueue = new MessageQueue<MessageNode>();
        messageQueue.Start();
        ReceiveMessage();
    }

    public void SetPortUdp(int port) {
        portUdp = port;
    }
    public int GetUdpPort() {
        return portUdp;
    }
    public bool ClientIpIsLocal() {
        return string.Equals(client_ip,NetUtils.GetLocalAddress());
    }

    public void SetUser(UserData userData) {
        myUser = userData;
    }
    public string GetIpAddress() {
        return client_ip;
    }

    public UserData GetUser() {
        return myUser;
    }
    ~Client()
    {
        _socket.Close();
        client_ip = null;
    }

    private void ReceiveMessage()
    {
        //Debug.Log(client_ip+"Start Receive Msg...");
        _socket.BeginReceive(buffHead, readIndex, 1, SocketFlags.None, callback, _socket);
    }
    /// <summary>
    /// Parse Message 
    /// Length : 4 -- 1 -- body.length
    /// </summary>
    /// <param name="ar"></param>
    private void callback(IAsyncResult ar)
    {
        try
        {
            int count = _socket.EndReceive(ar);
            if (count == 0)
            {
                HandleClientQuit();
            }
            else
            {
                readIndex += count;
                if (header == 0)
                {
                    //read message head : message total length
                    if (readIndex < 4)
                    {
                        ReceiveMessage();
                    }
                    else
                    {
                        bufferbyte.WriteBytes(buffHead);
                        header = bufferbyte.ReadInt();
                        bufferbyte.Clear();
                        readIndex = 0;
                        //start read protocol num+protocol message body
                        _socket.BeginReceive(bufferbyte.GetByteBuffer(), readIndex, header, SocketFlags.None, callback, _socket);
                    }
                }
                else
                {
                    if (header == readIndex)
                    {
                        bufferbyte.WriteIndex += readIndex;
                        CreateProtocolMessage(bufferbyte);
                        //CreateMessage(bufferbyte);
                        readIndex = 0;
                        header = 0;
                        bufferbyte.Clear();
                        ReceiveMessage();
                    }
                    else
                    {
                        // Receive();
                    }
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log("Exception:{" + e);
        }
        finally
        {

        }


    }

    private void CreateProtocolMessage(Bufferbyte buffer)
    {
        Bufferbyte newByte = new Bufferbyte(buffer.GetBytes().Length);
        newByte.WriteBytes(buffer.GetBytes(), buffer.GetBytes().Length);
        //byte [] buf = bufferbyte.GetBytes();
        //PBCommon.Csmsgid id  = (PBCommon.Csmsgid)buf[0];
        //byte[] bodydata = new byte[buf.Length-1];
        //Array.Copy(buf, 1, bodydata, 0, buf.Length - 1);
        //start parse message
        //TcpProtobuf.Instance.ParseProtobuf(id, bodydata);

        MessageNode messageNode = new MessageNode();
        messageNode.methodName = "";
        messageNode._clientGame = this;
        messageNode.bufferbyte = newByte;
        messageNode.client = _socket;
        AddDataToReceive(messageNode);
    }
    private void AddDataToReceive(MessageNode messageNode)
    {
        messageQueue.AppendMessage(messageNode);

    }

    public void SendMessage(byte[] buffer)
    {
        if (_socket!=null)
        {
            _socket.Send(buffer);
        }
    }
    /// <summary>
    /// 退出，做释放处理
    /// </summary>
    private void HandleClientQuit() {
        Debug.Log(myUser.account + " disconnect...");
        _socket.Close();
    }
}
