using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class SqlManager : Singleton<SqlManager>
{
    /// <summary>
    /// Create Database for name
    /// </summary>
    /// <param name="dataSource"></param>
    /// <param name="databaseName"></param>
    /// <param name="userId"></param>
    /// <param name="password"></param>
    public void CreateSqlDataBase(string dataSource, string databaseName, string userId, string password) {
        string constr = string.Format("Data Source={0};Persist Security Info=yes;UserId={1};Password={2};", dataSource, userId, password);
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
            Debug.Log("Create Database Exception : " + e);
            mySqlConnection.Close();
        }
        finally {

        }
    }
    /// <summary>
    /// Get SQLConnect obj
    /// </summary>
    public MySqlConnection GetSqlConnection(string dataSource, string userId, string password) {
        string constr = string.Format("Data Source={0};Persist Security Info=yes;UserId={1};Password={2};", dataSource, userId, password);
        MySqlConnection mySqlConnection = new MySqlConnection(constr);
        return mySqlConnection;
    }

    public MySqlConnection GetSqlDatabaseConnection(string dataSource, int port, string databaseName, string userId, string password) {
        string constr = string.Format("server={0};port={1};database={2};user={3};password={4};", dataSource, port, databaseName, userId, password);
        MySqlConnection mySqlConnection = new MySqlConnection(constr);
        return mySqlConnection;
    }

    public void CreateTable(string dataSource, int port, string databaseName, string userId, string password, string tableName, string valueContent) {
        MySqlConnection mySqlConnection = GetSqlDatabaseConnection(dataSource, port, databaseName, userId, password);
        if (mySqlConnection != null)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(string.Format("CREATE TABLE {0} {1}", tableName, valueContent), mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
        }
        else {
            mySqlConnection.Close();
            Debug.Log("Create Table Error : " + tableName);
        }
    }
    /// <summary>
    /// Create Data Table
    /// CREATE TABLE TestTable (Name VarChar(10),Age Integer)
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="valueContent"></param>
    public void CreateTable(MySqlConnection mySqlConnection, string tableName, string valueContent) {
        if (mySqlConnection != null)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("CREATE TABLE {0} {1}", tableName, valueContent), mySqlConnection);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                Debug.Log("Create Table Success : " + tableName);
            }
            catch (MySqlException e)
            {
                Debug.Log("Create Database Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Create Table Error : " + tableName);
        }
    }
    /// <summary>
    /// Insert data to table--
    /// INSERT INTO {tableName} {key} VALUES {value}
    /// INSERT INTO {tableName} (Name,Age) VALUES ('Ackerman','24')
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void InsertTable(MySqlConnection mySqlConnection, string tableName, string key, string value)
    {
        if (mySqlConnection != null)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("INSERT INTO {0} {1} VALUES {2}", tableName, key, value), mySqlConnection);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                Debug.Log("Insert Table Success : " + tableName);
            }
            catch (MySqlException e)
            {
                Debug.Log("Insert Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Insert Table Error : " + tableName);
        }
    }
    /// <summary>
    /// Select All Data 
    /// SELECT * FROM {tableName}
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public List<string> SelectAllDataTable(MySqlConnection mySqlConnection, string tableName)
    {
        List<string> result = new List<string>();
        if (mySqlConnection != null)
        {
            try
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("SELECT * FROM {0}", tableName), mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read()) {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < mySqlDataReader.FieldCount; i++) {
                        sb.Append(mySqlDataReader[i].ToString());
                        if (i != mySqlDataReader.FieldCount - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    result.Add(sb.ToString());
                }
            }
            catch (MySqlException e)
            {
                Debug.Log("Select All Data Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Select Table Error : " + tableName);
        }
        return result;
    }
    /// <summary>
    /// Select DataTable FOR WHERE Condition--
    /// SELECT {returnValue} FROM {tableName} WHERE {condition}
    /// SELECT Name,Age FROM tableName WHERE Name='Ackerman'
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="returnValue"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public List<string> SelectDataTableByCondition(MySqlConnection mySqlConnection, string tableName,string returnValue,string condition) {
        List<string> result = new List<string>();
        if (mySqlConnection != null)
        {
            try
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("SELECT {0} FROM {1} WHERE {2}", returnValue, tableName, condition), mySqlConnection);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < mySqlDataReader.FieldCount; i++)
                    {
                        sb.Append(mySqlDataReader[i].ToString());
                        if (i != mySqlDataReader.FieldCount - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    result.Add(sb.ToString());
                }
            }
            catch (MySqlException e)
            {
                Debug.Log("Select All Data Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Select Table Error : " + tableName);
        }
        return result;
    }

    /// <summary>
    /// Delete SQL Table Data for condition
    /// DELETE FROM TestTable WHERE TestTable.name = 'Ackerman'
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="condition"></param>
    public void DeleteTableDataByCondition(MySqlConnection mySqlConnection, string tableName,string condition)
    {
        if (mySqlConnection != null)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("DELETE FROM {0} WHERE {1}", tableName, condition), mySqlConnection);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                Debug.Log("Delete Table Data OK : " + tableName);
            }
            catch (MySqlException e)
            {
                Debug.Log("Delete Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Delete Table Error : " + tableName);
        }
    }

    /// <summary>
    /// Update All Data in dataTable
    /// UPDATE TestTable SET Age=Age+1
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="condition"></param>
    public void UpdateTable(MySqlConnection mySqlConnection,string tableName,string sqlContent)
    {
        if (mySqlConnection != null)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("UPDATE {0} SET {1}", tableName, sqlContent), mySqlConnection);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                Debug.Log("Update Table Data OK : " + tableName);
            }
            catch (MySqlException e)
            {
                Debug.Log("Update Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Update Table Error : " + tableName);
        }
    }
    /// <summary>
    /// Update Table Data by WHERE condition
    /// UPDATE TestTable SET Age=Age+1 WHERE Name='Jack'
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="sqlContent"></param>
    /// <param name="condition"></param>
    public void UpdateTableByCondition(MySqlConnection mySqlConnection, string tableName, string sqlContent,string condition) {
        if (mySqlConnection != null)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, sqlContent, condition), mySqlConnection);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                Debug.Log("Update Table Data OK : " + tableName);
            }
            catch (MySqlException e)
            {
                Debug.Log("Update Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Update Table Error : " + tableName);
        }
    }
    /// <summary>
    /// Alter ADD Param to table
    /// ALTER TABLE TestTable ADD uid Integer
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="paramName"></param>
    public void AlterTableADD(MySqlConnection mySqlConnection, string tableName,string paramName)
    {
        if (mySqlConnection != null)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("ALTER TABLE {0} ADD {1}", tableName, paramName), mySqlConnection);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                Debug.Log("Alter Table [ADD] OK : " + tableName);
            }
            catch (MySqlException e)
            {
                Debug.Log("Alter Table [ADD] Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Alter Table [ADD] Error : " + tableName);
        }
    }
    /// <summary>
    /// Alter Drop Param for table
    /// ALTER TABLE TestTable DROP uid
    /// </summary>
    /// <param name="mySqlConnection"></param>
    /// <param name="tableName"></param>
    /// <param name="paramName"></param>
    public void AlterTableDrop(MySqlConnection mySqlConnection, string tableName, string paramName) {
        if (mySqlConnection != null)
        {
            try
            {
                MySqlCommand mySqlCommand = new MySqlCommand(string.Format("ALTER TABLE {0} DROP {1}", tableName, paramName), mySqlConnection);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                Debug.Log("Alter Table [DROP] OK : " + tableName);
            }
            catch (MySqlException e)
            {
                Debug.Log("Alter Table [DROP] Exception : " + e);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        else
        {
            mySqlConnection.Close();
            Debug.Log("Alter Table [DROP] Error : " + tableName);
        }
    }
}
/*
SQL Base Data Type
1.String Type
VARCHAR ,CHAR
2.BigData Type
BLOB:Cache Byte file(audiosource,picture)
3.Number
INT,FLOAT,DOUBLE
4.BOOL
BIT 0 1
5.DATE TIME
DATE,TIME,DATETIME,TIMESTAMP
 */
