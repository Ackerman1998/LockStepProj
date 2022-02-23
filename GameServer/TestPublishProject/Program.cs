using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestProjs2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine(System.Environment.CurrentDirectory);
                string path = Path.Combine(System.Environment.CurrentDirectory,"test.txt");
                Console.WriteLine("Test RUn... write content = "+ path);
                Console.WriteLine("Test RUn... write content = "+ path);
                //File.WriteAllBytes(path,Encoding.UTF8.GetBytes("123456"));

                HttpLoginServer httpLoginServer = new HttpLoginServer();
                httpLoginServer.StartUp();
            }
        }
    }
}
