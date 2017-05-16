// Decompiled with JetBrains decompiler
// Type: DataAccess.SqlServerHelper
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DataAccess
{
  public class SqlServerHelper
  {
    public static readonly string default_connection_str = ConfigurationManager.ConnectionStrings["SqlServerHelper"].ConnectionString;
    private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

    public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        SqlServerHelper.PrepareCommand(cmd, conn, (SqlTransaction) null, cmdType, cmdText, commandParameters);
        int num = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return num;
      }
    }

    public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteNonQuery(SqlServerHelper.default_connection_str, cmdType, cmdText, commandParameters);
    }

    public static int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteNonQuery(SqlServerHelper.default_connection_str, CommandType.Text, cmdText, commandParameters);
    }

    public static int ExecuteNonQueryProc(string StoredProcedureName, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteNonQuery(SqlServerHelper.default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters);
    }

    public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      SqlServerHelper.PrepareCommand(cmd, connection, (SqlTransaction) null, cmdType, cmdText, commandParameters);
      int num = cmd.ExecuteNonQuery();
      cmd.Parameters.Clear();
      return num;
    }

    public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      SqlServerHelper.PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
      int num = cmd.ExecuteNonQuery();
      cmd.Parameters.Clear();
      return num;
    }

    public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      SqlConnection conn = new SqlConnection(connectionString);
      try
      {
        SqlServerHelper.PrepareCommand(cmd, conn, (SqlTransaction) null, cmdType, cmdText, commandParameters);
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

    public static SqlDataReader ExecuteReader(SqlConnection conn, string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteReader(SqlServerHelper.default_connection_str, CommandType.Text, cmdText, commandParameters);
    }

    public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteReader(SqlServerHelper.default_connection_str, CommandType.Text, cmdText, commandParameters);
    }

    public static SqlDataReader ExecuteReaderProc(string StoredProcedureName, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteReader(SqlServerHelper.default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters);
    }

    public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteReader(SqlServerHelper.default_connection_str, cmdType, cmdText, commandParameters);
    }

    public static DataTable ExecuteDataTable(string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand sqlCommand = new SqlCommand();
      SqlConnection conn = new SqlConnection(SqlServerHelper.default_connection_str);
      try
      {
        DataTable dataTable = new DataTable();
        SqlServerHelper.PrepareCommand(sqlCommand, conn, (SqlTransaction) null, CommandType.Text, cmdText, commandParameters);
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

    public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      using (SqlConnection conn = new SqlConnection(connectionString))
      {
        SqlServerHelper.PrepareCommand(cmd, conn, (SqlTransaction) null, cmdType, cmdText, commandParameters);
        object obj = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return obj;
      }
    }

    public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteScalar(SqlServerHelper.default_connection_str, CommandType.Text, cmdText, commandParameters);
    }

    public static object ExecuteScalarProc(string StoredProcedureName, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteScalar(SqlServerHelper.default_connection_str, CommandType.StoredProcedure, StoredProcedureName, commandParameters);
    }

    public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ExecuteScalar(SqlServerHelper.default_connection_str, cmdType, cmdText, commandParameters);
    }

    public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      SqlServerHelper.PrepareCommand(cmd, connection, (SqlTransaction) null, cmdType, cmdText, commandParameters);
      object obj = cmd.ExecuteScalar();
      cmd.Parameters.Clear();
      return obj;
    }

    public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
    {
      SqlServerHelper.parmCache[(object) cacheKey] = (object) commandParameters;
    }

    public static SqlParameter[] GetCachedParameters(string cacheKey)
    {
      SqlParameter[] sqlParameterArray1 = (SqlParameter[]) SqlServerHelper.parmCache[(object) cacheKey];
      if (sqlParameterArray1 == null)
        return (SqlParameter[]) null;
      SqlParameter[] sqlParameterArray2 = new SqlParameter[sqlParameterArray1.Length];
      int index = 0;
      for (int length = sqlParameterArray1.Length; index < length; ++index)
        sqlParameterArray2[index] = (SqlParameter) ((ICloneable) sqlParameterArray1[index]).Clone();
      return sqlParameterArray2;
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

    public static DataTable ReadTable(SqlTransaction transaction, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      SqlServerHelper.PrepareCommand(cmd, transaction.Connection, transaction, cmdType, cmdText, commandParameters);
      DataTable dataTable = HelperBase.ReadTable((DbCommand) cmd);
      cmd.Parameters.Clear();
      return dataTable;
    }

    public static SqlConnection GetConnection()
    {
      return new SqlConnection(SqlServerHelper.default_connection_str);
    }

    public static DataTable ReadTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return SqlServerHelper.ReadTable(connection, cmdType, cmdText, commandParameters);
      }
    }

    public static DataTable ReadTable(string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ReadTable(CommandType.Text, cmdText, commandParameters);
    }

    public static DataTable ReadTable(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      return SqlServerHelper.ReadTable(SqlServerHelper.default_connection_str, cmdType, cmdText, commandParameters);
    }

    public static DataTable ReadTable(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
    {
      SqlCommand cmd = new SqlCommand();
      SqlServerHelper.PrepareCommand(cmd, connection, (SqlTransaction) null, cmdType, cmdText, commandParameters);
      DataTable dataTable = HelperBase.ReadTable((DbCommand) cmd);
      cmd.Parameters.Clear();
      return dataTable;
    }

    public static SqlParameter CreateInputParameter(string paramName, SqlDbType dbtype, object value)
    {
      return SqlServerHelper.CreateParameter(ParameterDirection.Input, paramName, dbtype, 0, value);
    }

    public static SqlParameter CreateInputParameter(string paramName, SqlDbType dbtype, int size, object value)
    {
      return SqlServerHelper.CreateParameter(ParameterDirection.Input, paramName, dbtype, size, value);
    }

    public static SqlParameter CreateOutputParameter(string paramName, SqlDbType dbtype)
    {
      return SqlServerHelper.CreateParameter(ParameterDirection.Output, paramName, dbtype, 0, (object) DBNull.Value);
    }

    public static SqlParameter CreateOutputParameter(string paramName, SqlDbType dbtype, int size)
    {
      return SqlServerHelper.CreateParameter(ParameterDirection.Output, paramName, dbtype, size, (object) DBNull.Value);
    }

    public static SqlParameter CreateParameter(ParameterDirection direction, string paramName, SqlDbType dbtype, int size, object value)
    {
      SqlParameter sqlParameter = new SqlParameter(paramName, dbtype, size);
      sqlParameter.Value = value;
      sqlParameter.Direction = direction;
      return sqlParameter;
    }
  }
}
