// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.SqlHelper
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OddsDataLayer
{
  public class SqlHelper
  {
    private string default_connection_str = ConfigurationManager.ConnectionStrings["SqlServerHelper"].ConnectionString;

    public SqlHelper()
    {
    }

    public SqlHelper(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
        return;
      this.default_connection_str = connectionString;
    }

    public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        SqlHelper.PrepareCommand(cmd, conn, (SqlTransaction) null, cmdType, cmdText, commandParameters);
        int num = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return num;
      }
    }

    public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      return this.ExecuteNonQuery(this.default_connection_str, cmdType, cmdText, commandParameters);
    }

    public int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
    {
      return this.ExecuteNonQuery(this.default_connection_str, CommandType.Text, cmdText, commandParameters);
    }

    public SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      SqlConnection conn = new SqlConnection(connectionString);
      try
      {
        SqlHelper.PrepareCommand(cmd, conn, (SqlTransaction) null, cmdType, cmdText, commandParameters);
        SqlDataReader sqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        cmd.Parameters.Clear();
        return sqlDataReader;
      }
      catch
      {
        conn.Close();
        throw;
      }
    }

    public SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
    {
      return this.ExecuteReader(this.default_connection_str, CommandType.Text, cmdText, commandParameters);
    }

    public SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      return this.ExecuteReader(this.default_connection_str, cmdType, cmdText, commandParameters);
    }

    public DataTable ExecuteDataTable(string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand sqlCommand = new SqlCommand();
      SqlConnection conn = new SqlConnection(this.default_connection_str);
      try
      {
        DataTable dataTable = new DataTable();
        SqlHelper.PrepareCommand(sqlCommand, conn, (SqlTransaction) null, CommandType.Text, cmdText, commandParameters);
        new SqlDataAdapter(sqlCommand).Fill(dataTable);
        sqlCommand.Parameters.Clear();
        return dataTable;
      }
      catch
      {
        conn.Close();
        throw;
      }
      finally
      {
        conn.Close();
      }
    }

    public object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      return this.ExecuteScalar(this.default_connection_str, cmdType, cmdText, commandParameters);
    }

    public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        SqlHelper.PrepareCommand(cmd, conn, (SqlTransaction) null, cmdType, cmdText, commandParameters);
        object obj = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return obj;
      }
    }

    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
    {
      if (conn.State != ConnectionState.Open)
        conn.Open();
      cmd.Connection = conn;
      cmd.CommandText = cmdText;
      if (trans != null)
        cmd.Transaction = trans;
      cmd.CommandType = cmdType;
      if (cmdParms == null)
        return;
      foreach (SqlParameter sqlParameter in cmdParms)
        cmd.Parameters.Add(sqlParameter);
    }
  }
}
