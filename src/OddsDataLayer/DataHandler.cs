// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.DataHandler
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OddsDataLayer
{
  public class DataHandler
  {
    private SqlHelper _helper = new SqlHelper();
    private string _currentConnectionString = ConfigurationManager.ConnectionStrings["CurrentConnectionString"].ConnectionString;

    public DataHandler(bool isDaily)
    {
      if (!isDaily)
        return;
      this._helper = new SqlHelper(this._currentConnectionString);
    }

    public List<string> GetAllPeriods()
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select distinct periods from GameInfo_Daily order by periods desc"), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
        return list;
      }
    }

    public Dictionary<int, Company> GetAllCompanyInfos()
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, "select * from Data2014.dbo.CompanyInfo", new SqlParameter[0]))
      {
        Dictionary<int, Company> dictionary = new Dictionary<int, Company>();
        while (dataReader.Read())
        {
          Company company = new Company();
          company.Id = dataReader.GetInt32(0);
          company.Name = dataReader.GetString(1);
          company.Country = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
          company.IsLeading = dataReader.GetInt32(4);
          dictionary[company.Id] = company;
        }
        return dictionary;
      }
    }

    public List<string> GetAllLeagues(DateTime date, string tableName)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select distinct league_name from {1} where CAST(game_time as date)='{0}' order by league_name", (object) date.ToString("yyyy-MM-dd"), (object) tableName), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
        return list;
      }
    }

    public List<string> GetAllLeaguesByPeriod(string period)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select distinct league_name from GameInfo_Daily where periods='{0}' order by league_name", (object) period), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
        return list;
      }
    }

    public GameInfo GetGameInfoById(int gameId)
    {
      string cmdText = string.Format("SELECT * FROM GameInfo_Daily WHERE id='{0}';", (object) gameId);
      GameInfo gameInfo = (GameInfo) null;
      using (IDataReader reader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, cmdText, new SqlParameter[0]))
      {
        while (reader.Read())
          gameInfo = this.RetrieveGameInfo(reader);
      }
      return gameInfo;
    }

    public List<GameInfo> GetGameInfosByPeriod(string period)
    {
      return this.GetGameInfosBySQL(string.Format("SELECT * FROM GameInfo_Daily WHERE periods='{0}' order by serial;", (object) period));
    }

    public List<GameInfo> GetGameInfosByPeriodAndLeague(string period, string League)
    {
      return this.GetGameInfosBySQL(string.Format("SELECT * FROM GameInfo_Daily WHERE periods='{0}' AND league_name='{1}' order by serial;", (object) period, (object) League));
    }

    public Stack<OddsInfo> GetAllOddsInfoByGame(int gameId, string tableName)
    {
      return this.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' order by update_time desc;", (object) gameId, (object) tableName));
    }

    public Stack<OddsInfo> GetMainOddsInfoByGame(int gameId, string tableName)
    {
      return this.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM Data2014.dbo.CompanyInfo WHERE is_leading = 1) order by update_time desc;", (object) gameId, (object) tableName));
    }

    public Stack<OddsInfo> GetNotExchangeOddsInfoByGame(int gameId, string tableName)
    {
      return this.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM Data2014.dbo.CompanyInfo WHERE is_leading != 2) order by update_time desc;", (object) gameId, (object) tableName));
    }

    public Stack<OddsInfo> GetCustomOddsInfoByGame(int gameId, string tableName, int idType)
    {
      return this.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM Data2014.dbo.CustomCompanyList WHERE id = {2}) order by update_time desc;", (object) gameId, (object) tableName, (object) idType));
    }

    public Stack<OddsInfo> GetOddsInfo(string sql)
    {
      Stack<OddsInfo> stack = new Stack<OddsInfo>();
      using (IDataReader reader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, sql, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          OddsInfo oddsInfo = this.RetrieveOddsInfo(reader);
          stack.Push(oddsInfo);
        }
        return stack;
      }
    }

    public string GetCurrentPeriod()
    {
      object obj = this._helper.ExecuteScalar(CommandType.Text, string.Format("select period from FootballSinaDaily.dbo.CurrentPeriod"));
      return obj == null || obj is DBNull ? "" : obj.ToString();
    }

    private List<GameInfo> GetGameInfosBySQL(string sql)
    {
      List<GameInfo> list = new List<GameInfo>();
      using (IDataReader reader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, sql, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          GameInfo gameInfo = this.RetrieveGameInfo(reader);
          list.Add(gameInfo);
        }
        return list;
      }
    }

    private GameInfo RetrieveGameInfo(IDataReader reader)
    {
      GameInfo gameInfo = new GameInfo();
      gameInfo.GameId = reader.GetInt32(0);
      gameInfo.Periods = reader.GetString(1);
      gameInfo.League = reader.GetString(3);
      gameInfo.Serial = reader.GetInt32(4);
      gameInfo.GameTime = reader.GetDateTime(5);
      gameInfo.Host = reader.GetString(6);
      gameInfo.HostRank = reader.IsDBNull(7) ? string.Empty : reader.GetString(7).Trim();
      gameInfo.Guest = reader.GetString(8);
      gameInfo.GuestRank = reader.IsDBNull(9) ? string.Empty : reader.GetString(9).Trim();
      gameInfo.HalfScore = reader.IsDBNull(10) ? string.Empty : reader.GetString(10).Trim();
      gameInfo.FinalScore = reader.IsDBNull(11) ? string.Empty : reader.GetString(11).Trim();
      if (reader.FieldCount > 12)
      {
        gameInfo.WinAvg = reader.GetDecimal(12);
        gameInfo.TieAvg = reader.GetDecimal(13);
        gameInfo.LoseAvg = reader.GetDecimal(14);
        gameInfo.AsiaTape = reader.GetDecimal(15);
        gameInfo.AsiaTapeZh = reader.GetString(16).Trim();
        gameInfo.AsiaTapeResult = reader.GetInt32(17);
        gameInfo.ScoreTape = reader.GetDecimal(18);
        gameInfo.ScoreTapeZh = reader.GetString(19).Trim();
        gameInfo.ScoreTapeResult = reader.GetInt32(20);
      }
      if (reader.FieldCount > 21)
      {
        gameInfo.DataReady = reader.GetInt32(23);
        gameInfo.CompanyList = reader.IsDBNull(24) ? reader.GetString(22).Trim() : reader.GetString(24).Trim();
      }
      return gameInfo;
    }

    private OddsInfo RetrieveOddsInfo(IDataReader reader)
    {
      return new OddsInfo()
      {
        OddsId = reader.GetInt32(0),
        GameId = reader.GetInt32(1),
        CompanyId = reader.GetInt32(2),
        Win = reader.GetDecimal(3),
        Tie = reader.GetDecimal(4),
        Lose = reader.GetDecimal(5),
        UpdateTime = reader.GetDateTime(6)
      };
    }
  }
}
