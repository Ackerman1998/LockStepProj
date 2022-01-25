using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


class NetUtils
{
    public static string GetLocalAddress() {
        IPAddress[] iPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in iPAddresses) {
            if (ip.AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }
        return null;
    }
}

