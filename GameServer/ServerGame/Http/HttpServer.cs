using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


abstract class HttpServer : IServer
{
    private bool startUp = false;
   

    public string ServerIP { get; private set; }

    public int ServerPort { get; private set; }

    public string ServerRoot { get; private set; }
    
    private TcpListener serverListener;

    private X509Certificate serverCertificate = null;

    public void StartUp() {
        if (startUp) {
            return;
        }
        this.ServerRoot = AppDomain.CurrentDomain.BaseDirectory;
        serverListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), NetConfig.port_http));
        this.ServerIP = "127.0.0.1";
        this.ServerPort = NetConfig.port_http;
        serverListener.Start();
        startUp = true;
        while (startUp) {
            TcpClient client = serverListener.AcceptTcpClient();
       
            Thread thread = new Thread(()=> {
                HandleRequest(client);
            });
            thread.Start();
        }
    }
    private void HandleRequest(TcpClient client) {
        var receiveStream = client.GetStream();
        HttpRequest httpRequest = new HttpRequest(receiveStream);
        HttpResponse httpResponse = new HttpResponse(receiveStream);

        switch (httpRequest.Method)
        {
            case "GET":
                Get(httpRequest, httpResponse);
                break;
            case "POST":
                Post(httpRequest, httpResponse);
                break;
        }
    }


    public virtual void Get(HttpRequest httpRequest, HttpResponse httpResponse)
    {

    }

    public virtual void Post(HttpRequest httpRequest, HttpResponse httpResponse)
    {

    }
}

