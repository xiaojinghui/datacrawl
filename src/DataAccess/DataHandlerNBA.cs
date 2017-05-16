// Decompiled with JetBrains decompiler
// Type: DataAccess.DataHandlerNBA
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
  public class DataHandlerNBA
  {
    public static void SaveGameInfo(string gameId, string awayId, string homeId, string gameTime, string letPoint, string scoreHandicap, string halfScore, string finalScore, int letPointResult, int scoreResult, int gameType)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GameInfo (id,id_500,time,home,away,half,final,l_p_h,s_h,l_p_r,s_r,type) \r\nSELECT @id,0,@game_time,@home_id,@away_id,@score_half,@score_final,@let_point,@score_handicap,@let_point_result,@score_result,@game_type\r\nWHERE NOT EXISTS (SELECT 1 FROM GameInfo WHERE id=@id);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[11];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@game_time", (object) gameTime);
      sqlParameterArray[1].DbType = DbType.DateTime;
      sqlParameterArray[2] = new SqlParameter("@home_id", (object) homeId);
      sqlParameterArray[2].DbType = DbType.Int32;
      sqlParameterArray[3] = new SqlParameter("@away_id", (object) awayId);
      sqlParameterArray[3].DbType = DbType.Int32;
      sqlParameterArray[4] = new SqlParameter("@score_half", string.IsNullOrEmpty(halfScore) ? (object) "" : (object) halfScore);
      sqlParameterArray[4].DbType = DbType.AnsiString;
      sqlParameterArray[5] = new SqlParameter("@score_final", (object) finalScore);
      sqlParameterArray[5].DbType = DbType.AnsiString;
      sqlParameterArray[6] = new SqlParameter("@let_point", string.IsNullOrEmpty(letPoint) ? (object) "" : (object) letPoint);
      sqlParameterArray[6].DbType = DbType.AnsiString;
      sqlParameterArray[7] = new SqlParameter("@score_handicap", string.IsNullOrEmpty(scoreHandicap) ? (object) "" : (object) scoreHandicap);
      sqlParameterArray[7].DbType = DbType.AnsiString;
      sqlParameterArray[8] = new SqlParameter("@let_point_result", (object) letPointResult);
      sqlParameterArray[8].DbType = DbType.Int32;
      sqlParameterArray[9] = new SqlParameter("@score_result", (object) scoreResult);
      sqlParameterArray[9].DbType = DbType.Int32;
      sqlParameterArray[10] = new SqlParameter("@game_type", (object) gameType);
      sqlParameterArray[10].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateGameId(string gameId, string teamId, string gameTime)
    {
      string cmdText = "update GameInfo\r\nset id=@game_id\r\nwhere (home=@team_id or away=@team_id)\r\nand Convert(varchar(12), DateAdd(day, -1, [time]), 107)=@game_time";
      SqlParameter[] sqlParameterArray = new SqlParameter[3];
      sqlParameterArray[0] = new SqlParameter("@game_id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@game_time", (object) gameTime);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@team_id", (object) teamId);
      sqlParameterArray[2].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveGameStats(string gameId, string teamId, JToken data, int isHome)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GameStats (game_id,team_id,FGM,FGA,[FG%],[3FGM],[3FGA],[3FG%],FTM,FTA,[FT%],OREB,DREB,REB,AST,STL,BLK,TOV,PF,PTS,MP,is_home) \r\nSELECT @id,@team_id,@FGM,@FGA,@FG_P,@FGM_3,@FGA_3,@FG_3P,@FTM,@FTA,@FT_P,@OREB,@DREB,@REB,@AST,@STL,@BLK,@TOV,@PF,@PTS,@MP,@Is_Home\r\nWHERE NOT EXISTS (SELECT 1 FROM GameStats WHERE game_id=@id AND team_id=@team_id);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[22];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@team_id", (object) teamId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@FGM", (object) data[(object) 6].ToString());
      sqlParameterArray[2].DbType = DbType.Int32;
      sqlParameterArray[3] = new SqlParameter("@FGA", (object) data[(object) 7].ToString());
      sqlParameterArray[3].DbType = DbType.Int32;
      sqlParameterArray[4] = new SqlParameter("@FG_P", (object) data[(object) 8].ToString());
      sqlParameterArray[4].DbType = DbType.Decimal;
      sqlParameterArray[5] = new SqlParameter("@FGM_3", (object) data[(object) 9].ToString());
      sqlParameterArray[5].DbType = DbType.Int32;
      sqlParameterArray[6] = new SqlParameter("@FGA_3", (object) data[(object) 10].ToString());
      sqlParameterArray[6].DbType = DbType.Int32;
      sqlParameterArray[7] = new SqlParameter("@FG_3P", (object) data[(object) 11].ToString());
      sqlParameterArray[7].DbType = DbType.Decimal;
      sqlParameterArray[8] = new SqlParameter("@FTM", (object) data[(object) 12].ToString());
      sqlParameterArray[8].DbType = DbType.Int32;
      sqlParameterArray[9] = new SqlParameter("@FTA", (object) data[(object) 13].ToString());
      sqlParameterArray[9].DbType = DbType.Int32;
      sqlParameterArray[10] = new SqlParameter("@FT_P", (object) data[(object) 14].ToString());
      sqlParameterArray[10].DbType = DbType.Decimal;
      sqlParameterArray[11] = new SqlParameter("@OREB", (object) data[(object) 15].ToString());
      sqlParameterArray[11].DbType = DbType.Int32;
      sqlParameterArray[12] = new SqlParameter("@DREB", (object) data[(object) 16].ToString());
      sqlParameterArray[12].DbType = DbType.Int32;
      sqlParameterArray[13] = new SqlParameter("@REB", (object) data[(object) 17].ToString());
      sqlParameterArray[13].DbType = DbType.Int32;
      sqlParameterArray[14] = new SqlParameter("@AST", (object) data[(object) 18].ToString());
      sqlParameterArray[14].DbType = DbType.Int32;
      sqlParameterArray[15] = new SqlParameter("@STL", (object) data[(object) 19].ToString());
      sqlParameterArray[15].DbType = DbType.Int32;
      sqlParameterArray[16] = new SqlParameter("@BLK", (object) data[(object) 20].ToString());
      sqlParameterArray[16].DbType = DbType.Int32;
      sqlParameterArray[17] = new SqlParameter("@TOV", (object) data[(object) 21].ToString());
      sqlParameterArray[17].DbType = DbType.Int32;
      sqlParameterArray[18] = new SqlParameter("@PF", (object) data[(object) 22].ToString());
      sqlParameterArray[18].DbType = DbType.Int32;
      sqlParameterArray[19] = new SqlParameter("@PTS", (object) data[(object) 23].ToString());
      sqlParameterArray[19].DbType = DbType.Int32;
      string str1 = data[(object) 5].ToString().Replace("\"", "");
      string str2 = str1.Substring(0, str1.IndexOf(":"));
      sqlParameterArray[20] = new SqlParameter("@MP", (object) str2);
      sqlParameterArray[20].DbType = DbType.Int32;
      sqlParameterArray[21] = new SqlParameter("@Is_Home", (object) isHome);
      sqlParameterArray[21].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveGamePreStats(int gameId, int home, int away, DataRow homeRow, DataRow awayRow)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GamePreStats (game_id,home,away,FGM,FGA,[FG%],[3FGM],[3FGA],[3FG%],FTM,FTA,[FT%],OREB,DREB,REB,AST,STL,BLK,TOV,PF,\r\n    FGM_A,FGA_A,[FG%_A],[3FGM_A],[3FGA_A],[3FG%_A],FTM_A,FTA_A,[FT%_A],OREB_A,DREB_A,REB_A,AST_A,STL_A,BLK_A,TOV_A,PF_A) \r\nSELECT @id,@home,@away,@FGM,@FGA,@FG_P,@FGM_3,@FGA_3,@FG_3P,@FTM,@FTA,@FT_P,@OREB,@DREB,@REB,@AST,@STL,@BLK,@TOV,@PF,\r\n    @FGM_A,@FGA_A,@FG_P_A,@FGM_3_A,@FGA_3_A,@FG_3P_A,@FTM_A,@FTA_A,@FT_P_A,@OREB_A,@DREB_A,@REB_A,@AST_A,@STL_A,@BLK_A,@TOV_A,@PF_A\r\nWHERE NOT EXISTS (SELECT 1 FROM GamePreStats WHERE game_id=@id);\r\nSET NOCOUNT OFF;";
      int num1 = Convert.ToInt32(homeRow[14]);
      int num2 = Convert.ToInt32(awayRow[14]);
      SqlParameter[] sqlParameterArray = new SqlParameter[37];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@home", (object) home);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@FGM", (object) (Convert.ToDouble(homeRow[0]) / (double) num1));
      sqlParameterArray[2].DbType = DbType.Decimal;
      sqlParameterArray[3] = new SqlParameter("@FGA", (object) (Convert.ToDouble(homeRow[1]) / (double) num1));
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@FG_P", (object) (Convert.ToDouble(homeRow[0]) * 100.0 / Convert.ToDouble(homeRow[1])));
      sqlParameterArray[4].DbType = DbType.Decimal;
      sqlParameterArray[5] = new SqlParameter("@FGM_3", (object) (Convert.ToDouble(homeRow[2]) / (double) num1));
      sqlParameterArray[5].DbType = DbType.Decimal;
      sqlParameterArray[6] = new SqlParameter("@FGA_3", (object) (Convert.ToDouble(homeRow[3]) / (double) num1));
      sqlParameterArray[6].DbType = DbType.Decimal;
      sqlParameterArray[7] = new SqlParameter("@FG_3P", (object) (Convert.ToDouble(homeRow[2]) * 100.0 / Convert.ToDouble(homeRow[3])));
      sqlParameterArray[7].DbType = DbType.Decimal;
      sqlParameterArray[8] = new SqlParameter("@FTM", (object) (Convert.ToDouble(homeRow[4]) / (double) num1));
      sqlParameterArray[8].DbType = DbType.Decimal;
      sqlParameterArray[9] = new SqlParameter("@FTA", (object) (Convert.ToDouble(homeRow[5]) / (double) num1));
      sqlParameterArray[9].DbType = DbType.Decimal;
      sqlParameterArray[10] = new SqlParameter("@FT_P", (object) (Convert.ToDouble(homeRow[4]) * 100.0 / Convert.ToDouble(homeRow[5])));
      sqlParameterArray[10].DbType = DbType.Decimal;
      sqlParameterArray[11] = new SqlParameter("@OREB", (object) (Convert.ToDouble(homeRow[6]) / (double) num1));
      sqlParameterArray[11].DbType = DbType.Decimal;
      sqlParameterArray[12] = new SqlParameter("@DREB", (object) (Convert.ToDouble(homeRow[7]) / (double) num1));
      sqlParameterArray[12].DbType = DbType.Decimal;
      sqlParameterArray[13] = new SqlParameter("@REB", (object) (Convert.ToDouble(homeRow[8]) / (double) num1));
      sqlParameterArray[13].DbType = DbType.Decimal;
      sqlParameterArray[14] = new SqlParameter("@AST", (object) (Convert.ToDouble(homeRow[9]) / (double) num1));
      sqlParameterArray[14].DbType = DbType.Decimal;
      sqlParameterArray[15] = new SqlParameter("@STL", (object) (Convert.ToDouble(homeRow[10]) / (double) num1));
      sqlParameterArray[15].DbType = DbType.Decimal;
      sqlParameterArray[16] = new SqlParameter("@BLK", (object) (Convert.ToDouble(homeRow[11]) / (double) num1));
      sqlParameterArray[16].DbType = DbType.Decimal;
      sqlParameterArray[17] = new SqlParameter("@TOV", (object) (Convert.ToDouble(homeRow[12]) / (double) num1));
      sqlParameterArray[17].DbType = DbType.Decimal;
      sqlParameterArray[18] = new SqlParameter("@PF", (object) (Convert.ToDouble(homeRow[13]) / (double) num1));
      sqlParameterArray[18].DbType = DbType.Decimal;
      sqlParameterArray[19] = new SqlParameter("@away", (object) away);
      sqlParameterArray[19].DbType = DbType.Int32;
      sqlParameterArray[20] = new SqlParameter("@FGM_A", (object) (Convert.ToDouble(awayRow[0]) / (double) num2));
      sqlParameterArray[20].DbType = DbType.Decimal;
      sqlParameterArray[21] = new SqlParameter("@FGA_A", (object) (Convert.ToDouble(awayRow[1]) / (double) num2));
      sqlParameterArray[21].DbType = DbType.Decimal;
      sqlParameterArray[22] = new SqlParameter("@FG_P_A", (object) (Convert.ToDouble(awayRow[0]) * 100.0 / Convert.ToDouble(awayRow[1])));
      sqlParameterArray[22].DbType = DbType.Decimal;
      sqlParameterArray[23] = new SqlParameter("@FGM_3_A", (object) (Convert.ToDouble(awayRow[2]) / (double) num2));
      sqlParameterArray[23].DbType = DbType.Decimal;
      sqlParameterArray[24] = new SqlParameter("@FGA_3_A", (object) (Convert.ToDouble(awayRow[3]) / (double) num2));
      sqlParameterArray[24].DbType = DbType.Decimal;
      sqlParameterArray[25] = new SqlParameter("@FG_3P_A", (object) (Convert.ToDouble(awayRow[2]) * 100.0 / Convert.ToDouble(awayRow[3])));
      sqlParameterArray[25].DbType = DbType.Decimal;
      sqlParameterArray[26] = new SqlParameter("@FTM_A", (object) (Convert.ToDouble(awayRow[4]) / (double) num2));
      sqlParameterArray[26].DbType = DbType.Decimal;
      sqlParameterArray[27] = new SqlParameter("@FTA_A", (object) (Convert.ToDouble(awayRow[5]) / (double) num2));
      sqlParameterArray[27].DbType = DbType.Decimal;
      sqlParameterArray[28] = new SqlParameter("@FT_P_A", (object) (Convert.ToDouble(awayRow[4]) * 100.0 / Convert.ToDouble(awayRow[5])));
      sqlParameterArray[28].DbType = DbType.Decimal;
      sqlParameterArray[29] = new SqlParameter("@OREB_A", (object) (Convert.ToDouble(awayRow[6]) / (double) num2));
      sqlParameterArray[29].DbType = DbType.Decimal;
      sqlParameterArray[30] = new SqlParameter("@DREB_A", (object) (Convert.ToDouble(awayRow[7]) / (double) num2));
      sqlParameterArray[30].DbType = DbType.Decimal;
      sqlParameterArray[31] = new SqlParameter("@REB_A", (object) (Convert.ToDouble(awayRow[8]) / (double) num2));
      sqlParameterArray[31].DbType = DbType.Decimal;
      sqlParameterArray[32] = new SqlParameter("@AST_A", (object) (Convert.ToDouble(awayRow[9]) / (double) num2));
      sqlParameterArray[32].DbType = DbType.Decimal;
      sqlParameterArray[33] = new SqlParameter("@STL_A", (object) (Convert.ToDouble(awayRow[10]) / (double) num2));
      sqlParameterArray[33].DbType = DbType.Decimal;
      sqlParameterArray[34] = new SqlParameter("@BLK_A", (object) (Convert.ToDouble(awayRow[11]) / (double) num2));
      sqlParameterArray[34].DbType = DbType.Decimal;
      sqlParameterArray[35] = new SqlParameter("@TOV_A", (object) (Convert.ToDouble(awayRow[12]) / (double) num2));
      sqlParameterArray[35].DbType = DbType.Decimal;
      sqlParameterArray[36] = new SqlParameter("@PF_A", (object) (Convert.ToDouble(awayRow[13]) / (double) num2));
      sqlParameterArray[36].DbType = DbType.Decimal;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveAsiaHandicap(string gameId, string name, string date, Decimal awayOdds, string handicap, Decimal homeOdds)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO AsiaHandicap (game_id,cmp_name,date,away_odds,handicap,home_odds) \r\nSELECT @id,@cmp_name,@date,@away_odds,@handicap,@home_odds\r\nWHERE NOT EXISTS (SELECT 1 FROM AsiaHandicap WHERE game_id=@id AND cmp_name=@cmp_name AND date=@date);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[6];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@cmp_name", (object) name);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@date", (object) date);
      sqlParameterArray[2].DbType = DbType.DateTime;
      sqlParameterArray[3] = new SqlParameter("@away_odds", (object) awayOdds);
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@handicap", (object) handicap);
      sqlParameterArray[4].DbType = DbType.AnsiString;
      sqlParameterArray[5] = new SqlParameter("@home_odds", (object) homeOdds);
      sqlParameterArray[5].DbType = DbType.Decimal;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveScoreHandicap(string gameId, string name, string date, Decimal bigOdds, string handicap, Decimal smallOdds)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO ScoreHandicap (game_id,cmp_name,date,big_odds,handicap,small_odds) \r\nSELECT @id,@cmp_name,@date,@big_odds,@handicap,@small_odds\r\nWHERE NOT EXISTS (SELECT 1 FROM ScoreHandicap WHERE game_id=@id AND cmp_name=@cmp_name AND date=@date);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[6];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@cmp_name", (object) name);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@date", (object) date);
      sqlParameterArray[2].DbType = DbType.DateTime;
      sqlParameterArray[3] = new SqlParameter("@big_odds", (object) bigOdds);
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@handicap", (object) handicap);
      sqlParameterArray[4].DbType = DbType.AnsiString;
      sqlParameterArray[5] = new SqlParameter("@small_odds", (object) smallOdds);
      sqlParameterArray[5].DbType = DbType.Decimal;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateGameScore(int gameId, string teamId, int Q1, int Q2, int Q3, int Q4, int OT1, int OT2, int OT3, int OT4)
    {
      string cmdText = "SET NOCOUNT ON;\r\nUPDATE GameStats \r\nSET Q1=@q1, Q2=@q2, Q3=@q3, Q4=@q4, OT1=@ot1, OT2=@ot2, OT3=@ot3, OT4=@ot4\r\nWHERE game_id=@id AND team_id=@team_id;\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[10];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@team_id", (object) teamId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@q1", (object) Q1);
      sqlParameterArray[2].DbType = DbType.Int32;
      sqlParameterArray[3] = new SqlParameter("@q2", (object) Q2);
      sqlParameterArray[3].DbType = DbType.Int32;
      sqlParameterArray[4] = new SqlParameter("@q3", (object) Q3);
      sqlParameterArray[4].DbType = DbType.Int32;
      sqlParameterArray[5] = new SqlParameter("@q4", (object) Q4);
      sqlParameterArray[5].DbType = DbType.Int32;
      sqlParameterArray[6] = new SqlParameter("@ot1", (object) OT1);
      sqlParameterArray[6].DbType = DbType.Int32;
      sqlParameterArray[7] = new SqlParameter("@ot2", (object) OT2);
      sqlParameterArray[7].DbType = DbType.Int32;
      sqlParameterArray[8] = new SqlParameter("@ot3", (object) OT3);
      sqlParameterArray[8].DbType = DbType.Int32;
      sqlParameterArray[9] = new SqlParameter("@ot4", (object) OT4);
      sqlParameterArray[9].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void Update500Odds(string gameId, string letPoint, string scoreHandicap)
    {
      string cmdText = "SET NOCOUNT ON;\r\nUPDATE GameInfo \r\nSET l_p_h=@let_point, s_h=@score_handicap\r\nWHERE id=@id;\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[3];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@let_point", (object) letPoint);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@score_handicap", (object) scoreHandicap);
      sqlParameterArray[2].DbType = DbType.AnsiString;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void UpdateGameResults()
    {
      SqlServerHelper.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.updateGameScores", new SqlParameter[0]);
    }

    public static int GetMaxGameId(string season)
    {
      object obj = SqlServerHelper.ExecuteScalar(CommandType.Text, string.Format("select max(id) from GameInfo where id like '{0}%'", (object) season), new SqlParameter[0]);
      if (!(obj is DBNull))
        return Convert.ToInt32(obj);
      return 0;
    }

    public static Dictionary<int, string> GetAllGameTypes()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, "select * from GameType", new SqlParameter[0]))
      {
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        while (dataReader.Read())
          dictionary[dataReader.GetInt32(0)] = dataReader.GetString(1);
        return dictionary;
      }
    }

    public static DataTable GetGameInfos()
    {
      return SqlServerHelper.ExecuteDataTable("select g.id, a.name_en as away, h.name_en as home, g.[time], g.l_p_h, g.l_p_r, \r\n\tg.s_h, g.s_r, g.half, g.final, t.[type]\r\nfrom GameInfo g\r\nleft join TeamInfo a on g.away = a.id\r\nleft join TeamInfo h on g.home = h.id\r\nleft join GameType t on g.[type] = t.id\r\norder by [time]");
    }

    public static DataTable GetSumStatistics(int teamId, List<int> gameIds)
    {
      return SqlServerHelper.ExecuteDataTable(string.Format("select SUM(FGM),SUM(FGA),SUM([3FGM]),SUM([3FGA]),SUM(FTM),SUM(FTA),SUM(OREB),SUM(DREB),SUM(REB),SUM(AST),SUM(STL),SUM(BLK),SUM(TOV),SUM(PF),COUNT(*) \r\nfrom dbo.GameStats\r\nwhere game_id in ({1})\r\nand team_id={0}", (object) teamId, (object) string.Join<int>(",", (IEnumerable<int>) gameIds)));
    }

    public static DataTable GetMissedGameInfo()
    {
      return SqlServerHelper.ExecuteDataTable("select * from GameInfo\r\nWHERE len(LTRIM(RTRIM(final))) = 0");
    }

    public static List<NBAGameInfo> GetGamesByYear(int year)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo where [time] between '{0}-10-1' and '{1}-7-1' and [id] < 40000000", (object) year, (object) (year + 1)), new SqlParameter[0]))
      {
        List<NBAGameInfo> list = new List<NBAGameInfo>();
        while (dataReader.Read())
          list.Add(new NBAGameInfo()
          {
            GameId = dataReader.GetInt32(0),
            AwayId = dataReader.GetInt32(2),
            HomeId = dataReader.GetInt32(3),
            GameTime = dataReader.GetDateTime(4),
            HalfScore = dataReader.GetString(5),
            FinalScore = dataReader.GetString(6)
          });
        return list;
      }
    }

    public static DataTable GetGameInfosByDate(DateTime gameTime)
    {
      return SqlServerHelper.ExecuteDataTable(string.Format("select * from GameInfo where [time] = '{0}'", (object) gameTime.ToString("yyyy-MM-dd")));
    }

    public static List<int> GetPreviousGames(int teamId, int year, DateTime gameTime)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select [id] from GameInfo\r\nwhere [time] > '{1}-10-1' \r\nand [time] < '{2}'\r\nand [id] < 40000000\r\nand (away={0} or home={0})", (object) teamId, (object) year, (object) gameTime), new SqlParameter[0]))
      {
        List<int> list = new List<int>();
        while (dataReader.Read())
          list.Add(dataReader.GetInt32(0));
        return list;
      }
    }

    public static List<int> GetLastSeasonGames(int teamId, int year)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select [id] from GameInfo\r\nwhere [time] > '{1}-10-1' \r\nand [time] < '{2}-7-1'\r\nand [id] < 40000000\r\nand (away={0} or home={0})", (object) teamId, (object) (year - 1), (object) year), new SqlParameter[0]))
      {
        List<int> list = new List<int>();
        while (dataReader.Read())
          list.Add(dataReader.GetInt32(0));
        return list;
      }
    }

    public static Dictionary<int, NBATeamInfo> GetAllTeamInfos()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, "select * from TeamInfo", new SqlParameter[0]))
      {
        Dictionary<int, NBATeamInfo> dictionary = new Dictionary<int, NBATeamInfo>();
        while (dataReader.Read())
        {
          NBATeamInfo nbaTeamInfo = new NBATeamInfo();
          nbaTeamInfo.TeamId = dataReader.GetInt32(0);
          nbaTeamInfo.TeamId500 = dataReader.GetInt32(1);
          nbaTeamInfo.Name = dataReader.GetString(2);
          nbaTeamInfo.ENName = dataReader.GetString(3);
          nbaTeamInfo.League = dataReader.GetInt32(4);
          nbaTeamInfo.Abbr = dataReader.GetString(5);
          nbaTeamInfo.TeamIdSina = dataReader.GetInt32(6);
          dictionary[nbaTeamInfo.TeamId] = nbaTeamInfo;
        }
        return dictionary;
      }
    }

    public static List<int> GetNoDetailScoreGame()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, "select game_id from GameStats where Q1 is null and Q2 is null and Q3 is null and Q4 is null", new SqlParameter[0]))
      {
        List<int> list = new List<int>();
        while (dataReader.Read())
          list.Add(dataReader.GetInt32(0));
        return list;
      }
    }

    public static List<int> GetNoDetailScoreGame(int year)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select game_id from GameStats where Q1 is null and Q2 is null and Q3 is null and Q4 is null and game_id like '2{0}%'", (object) year.ToString("D2")), new SqlParameter[0]))
      {
        List<int> list = new List<int>();
        while (dataReader.Read())
          list.Add(dataReader.GetInt32(0));
        return list;
      }
    }

    public static string GetGameIdBy500TeamId(string gameTime, string awayId, string homeId)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo g\r\nleft join TeamInfo a on g.away=a.id\r\nleft join TeamInfo h on g.home=h.id\r\nwhere g.time = '{0}' and a.id_500={1} and h.id_500={2}", (object) gameTime, (object) awayId, (object) homeId), new SqlParameter[0]))
      {
        string str = string.Empty;
        while (dataReader.Read())
          str = dataReader.GetInt32(0).ToString();
        return str;
      }
    }

    public static string GetGameIdByName(string gameTime, string awayName, string homeName)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo g\r\nleft join TeamInfo a on g.away=a.id\r\nleft join TeamInfo h on g.home=h.id\r\nwhere g.time = '{0}' and a.name like '%{1}%' and h.name like '%{2}%'", (object) gameTime, (object) awayName, (object) homeName), new SqlParameter[0]))
      {
        string str = string.Empty;
        while (dataReader.Read())
          str = dataReader.GetInt32(0).ToString();
        return str;
      }
    }

    public static bool IsPredictFinished(string date)
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo g\r\nleft join GamePredictPTS pts on g.id = pts.game_id\r\nwhere g.time='{0}' and pts.game_id is null", (object) date), new SqlParameter[0]))
      {
        if (dataReader.Read())
          return false;
      }
      return true;
    }
  }
}
