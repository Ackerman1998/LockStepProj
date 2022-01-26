using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


class NetUtils
{
    public static string GetLocalAddress() {
#if UNITY_ANDROID
        return "192.168.4.52";
#endif
        IPAddress[] iPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in iPAddresses) {
            if (ip.AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }
        return null;
    }
}

