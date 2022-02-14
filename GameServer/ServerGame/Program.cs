using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static UdpClient _udpClient = null;
        static void Main(string[] args)
        {
            //TcpManager.Instance.Start(NetUtils.GetLocalAddress(), null);
            //HttpLoginServer httpLoginServer = new HttpLoginServer();
            //httpLoginServer.StartUp();
            TestSql();
            Console.ReadKey();
        }

        private static void TestSql() {
            //SqlManager.Instance.CreateSqlDataBase("localhost","TestDatabase","root", "123456");
            MySql.Data.MySqlClient.MySqlConnection mySqlConnection = SqlManager.Instance.GetSqlDatabaseConnection("localhost", 3306, "TestDatabase", "root", "123456");
            //SqlManager.Instance.CreateTable(mySqlConnection,"TestTable","(Name VarChar(10),Age Integer)");
            //SqlManager.Instance.InsertTable(mySqlConnection, "TestTable", "(Name,Age)", "('Ackerman','24')");
            //List<string> result = SqlManager.Instance.SelectAllDataTable(mySqlConnection, "TestTable");
            //foreach (string msg in result) {
            //    Debug.Log(msg);
            //}

            //List<string> result = SqlManager.Instance.SelectDataTableByCondition(mySqlConnection, "TestTable","Name Age","Name = 'Ackerman'");
            //foreach (string msg in result)
            //{
            //    Debug.Log(msg);
            //}

            //SqlManager.Instance.DeleteTableDataByCondition(mySqlConnection, "TestTable", "TestTable.name = 'Ackerman'");
            //SqlManager.Instance.UpdateTable(mySqlConnection,"TestTable", "Age=Age+1");
            //SqlManager.Instance.UpdateTableByCondition(mySqlConnection,"TestTable", "Age=Age+10","Name = 'Jack'");
            //SqlManager.Instance.AlterTableADD(mySqlConnection, "TestTable", "uid Integer");
            //SqlManager.Instance.AlterTableDrop(mySqlConnection, "TestTable", "uid");
        }
        public static void Create() {
          

            _udpClient = new UdpClient(NetConfig.port_udp);
        }
    }
}
