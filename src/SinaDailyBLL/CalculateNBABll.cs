// Decompiled with JetBrains decompiler
// Type: SinaDailyBLL.CalculateNBABll
// Assembly: SinaDailyBLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 780ED807-CE3C-455E-8D24-8A4F56965874
// Assembly location: D:\BaiduYunDownload\Data\SinaDailyBLL.dll

using DataAccess;
using System.Collections.Generic;
using System.Data;

namespace SinaDailyBLL
{
  public class CalculateNBABll
  {
    public void Calculate(int year)
    {
      foreach (NBAGameInfo nbaGameInfo in DataHandlerNBA.GetGamesByYear(year))
      {
        List<int> gameIds1 = DataHandlerNBA.GetPreviousGames(nbaGameInfo.HomeId, year, nbaGameInfo.GameTime);
        if (gameIds1.Count == 0)
          gameIds1 = DataHandlerNBA.GetLastSeasonGames(nbaGameInfo.HomeId, year);
        DataTable sumStatistics1 = DataHandlerNBA.GetSumStatistics(nbaGameInfo.HomeId, gameIds1);
        List<int> gameIds2 = DataHandlerNBA.GetPreviousGames(nbaGameInfo.AwayId, year, nbaGameInfo.GameTime);
        if (gameIds2.Count == 0)
          gameIds2 = DataHandlerNBA.GetLastSeasonGames(nbaGameInfo.AwayId, year);
        DataTable sumStatistics2 = DataHandlerNBA.GetSumStatistics(nbaGameInfo.AwayId, gameIds2);
        DataHandlerNBA.SaveGamePreStats(nbaGameInfo.GameId, nbaGameInfo.HomeId, nbaGameInfo.AwayId, sumStatistics1.Rows[0], sumStatistics2.Rows[0]);
      }
    }
  }
}
