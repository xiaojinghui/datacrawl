// Decompiled with JetBrains decompiler
// Type: DataAccess.HelperBase
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;
using System.Data;
using System.Data.Common;

namespace DataAccess
{
  public abstract class HelperBase : IDisposable
  {
    internal bool isOpen;
    private DbConnection conn;
    private DbCommand cmd;
    private string connection_str;

    public virtual DbConnection Connection
    {
      get
      {
        return this.conn;
      }
      set
      {
        this.conn = value;
      }
    }

    public DbCommand Command
    {
      get
      {
        return this.cmd;
      }
      set
      {
        this.cmd = value;
      }
    }

    public string ConnectionString
    {
      get
      {
        return this.connection_str;
      }
      set
      {
        this.connection_str = value;
      }
    }

    public int ExecuteStoredProcedure(string StoredProcedureName)
    {
      this.cmd.CommandType = CommandType.StoredProcedure;
      this.cmd.CommandText = StoredProcedureName;
      return this.cmd.ExecuteNonQuery();
    }

    public int ExecuteNoneQuery()
    {
      return this.cmd.ExecuteNonQuery();
    }

    public string ExecuteScalarString()
    {
      return this.cmd.ExecuteScalar().ToString();
    }

    public int ExecuteScalarInt()
    {
      return Convert.ToInt32(this.cmd.ExecuteScalar());
    }

    public static DataTable ReadTable(DbCommand cmd)
    {
      DataTable dataTable = new DataTable();
      DbDataReader dbDataReader = (DbDataReader) null;
      try
      {
        dbDataReader = cmd.ExecuteReader();
        int fieldCount = dbDataReader.FieldCount;
        for (int ordinal = 0; ordinal < fieldCount; ++ordinal)
        {
          DataColumn column = new DataColumn(dbDataReader.GetName(ordinal), dbDataReader.GetFieldType(ordinal));
          dataTable.Columns.Add(column);
        }
        while (dbDataReader.Read())
        {
          DataRow row = dataTable.NewRow();
          for (int index = 0; index < fieldCount; ++index)
            row[index] = dbDataReader[index];
          dataTable.Rows.Add(row);
        }
        return dataTable;
      }
      finally
      {
        if (dbDataReader != null)
          dbDataReader.Close();
      }
    }

    public DataTable ReadTable()
    {
      return HelperBase.ReadTable(this.cmd);
    }

    public virtual void Open()
    {
      this.conn.ConnectionString = this.ConnectionString;
      this.conn.Open();
      this.isOpen = true;
    }

    public virtual void Close()
    {
      if (!this.isOpen || this.conn == null)
        return;
      this.conn.Close();
    }

    public void Dispose()
    {
      this.Close();
    }
  }
}
