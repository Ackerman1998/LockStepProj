using System;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static UdpClient _udpClient = null;
        static void Main(string[] args)
        {
            //TcpManager.Instance.Start(NetUtils.GetLocalAddress(), null);
            HttpLoginServer httpLoginServer = new HttpLoginServer();
            httpLoginServer.StartUp();
            Console.ReadKey();
        }
        public static void Create() {
          

            _udpClient = new UdpClient(NetConfig.port_udp);
        }
    }
}
