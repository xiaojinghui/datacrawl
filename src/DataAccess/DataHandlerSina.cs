// Decompiled with JetBrains decompiler
// Type: DataAccess.DataHandlerSina
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
  public class DataHandlerSina
  {
    public static void SaveLeagueInfo(string id, string name, string country, string area)
    {
      string cmdText = "INSERT INTO LeagueInfo (id, name, country, area, odds_table) \r\nSELECT @id, @name, @country, @area, '' WHERE NOT EXISTS (SELECT 1 FROM LeagueInfo WHERE id=@id);";
      SqlParameter[] sqlParameterArray = new SqlParameter[4];
      sqlParameterArray[0] = new SqlParameter("@id", (object) id);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@name", (object) name);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@country", (object) country);
      sqlParameterArray[2].DbType = DbType.AnsiString;
      sqlParameterArray[3] = new SqlParameter("@area", (object) area);
      sqlParameterArray[3].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveWin007LeagueInfo(string id, string name)
    {
      string cmdText = "INSERT INTO Win007League (id, name) \r\nSELECT @id, @name WHERE NOT EXISTS (SELECT 1 FROM Win007League WHERE id=@id);";
      SqlParameter[] sqlParameterArray = new SqlParameter[2]
      {
        new SqlParameter("@id", (object) id),
        null
      };
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@name", (object) name);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveGameInfo(GameInfo game, int leagueId, string season)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GameInfo (id, league_id, league_name, periods, serial, game_time, host, host_rank, guest, guest_rank, score_half, score_final, \r\n    euro_win_avg, euro_tie_avg, euro_lose_avg, asia_tape, asia_tape_zh, asia_tape_result, score_tape, score_tape_zh, score_tape_result) \r\nSELECT @id, @league_id, @league_name, @periods, @serial, @game_time, @host, @host_rank, @guest, @guest_rank, @score_half, @score_final, \r\n    @euro_win_avg, @euro_tie_avg, @euro_lose_avg, @asia_tape, @asia_tape_zh, @asia_tape_result, @score_tape, @score_tape_zh, @score_tape_result\r\nWHERE NOT EXISTS (SELECT 1 FROM GameInfo WHERE id=@id);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[21];
      sqlParameterArray[0] = new SqlParameter("@id", (object) game.GameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@league_id", (object) leagueId);
      sqlParameterArray[1].DbType = DbType.Int32;
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
      sqlParameterArray[9] = new SqlParameter("@league_name", (object) game.League);
      sqlParameterArray[9].DbType = DbType.AnsiString;
      sqlParameterArray[10] = new SqlParameter("@euro_win_avg", (object) game.WinAvg);
      sqlParameterArray[10].DbType = DbType.Decimal;
      sqlParameterArray[11] = new SqlParameter("@euro_tie_avg", (object) game.TieAvg);
      sqlParameterArray[11].DbType = DbType.Decimal;
      sqlParameterArray[12] = new SqlParameter("@euro_lose_avg", (object) game.LoseAvg);
      sqlParameterArray[12].DbType = DbType.Decimal;
      sqlParameterArray[13] = new SqlParameter("@asia_tape", (object) game.AsiaTape);
      sqlParameterArray[13].DbType = DbType.Decimal;
      sqlParameterArray[14] = new SqlParameter("@asia_tape_zh", (object) game.AsiaTapeZh);
      sqlParameterArray[14].DbType = DbType.AnsiString;
      sqlParameterArray[15] = new SqlParameter("@asia_tape_result", (object) game.AsiaTapeResult);
      sqlParameterArray[15].DbType = DbType.Int32;
      sqlParameterArray[16] = new SqlParameter("@score_tape", (object) game.ScoreTape);
      sqlParameterArray[16].DbType = DbType.Decimal;
      sqlParameterArray[17] = new SqlParameter("@score_tape_zh", (object) game.ScoreTapeZh);
      sqlParameterArray[17].DbType = DbType.AnsiString;
      sqlParameterArray[18] = new SqlParameter("@score_tape_result", (object) game.ScoreTapeResult);
      sqlParameterArray[18].DbType = DbType.Int32;
      sqlParameterArray[19] = new SqlParameter("@serial", (object) game.Serial);
      sqlParameterArray[19].DbType = DbType.Int32;
      sqlParameterArray[20] = new SqlParameter("@periods", (object) season);
      sqlParameterArray[20].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveDailyGameInfo(GameInfo game, string leagueId)
    {
      DataHandlerSina.SaveDailyGameInfo(game, leagueId, "FootballSina");
    }

    public static void SaveDailyGameInfo(GameInfo game, string leagueId, string databaseName)
    {
      string cmdText = string.Format("SET NOCOUNT ON;\r\nINSERT INTO {0}.dbo.GameInfo_Daily (id, league_id, league_name, periods, serial, game_time, host, host_rank, guest, guest_rank, score_half, score_final) \r\nSELECT @id, @league_id, @league_name, @period, @serial, @game_time, @host, @host_rank, @guest, @guest_rank, @score_half, @score_final\r\nWHERE NOT EXISTS (SELECT 1 FROM {0}.dbo.GameInfo_Daily WHERE id=@id);\r\nSET NOCOUNT OFF;", (object) databaseName);
      SqlParameter[] sqlParameterArray = new SqlParameter[12];
      sqlParameterArray[0] = new SqlParameter("@id", (object) game.GameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@league_name", (object) game.League);
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
      sqlParameterArray[9] = new SqlParameter("@league_id", (object) leagueId);
      sqlParameterArray[9].DbType = DbType.Int32;
      sqlParameterArray[10] = new SqlParameter("@period", (object) game.Periods);
      sqlParameterArray[10].DbType = DbType.AnsiString;
      sqlParameterArray[11] = new SqlParameter("@serial", (object) game.Serial);
      sqlParameterArray[11].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveGameCompanyMap(int gameId, string companyList)
    {
      DataHandlerSina.SaveGameCompanyMap(gameId, companyList, "FootballSina");
    }

    public static void SaveGameCompanyMap(int gameId, string companyList, string databaseName)
    {
      string cmdText = string.Format("INSERT INTO {0}.dbo.GameCompanyMap (game_id, company_ids, data_ready) \r\nSELECT @game_id, @company_ids, 0 WHERE NOT EXISTS (SELECT 1 FROM {0}.dbo.GameCompanyMap WHERE game_id=@game_id);", (object) databaseName);
      SqlParameter[] sqlParameterArray = new SqlParameter[2]
      {
        new SqlParameter("@game_id", (object) gameId),
        null
      };
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@company_ids", (object) companyList);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveLeagueSeason(int leagueId, string season)
    {
      string cmdText = "INSERT INTO LeagueSeasons (league_id, season) SELECT @league_id, @season;";
      SqlParameter[] sqlParameterArray = new SqlParameter[2]
      {
        new SqlParameter("@league_id", (object) leagueId),
        null
      };
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@season", (object) season);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateFailureIds(int gameId, string ids)
    {
      string cmdText = "UPDATE GameCompanyMap SET failure_ids=@failure_ids, data_ready=2 WHERE game_id=@game_id;";
      SqlParameter[] sqlParameterArray = new SqlParameter[2]
      {
        new SqlParameter("@game_id", (object) gameId),
        null
      };
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@failure_ids", (object) ids);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateDataReady(int gameId, DataReady dataReady)
    {
      DataHandlerSina.UpdateDataReady(gameId, dataReady, "FootballSina");
    }

    public static void UpdateDataReady(int gameId, DataReady dataReady, string databaseName)
    {
      string cmdText = string.Format("UPDATE {0}.dbo.GameCompanyMap SET data_ready=@data_ready WHERE game_id=@game_id;", (object) databaseName);
      SqlParameter[] sqlParameterArray = new SqlParameter[2]
      {
        new SqlParameter("@game_id", (object) gameId),
        null
      };
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@data_ready", (object) dataReady);
      sqlParameterArray[1].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateDailyGameInfo(GameInfo game)
    {
      DataHandlerSina.UpdateDailyGameInfo(game, "FootballSina");
    }

    public static void UpdateDailyGameInfo(GameInfo game, string tableName)
    {
      string cmdText = string.Format("SET NOCOUNT ON;\r\nUPDATE {0}.dbo.GameInfo_Daily \r\nSET score_half=@score_half, score_final=@score_final WHERE id=@id;\r\nSET NOCOUNT OFF;", (object) tableName);
      SqlParameter[] sqlParameterArray = new SqlParameter[3];
      sqlParameterArray[0] = new SqlParameter("@id", (object) game.GameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@score_half", string.IsNullOrEmpty(game.HalfScore) ? (object) "" : (object) game.HalfScore);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@score_final", string.IsNullOrEmpty(game.FinalScore) ? (object) "" : (object) game.FinalScore);
      sqlParameterArray[2].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateCurrentPeriod(string period)
    {
      string cmdText = "SET NOCOUNT ON;\r\nUPDATE FootballSinaDaily.dbo.CurrentPeriod \r\nSET period=@period;\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[1]
      {
        new SqlParameter("@period", (object) period)
      };
      sqlParameterArray[0].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static Dictionary<int, List<string>> GetExistLeagueSeasons()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select * from LeagueSeasons order by league_id"), new SqlParameter[0]))
      {
        Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
        while (dataReader.Read())
        {
          int int32 = dataReader.GetInt32(0);
          List<string> list;
          if (!dictionary.TryGetValue(int32, out list))
          {
            list = new List<string>();
            dictionary.Add(int32, list);
          }
          list.Add(dataReader.GetString(1).Trim());
        }
        return dictionary;
      }
    }

    public static List<GameInfo> GetGameInfos(DataReady dataReady)
    {
      return DataHandlerSina.GetGameInfosBySQL(string.Format("SELECT * FROM GameInfo g left join GameCompanyMap c on g.id=c.game_id WHERE data_ready={0};", (object) dataReady));
    }

    public static List<GameInfo> GetDailyGameInfos(DataReady dataReady)
    {
      return DataHandlerSina.GetGameInfosBySQL2(string.Format("SELECT * FROM FootballSinaDaily.dbo.GameInfo_Daily g left join FootballSinaDaily.dbo.GameCompanyMap c on g.id=c.game_id WHERE data_ready={0};", (object) dataReady));
    }

    public static List<string> GetAllLeagues(DateTime date)
    {
      return DataHandlerSina.GetAllLeagues(date, "GameInfo");
    }

    public static List<string> GetAllLeagues(DateTime date, string tableName)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select distinct league_name from {1} where CAST(game_time as date)='{0}' order by league_name", (object) date.ToString("yyyy-MM-dd"), (object) tableName), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
        return list;
      }
    }

    public static List<string> GetAllLeaguesByPeriod(string period)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select distinct league_name from GameInfo_Daily where periods='{0}' order by league_name", (object) period), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
        return list;
      }
    }

    public static List<string> GetAllPeriods()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select distinct periods from GameInfo_Daily order by periods desc"), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
        return list;
      }
    }

    public static string GetCurrentPeriod()
    {
      object obj = SqlServerHelper.ExecuteScalar(CommandType.Text, string.Format("select period from FootballSinaDaily.dbo.CurrentPeriod"), new SqlParameter[0]);
      if (obj != null && !(obj is DBNull))
        return obj.ToString();
      return "";
    }

    public static Dictionary<int, DateTime> GetOddsUpdateTime(int gameId)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select company_id,MAX(update_time) as update_time from OddsInfo_Daily\r\nwhere game_id={0} group by company_id", (object) gameId), new SqlParameter[0]))
      {
        Dictionary<int, DateTime> dictionary = new Dictionary<int, DateTime>();
        while (dataReader.Read())
          dictionary[dataReader.GetInt32(0)] = dataReader.GetDateTime(1);
        return dictionary;
      }
    }

    public static Dictionary<int, DateTime> GetCurrentOddsUpdateTime(int gameId)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select company_id,MAX(update_time) as update_time from FootballSinaDaily.dbo.OddsInfo_Daily\r\nwhere game_id={0} group by company_id", (object) gameId), new SqlParameter[0]))
      {
        Dictionary<int, DateTime> dictionary = new Dictionary<int, DateTime>();
        while (dataReader.Read())
          dictionary[dataReader.GetInt32(0)] = dataReader.GetDateTime(1);
        return dictionary;
      }
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
          company.Country = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
          company.IsLeading = dataReader.GetInt32(4);
          dictionary[company.Id] = company;
        }
        return dictionary;
      }
    }

    public static List<GameInfo> GetGameInfosByDate(DateTime gameTime, string tableName)
    {
      return DataHandlerSina.GetGameInfosByDate(gameTime.ToString("yyyy-MM-dd"), tableName);
    }

    public static List<GameInfo> GetGameInfosByDate(string gameTime, string tableName)
    {
      return DataHandlerSina.GetGameInfosBySQL(string.Format("SELECT * FROM {1} WHERE CAST(game_time as date)='{0}';", (object) gameTime, (object) tableName));
    }

    public static List<GameInfo> GetGameInfosByPeriod(string period)
    {
      return DataHandlerSina.GetGameInfosBySQL(string.Format("SELECT * FROM GameInfo_Daily WHERE periods='{0}' order by serial;", (object) period));
    }

    public static List<GameInfo> GetGameInfosByPeriod(string period, string tableName)
    {
      return DataHandlerSina.GetGameInfosBySQL(string.Format("SELECT * FROM {1} WHERE periods='{0}' order by serial;", (object) period, (object) tableName));
    }

    public static List<GameInfo> GetGameInfosByDateAndLeague(DateTime gameTime, string League, string tableName)
    {
      return DataHandlerSina.GetGameInfosBySQL(string.Format("SELECT * FROM {2} WHERE CAST(game_time as date)='{0}' AND league_name='{1}';", (object) gameTime.ToString("yyyy-MM-dd"), (object) League, (object) tableName));
    }

    public static List<GameInfo> GetGameInfosByPeriodAndLeague(string period, string League)
    {
      return DataHandlerSina.GetGameInfosBySQL(string.Format("SELECT * FROM GameInfo_Daily WHERE periods='{0}' AND league_name='{1}' order by serial;", (object) period, (object) League));
    }

    private static List<GameInfo> GetGameInfosBySQL(string sql)
    {
      List<GameInfo> list = new List<GameInfo>();
      using (IDataReader reader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          GameInfo gameInfo = DataHandlerSina.RetrieveGameInfo(reader);
          list.Add(gameInfo);
        }
        return list;
      }
    }

    private static List<GameInfo> GetGameInfosBySQL2(string sql)
    {
      List<GameInfo> list = new List<GameInfo>();
      using (IDataReader reader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          GameInfo gameInfo = DataHandlerSina.RetrieveGameInfo2(reader);
          list.Add(gameInfo);
        }
        return list;
      }
    }

    public static Stack<OddsInfo> GetAllOddsInfoByGame(int gameId, string tableName)
    {
      return DataHandlerSina.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' order by update_time desc;", (object) gameId, (object) tableName));
    }

    public static Stack<OddsInfo> GetMainOddsInfoByGame(int gameId, string tableName)
    {
      return DataHandlerSina.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM CompanyInfo WHERE is_leading = 1) order by update_time desc;", (object) gameId, (object) tableName));
    }

    public static Stack<OddsInfo> GetNotExchangeOddsInfoByGame(int gameId, string tableName)
    {
      return DataHandlerSina.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM CompanyInfo WHERE is_leading != 2) order by update_time desc;", (object) gameId, (object) tableName));
    }

    public static Stack<OddsInfo> GetCustomOddsInfoByGame(int gameId, string tableName, int idType)
    {
      return DataHandlerSina.GetOddsInfo(string.Format("SELECT * FROM {1} WHERE game_id='{0}' AND company_id IN (SELECT company_id FROM CustomCompanyList WHERE id = {2}) order by update_time desc;", (object) gameId, (object) tableName, (object) idType));
    }

    public static Stack<OddsInfo> GetOddsInfo(string sql)
    {
      Stack<OddsInfo> stack = new Stack<OddsInfo>();
      using (IDataReader reader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter[0]))
      {
        while (reader.Read())
        {
          OddsInfo oddsInfo = DataHandlerSina.RetrieveOddsInfo(reader);
          stack.Push(oddsInfo);
        }
        return stack;
      }
    }

    public static void CallingSQLJob(string jobName)
    {
      SqlParameter[] sqlParameterArray = new SqlParameter[1]
      {
        new SqlParameter("@job_name", (object) jobName)
      };
      sqlParameterArray[0].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.StoredProcedure, "msdb.dbo.sp_start_job", sqlParameterArray);
    }

    public static int GetLastUpdatedSerial()
    {
      object obj = SqlServerHelper.ExecuteScalar(CommandType.Text, string.Format("select MIN(serial) from FootballSinaDaily.dbo.GameInfo_Daily where LEN(score_half) = 0"), new SqlParameter[0]);
      if (obj != null && !(obj is DBNull))
        return Convert.ToInt32(obj);
      return 1;
    }

    private static GameInfo RetrieveGameInfo(IDataReader reader)
    {
      GameInfo gameInfo = new GameInfo();
      gameInfo.GameId = Convert.ToInt32(reader["id"]);
      gameInfo.Periods = reader["periods"].ToString().Trim();
      gameInfo.League = reader["league_name"].ToString().Trim();
      gameInfo.Serial = Convert.ToInt32(reader["serial"]);
      gameInfo.GameTime = Convert.ToDateTime(reader["game_time"]);
      gameInfo.Host = reader["host"].ToString().Trim();
      gameInfo.HostRank = reader["host_rank"] is DBNull ? string.Empty : reader["host_rank"].ToString().Trim();
      gameInfo.Guest = reader["guest"].ToString().Trim();
      gameInfo.GuestRank = reader["guest_rank"] is DBNull ? string.Empty : reader["guest_rank"].ToString().Trim();
      gameInfo.HalfScore = reader["score_half"] is DBNull ? string.Empty : reader["score_half"].ToString().Trim();
      gameInfo.FinalScore = reader["score_final"] is DBNull ? string.Empty : reader["score_final"].ToString().Trim();
      if (reader.FieldCount > 12)
      {
        gameInfo.WinAvg = reader.GetDecimal(12);
        gameInfo.TieAvg = reader.GetDecimal(13);
        gameInfo.LoseAvg = reader.GetDecimal(14);
        gameInfo.AsiaTape = reader.GetDecimal(15);
        gameInfo.AsiaTapeZh = reader.GetString(16);
        gameInfo.AsiaTapeResult = reader.GetInt32(17);
        gameInfo.ScoreTape = reader.GetDecimal(18);
        gameInfo.ScoreTapeZh = reader.GetString(19);
        gameInfo.ScoreTapeResult = reader.GetInt32(20);
      }
      if (reader.FieldCount > 21)
      {
        gameInfo.DataReady = reader.GetInt32(23);
        gameInfo.CompanyList = reader.IsDBNull(24) ? reader.GetString(22) : reader.GetString(24);
      }
      return gameInfo;
    }

    private static GameInfo RetrieveGameInfo2(IDataReader reader)
    {
      GameInfo gameInfo = new GameInfo();
      gameInfo.GameId = Convert.ToInt32(reader["id"]);
      gameInfo.Periods = reader["periods"].ToString();
      gameInfo.League = reader["league_name"].ToString();
      gameInfo.Serial = Convert.ToInt32(reader["serial"]);
      gameInfo.GameTime = Convert.ToDateTime(reader["game_time"]);
      gameInfo.Host = reader["host"].ToString();
      gameInfo.HostRank = reader["host_rank"] is DBNull ? string.Empty : reader["host_rank"].ToString();
      gameInfo.Guest = reader["guest"].ToString();
      gameInfo.GuestRank = reader["guest_rank"] is DBNull ? string.Empty : reader["guest_rank"].ToString();
      gameInfo.HalfScore = reader["score_half"] is DBNull ? string.Empty : reader["score_half"].ToString();
      gameInfo.FinalScore = reader["score_final"] is DBNull ? string.Empty : reader["score_final"].ToString();
      if (reader.FieldCount > 12)
      {
        gameInfo.DataReady = reader.GetInt32(14);
        gameInfo.CompanyList = reader.IsDBNull(15) ? reader.GetString(13) : reader.GetString(15);
      }
      return gameInfo;
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
