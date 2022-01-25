using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TcpManager : Singleton<TcpManager>
{
    private Socket _socket = null;
    MessageQueue<MessageNode> messageQueue = null;
    Action<bool> callbackConnect = null;
    private bool running = false;
    byte[] messageBuffer = new byte[1024];
    byte[] buffHead = new byte[4];//read mesage head byte (integer)
    int header = 0;
    int readIndex = 0;
    Bufferbyte bufferbyte = new Bufferbyte(1024 * 1024);
    public UserData userData;
    public void Connect(string addr, Action<bool> result) {
        callbackConnect = result;
        messageQueue = new MessageQueue<MessageNode>();
        messageQueue.Start();
        IPAddress address;
        bool ok = IPAddress.TryParse(addr, out address);
        if (!ok) {
            Debug.Log("Address is error!");
            result(false);
            return;
        }
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(address, NetConfig.port);
        _socket.BeginConnect(iPEndPoint, ConnectSuccess, _socket);
    }

    private void ConnectSuccess(IAsyncResult ar)
    {
        try {
            Socket client = ar.AsyncState as Socket;
            client.EndConnect(ar);
            Debug.Log("Connect Success");
            running = true;
            callbackConnect(true);
            
            ReceiveMessage();
        } catch (Exception e) {
            callbackConnect(false);
            Debug.Log("Connect is error!" + e);
        } finally {

        }

    }
    private void ReceiveMessage() {
        _socket.BeginReceive(buffHead, readIndex, 1, SocketFlags.None, callback, null);
    }
    /// <summary>
    /// Parse Message 
    /// Length : 4 -- 1 -- body.length
    /// </summary>
    /// <param name="ar"></param>
    private void callback(IAsyncResult ar)
    {
      
        try {
            int count = _socket.EndReceive(ar);
            if (count == 0)
            {
                Debug.Log("Server is disconnect...");
                _socket.Close();
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
        Bufferbyte bufferbyte = new Bufferbyte(buffer.GetBytes().Length);
        bufferbyte.WriteBytes(buffer.GetBytes(), buffer.GetBytes().Length);
        //byte [] buf = bufferbyte.GetBytes();
        //PBCommon.Csmsgid id  = (PBCommon.Csmsgid)buf[0];
        //byte[] bodydata = new byte[buf.Length-1];
        //Array.Copy(buf, 1, bodydata, 0, buf.Length - 1);
        //start parse message
        //TcpProtobuf.Instance.ParseProtobuf(id, bodydata);

        MessageNode messageNode = new MessageNode();
        messageNode.methodName = "";
        messageNode.bufferbyte = bufferbyte;
        messageNode.client = _socket;
        AddDataToReceive(messageNode);
    }

    private void CreateMessage(Bufferbyte buffer)
    {
        string methodName = buffer.ReadString();
        Bufferbyte bufferbyte = new Bufferbyte(buffer.GetBytes().Length);
        bufferbyte.WriteBytes(buffer.GetBytes(), buffer.ReadIndex);
        MessageNode messageNode = new MessageNode();
        messageNode.methodName = methodName;
        messageNode.bufferbyte = bufferbyte;
        messageNode.client = _socket;
        AddDataToReceive(messageNode);
    }

    private void AddDataToReceive(MessageNode messageNode)
    {
        messageQueue.AppendMessage(messageNode);

    }

    public void SendMessage(byte [] buffer)
    {
        if (running) {
            _socket.Send(buffer);
        }
    }

    public void Close() {
        if (running)
        {
            _socket.Close();
        }
        if (messageQueue!=null) {
            messageQueue.Dispose();
        }
    }
}
// totalLength(int),csid(int),message(proto class)