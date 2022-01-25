using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpManager.Instance.Start(NetUtils.GetLocalAddress(), null);
            Console.ReadKey();
        }

    }
}
