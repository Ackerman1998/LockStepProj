using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class SqlManager:Singleton<SqlManager>
{
    public void CreateSqlDataBase(string dataSource,string databaseName,string userId,string password) {
        string constr = string.Format("Data Source={0};Persist Security Info=yes;UserId={1};Password={2};",dataSource,userId,password);
        MySqlConnection mySqlConnection = new MySqlConnection(constr);
        try
        {
            MySqlCommand mySqlCommand = new MySqlCommand("CREATE DATABASE " + databaseName, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            Debug.Log("Create Database Success : " + databaseName);
        }
        catch (MySqlException e) {
            Debug.Log("Create Exception : "+e);
        }
        finally { 
        
        }
         
    }

    public void ConnectSqlDataBase() {
        MySqlConnection mySqlConnection = new MySqlConnection();
    }
}

