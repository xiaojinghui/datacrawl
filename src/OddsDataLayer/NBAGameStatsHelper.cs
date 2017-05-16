// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.NBAGameStatsHelper
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OddsDataLayer
{
  public class NBAGameStatsHelper
  {
    private DataHandlerNBA handler = (DataHandlerNBA) null;

    public NBAGameStatsHelper()
    {
      this.handler = new DataHandlerNBA();
    }

    public string GetJsonString(string awayId, string homeId, int year, DateTime gameTime)
    {
      Dictionary<int, List<NBAGameStats>> predictData = this.GetPredictData(awayId, homeId, year, gameTime, false);
      string[] strArray = new string[6]
      {
        "Last 4 Games at Home/Away",
        "Last 4 Games",
        "Last 7 Games",
        "Last 10 Games",
        "去年至今客场/主场",
        "去年至今所有场次"
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[");
      foreach (KeyValuePair<int, List<NBAGameStats>> keyValuePair in (IEnumerable<KeyValuePair<int, List<NBAGameStats>>>) Enumerable.OrderBy<KeyValuePair<int, List<NBAGameStats>>, int>((IEnumerable<KeyValuePair<int, List<NBAGameStats>>>) predictData, (Func<KeyValuePair<int, List<NBAGameStats>>, int>) (d => d.Key)))
      {
        string str = JsonConvert.SerializeObject((object) keyValuePair.Value);
        stringBuilder.AppendFormat("{{\"name\":\"{1}\", \"stats\": {0}}},", (object) str, (object) strArray[keyValuePair.Key]);
      }
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }

    public void ExportFourFactorsPerGameToCSV(string date)
    {
      this.ExportFourFactorsPerGameToCSV(date, false);
    }

    public void ExportFourFactorsPerGameToCSV(string date, bool fromDB)
    {
      string[] strArray = new string[15]
      {
        "Team",
        "PTS",
        "Home",
        "LP",
        "Pace_Calc",
        "O_eFG",
        "O_TOV",
        "ORB",
        "O_FT_FGA",
        "ORtg",
        "D_eFG",
        "D_TOV",
        "DRB",
        "D_FT_FGA",
        "DRtg"
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Join(",", strArray));
      foreach (IGrouping<int, NBAGameStats> grouping in Enumerable.GroupBy<NBAGameStats, int>(!fromDB ? (IEnumerable<NBAGameStats>) this.handler.GetAllGameStats("214", date) : (IEnumerable<NBAGameStats>) this.handler.GetFourFactors("214", date), (Func<NBAGameStats, int>) (g => g.GameId)))
      {
        NBAGameStats team = (NBAGameStats) null;
        NBAGameStats opponent = (NBAGameStats) null;
        foreach (NBAGameStats nbaGameStats in (IEnumerable<NBAGameStats>) grouping)
        {
          if (team == null)
            team = nbaGameStats;
          else if (opponent == null)
            opponent = nbaGameStats;
        }
        if (!fromDB)
          this.CaclulateFourFactors(team, opponent);
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", (object) opponent.TeamName, (object) opponent.PTS, (object) opponent.Home, (object) (opponent.PTS - team.PTS), (object) opponent.Pace, (object) team.O_eFG, (object) team.O_TOV, (object) team.ORB, (object) team.O_FT_FGA, (object) team.ORtg, (object) team.D_eFG, (object) team.D_TOV, (object) team.DRB, (object) team.D_FT_FGA, (object) team.DRtg);
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", (object) team.TeamName, (object) team.PTS, (object) team.Home, (object) (team.PTS - opponent.PTS), (object) team.Pace, (object) opponent.O_eFG, (object) opponent.O_TOV, (object) opponent.ORB, (object) opponent.O_FT_FGA, (object) opponent.ORtg, (object) opponent.D_eFG, (object) opponent.D_TOV, (object) opponent.DRB, (object) opponent.D_FT_FGA, (object) opponent.DRtg);
      }
      File.WriteAllText("D:\\Derek\\NBA\\NBA_FourFactors_PerGame.csv", stringBuilder.ToString());
    }

    public void ExportFourFactorsPerTeamToCSV(string date, bool fromDB)
    {
      string[] strArray = new string[16]
      {
        "Team",
        "PTS",
        "Home",
        "LP",
        "Pace_Calc",
        "O_eFG",
        "O_TOV",
        "ORB",
        "O_FT_FGA",
        "ORtg",
        "D_eFG",
        "D_TOV",
        "DRB",
        "D_FT_FGA",
        "DRtg",
        "Type"
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Join(",", strArray));
      if (fromDB)
      {
        //foreach (IGrouping<\u003C\u003Ef__AnonymousType1<int, int>, NBAGameStats> grouping in Enumerable.GroupBy((IEnumerable<NBAGameStats>) this.handler.GetFourFactorsByOpp("214", date), g =>
        //{
        //  var fAnonymousType1 = new
        //  {
        //    GameId = g.GameId,
        //    Type = g.Type
        //  };
        //  return fAnonymousType1;
        //}))
        //{
        //  NBAGameStats nbaGameStats1 = (NBAGameStats) null;
        //  NBAGameStats nbaGameStats2 = (NBAGameStats) null;
        //  foreach (NBAGameStats nbaGameStats3 in (IEnumerable<NBAGameStats>) grouping)
        //  {
        //    if (nbaGameStats1 == null)
        //      nbaGameStats1 = nbaGameStats3;
        //    else if (nbaGameStats2 == null)
        //      nbaGameStats2 = nbaGameStats3;
        //  }
        //  stringBuilder.AppendLine();
        //  stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", (object) nbaGameStats2.TeamName, (object) nbaGameStats2.PTS, (object) nbaGameStats2.Home, (object) (nbaGameStats2.PTS - nbaGameStats1.PTS), (object) nbaGameStats1.Pace, (object) nbaGameStats1.O_eFG, (object) nbaGameStats1.O_TOV, (object) nbaGameStats1.ORB, (object) nbaGameStats1.O_FT_FGA, (object) nbaGameStats1.ORtg, (object) nbaGameStats1.D_eFG, (object) nbaGameStats1.D_TOV, (object) nbaGameStats1.DRB, (object) nbaGameStats1.D_FT_FGA, (object) nbaGameStats1.DRtg, (object) nbaGameStats2.Type);
        //  stringBuilder.AppendLine();
        //  stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", (object) nbaGameStats1.TeamName, (object) nbaGameStats1.PTS, (object) nbaGameStats1.Home, (object) (nbaGameStats1.PTS - nbaGameStats2.PTS), (object) nbaGameStats2.Pace, (object) nbaGameStats2.O_eFG, (object) nbaGameStats2.O_TOV, (object) nbaGameStats2.ORB, (object) nbaGameStats2.O_FT_FGA, (object) nbaGameStats2.ORtg, (object) nbaGameStats2.D_eFG, (object) nbaGameStats2.D_TOV, (object) nbaGameStats2.DRB, (object) nbaGameStats2.D_FT_FGA, (object) nbaGameStats2.DRtg, (object) nbaGameStats1.Type);
        //}
      }
      else
      {
        List<NBAGameInfo> gamesByDate = this.handler.GetGamesByDate(date);
        Convert.ToDateTime(date);
        foreach (NBAGameInfo nbaGameInfo in gamesByDate)
        {
          foreach (KeyValuePair<int, List<NBAGameStats>> keyValuePair in this.GetPredictData(nbaGameInfo.AwayId.ToString(), nbaGameInfo.HomeId.ToString(), 2014, nbaGameInfo.GameTime, true))
          {
            if (keyValuePair.Key != 3 && keyValuePair.Value.Count == 2)
            {
              NBAGameStats nbaGameStats1 = keyValuePair.Value[0];
              NBAGameStats nbaGameStats2 = keyValuePair.Value[1];
              stringBuilder.AppendLine();
              stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", (object) nbaGameStats2.TeamName, (object) nbaGameInfo.AwayPTS, (object) nbaGameStats2.Home, (object) (nbaGameStats2.PTS - nbaGameStats1.PTS), (object) nbaGameStats1.Pace, (object) nbaGameStats1.O_eFG, (object) nbaGameStats1.O_TOV, (object) nbaGameStats1.ORB, (object) nbaGameStats1.O_FT_FGA, (object) nbaGameStats1.ORtg, (object) nbaGameStats1.D_eFG, (object) nbaGameStats1.D_TOV, (object) nbaGameStats1.DRB, (object) nbaGameStats1.D_FT_FGA, (object) nbaGameStats1.DRtg, (object) keyValuePair.Key);
              stringBuilder.AppendLine();
              stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", (object) nbaGameStats1.TeamName, (object) nbaGameInfo.HomePTS, (object) nbaGameStats1.Home, (object) (nbaGameStats1.PTS - nbaGameStats2.PTS), (object) nbaGameStats2.Pace, (object) nbaGameStats2.O_eFG, (object) nbaGameStats2.O_TOV, (object) nbaGameStats2.ORB, (object) nbaGameStats2.O_FT_FGA, (object) nbaGameStats2.ORtg, (object) nbaGameStats2.D_eFG, (object) nbaGameStats2.D_TOV, (object) nbaGameStats2.DRB, (object) nbaGameStats2.D_FT_FGA, (object) nbaGameStats2.DRtg, (object) keyValuePair.Key);
            }
          }
        }
      }
      File.WriteAllText("D:\\Derek\\NBA\\NBA_FourFactors_PerTeam.csv", stringBuilder.ToString());
    }

    public void ExportPredictionToCSV(string date)
    {
      string[] strArray1 = new string[8]
      {
        "Team",
        "Score",
        "AisaTape",
        "ScoreTape",
        "PTS_6",
        "PTS_6M",
        "PTS_10",
        "PTS_10M"
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Join(",", strArray1));
      List<NBAGameInfo> gamesByDate = this.handler.GetGamesByDate(date);
      DateTime gameTime = Convert.ToDateTime(date);
      foreach (NBAGameInfo nbaGameInfo in gamesByDate)
      {
        Dictionary<int, List<NBAGameStats>> predictData = this.GetPredictData(nbaGameInfo.AwayId.ToString(), nbaGameInfo.HomeId.ToString(), 2014, gameTime, false);
        List<NBAGameStats> list1 = predictData[0];
        List<NBAGameStats> list2 = predictData[1];
        string[] strArray2 = nbaGameInfo.FinalScore.Split(new string[1]
        {
          "-"
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < list1.Count; ++index)
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}", (object) list1[index].TeamName, list1[index].Home == 1 ? (object) strArray2[1] : (object) strArray2[0], (object) (list1[index].Home == 1 ? nbaGameInfo.AsiaTape : -nbaGameInfo.AsiaTape), (object) nbaGameInfo.ScoreTape, (object) list1[index].PTS, (object) list1[index].Mix_PTS, (object) list2[index].PTS, (object) list2[index].Mix_PTS);
        }
      }
      File.WriteAllText(string.Format("D:\\Derek\\NBA\\Predict_{0}.csv", (object) gameTime.ToString("yyyy-MM-dd")), stringBuilder.ToString(), Encoding.GetEncoding("GB2312"));
    }

    public void ExportPaceBeforeGamePlay()
    {
      this.ExportPaceBeforeGamePlay(10, GamePos.All, DateTime.MinValue, string.Empty, string.Empty);
    }

    public void ExportPaceBeforeGamePlay(int preGameCount, GamePos pos, DateTime gameTime, string awayId, string homeId)
    {
      string[] strArray = new string[5]
      {
        "Team_Id",
        "Home",
        "Pace",
        "Opp_Pace",
        "Act_Pace"
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Join(",", strArray));
      IEnumerable<NBAGameInfo> source = Enumerable.Where<NBAGameInfo>((IEnumerable<NBAGameInfo>) this.handler.GetGamesBySeason("214"), (Func<NBAGameInfo, bool>) (g => !string.IsNullOrWhiteSpace(g.FinalScore)));
      if (gameTime != DateTime.MinValue)
        source = Enumerable.Where<NBAGameInfo>(source, (Func<NBAGameInfo, bool>) (g => g.GameTime < gameTime));
      if (!string.IsNullOrEmpty(awayId) && !string.IsNullOrEmpty(homeId))
        source = Enumerable.Where<NBAGameInfo>(source, (Func<NBAGameInfo, bool>) (g => g.AwayId == Convert.ToInt32(awayId) || g.HomeId == Convert.ToInt32(awayId) || (g.AwayId == Convert.ToInt32(homeId) || g.HomeId == Convert.ToInt32(homeId))));
      Dictionary<int, Decimal> eachGamePace = this.handler.GetEachGamePace("214");
      foreach (NBAGameInfo nbaGameInfo in (IEnumerable<NBAGameInfo>) Enumerable.OrderBy<NBAGameInfo, int>(source, (Func<NBAGameInfo, int>) (g => g.GameId)))
      {
        List<NBAGameStats> list = this.PrepareTeamsData(nbaGameInfo.AwayId.ToString(), nbaGameInfo.AwayEnName, nbaGameInfo.HomeId.ToString(), nbaGameInfo.HomeEnName, 2014, nbaGameInfo.GameTime, pos, preGameCount);
        if (list.Count != 0 && list.Count != 1)
        {
          Decimal num1 = list[0].Home == 0 ? Convert.ToDecimal(list[0].Pace) : Convert.ToDecimal(list[1].Pace);
          Decimal num2 = list[0].Home == 0 ? Convert.ToDecimal(list[1].Pace) : Convert.ToDecimal(list[0].Pace);
          stringBuilder.AppendLine();
          stringBuilder.AppendFormat("{0},{1},{2},{3},{4}", (object) nbaGameInfo.HomeAbbr, (object) 1, (object) num2, (object) num1, (object) eachGamePace[nbaGameInfo.GameId]);
          stringBuilder.AppendLine();
          stringBuilder.AppendFormat("{0},{1},{2},{3},{4}", (object) nbaGameInfo.AwayAbbr, (object) 0, (object) num1, (object) num2, (object) eachGamePace[nbaGameInfo.GameId]);
        }
      }
      if (pos == GamePos.None)
        preGameCount = 5;
      File.WriteAllText(string.Format("D:\\Derek\\NBA\\Paces{0}.csv", (object) preGameCount), stringBuilder.ToString(), Encoding.GetEncoding("GB2312"));
    }

    public void ExportRatingBeforeGamePlay()
    {
      this.ExportRatingBeforeGamePlay(6, GamePos.All, DateTime.MinValue, string.Empty, string.Empty);
    }

    public void ExportRatingBeforeGamePlay(int preGameCount, GamePos pos, DateTime gameTime, string awayId, string homeId)
    {
      string[] strArray = new string[7]
      {
        "Team_Id",
        "ORtg",
        "DRtg",
        "Opp_ORtg",
        "Opp_DRtg",
        "Act_ORtg",
        "Home"
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Join(",", strArray));
      IEnumerable<NBAGameInfo> source = Enumerable.Where<NBAGameInfo>((IEnumerable<NBAGameInfo>) this.handler.GetGamesBySeason("214"), (Func<NBAGameInfo, bool>) (g => !string.IsNullOrWhiteSpace(g.FinalScore)));
      if (gameTime != DateTime.MinValue)
        source = Enumerable.Where<NBAGameInfo>(source, (Func<NBAGameInfo, bool>) (g => g.GameTime < gameTime));
      if (!string.IsNullOrEmpty(awayId) && !string.IsNullOrEmpty(homeId))
        source = Enumerable.Where<NBAGameInfo>(source, (Func<NBAGameInfo, bool>) (g => g.AwayId == Convert.ToInt32(awayId) || g.HomeId == Convert.ToInt32(awayId) || (g.AwayId == Convert.ToInt32(homeId) || g.HomeId == Convert.ToInt32(homeId))));
      Dictionary<int, Tuple<Decimal, Decimal>> eachGameRating = this.handler.GetEachGameRating("214");
      foreach (NBAGameInfo nbaGameInfo in (IEnumerable<NBAGameInfo>) Enumerable.OrderBy<NBAGameInfo, int>(source, (Func<NBAGameInfo, int>) (g => g.GameId)))
      {
        List<NBAGameStats> list = this.PrepareTeamsData(nbaGameInfo.AwayId.ToString(), nbaGameInfo.AwayEnName, nbaGameInfo.HomeId.ToString(), nbaGameInfo.HomeEnName, 2014, nbaGameInfo.GameTime, pos, preGameCount);
        if (list.Count != 0 && list.Count != 1)
        {
          NBAGameStats nbaGameStats1 = Enumerable.FirstOrDefault<NBAGameStats>((IEnumerable<NBAGameStats>) list, (Func<NBAGameStats, bool>) (g => g.Home == 1));
          NBAGameStats nbaGameStats2 = Enumerable.FirstOrDefault<NBAGameStats>((IEnumerable<NBAGameStats>) list, (Func<NBAGameStats, bool>) (g => g.Home == 0));
          stringBuilder.AppendLine();
          stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", (object) nbaGameInfo.HomeAbbr, (object) nbaGameStats1.ORtg, (object) nbaGameStats1.DRtg, (object) nbaGameStats2.ORtg, (object) nbaGameStats2.DRtg, (object) eachGameRating[nbaGameInfo.GameId].Item1, (object) 1);
          stringBuilder.AppendLine();
          stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", (object) nbaGameInfo.AwayAbbr, (object) nbaGameStats2.ORtg, (object) nbaGameStats2.DRtg, (object) nbaGameStats1.ORtg, (object) nbaGameStats1.DRtg, (object) eachGameRating[nbaGameInfo.GameId].Item2, (object) 0);
        }
      }
      if (pos == GamePos.None)
        preGameCount = 5;
      File.WriteAllText(string.Format("D:\\Derek\\NBA\\Ratings{0}.csv", (object) preGameCount), stringBuilder.ToString(), Encoding.GetEncoding("GB2312"));
    }

    public void SavePredictionToDB(string date)
    {
      List<NBAGameInfo> gamesByDate = this.handler.GetGamesByDate(date);
      DateTime dateTime = Convert.ToDateTime(date);
      foreach (NBAGameInfo nbaGameInfo in gamesByDate)
      {
        int num1 = nbaGameInfo.AwayId;
        string awayId = num1.ToString();
        num1 = nbaGameInfo.HomeId;
        string homeId = num1.ToString();
        int year = 2014;
        DateTime gameTime = dateTime;
        int num2 = 0;
        Dictionary<int, List<NBAGameStats>> predictData = this.GetPredictData(awayId, homeId, year, gameTime, num2 != 0);
        List<NBAGameStats> list1 = (List<NBAGameStats>) null;
        List<NBAGameStats> list2 = (List<NBAGameStats>) null;
        List<NBAGameStats> list3 = (List<NBAGameStats>) null;
        List<NBAGameStats> list4 = (List<NBAGameStats>) null;
        predictData.TryGetValue(0, out list1);
        predictData.TryGetValue(1, out list2);
        predictData.TryGetValue(2, out list3);
        predictData.TryGetValue(3, out list4);
        List<NBAGameStats> list5 = (list1 == null ? list2 : list1) ?? (list3 == null ? list4 : list3);
        for (int index = 0; index < 2; ++index)
          this.handler.UpdatePredictPTS(nbaGameInfo.GameId, list5[index].TeamId, list1[index].Pred_PTS2, list2[index].Pred_PTS2, list3[index].Pred_PTS2, list4[index].Pred_PTS2);
      }
    }

    public void SaveNewPredictionToDB(string date)
    {
      List<NBAGameInfo> gamesByDate = this.handler.GetGamesByDate(date);
      DateTime dateTime = Convert.ToDateTime(date);
      foreach (NBAGameInfo nbaGameInfo in gamesByDate)
      {
        int num1 = nbaGameInfo.AwayId;
        string awayId = num1.ToString();
        num1 = nbaGameInfo.HomeId;
        string homeId = num1.ToString();
        int year = 2014;
        DateTime gameTime = dateTime;
        int num2 = 0;
        Dictionary<int, List<NBAGameStats>> predictData = this.GetPredictData(awayId, homeId, year, gameTime, num2 != 0);
        List<NBAGameStats> list1 = (List<NBAGameStats>) null;
        List<NBAGameStats> list2 = (List<NBAGameStats>) null;
        List<NBAGameStats> list3 = (List<NBAGameStats>) null;
        List<NBAGameStats> list4 = (List<NBAGameStats>) null;
        predictData.TryGetValue(1, out list1);
        predictData.TryGetValue(2, out list2);
        predictData.TryGetValue(3, out list3);
        predictData.TryGetValue(5, out list4);
        List<NBAGameStats> list5 = (list1 == null ? list2 : list1) ?? (list3 == null ? list4 : list3);
        for (int index = 0; index < 2; ++index)
          this.handler.SaveNewPredictPTS(nbaGameInfo.GameId, list5[index].TeamId, list1[index].Pred_PTS, list2[index].Pred_PTS, list3[index].Pred_PTS, list4[index].Pred_PTS, 6);
      }
    }

    public void SaveFourFactorPerGameToDB(string date)
    {
      this.SaveFourFactorPerGameToDB(DateTime.Parse(date));
    }

    public void SaveFourFactorPerGameToDB(DateTime date)
    {
      foreach (IGrouping<int, NBAGameStats> grouping in Enumerable.GroupBy<NBAGameStats, int>((IEnumerable<NBAGameStats>) this.handler.GetAllGameStats("214", date.ToString("yyyy-MM-dd")), (Func<NBAGameStats, int>) (g => g.GameId)))
      {
        NBAGameStats team = (NBAGameStats) null;
        NBAGameStats nbaGameStats1 = (NBAGameStats) null;
        foreach (NBAGameStats nbaGameStats2 in (IEnumerable<NBAGameStats>) grouping)
        {
          if (team == null)
            team = nbaGameStats2;
          else if (nbaGameStats1 == null)
            nbaGameStats1 = nbaGameStats2;
        }
        this.CaclulateFourFactors(team, nbaGameStats1);
        this.handler.SaveFourFactor(team);
        this.handler.SaveFourFactor(nbaGameStats1);
      }
    }

    public void SaveFourFactorPerTeamToDB(string date)
    {
      this.SaveFourFactorPerTeamToDB(DateTime.Parse(date));
    }

    public void SaveFourFactorPerTeamToDB(DateTime date)
    {
      foreach (NBAGameInfo nbaGameInfo in this.handler.GetGamesByDate(date.ToString("yyyy-MM-dd")))
      {
        int num1 = nbaGameInfo.AwayId;
        string awayId = num1.ToString();
        num1 = nbaGameInfo.HomeId;
        string homeId = num1.ToString();
        int year = 2014;
        DateTime gameTime = nbaGameInfo.GameTime;
        int num2 = 1;
        foreach (KeyValuePair<int, List<NBAGameStats>> keyValuePair in this.GetPredictData(awayId, homeId, year, gameTime, num2 != 0))
        {
          if (keyValuePair.Key >= 2 && keyValuePair.Value.Count == 2)
          {
            NBAGameStats team1 = keyValuePair.Value[0];
            NBAGameStats team2 = keyValuePair.Value[1];
            this.handler.SaveFourFactorByOpp(team1, nbaGameInfo.GameId, team1.Home == 0 ? nbaGameInfo.AwayPTS : nbaGameInfo.HomePTS, keyValuePair.Key);
            this.handler.SaveFourFactorByOpp(team2, nbaGameInfo.GameId, team2.Home == 0 ? nbaGameInfo.AwayPTS : nbaGameInfo.HomePTS, keyValuePair.Key);
          }
        }
      }
    }

    private Dictionary<int, List<NBAGameStats>> GetPredictData(string awayId, string homeId, int year, DateTime gameTime, bool fromDB)
    {
      NBATeamInfo teamInfoById1 = this.handler.GetTeamInfoById(awayId);
      NBATeamInfo teamInfoById2 = this.handler.GetTeamInfoById(homeId);
      string name1 = teamInfoById1.Name;
      string name2 = teamInfoById2.Name;
      List<NBAGameStats> gameStats = new List<NBAGameStats>();
      Dictionary<int, List<NBAGameStats>> dictionary = new Dictionary<int, List<NBAGameStats>>();
      List<NBAGameStats> list1 = this.PrepareTeamsData(awayId, name1, homeId, name2, year, gameTime, GamePos.None, 4);
      dictionary.Add(0, list1);
      List<NBAGameStats> list2 = this.PrepareTeamsData(awayId, name1, homeId, name2, year, gameTime, GamePos.All, 4);
      dictionary.Add(1, list2);
      List<NBAGameStats> list3 = this.PrepareTeamsData(awayId, name1, homeId, name2, year, gameTime, GamePos.All, 6);
      dictionary.Add(2, list3);
      List<NBAGameStats> list4 = this.PrepareTeamsData(awayId, name1, homeId, name2, year, gameTime, GamePos.All, 10);
      dictionary.Add(3, list4);
      List<NBAGameStats> list5 = this.PrepareTeamsData(awayId, name1, homeId, name2, year, gameTime, GamePos.All, 0);
      dictionary.Add(5, list5);
      foreach (KeyValuePair<int, List<NBAGameStats>> keyValuePair in (IEnumerable<KeyValuePair<int, List<NBAGameStats>>>) Enumerable.OrderBy<KeyValuePair<int, List<NBAGameStats>>, int>((IEnumerable<KeyValuePair<int, List<NBAGameStats>>>) dictionary, (Func<KeyValuePair<int, List<NBAGameStats>>, int>) (d => d.Key)))
        gameStats.AddRange((IEnumerable<NBAGameStats>) keyValuePair.Value);
      if (!fromDB)
      {
        this.ExportFourFactorsPerTeamToCSV(gameTime.ToString("yyyy-MM-dd"), true);
        this.ExportFourFactorsToPredict(gameStats, gameTime.ToString("yyyy-MM-dd"), teamInfoById1.Abbreviate, teamInfoById2.Abbreviate);
      }
      return dictionary;
    }

    private List<NBAGameStats> PrepareTeamsData(string awayId, string awayName, string homeId, string homeName, int year, DateTime gameTime, GamePos pos, int gameCount)
    {
      List<NBAGameStats> gameStats = new List<NBAGameStats>();
      NBAGameStats nbaGameStats1 = this.RetrieveGameStats(awayId, year, gameTime, pos == GamePos.None ? GamePos.Away : pos, gameCount);
      if (nbaGameStats1 != null)
      {
        nbaGameStats1.Home = 0;
        nbaGameStats1.TeamName = awayName;
        gameStats.Add(nbaGameStats1);
      }
      NBAGameStats nbaGameStats2 = this.RetrieveGameStats(homeId, year, gameTime, pos == GamePos.None ? GamePos.Home : pos, gameCount);
      if (nbaGameStats2 != null)
      {
        nbaGameStats2.Home = 1;
        nbaGameStats2.TeamName = homeName;
        gameStats.Add(nbaGameStats2);
      }
      if (gameStats.Count > 0)
        this.CaclulateMixPace(gameStats);
      return gameStats;
    }

    private void ExportFourFactorsToPredict(List<NBAGameStats> gameStats, string gameDate, string awayAbbr, string homeAbbr)
    {
      string[] strArray1 = new string[14]
      {
        "Team",
        "Home",
        "Pace_Calc",
        "O_eFG",
        "O_TOV",
        "ORB",
        "O_FT_FGA",
        "ORtg",
        "D_eFG",
        "D_TOV",
        "DRB",
        "D_FT_FGA",
        "DRtg",
        "Pace_Mix"
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Join(",", strArray1));
      int num = gameStats.Count / 2;
      for (int index = 0; index < num; ++index)
      {
        NBAGameStats nbaGameStats1 = gameStats[2 * index];
        NBAGameStats nbaGameStats2 = gameStats[2 * index + 1];
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", (object) awayAbbr, (object) nbaGameStats1.Home, (object) nbaGameStats2.Pace, (object) nbaGameStats2.O_eFG, (object) nbaGameStats2.O_TOV, (object) nbaGameStats2.ORB, (object) nbaGameStats2.O_FT_FGA, (object) nbaGameStats2.ORtg, (object) nbaGameStats2.D_eFG, (object) nbaGameStats2.D_TOV, (object) nbaGameStats2.DRB, (object) nbaGameStats2.D_FT_FGA, (object) nbaGameStats2.DRtg, (object) nbaGameStats2.Mix_Pace);
        stringBuilder.AppendLine();
        stringBuilder.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", (object) homeAbbr, (object) nbaGameStats2.Home, (object) nbaGameStats1.Pace, (object) nbaGameStats1.O_eFG, (object) nbaGameStats1.O_TOV, (object) nbaGameStats1.ORB, (object) nbaGameStats1.O_FT_FGA, (object) nbaGameStats1.ORtg, (object) nbaGameStats1.D_eFG, (object) nbaGameStats1.D_TOV, (object) nbaGameStats1.DRB, (object) nbaGameStats1.D_FT_FGA, (object) nbaGameStats1.DRtg, (object) nbaGameStats1.Mix_Pace);
      }
      string path = string.Format("D:\\Derek\\NBA\\{0}", (object) gameDate);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string str = string.Format("{0}\\{1}vs{2}.csv", (object) path, (object) awayAbbr, (object) homeAbbr);
      File.WriteAllText(str, stringBuilder.ToString());
      this.StartRConsole(str);
      string[] strArray2 = File.ReadAllLines(str);
      for (int index = 1; index < strArray2.Length; ++index)
      {
        string[] strArray3 = strArray2[index].Split(new string[1]
        {
          ","
        }, StringSplitOptions.RemoveEmptyEntries);
        gameStats[index - 1].Pred_PTS = Convert.ToDouble(strArray3[13]);
        if (index % 2 == 0)
        {
          gameStats[index - 1].LP = gameStats[index - 1].Pred_PTS - gameStats[index - 2].Pred_PTS;
          gameStats[index - 2].LP = -gameStats[index - 1].LP;
          gameStats[index - 1].SP = gameStats[index - 1].Pred_PTS + gameStats[index - 2].Pred_PTS;
          gameStats[index - 2].SP = gameStats[index - 1].SP;
        }
      }
    }

    private void StartRConsole(string filePath)
    {
      Process.Start(new ProcessStartInfo()
      {
        FileName = "C:\\Users\\dxiao1\\Desktop\\Data\\ConsoleR.exe",
        CreateNoWindow = true,
        UseShellExecute = true,
        Arguments = filePath
      }).Start();
      Thread.Sleep(500);
      int num = 100;
      for (int index = 0; !this.CheckFileStatus(filePath) && index < num; ++index)
        Thread.Sleep(100);
    }

    private bool CheckFileStatus(string filePath)
    {
      string fileName = Path.GetFileName(filePath);
      string path = Path.Combine(Directory.GetParent(filePath).FullName, "status.txt");
      if (!File.Exists(path))
        return false;
      string[] strArray1 = (string[]) null;
      try
      {
        strArray1 = File.ReadAllLines(path);
      }
      catch
      {
        Thread.Sleep(100);
      }
      if (strArray1 == null)
        return false;
      foreach (string str in strArray1)
      {
        string[] separator = new string[1]
        {
          "#"
        };
        int num = 1;
        string[] strArray2 = str.Split(separator, (StringSplitOptions) num);
        if (strArray2[0] == fileName && strArray2[1] == "1")
        {
          File.WriteAllText(path, "");
          return true;
        }
      }
      return false;
    }

    private NBAGameStats RetrieveGameStats(string teamId, int year, DateTime gameTime, GamePos pos)
    {
      return this.RetrieveGameStats(teamId, year, gameTime, pos, 0);
    }

    private NBAGameStats RetrieveGameStats(string teamId, int year, DateTime gameTime, GamePos pos, int gameCount)
    {
      List<int> list = this.handler.GetPreviousGames(teamId, year, gameTime, pos);
      NBAGameStats team = (NBAGameStats) null;
      if (list.Count > 0)
      {
        if (gameCount > 0)
          list = Enumerable.ToList<int>(Enumerable.Take<int>((IEnumerable<int>) list, gameCount));
        team = this.RetrieveStatsFromTable(this.handler.GetSumStatistics(teamId, (IEnumerable<int>) list));
        team.TeamId = Convert.ToInt32(teamId);
        team.Home = 1;
        NBAGameStats opponent = this.RetrieveStatsFromTable(this.handler.GetOppoSumStatistics(teamId, (IEnumerable<int>) list));
        opponent.Home = 0;
        this.CaclulateFourFactors(team, opponent);
      }
      return team;
    }

    private NBAGameStats RetrieveStatsFromTable(DataTable team)
    {
      DataRow dataRow = team.Rows[0];
      int num = Convert.ToInt32(dataRow[16]);
      NBAGameStats nbaGameStats = new NBAGameStats();
      nbaGameStats.GamePlayed = num;
      nbaGameStats.FGM = Convert.ToDouble(dataRow[0]) / (double) num;
      nbaGameStats.FGA = Convert.ToDouble(dataRow[1]) / (double) num;
      nbaGameStats.FGP = nbaGameStats.FGM * 100.0 / nbaGameStats.FGA;
      nbaGameStats.FG3M = Convert.ToDouble(dataRow[2]) / (double) num;
      nbaGameStats.FG3A = Convert.ToDouble(dataRow[3]) / (double) num;
      nbaGameStats.FG3P = nbaGameStats.FG3M * 100.0 / nbaGameStats.FG3A;
      nbaGameStats.FTM = Convert.ToDouble(dataRow[4]) / (double) num;
      nbaGameStats.FTA = Convert.ToDouble(dataRow[5]) / (double) num;
      nbaGameStats.FTP = nbaGameStats.FTM * 100.0 / nbaGameStats.FTA;
      nbaGameStats.OREB = Convert.ToDouble(dataRow[6]) / (double) num;
      nbaGameStats.DREB = Convert.ToDouble(dataRow[7]) / (double) num;
      nbaGameStats.REB = Convert.ToDouble(dataRow[8]) / (double) num;
      nbaGameStats.AST = Convert.ToDouble(dataRow[9]) / (double) num;
      nbaGameStats.STL = Convert.ToDouble(dataRow[10]) / (double) num;
      nbaGameStats.BLK = Convert.ToDouble(dataRow[11]) / (double) num;
      nbaGameStats.TOV = Convert.ToDouble(dataRow[12]) / (double) num;
      nbaGameStats.PF = Convert.ToDouble(dataRow[13]) / (double) num;
      nbaGameStats.PTS = Convert.ToDouble(dataRow[14]) / (double) num;
      nbaGameStats.MP = Convert.ToDouble(dataRow[15]) / (double) num;
      return nbaGameStats;
    }

    private void CaclulateFourFactors(NBAGameStats team, NBAGameStats opponent)
    {
      team.O_eFG = (team.FGM + 0.5 * team.FG3M) / team.FGA;
      team.D_eFG = (opponent.FGM + 0.5 * opponent.FG3M) / opponent.FGA;
      team.O_TOV = team.TOV * 100.0 / (team.FGA + 0.44 * team.FTA + team.TOV);
      team.D_TOV = opponent.TOV * 100.0 / (opponent.FGA + 0.44 * opponent.FTA + opponent.TOV);
      team.ORB = team.OREB * 100.0 / (team.OREB + opponent.DREB);
      team.DRB = team.DREB * 100.0 / (team.DREB + opponent.OREB);
      team.O_FT_FGA = team.FTM / team.FGA;
      team.D_FT_FGA = opponent.FTM / opponent.FGA;
      double num = 0.5 * (team.FGA + 0.4 * team.FTA - 1.07 * (team.OREB / (team.OREB + opponent.DREB)) * (team.FGA - team.FGM) + team.TOV + (opponent.FGA + 0.4 * opponent.FTA - 1.07 * (opponent.OREB / (opponent.OREB + team.DREB)) * (opponent.FGA - opponent.FGM) + opponent.TOV));
      team.Pace = 48.0 * (num + num) / (2.0 * team.MP / 5.0);
      team.ORtg = team.PTS * 100.0 / (team.Pace * team.MP / 240.0);
      team.DRtg = opponent.PTS * 100.0 / (team.Pace * opponent.MP / 240.0);
      opponent.O_eFG = team.D_eFG;
      opponent.D_eFG = team.O_eFG;
      opponent.O_TOV = team.D_TOV;
      opponent.D_TOV = team.O_TOV;
      opponent.ORB = opponent.OREB * 100.0 / (opponent.OREB + team.DREB);
      opponent.DRB = opponent.DREB * 100.0 / (opponent.DREB + team.OREB);
      opponent.O_FT_FGA = team.D_FT_FGA;
      opponent.D_FT_FGA = team.O_FT_FGA;
      opponent.Pace = team.Pace;
      opponent.ORtg = team.DRtg;
      opponent.DRtg = team.ORtg;
    }

    private void CaclulateMixPace(List<NBAGameStats> gameStats)
    {
      if (gameStats.Count != 2)
        return;
      NBAGameStats nbaGameStats1 = gameStats[0];
      NBAGameStats nbaGameStats2 = gameStats[1];
      double num = 0.5 * (nbaGameStats1.FGA + 0.4 * nbaGameStats1.FTA - 1.07 * (nbaGameStats1.OREB / nbaGameStats1.REB) * (nbaGameStats1.FGA - nbaGameStats1.FGM) + nbaGameStats1.TOV + (nbaGameStats2.FGA + 0.4 * nbaGameStats2.FTA - 1.07 * (nbaGameStats2.OREB / nbaGameStats2.REB) * (nbaGameStats2.FGA - nbaGameStats2.FGM) + nbaGameStats2.TOV));
      nbaGameStats1.Mix_Pace = 48.0 * (num + num) / (2.0 * nbaGameStats1.MP / 5.0);
      nbaGameStats2.Mix_Pace = nbaGameStats1.Mix_Pace;
    }

    private void CalculateLetPoints(List<NBAGameStats> gameStats)
    {
      if (gameStats.Count != 2)
        return;
      NBAGameStats nbaGameStats1 = gameStats[0];
      NBAGameStats nbaGameStats2 = gameStats[1];
      double num1 = nbaGameStats1.FGA - 0.35 * nbaGameStats2.BLK + 0.45 * nbaGameStats1.FTA;
      double num2 = nbaGameStats2.FGA - 0.35 * nbaGameStats1.BLK + 0.45 * nbaGameStats2.FTA;
      double num3 = nbaGameStats1.FGM + 0.45 * nbaGameStats1.FTM;
      double num4 = nbaGameStats2.FGM + 0.45 * nbaGameStats2.FTM;
      double num5 = 1.0 - (nbaGameStats1.OREB + nbaGameStats2.OREB) / (num1 + num2 - (num3 + num4));
      double num6 = num3 + (num1 - num3) * num5;
      double num7 = num4 + (num2 - num4) * num5;
      double num8 = (nbaGameStats1.PTS + nbaGameStats2.PTS) / (num6 + num7);
      double num9 = (nbaGameStats1.PTS - num6 * num8) * 2.0;
      double num10 = (nbaGameStats2.PTS - num7 * num8) * 2.0;
      double num11 = nbaGameStats1.OREB * num5 + (num2 - num4 - nbaGameStats2.OREB) * (1.0 - num5);
      double num12 = nbaGameStats2.OREB * num5 + (num1 - num3 - nbaGameStats1.OREB) * (1.0 - num5);
      double num13 = num1 - nbaGameStats1.OREB + nbaGameStats1.TOV;
      double num14 = num2 - nbaGameStats2.OREB + nbaGameStats2.TOV;
      double num15 = -(nbaGameStats1.TOV - 0.5 * nbaGameStats2.STL) / nbaGameStats1.TOV;
      double num16 = -(nbaGameStats2.TOV - 0.5 * nbaGameStats1.STL) / nbaGameStats2.TOV;
      double num17 = nbaGameStats1.TOV * num15;
      double num18 = nbaGameStats2.TOV * num16;
      double num19 = nbaGameStats1.STL * 0.5;
      double num20 = nbaGameStats2.STL * 0.5;
      double num21 = num11 + num19 + num17 - num12 - num20 - num18 + num13 - num14;
      double num22 = num12 + num20 + num18 - num11 - num19 - num17 + num14 - num13;
      double num23 = num9 + num21 * num8;
      double num24 = num10 + num22 * num8;
      nbaGameStats1.LP = num23;
      nbaGameStats2.LP = num24;
    }
  }
}
