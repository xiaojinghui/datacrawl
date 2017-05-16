// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.DataHandlerNBA
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
  public class DataHandlerNBA
  {
    private SqlHelper _helper = (SqlHelper) null;
    private string _connectionString = ConfigurationManager.ConnectionStrings["NBAConnectionString"].ConnectionString;

    public DataHandlerNBA()
    {
      this._helper = new SqlHelper(this._connectionString);
    }

    public void SavePredictPTS(int gameId, int teamId, double pace1, double pts1, double pace1_1, double pts1_1, double pace2, double pts2, double pace2_1, double pts2_1, double pace3, double pts3, double pace3_1, double pts3_1, double pace4, double pts4, double pace4_1, double pts4_1)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GamePredictPTS (game_id,team_id,[pace1],[pts1],[pace1_1],[pts1_1],[pace2],[pts2],[pace2_1],[pts2_1],[pace3],[pts3],[pace3_1],[pts3_1],[pace4],[pts4],[pace4_1],[pts4_1]) \r\nSELECT @id,@team_id,@pace1,@pts1,@pace1_1,@pts1_1,@pace2,@pts2,@pace2_1,@pts2_1,@pace3,@pts3,@pace3_1,@pts3_1,@pace4,@pts4,@pace4_1,@pts4_1\r\nWHERE NOT EXISTS (SELECT 1 FROM GamePredictPTS WHERE game_id=@id AND team_id=@team_id);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[18];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@team_id", (object) teamId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@pace1", (object) pace1);
      sqlParameterArray[2].DbType = DbType.Decimal;
      sqlParameterArray[3] = new SqlParameter("@pts1", (object) pts1);
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@pace1_1", (object) pace1_1);
      sqlParameterArray[4].DbType = DbType.Decimal;
      sqlParameterArray[5] = new SqlParameter("@pts1_1", (object) pts1_1);
      sqlParameterArray[5].DbType = DbType.Decimal;
      sqlParameterArray[6] = new SqlParameter("@pace2", (object) pace2);
      sqlParameterArray[6].DbType = DbType.Decimal;
      sqlParameterArray[7] = new SqlParameter("@pts2", (object) pts2);
      sqlParameterArray[7].DbType = DbType.Decimal;
      sqlParameterArray[8] = new SqlParameter("@pace2_1", (object) pace2_1);
      sqlParameterArray[8].DbType = DbType.Decimal;
      sqlParameterArray[9] = new SqlParameter("@pts2_1", (object) pts2_1);
      sqlParameterArray[9].DbType = DbType.Decimal;
      sqlParameterArray[10] = new SqlParameter("@pace3", (object) pace3);
      sqlParameterArray[10].DbType = DbType.Decimal;
      sqlParameterArray[11] = new SqlParameter("@pts3", (object) pts3);
      sqlParameterArray[11].DbType = DbType.Decimal;
      sqlParameterArray[12] = new SqlParameter("@pace3_1", (object) pace3_1);
      sqlParameterArray[12].DbType = DbType.Decimal;
      sqlParameterArray[13] = new SqlParameter("@pts3_1", (object) pts3_1);
      sqlParameterArray[13].DbType = DbType.Decimal;
      sqlParameterArray[14] = new SqlParameter("@pace4", (object) pace4);
      sqlParameterArray[14].DbType = DbType.Decimal;
      sqlParameterArray[15] = new SqlParameter("@pts4", (object) pts4);
      sqlParameterArray[15].DbType = DbType.Decimal;
      sqlParameterArray[16] = new SqlParameter("@pace4_1", (object) pace4_1);
      sqlParameterArray[16].DbType = DbType.Decimal;
      sqlParameterArray[17] = new SqlParameter("@pts4_1", (object) pts4_1);
      sqlParameterArray[17].DbType = DbType.Decimal;
      this._helper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public void SaveNewPredictPTS(int gameId, int teamId, double pts1, double pts2, double pts3, double pts4, int mode)
    {
      string cmdText = string.Format("SET NOCOUNT ON;\r\nINSERT INTO GamePredictPTS{0} (game_id,team_id,[pts1],[pts2],[pts3],[pts4]) \r\nSELECT @id,@team_id,@pts1,@pts2,@pts3,@pts4\r\nWHERE NOT EXISTS (SELECT 1 FROM GamePredictPTS{0} WHERE game_id=@id AND team_id=@team_id);\r\nSET NOCOUNT OFF;", (object) mode);
      SqlParameter[] sqlParameterArray = new SqlParameter[6];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@team_id", (object) teamId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@pts1", (object) pts1);
      sqlParameterArray[2].DbType = DbType.Decimal;
      sqlParameterArray[3] = new SqlParameter("@pts2", (object) pts2);
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@pts3", (object) pts3);
      sqlParameterArray[4].DbType = DbType.Decimal;
      sqlParameterArray[5] = new SqlParameter("@pts4", (object) pts4);
      sqlParameterArray[5].DbType = DbType.Decimal;
      this._helper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public void SaveFourFactor(NBAGameStats team)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GameFourFactors (game_id,team_id,[Pace],[O_eFG],[O_TOV],[ORB],[O_FT_FGA],[ORtg],[D_eFG],[D_TOV],[DRB],[D_FT_FGA],[DRtg],[PTS],[Home]) \r\nSELECT @id,@team_id,@pace,@oefg,@otov,@orb,@oftfga,@ortg,@defg,@dtov,@drb,@dftfga,@drtg,@pts,@home\r\nWHERE NOT EXISTS (SELECT 1 FROM GameFourFactors WHERE game_id=@id AND team_id=@team_id);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[15];
      sqlParameterArray[0] = new SqlParameter("@id", (object) team.GameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@team_id", (object) team.TeamId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@pace", (object) team.Pace);
      sqlParameterArray[2].DbType = DbType.Decimal;
      sqlParameterArray[3] = new SqlParameter("@oefg", (object) team.O_eFG);
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@otov", (object) team.O_TOV);
      sqlParameterArray[4].DbType = DbType.Decimal;
      sqlParameterArray[5] = new SqlParameter("@orb", (object) team.ORB);
      sqlParameterArray[5].DbType = DbType.Decimal;
      sqlParameterArray[6] = new SqlParameter("@oftfga", (object) team.O_FT_FGA);
      sqlParameterArray[6].DbType = DbType.Decimal;
      sqlParameterArray[7] = new SqlParameter("@ortg", (object) team.ORtg);
      sqlParameterArray[7].DbType = DbType.Decimal;
      sqlParameterArray[8] = new SqlParameter("@defg", (object) team.D_eFG);
      sqlParameterArray[8].DbType = DbType.Decimal;
      sqlParameterArray[9] = new SqlParameter("@dtov", (object) team.D_TOV);
      sqlParameterArray[9].DbType = DbType.Decimal;
      sqlParameterArray[10] = new SqlParameter("@drb", (object) team.DRB);
      sqlParameterArray[10].DbType = DbType.Decimal;
      sqlParameterArray[11] = new SqlParameter("@dftfga", (object) team.D_FT_FGA);
      sqlParameterArray[11].DbType = DbType.Decimal;
      sqlParameterArray[12] = new SqlParameter("@drtg", (object) team.DRtg);
      sqlParameterArray[12].DbType = DbType.Decimal;
      sqlParameterArray[13] = new SqlParameter("@pts", (object) team.PTS);
      sqlParameterArray[13].DbType = DbType.Decimal;
      sqlParameterArray[14] = new SqlParameter("@home", (object) team.Home);
      sqlParameterArray[14].DbType = DbType.Int32;
      this._helper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public void SaveFourFactorByOpp(NBAGameStats team, int gameId, int pts, int type)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GameFourFactorsByOpp (game_id,team_id,type,[Pace],[O_eFG],[O_TOV],[ORB],[O_FT_FGA],[ORtg],[D_eFG],[D_TOV],[DRB],[D_FT_FGA],[DRtg],[PTS],[Home]) \r\nSELECT @id,@team_id,@type,@pace,@oefg,@otov,@orb,@oftfga,@ortg,@defg,@dtov,@drb,@dftfga,@drtg,@pts,@home\r\nWHERE NOT EXISTS (SELECT 1 FROM GameFourFactorsByOpp WHERE game_id=@id AND team_id=@team_id AND type=@type);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[16];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@team_id", (object) team.TeamId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@pace", (object) team.Pace);
      sqlParameterArray[2].DbType = DbType.Decimal;
      sqlParameterArray[3] = new SqlParameter("@oefg", (object) team.O_eFG);
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@otov", (object) team.O_TOV);
      sqlParameterArray[4].DbType = DbType.Decimal;
      sqlParameterArray[5] = new SqlParameter("@orb", (object) team.ORB);
      sqlParameterArray[5].DbType = DbType.Decimal;
      sqlParameterArray[6] = new SqlParameter("@oftfga", (object) team.O_FT_FGA);
      sqlParameterArray[6].DbType = DbType.Decimal;
      sqlParameterArray[7] = new SqlParameter("@ortg", (object) team.ORtg);
      sqlParameterArray[7].DbType = DbType.Decimal;
      sqlParameterArray[8] = new SqlParameter("@defg", (object) team.D_eFG);
      sqlParameterArray[8].DbType = DbType.Decimal;
      sqlParameterArray[9] = new SqlParameter("@dtov", (object) team.D_TOV);
      sqlParameterArray[9].DbType = DbType.Decimal;
      sqlParameterArray[10] = new SqlParameter("@drb", (object) team.DRB);
      sqlParameterArray[10].DbType = DbType.Decimal;
      sqlParameterArray[11] = new SqlParameter("@dftfga", (object) team.D_FT_FGA);
      sqlParameterArray[11].DbType = DbType.Decimal;
      sqlParameterArray[12] = new SqlParameter("@drtg", (object) team.DRtg);
      sqlParameterArray[12].DbType = DbType.Decimal;
      sqlParameterArray[13] = new SqlParameter("@pts", (object) pts);
      sqlParameterArray[13].DbType = DbType.Decimal;
      sqlParameterArray[14] = new SqlParameter("@home", (object) team.Home);
      sqlParameterArray[14].DbType = DbType.Int32;
      sqlParameterArray[15] = new SqlParameter("@type", (object) type);
      sqlParameterArray[15].DbType = DbType.Int32;
      this._helper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public void UpdatePredictPTS(int gameId, int teamId, double pts1_1, double pts2_1, double pts3_1, double pts4_1)
    {
      string cmdText = "SET NOCOUNT ON;\r\nUPDATE GamePredictPTS \r\nSET [pts1_1]=@pts1_1,[pts2_1]=@pts2_1,[pts3_1]=@pts3_1,[pts4_1]=@pts4_1\r\nWHERE game_id=@id AND team_id=@team_id;\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[6];
      sqlParameterArray[0] = new SqlParameter("@id", (object) gameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@team_id", (object) teamId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@pts1_1", (object) pts1_1);
      sqlParameterArray[2].DbType = DbType.Decimal;
      sqlParameterArray[3] = new SqlParameter("@pts2_1", (object) pts2_1);
      sqlParameterArray[3].DbType = DbType.Decimal;
      sqlParameterArray[4] = new SqlParameter("@pts3_1", (object) pts3_1);
      sqlParameterArray[4].DbType = DbType.Decimal;
      sqlParameterArray[5] = new SqlParameter("@pts4_1", (object) pts4_1);
      sqlParameterArray[5].DbType = DbType.Decimal;
      this._helper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public List<string> GetGameDates(string season)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select distinct convert(date, [time]) as [time] from GameInfo where id like '{0}%' and [time] < GETDATE() order by [time] desc", (object) season), new SqlParameter[0]))
      {
        List<string> list = new List<string>();
        while (dataReader.Read())
          list.Add(dataReader.GetDateTime(0).ToString("yyyy-MM-dd"));
        return list;
      }
    }

    public NBAGameInfo GetGameById(int gameId)
    {
      using (IDataReader reader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo g\r\nleft join TeamInfo a on g.away = a.id\r\nleft join TeamInfo h on g.home = h.id\r\nwhere g.id = '{0}'", (object) gameId), new SqlParameter[0]))
      {
        NBAGameInfo nbaGameInfo = new NBAGameInfo();
        while (reader.Read())
          nbaGameInfo = this.ReadGameInfo(reader);
        return nbaGameInfo;
      }
    }

    public List<NBAGameInfo> GetGamesByDate(string date)
    {
      using (IDataReader reader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo g\r\nleft join TeamInfo a on g.away = a.id\r\nleft join TeamInfo h on g.home = h.id\r\nwhere [time] = '{0}'", (object) date), new SqlParameter[0]))
      {
        List<NBAGameInfo> list = new List<NBAGameInfo>();
        while (reader.Read())
        {
          NBAGameInfo nbaGameInfo = this.ReadGameInfo(reader);
          list.Add(nbaGameInfo);
        }
        return list;
      }
    }

    public List<NBAGameInfo> GetGamesBySeason(string season)
    {
      using (IDataReader reader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select * from GameInfo g\r\nleft join TeamInfo a on g.away = a.id\r\nleft join TeamInfo h on g.home = h.id\r\nwhere g.id like '{0}%' \r\norder by g.id desc", (object) season), new SqlParameter[0]))
      {
        List<NBAGameInfo> list = new List<NBAGameInfo>();
        while (reader.Read())
        {
          NBAGameInfo nbaGameInfo = this.ReadGameInfo(reader);
          list.Add(nbaGameInfo);
        }
        return list;
      }
    }

    public List<NBAGamePredict> GetPredictions(string season, string month, string mode, string company)
    {
      SqlParameter[] sqlParameterArray = new SqlParameter[4];
      sqlParameterArray[0] = new SqlParameter("@season", (object) (season + "%"));
      sqlParameterArray[0].DbType = DbType.AnsiString;
      sqlParameterArray[1] = new SqlParameter("@month", (object) month);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@mode", (object) mode);
      sqlParameterArray[2].DbType = DbType.Int32;
      sqlParameterArray[3] = new SqlParameter("@company", (object) company);
      sqlParameterArray[3].DbType = DbType.Int32;
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.StoredProcedure, "dbo.getPredictPTS", sqlParameterArray))
      {
        List<NBAGamePredict> list = new List<NBAGamePredict>();
        while (dataReader.Read())
        {
          NBAGamePredict nbaGamePredict = new NBAGamePredict();
          nbaGamePredict.GameId = Convert.ToInt32(dataReader["id"]);
          nbaGamePredict.GameTime = Convert.ToDateTime(dataReader["time"]);
          string str1 = dataReader["l_p_h"].ToString();
          if (!string.IsNullOrWhiteSpace(str1) && str1.Trim() != "-")
            nbaGamePredict.LP = Convert.ToDecimal(str1);
          string str2 = dataReader["s_h"].ToString();
          if (!string.IsNullOrWhiteSpace(str2) && str2.Trim() != "-")
            nbaGamePredict.SP = Convert.ToDecimal(str2);
          nbaGamePredict.FirstPoint = dataReader["first_point"] is DBNull ? new Decimal(0) : Convert.ToDecimal(dataReader["first_point"]);
          nbaGamePredict.LastPoint = dataReader["last_point"] is DBNull ? new Decimal(0) : Convert.ToDecimal(dataReader["last_point"]);
          nbaGamePredict.HomeId = Convert.ToInt32(dataReader["hme_id"]);
          if (DBNull.Value != dataReader["pace1"])
          {
            nbaGamePredict.HomePace1 = Convert.ToDecimal(dataReader["pace1"]);
            nbaGamePredict.HomePTS1 = Convert.ToDecimal(dataReader["pts1"]);
            nbaGamePredict.HomePace1_1 = Convert.ToDecimal(dataReader["pace1_1"]);
            nbaGamePredict.HomePTS1_1 = Convert.ToDecimal(dataReader["pts1_1"]);
            nbaGamePredict.HomePace2 = Convert.ToDecimal(dataReader["pace2"]);
            nbaGamePredict.HomePTS2 = Convert.ToDecimal(dataReader["pts2"]);
            nbaGamePredict.HomePace2_1 = Convert.ToDecimal(dataReader["pace2_1"]);
            nbaGamePredict.HomePTS2_1 = Convert.ToDecimal(dataReader["pts2_1"]);
            nbaGamePredict.HomePace3 = Convert.ToDecimal(dataReader["pace3"]);
            nbaGamePredict.HomePTS3 = Convert.ToDecimal(dataReader["pts3"]);
            nbaGamePredict.HomePace3_1 = Convert.ToDecimal(dataReader["pace3_1"]);
            nbaGamePredict.HomePTS3_1 = Convert.ToDecimal(dataReader["pts3_1"]);
            nbaGamePredict.HomePace4 = Convert.ToDecimal(dataReader["pace4"]);
            nbaGamePredict.HomePTS4 = Convert.ToDecimal(dataReader["pts4"]);
            nbaGamePredict.HomePace4_1 = Convert.ToDecimal(dataReader["pace4_1"]);
            nbaGamePredict.HomePTS4_1 = Convert.ToDecimal(dataReader["pts4_1"]);
            nbaGamePredict.HomePTS = dataReader["pts"] is DBNull ? 0 : Convert.ToInt32(dataReader["pts"]);
          }
          nbaGamePredict.HomeName = dataReader["name"].ToString();
          nbaGamePredict.AwayId = Convert.ToInt32(dataReader["opp_id"]);
          if (DBNull.Value != dataReader["opp_p1"])
          {
            nbaGamePredict.AwayPace1 = Convert.ToDecimal(dataReader["opp_p1"]);
            nbaGamePredict.AwayPTS1 = Convert.ToDecimal(dataReader["opp_pts1"]);
            nbaGamePredict.AwayPace1_1 = Convert.ToDecimal(dataReader["opp_p1_1"]);
            nbaGamePredict.AwayPTS1_1 = Convert.ToDecimal(dataReader["opp_pts1_1"]);
            nbaGamePredict.AwayPace2 = Convert.ToDecimal(dataReader["opp_p2"]);
            nbaGamePredict.AwayPTS2 = Convert.ToDecimal(dataReader["opp_pts2"]);
            nbaGamePredict.AwayPace2_1 = Convert.ToDecimal(dataReader["opp_p2_1"]);
            nbaGamePredict.AwayPTS2_1 = Convert.ToDecimal(dataReader["opp_pts2_1"]);
            nbaGamePredict.AwayPace3 = Convert.ToDecimal(dataReader["opp_p3"]);
            nbaGamePredict.AwayPTS3 = Convert.ToDecimal(dataReader["opp_pts3"]);
            nbaGamePredict.AwayPace3_1 = Convert.ToDecimal(dataReader["opp_p3_1"]);
            nbaGamePredict.AwayPTS3_1 = Convert.ToDecimal(dataReader["opp_pts3_1"]);
            nbaGamePredict.AwayPace4 = Convert.ToDecimal(dataReader["opp_p4"]);
            nbaGamePredict.AwayPTS4 = Convert.ToDecimal(dataReader["opp_pts4"]);
            nbaGamePredict.AwayPace4_1 = Convert.ToDecimal(dataReader["opp_p4_1"]);
            nbaGamePredict.AwayPTS4_1 = Convert.ToDecimal(dataReader["opp_pts4_1"]);
            nbaGamePredict.AwayPTS = dataReader["opp_pts"] is DBNull ? 0 : Convert.ToInt32(dataReader["opp_pts"]);
          }
          nbaGamePredict.AwayName = dataReader["opp_name"].ToString();
          list.Add(nbaGamePredict);
        }
        return list;
      }
    }

    public List<NBAGamePredict> GetNewPredictions(string season, string month, string mode, string company)
    {
      SqlParameter[] sqlParameterArray = new SqlParameter[4];
      sqlParameterArray[0] = new SqlParameter("@season", (object) (season + "%"));
      sqlParameterArray[0].DbType = DbType.AnsiString;
      sqlParameterArray[1] = new SqlParameter("@month", (object) month);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@mode", (object) mode);
      sqlParameterArray[2].DbType = DbType.Int32;
      sqlParameterArray[3] = new SqlParameter("@company", (object) company);
      sqlParameterArray[3].DbType = DbType.Int32;
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.StoredProcedure, "dbo.getNewPredictPTS", sqlParameterArray))
      {
        List<NBAGamePredict> list = new List<NBAGamePredict>();
        while (dataReader.Read())
        {
          NBAGamePredict nbaGamePredict = new NBAGamePredict();
          nbaGamePredict.GameId = Convert.ToInt32(dataReader["id"]);
          nbaGamePredict.GameTime = Convert.ToDateTime(dataReader["time"]);
          string str1 = dataReader["l_p_h"].ToString();
          if (!string.IsNullOrWhiteSpace(str1) && str1.Trim() != "-")
            nbaGamePredict.LP = Convert.ToDecimal(str1);
          string str2 = dataReader["s_h"].ToString();
          if (!string.IsNullOrWhiteSpace(str2) && str2.Trim() != "-")
            nbaGamePredict.SP = Convert.ToDecimal(str2);
          nbaGamePredict.FirstPoint = dataReader["first_point"] is DBNull ? new Decimal(0) : Convert.ToDecimal(dataReader["first_point"]);
          nbaGamePredict.LastPoint = dataReader["last_point"] is DBNull ? new Decimal(0) : Convert.ToDecimal(dataReader["last_point"]);
          nbaGamePredict.HomeId = Convert.ToInt32(dataReader["hme_id"]);
          if (DBNull.Value != dataReader["pts1"])
          {
            nbaGamePredict.HomePTS1 = Convert.ToDecimal(dataReader["pts1"]);
            nbaGamePredict.HomePTS2 = Convert.ToDecimal(dataReader["pts2"]);
            nbaGamePredict.HomePTS3 = Convert.ToDecimal(dataReader["pts3"]);
            nbaGamePredict.HomePTS4 = Convert.ToDecimal(dataReader["pts4"]);
            nbaGamePredict.HomePTS = dataReader["pts"] is DBNull ? 0 : Convert.ToInt32(dataReader["pts"]);
          }
          nbaGamePredict.HomeName = dataReader["name"].ToString();
          nbaGamePredict.AwayId = Convert.ToInt32(dataReader["opp_id"]);
          if (DBNull.Value != dataReader["opp_pts1"])
          {
            nbaGamePredict.AwayPTS1 = Convert.ToDecimal(dataReader["opp_pts1"]);
            nbaGamePredict.AwayPTS2 = Convert.ToDecimal(dataReader["opp_pts2"]);
            nbaGamePredict.AwayPTS3 = Convert.ToDecimal(dataReader["opp_pts3"]);
            nbaGamePredict.AwayPTS4 = Convert.ToDecimal(dataReader["opp_pts4"]);
            nbaGamePredict.AwayPTS = dataReader["opp_pts"] is DBNull ? 0 : Convert.ToInt32(dataReader["opp_pts"]);
          }
          nbaGamePredict.AwayName = dataReader["opp_name"].ToString();
          list.Add(nbaGamePredict);
        }
        return list;
      }
    }

    public Dictionary<int, Tuple<int, Decimal>> GetYearRollOverPace(string year)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, "select * from TeamPaceRollOver where year=" + year, new SqlParameter[0]))
      {
        Dictionary<int, Tuple<int, Decimal>> dictionary = new Dictionary<int, Tuple<int, Decimal>>();
        while (dataReader.Read())
        {
          int int32 = dataReader.GetInt32(0);
          Decimal @decimal = dataReader.GetDecimal(2);
          dictionary.Add(int32, Tuple.Create<int, Decimal>(Convert.ToInt32(year), @decimal));
        }
        return dictionary;
      }
    }

    public Dictionary<int, Decimal> GetEachGamePace(string year)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select distinct game_id, Pace from GameFourFactors where game_id like '{0}%'", (object) year), new SqlParameter[0]))
      {
        Dictionary<int, Decimal> dictionary = new Dictionary<int, Decimal>();
        while (dataReader.Read())
        {
          int int32 = dataReader.GetInt32(0);
          Decimal @decimal = dataReader.GetDecimal(1);
          dictionary.Add(int32, @decimal);
        }
        return dictionary;
      }
    }

    public Dictionary<int, Tuple<Decimal, Decimal>> GetEachGameRating(string year)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select game_id, ORtg, DRtg from GameFourFactors where game_id like '{0}%' and Home=1", (object) year), new SqlParameter[0]))
      {
        Dictionary<int, Tuple<Decimal, Decimal>> dictionary = new Dictionary<int, Tuple<Decimal, Decimal>>();
        while (dataReader.Read())
        {
          int int32 = dataReader.GetInt32(0);
          Decimal decimal1 = dataReader.GetDecimal(1);
          Decimal decimal2 = dataReader.GetDecimal(2);
          dictionary.Add(int32, Tuple.Create<Decimal, Decimal>(decimal1, decimal2));
        }
        return dictionary;
      }
    }

    private NBAGameInfo ReadGameInfo(IDataReader reader)
    {
      NBAGameInfo nbaGameInfo = new NBAGameInfo();
      nbaGameInfo.GameId = reader.GetInt32(0);
      nbaGameInfo.AwayId = reader.GetInt32(2);
      nbaGameInfo.HomeId = reader.GetInt32(3);
      nbaGameInfo.GameTime = reader.GetDateTime(4);
      nbaGameInfo.HalfScore = reader.GetString(5);
      nbaGameInfo.FinalScore = reader.GetString(6);
      nbaGameInfo.AsiaTapeResult = reader.GetInt32(9);
      string string1 = reader.GetString(8);
      if (!string.IsNullOrWhiteSpace(string1) && string1.Trim() != "-")
        nbaGameInfo.AsiaTape = Convert.ToDecimal(string1);
      nbaGameInfo.ScoreTapeResult = reader.GetInt32(11);
      string string2 = reader.GetString(10);
      if (!string.IsNullOrWhiteSpace(string2) && string2.Trim() != "-")
        nbaGameInfo.ScoreTape = Convert.ToDecimal(string2);
      nbaGameInfo.AwayName = reader.GetString(14);
      nbaGameInfo.AwayEnName = reader.GetString(15);
      nbaGameInfo.AwayLeague = reader.GetInt32(16);
      nbaGameInfo.AwayAbbr = reader.GetString(17);
      nbaGameInfo.HomeName = reader.GetString(21);
      nbaGameInfo.HomeEnName = reader.GetString(22);
      nbaGameInfo.HomeLeague = reader.GetInt32(23);
      nbaGameInfo.HomeAbbr = reader.GetString(24);
      string[] strArray = nbaGameInfo.FinalScore.Split(new string[1]
      {
        "-"
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length == 2)
      {
        nbaGameInfo.AwayPTS = Convert.ToInt32(strArray[0]);
        nbaGameInfo.HomePTS = Convert.ToInt32(strArray[1]);
      }
      return nbaGameInfo;
    }

    public Dictionary<GameStats, NBAGameStats> GetGameStatsByTeam(int id, string year)
    {
      SqlParameter[] sqlParameterArray = new SqlParameter[2]
      {
        new SqlParameter("@team_id", (object) id),
        null
      };
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@year_filter", (object) year);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      using (IDataReader reader = (IDataReader) this._helper.ExecuteReader(CommandType.StoredProcedure, "dbo.getTeamStats", sqlParameterArray))
      {
        Dictionary<GameStats, NBAGameStats> dictionary = new Dictionary<GameStats, NBAGameStats>();
        NBAGameStats team1 = new NBAGameStats();
        NBAGameStats team2 = new NBAGameStats();
        dictionary.Add(GameStats.Team, team1);
        dictionary.Add(GameStats.Opponent, team2);
        while (reader.Read())
        {
          int int32 = reader.GetInt32(1);
          if (int32 == id)
          {
            team1.TeamId = int32;
            this.SumTeamStats(reader, team1);
          }
          else
          {
            team2.TeamId = int32;
            this.SumTeamStats(reader, team2);
          }
        }
        team1.FGP = team1.FGM / team1.FGA;
        team1.FG3P = team1.FG3M / team1.FG3A;
        team1.FTP = team1.FTM / team1.FTA;
        team2.FGP = team1.FGM / team1.FGA;
        team2.FG3P = team1.FG3M / team1.FG3A;
        team2.FTP = team1.FTM / team1.FTA;
        return dictionary;
      }
    }

    public List<NBAGameStats> GetAllGameStats(string year, string date)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select g.*, t.*, Home=(case when gi.id IS null then 0 else 1 end) \r\nfrom GameStats g \r\nleft join TeamInfo t on g.team_id=t.id \r\nleft join GameInfo gi on g.game_id=gi.id and g.team_id = gi.home \r\nwhere game_id in(\r\n\tselect id from GameInfo where id like '{0}%' and time<'{1}'\r\n)", (object) year, (object) date), new SqlParameter[0]))
      {
        List<NBAGameStats> list = new List<NBAGameStats>();
        while (dataReader.Read())
        {
          NBAGameStats nbaGameStats = new NBAGameStats();
          nbaGameStats.GameId = dataReader.GetInt32(0);
          nbaGameStats.TeamId = dataReader.GetInt32(1);
          nbaGameStats.FGM = (double) dataReader.GetInt32(2);
          nbaGameStats.FGA = (double) dataReader.GetInt32(3);
          nbaGameStats.FGP = nbaGameStats.FGM / nbaGameStats.FGA;
          nbaGameStats.FG3M = (double) dataReader.GetInt32(5);
          nbaGameStats.FG3A = (double) dataReader.GetInt32(6);
          nbaGameStats.FG3P = nbaGameStats.FG3M / nbaGameStats.FG3A;
          nbaGameStats.FTM = (double) dataReader.GetInt32(8);
          nbaGameStats.FTA = (double) dataReader.GetInt32(9);
          nbaGameStats.FTP = nbaGameStats.FTM / nbaGameStats.FTA;
          nbaGameStats.OREB = (double) dataReader.GetInt32(11);
          nbaGameStats.DREB = (double) dataReader.GetInt32(12);
          nbaGameStats.REB = (double) dataReader.GetInt32(13);
          nbaGameStats.AST = (double) dataReader.GetInt32(14);
          nbaGameStats.STL = (double) dataReader.GetInt32(15);
          nbaGameStats.BLK = (double) dataReader.GetInt32(16);
          nbaGameStats.TOV = (double) dataReader.GetInt32(17);
          nbaGameStats.PF = (double) dataReader.GetInt32(18);
          nbaGameStats.PTS = (double) dataReader.GetInt32(27);
          nbaGameStats.MP = (double) dataReader.GetInt32(28);
          nbaGameStats.TeamName = dataReader.GetString(35);
          nbaGameStats.Home = dataReader.GetInt32(29);
          list.Add(nbaGameStats);
        }
        return list;
      }
    }

    public List<NBAGameStats> GetFourFactors(string year, string date)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select * from GameFourFactors g\r\nleft join TeamInfo t on g.team_id=t.id \r\nwhere game_id in(\r\n\tselect id from GameInfo where id like '{0}%' and time<'{1}'\r\n)\r\n", (object) year, (object) date), new SqlParameter[0]))
      {
        List<NBAGameStats> list = new List<NBAGameStats>();
        while (dataReader.Read())
          list.Add(new NBAGameStats()
          {
            GameId = Convert.ToInt32(dataReader["game_id"]),
            TeamId = Convert.ToInt32(dataReader["team_id"]),
            Pace = Convert.ToDouble(dataReader["Pace"]),
            O_eFG = Convert.ToDouble(dataReader["O_eFG"]),
            O_TOV = Convert.ToDouble(dataReader["O_TOV"]),
            ORB = Convert.ToDouble(dataReader["ORB"]),
            O_FT_FGA = Convert.ToDouble(dataReader["O_FT_FGA"]),
            ORtg = Convert.ToDouble(dataReader["ORtg"]),
            D_eFG = Convert.ToDouble(dataReader["D_eFG"]),
            D_TOV = Convert.ToDouble(dataReader["D_TOV"]),
            DRB = Convert.ToDouble(dataReader["DRB"]),
            D_FT_FGA = Convert.ToDouble(dataReader["D_FT_FGA"]),
            DRtg = Convert.ToDouble(dataReader["DRtg"]),
            PTS = Convert.ToDouble(dataReader["PTS"]),
            TeamName = dataReader["abbr"].ToString(),
            Home = Convert.ToInt32(dataReader["Home"])
          });
        return list;
      }
    }

    public List<NBAGameStats> GetFourFactorsByOpp(string year, string date)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select * from GameFourFactorsByOpp g\r\nleft join TeamInfo t on g.team_id=t.id \r\nwhere game_id in(\r\n\tselect id from GameInfo where id like '{0}%' and time<'{1}'\r\n)\r\n", (object) year, (object) date), new SqlParameter[0]))
      {
        List<NBAGameStats> list = new List<NBAGameStats>();
        while (dataReader.Read())
          list.Add(new NBAGameStats()
          {
            GameId = Convert.ToInt32(dataReader["game_id"]),
            TeamId = Convert.ToInt32(dataReader["team_id"]),
            Pace = Convert.ToDouble(dataReader["Pace"]),
            O_eFG = Convert.ToDouble(dataReader["O_eFG"]),
            O_TOV = Convert.ToDouble(dataReader["O_TOV"]),
            ORB = Convert.ToDouble(dataReader["ORB"]),
            O_FT_FGA = Convert.ToDouble(dataReader["O_FT_FGA"]),
            ORtg = Convert.ToDouble(dataReader["ORtg"]),
            D_eFG = Convert.ToDouble(dataReader["D_eFG"]),
            D_TOV = Convert.ToDouble(dataReader["D_TOV"]),
            DRB = Convert.ToDouble(dataReader["DRB"]),
            D_FT_FGA = Convert.ToDouble(dataReader["D_FT_FGA"]),
            DRtg = Convert.ToDouble(dataReader["DRtg"]),
            PTS = Convert.ToDouble(dataReader["PTS"]),
            TeamName = dataReader["abbr"].ToString(),
            Home = Convert.ToInt32(dataReader["Home"]),
            Type = Convert.ToInt32(dataReader["type"])
          });
        return list;
      }
    }

    public DataTable GetHandicaps(string gameId, string type)
    {
      return this._helper.ExecuteDataTable(!(type == "0") ? string.Format("select * from ScoreHandicap where game_id={0} order by date", (object) gameId) : string.Format("select * from AsiaHandicap where game_id={0} order by date", (object) gameId), (SqlParameter[]) null);
    }

    public DataTable GetLatestPrediction(DateTime gameTime, string teamId)
    {
      return this._helper.ExecuteDataTable(string.Format("select top 1 * from dbo.GamePredictPTS pts \r\nleft join GameInfo g on pts.game_id=g.id\r\nwhere g.time <= '{1}'\r\nand pts.team_id={0}\r\norder by g.time desc", (object) teamId, (object) gameTime.ToString("yyyy-MM-dd")), (SqlParameter[]) null);
    }

    private void SumTeamStats(IDataReader reader, NBAGameStats team)
    {
      team.GameId = reader.GetInt32(0);
      ++team.GamePlayed;
      team.FGM += (double) reader.GetInt32(2);
      team.FGA += (double) reader.GetInt32(3);
      team.FG3M += (double) reader.GetInt32(5);
      team.FG3A += (double) reader.GetInt32(6);
      team.FTM += (double) reader.GetInt32(8);
      team.FTA += (double) reader.GetInt32(9);
      team.OREB += (double) reader.GetInt32(11);
      team.DREB += (double) reader.GetInt32(12);
      team.REB += (double) reader.GetInt32(13);
      team.AST += (double) reader.GetInt32(14);
      team.STL += (double) reader.GetInt32(15);
      team.BLK += (double) reader.GetInt32(16);
      team.TOV += (double) reader.GetInt32(17);
      team.PF += (double) reader.GetInt32(18);
      team.PTS += (double) reader.GetInt32(27);
      team.MP += (double) reader.GetInt32(28);
    }

    public DataTable GetSumStatistics(string teamId, IEnumerable<int> gameIds)
    {
      return this._helper.ExecuteDataTable(string.Format("select SUM(FGM),SUM(FGA),SUM([3FGM]),SUM([3FGA]),SUM(FTM),SUM(FTA),SUM(OREB),SUM(DREB),SUM(REB),SUM(AST),SUM(STL),SUM(BLK),SUM(TOV),SUM(PF),SUM(PTS),SUM(MP),COUNT(*) \r\nfrom dbo.GameStats\r\nwhere game_id in ({1})\r\nand team_id={0}", (object) teamId, (object) string.Join<int>(",", gameIds)));
    }

    public DataTable GetOppoSumStatistics(string teamId, IEnumerable<int> gameIds)
    {
      return this._helper.ExecuteDataTable(string.Format("select SUM(FGM),SUM(FGA),SUM([3FGM]),SUM([3FGA]),SUM(FTM),SUM(FTA),SUM(OREB),SUM(DREB),SUM(REB),SUM(AST),SUM(STL),SUM(BLK),SUM(TOV),SUM(PF),SUM(PTS),SUM(MP),COUNT(*) \r\nfrom dbo.GameStats\r\nwhere game_id in ({1})\r\nand team_id<>{0}", (object) teamId, (object) string.Join<int>(",", gameIds)));
    }

    public List<int> GetPreviousGames(string teamId, int year, DateTime gameTime, GamePos pos)
    {
      string str = string.Format("select [id] from GameInfo\r\nwhere [time] > '{0}-10-1' \r\nand [time] < '{1}'\r\nand [type] = 2", (object) year, (object) gameTime);
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, pos != GamePos.Home ? (pos != GamePos.Away ? str + string.Format("and (away={0} or home={0}) order by [id] desc", (object) teamId) : str + string.Format("and away={0} order by [id] desc", (object) teamId)) : str + string.Format("and home={0} order by [id] desc", (object) teamId), new SqlParameter[0]))
      {
        List<int> list = new List<int>();
        while (dataReader.Read())
          list.Add(dataReader.GetInt32(0));
        return list;
      }
    }

    public NBATeamInfo GetTeamInfoById(string teamId)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select * from TeamInfo where id={0}", (object) teamId), new SqlParameter[0]))
      {
        NBATeamInfo nbaTeamInfo = new NBATeamInfo();
        while (dataReader.Read())
        {
          nbaTeamInfo.Id = dataReader.GetInt32(0);
          nbaTeamInfo.Name = dataReader.GetString(2);
          nbaTeamInfo.EnName = dataReader.GetString(3);
          nbaTeamInfo.Division = dataReader.GetInt32(4);
          nbaTeamInfo.Abbreviate = dataReader.GetString(5);
        }
        return nbaTeamInfo;
      }
    }

    public List<NBATeamInfo> GetAllTeamInfos()
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, "select * from TeamInfo order by name", new SqlParameter[0]))
      {
        List<NBATeamInfo> list = new List<NBATeamInfo>();
        while (dataReader.Read())
          list.Add(new NBATeamInfo()
          {
            Id = dataReader.GetInt32(0),
            Name = dataReader.GetString(2),
            EnName = dataReader.GetString(3),
            Division = dataReader.GetInt32(4),
            Abbreviate = dataReader.GetString(5)
          });
        return list;
      }
    }

    public NBATeamInfo GetTeamInfoByAbbr(string abbr)
    {
      using (IDataReader dataReader = (IDataReader) this._helper.ExecuteReader(CommandType.Text, string.Format("select * from TeamInfo where abbr='{0}'", (object) abbr), new SqlParameter[0]))
      {
        NBATeamInfo nbaTeamInfo = new NBATeamInfo();
        while (dataReader.Read())
        {
          nbaTeamInfo.Id = dataReader.GetInt32(0);
          nbaTeamInfo.Name = dataReader.GetString(2);
          nbaTeamInfo.EnName = dataReader.GetString(3);
          nbaTeamInfo.Division = dataReader.GetInt32(4);
          nbaTeamInfo.Abbreviate = dataReader.GetString(5);
        }
        return nbaTeamInfo;
      }
    }
  }
}
