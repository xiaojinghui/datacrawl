// Decompiled with JetBrains decompiler
// Type: DataAccess.DataHandler
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
  public class DataHandler
  {
    public static void SaveGameInfo(GameInfo game)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GameInfo (id, league, game_time, host, host_rank, guest, guest_rank, score_half, score_final, odds_url, data_ready) \r\nSELECT @id, @league, @game_time, @host, @host_rank, @guest, @guest_rank, @score_half, @score_final, @odds_url, 0 \r\nWHERE NOT EXISTS (SELECT 1 FROM GameInfo WHERE id=@id);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[10];
      sqlParameterArray[0] = new SqlParameter("@id", (object) game.GameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@league", (object) game.League);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@game_time", (object) game.GameTime);
      sqlParameterArray[2].DbType = DbType.DateTime;
      sqlParameterArray[3] = new SqlParameter("@host", (object) game.Host);
      sqlParameterArray[3].DbType = DbType.AnsiString;
      sqlParameterArray[4] = new SqlParameter("@host_rank", string.IsNullOrEmpty(game.HostRank) ? (object) "" : (object) game.HostRank);
      sqlParameterArray[4].DbType = DbType.AnsiString;
      sqlParameterArray[5] = new SqlParameter("@guest", (object) game.Guest);
      sqlParameterArray[5].DbType = DbType.AnsiString;
      sqlParameterArray[6] = new SqlParameter("@guest_rank", string.IsNullOrEmpty(game.GuestRank) ? (object) "" : (object) game.GuestRank);
      sqlParameterArray[6].DbType = DbType.AnsiString;
      sqlParameterArray[7] = new SqlParameter("@score_half", string.IsNullOrEmpty(game.HalfScore) ? (object) "" : (object) game.HalfScore);
      sqlParameterArray[7].DbType = DbType.AnsiString;
      sqlParameterArray[8] = new SqlParameter("@score_final", string.IsNullOrEmpty(game.FinalScore) ? (object) "" : (object) game.FinalScore);
      sqlParameterArray[8].DbType = DbType.AnsiString;
      sqlParameterArray[9] = new SqlParameter("@odds_url", (object) game.OddsUrl);
      sqlParameterArray[9].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveOddsInfo(List<OddsInfo> oddsInfo)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (OddsInfo oddsInfo1 in oddsInfo)
        stringBuilder.AppendFormat("INSERT INTO OddsInfo (odds_id, game_Id, company_id, win, tie, lose, update_time) SELECT {0}, {1}, {2}, {3}, {4}, {5}, '{6}' \r\nWHERE NOT EXISTS (SELECT 1 FROM OddsInfo WHERE odds_id={0});", (object) oddsInfo1.OddsId, (object) oddsInfo1.GameId, (object) oddsInfo1.CompanyId, (object) oddsInfo1.Win, (object) oddsInfo1.Tie, (object) oddsInfo1.Lose, (object) oddsInfo1.UpdateTime.ToString("yyyy-MM-dd HH:mm"));
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), new SqlParameter[0]);
    }

    public static void SaveCompanyInfo(Company company)
    {
      string cmdText = "INSERT INTO CompanyInfo (company_id, name, country, is_leading) \r\nSELECT @company_id, @name, @country, @is_leading WHERE NOT EXISTS (SELECT 1 FROM CompanyInfo WHERE company_id=@company_id);";
      SqlParameter[] sqlParameterArray = new SqlParameter[4];
      sqlParameterArray[0] = new SqlParameter("@company_id", (object) company.Id);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@name", (object) company.Name);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@country", (object) company.Country);
      sqlParameterArray[2].DbType = DbType.AnsiString;
      sqlParameterArray[3] = new SqlParameter("@is_leading", (object) company.IsLeading);
      sqlParameterArray[3].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateGameDataReady(int gameId)
    {
      string cmdText = "UPDATE GameInfo SET data_ready=1 WHERE id=@id;";
      SqlParameter sqlParameter = new SqlParameter("@id", (object) gameId);
      sqlParameter.DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameter);
    }

    public static Dictionary<int, Company> GetAllCompanyInfos()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, "select * from CompanyInfo", new SqlParameter[0]))
      {
        Dictionary<int, Company> dictionary = new Dictionary<int, Company>();
        while (dataReader.Read())
        {
          Company company = new Company();
          company.Id = dataReader.GetInt32(0);
          company.Name = dataReader.GetString(1);
          company.Country = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
          company.IsLeading = dataReader.GetInt32(3);
          dictionary[company.Id] = company;
        }
        return dictionary;
      }
    }

    public List<GameInfo> GetGameInfosByLeagues(IEnumerable<string> leagues)
    {
      List<GameInfo> list = new List<GameInfo>();
      using (IDataReader reader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo where league IN N('{0}') and data_ready=0;", (object) string.Join("','", leagues)), new SqlParameter[0]))
      {
        while (reader.Read())
        {
          GameInfo gameInfo = DataHandler.RetrieveGameInfo(reader);
          list.Add(gameInfo);
        }
        return list;
      }
    }

    public static List<string> GetFilteredLeagues()
    {
      List<string> list = new List<string>();
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, "select * from AnalyticalLeagues", new SqlParameter[0]))
      {
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
      }
      return list;
    }

    public static List<string> GetAllLeagues(DateTime date)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select distinct league_name from GameInfo where CAST(game_time as date)='{0}' order by league_name", (object) date.ToString("yyyy-MM-dd")), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
        return list;
      }
    }

    public static List<GameInfo> GetGameInfosByDate(DateTime gameTime, bool dataReady)
    {
      return DataHandler.GetGameInfosByDate(gameTime.ToString("yyyy-MM-dd"), dataReady);
    }

    public static List<GameInfo> GetGameInfosByDate(string gameTime, bool dataReady)
    {
      string cmdText = string.Format("SELECT * FROM GameInfo WHERE CAST(game_time as date)='{0}';", (object) gameTime, (object) (dataReady ? 1 : 0));
      List<GameInfo> list = new List<GameInfo>();
      using (IDataReader reader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, cmdText, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          GameInfo gameInfo = DataHandler.RetrieveGameInfo(reader);
          list.Add(gameInfo);
        }
        return list;
      }
    }

    public static List<GameInfo> GetGameInfosByDateAndLeague(DateTime gameTime, string League)
    {
      string cmdText = string.Format("SELECT * FROM GameInfo WHERE CAST(game_time as date)='{0}' AND league_name='{1}';", (object) gameTime.ToString("yyyy-MM-dd"), (object) League);
      List<GameInfo> list = new List<GameInfo>();
      using (IDataReader reader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, cmdText, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          GameInfo gameInfo = DataHandler.RetrieveGameInfo(reader);
          list.Add(gameInfo);
        }
        return list;
      }
    }

    public static void GetGameTimeRange(ref DateTime maxDate, ref DateTime minDate)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("SELECT max(game_time), min(game_time) FROM GameInfo;"), new SqlParameter[0]))
      {
        while (dataReader.Read())
        {
          maxDate = dataReader.GetDateTime(0);
          minDate = dataReader.GetDateTime(1);
        }
      }
    }

    public static void GetOddsTimeRange(ref DateTime maxDate, ref DateTime minDate)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("SELECT max(game_time), min(game_time) FROM GameInfo where data_ready=1;"), new SqlParameter[0]))
      {
        while (dataReader.Read())
        {
          maxDate = dataReader.GetDateTime(0);
          minDate = dataReader.GetDateTime(1);
        }
      }
    }

    public static Stack<OddsInfo> GetAllOddsInfoByGame(int gameId)
    {
      return DataHandler.GetOddsInfoByGame(string.Format("SELECT * FROM OddsInfo WHERE game_id='{0}' order by update_time desc;", (object) gameId));
    }

    public static Stack<OddsInfo> GetMainOddsInfoByGame(int gameId)
    {
      return DataHandler.GetOddsInfoByGame(string.Format("SELECT * FROM OddsInfo WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM CompanyInfo WHERE is_leading = 1) order by update_time desc;", (object) gameId));
    }

    public static Stack<OddsInfo> GetNotExchangeOddsInfoByGame(int gameId)
    {
      return DataHandler.GetOddsInfoByGame(string.Format("SELECT * FROM OddsInfo WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM CompanyInfo WHERE is_leading != 2) order by update_time desc;", (object) gameId));
    }

    private static Stack<OddsInfo> GetOddsInfoByGame(string sql)
    {
      Stack<OddsInfo> stack = new Stack<OddsInfo>();
      using (IDataReader reader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          OddsInfo oddsInfo = DataHandler.RetrieveOddsInfo(reader);
          stack.Push(oddsInfo);
        }
        return stack;
      }
    }

    private static GameInfo RetrieveGameInfo(IDataReader reader)
    {
      return new GameInfo()
      {
        GameId = reader.GetInt32(0),
        League = reader.GetString(1).Trim(),
        GameTime = reader.GetDateTime(2),
        Host = reader.GetString(3).Trim(),
        HostRank = reader.IsDBNull(4) ? string.Empty : reader.GetString(4).Trim(),
        Guest = reader.GetString(5).Trim(),
        GuestRank = reader.IsDBNull(6) ? string.Empty : reader.GetString(6).Trim(),
        HalfScore = reader.IsDBNull(7) ? string.Empty : reader.GetString(7).Trim(),
        FinalScore = reader.IsDBNull(8) ? string.Empty : reader.GetString(8).Trim(),
        OddsUrl = reader.GetString(9).Trim(),
        DataReady = reader.GetInt32(10)
      };
    }

    private static OddsInfo RetrieveOddsInfo(IDataReader reader)
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
