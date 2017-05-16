// Decompiled with JetBrains decompiler
// Type: ConsoleSinaDaily.Program
// Assembly: consdd, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CD8DFD15-5917-46C6-B8A5-F5B2EFDF6953
// Assembly location: D:\BaiduYunDownload\Data\consdd.exe

using OddsDataLayer;
using SinaDailyBLL;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleSinaDaily
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      if (args.Length > 0)
      {
        switch (args[0])
        {
          case "-u":
            Program.UpdateHistoryOdds(args);
            break;
          case "-uc":
            Program.UpdateCurrentOdds(args);
            break;
          case "-d":
            Program.DownloadHistoryOdds(args);
            break;
          case "-c":
            Program.DownloadCurrentOdds(args);
            break;
          case "-nba":
            Program.DownloadNBAStats(args);
            break;
          case "-cnba":
            Program.CalculateNBAStats(args);
            break;
          case "-nba500":
            Program.DownloadOddsFrom500(args);
            break;
          case "-nbasina":
            Program.DownloadOddsFromSina(args);
            break;
          case "-pred":
            Program.PredictPTSByR(args);
            break;
          case "-ff":
            Program.CalculateFourFactors(args);
            break;
          case "-ffo":
            Program.CalculateFourFactorsByOpp(args);
            break;
        }
      }
      else
        new DownloadSinaDailyBll().Download(0);
      Console.WriteLine("Download complete! Press Enter key to continue...");
    }

    private static void UpdateHistoryOdds(string[] args)
    {
      if (args.Length <= 1)
        return;
      int result = 0;
      int.TryParse(args[1], out result);
      new DownloadSinaHistoryBll().Update(result);
    }

    private static void UpdateCurrentOdds(string[] args)
    {
      if (args.Length <= 1)
        return;
      int result = 0;
      int.TryParse(args[1], out result);
      new DownloadSinaDailyBll().Update(result);
    }

    private static void DownloadHistoryOdds(string[] args)
    {
      if (args.Length <= 1)
        return;
      int result = 0;
      int.TryParse(args[1], out result);
      new DownloadSinaHistoryBll().DownloadHistory(result);
    }

    private static void DownloadCurrentOdds(string[] args)
    {
      if (args.Length <= 1)
        return;
      int result = 0;
      int.TryParse(args[1], out result);
      new DownloadSinaDailyBll().Download(result);
    }

    private static void DownloadNBAStats(string[] args)
    {
      if (args.Length <= 1)
        return;
      DownloadNBABll downloadNbaBll = new DownloadNBABll();
      Match match = Regex.Match(args[1], "^[\\d]{2}$");
      if (match.Success)
      {
        downloadNbaBll.UpdateNBADetailScore(Convert.ToInt32(match.Value));
      }
      else
      {
        string date1 = args[1];
        if (string.Compare(args[1], "today", true) == 0)
        {
          string date2 = DateTime.Today.AddDays(-1.0).ToString("yyyyMMdd");
          downloadNbaBll.DownloadNBAStats2(date2);
          string date3 = DateTime.Today.ToString("yyyyMMdd");
          downloadNbaBll.DownloadNBAStats2(date3);
        }
        else
          downloadNbaBll.DownloadNBAStats2(date1);
      }
    }

    private static void CalculateNBAStats(string[] args)
    {
      if (args.Length <= 1)
        return;
      int result = 0;
      int.TryParse(args[1], out result);
      new CalculateNBABll().Calculate(result);
    }

    private static void DownloadOddsFrom500(string[] args)
    {
      if (args.Length <= 1)
        return;
      string gameTime = args[1];
      DownloadNBABll downloadNbaBll = new DownloadNBABll();
      if (string.Compare(args[1], "today", true) == 0)
        gameTime = DateTime.Today.ToString("yyyy-MM-dd");
      DateTime now = DateTime.Now;
      if (args.Length > 2)
      {
        DateTime dateTime1 = Convert.ToDateTime(args[2]);
        for (DateTime dateTime2 = Convert.ToDateTime(args[1]); dateTime1 <= dateTime2; dateTime1 = dateTime1.AddDays(1.0))
          downloadNbaBll.DownloadOddsFrom500(dateTime1.ToString("yyyy-MM-dd"));
      }
      else
        downloadNbaBll.DownloadOddsFrom500(gameTime);
    }

    private static void DownloadOddsFromSina(string[] args)
    {
      if (args.Length <= 1)
        return;
      string gameTime1 = args[1];
      DownloadNBABll downloadNbaBll = new DownloadNBABll();
      if (string.Compare(args[1], "today", true) == 0)
      {
        DateTime gameTime2 = DateTime.Now;
        string path = "C:\\Users\\dxiao1\\Desktop\\Data\\date.txt";
        if (File.Exists(path))
        {
          try
          {
            gameTime2 = Convert.ToDateTime(File.ReadAllText(path));
          }
          catch
          {
            gameTime2 = DateTime.Now;
          }
        }
        gameTime2.ToString("yyyy-MM-dd");
        downloadNbaBll.DownloadTodayOddsFromSina(gameTime2);
      }
      else
      {
        DateTime now = DateTime.Now;
        if (args.Length > 2)
        {
          DateTime dateTime1 = Convert.ToDateTime(args[2]);
          for (DateTime dateTime2 = Convert.ToDateTime(args[1]); dateTime1 <= dateTime2; dateTime1 = dateTime1.AddDays(1.0))
            downloadNbaBll.DownloadHistroyOddsFromSina(dateTime1.ToString("yyyy-MM-dd"));
        }
        else
          downloadNbaBll.DownloadHistroyOddsFromSina(gameTime1);
      }
    }

    private static void PredictPTSByR(string[] args)
    {
      if (args.Length <= 1)
        return;
      string date1 = args[1];
      NBAGameStatsHelper nbaGameStatsHelper = new NBAGameStatsHelper();
      if (string.Compare(args[1], "today", true) == 0)
      {
        string date2 = DateTime.Today.ToString("yyyy-MM-dd");
        nbaGameStatsHelper.SaveNewPredictionToDB(date2);
      }
      else if (args.Length > 2)
      {
        DateTime dateTime1 = Convert.ToDateTime(args[2]);
        for (DateTime dateTime2 = Convert.ToDateTime(args[1]); dateTime1 <= dateTime2; dateTime1 = dateTime1.AddDays(1.0))
          nbaGameStatsHelper.SaveNewPredictionToDB(dateTime1.ToString("yyyy-MM-dd"));
      }
      else
        nbaGameStatsHelper.SaveNewPredictionToDB(date1);
    }

    private static void CalculateFourFactors(string[] args)
    {
      if (args.Length <= 1)
        return;
      string str = args[1];
      NBAGameStatsHelper nbaGameStatsHelper = new NBAGameStatsHelper();
      if (string.Compare(args[1], "today", true) == 0)
        str = DateTime.Today.ToString("yyyy-MM-dd");
      nbaGameStatsHelper.SaveFourFactorPerGameToDB(Convert.ToDateTime(str));
    }

    private static void CalculateFourFactorsByOpp(string[] args)
    {
      if (args.Length <= 1)
        return;
      NBAGameStatsHelper nbaGameStatsHelper = new NBAGameStatsHelper();
      string date1 = args[1];
      if (string.Compare(args[1], "today", true) == 0)
      {
        string date2 = DateTime.Today.ToString("yyyy-MM-dd");
        nbaGameStatsHelper.SaveFourFactorPerTeamToDB(date2);
      }
      else if (args.Length > 2)
      {
        DateTime date2 = Convert.ToDateTime(args[2]);
        for (DateTime dateTime = Convert.ToDateTime(args[1]); date2 <= dateTime; date2 = date2.AddDays(1.0))
          nbaGameStatsHelper.SaveFourFactorPerTeamToDB(date2);
      }
      else
        nbaGameStatsHelper.SaveFourFactorPerTeamToDB(date1);
    }
  }
}
