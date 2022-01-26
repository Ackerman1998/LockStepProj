using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


class UdpManager:Singleton<UdpManager>
{
    private Dictionary<int, UdpClient> udpContainer = new Dictionary<int, UdpClient>();
    public UdpClient GetUdpClient(int port) {
        if (udpContainer.ContainsKey(port))
        {
            return udpContainer[port];
        }
        else {
            UdpClient udpClient = new UdpClient(port);
            udpContainer.Add(port,udpClient);
            return udpClient;
        }
    }
}

