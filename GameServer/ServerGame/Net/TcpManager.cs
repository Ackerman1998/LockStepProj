using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TcpManager : Singleton<TcpManager>
{
    private Socket _socket = null;
    //login backward join dicClient
    private Dictionary<string, Client> dictClient = new Dictionary<string, Client>();
    Action<bool> callbackConnect = null;
    private bool running = false;

    public void Start(string addr, Action<bool> result) {
        callbackConnect = result;
        IPAddress address;
        bool ok = IPAddress.TryParse(addr, out address);
        if (!ok) {
            Debug.Log("Address is error!");
            result?.Invoke(false);
            return;
        }
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(address, NetConfig.port_tcp);
        _socket.Bind(iPEndPoint);
        _socket.Listen(50);
        Debug.Log("Start Launch Success..."+ addr);
        running = true;
        //Thread thread = new Thread(ConnectSuccess);
        //thread.Start();
        ConnectSuccess();
    }

    private void ConnectSuccess()
    {
        _socket.BeginAccept(AcceptCallBack, _socket);
    }

    private void AcceptCallBack(IAsyncResult ar)
    {
        Socket server = ar.AsyncState as Socket;
        Socket client = server.EndAccept(ar);
        string ip = client.RemoteEndPoint.ToString().Split(':')[0];
        IPEndPoint ipep = client.LocalEndPoint as IPEndPoint;
        Debug.Log(ip + " is Connnected...");
        Client cc = new Client(client, ip);
        server.BeginAccept(AcceptCallBack, server);
    }

    public void AddUser(string account,string uid,Client client) {
        dictClient.Add(account,client);
        UserData user = new UserData();
        user.account = account;
        user.uid = uid;
        client.SetUser(user);
    }

    public bool Check(string account,string password) {
        bool result = false;
        if (dictClient.ContainsKey(account))
        {
            result = false;
        }
        else {
            result = true;
        }
        return result;
    }
    public void SendMessage(byte [] buffer)
    {
        if (running) {
            _socket.Send(buffer);
        }
    }
}
// totalLength(int),csid(int),message(proto class)