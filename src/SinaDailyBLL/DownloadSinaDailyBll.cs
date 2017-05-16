// Decompiled with JetBrains decompiler
// Type: SinaDailyBLL.DownloadSinaDailyBll
// Assembly: SinaDailyBLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 780ED807-CE3C-455E-8D24-8A4F56965874
// Assembly location: D:\BaiduYunDownload\Data\SinaDailyBLL.dll

using DataAccess;
using HtmlAgilityPack;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SinaDailyBLL
{
  public class DownloadSinaDailyBll
  {
    private DateTime m_UTCDate = new DateTime(1970, 1, 1);
    private const string m_Url = "http://www.aicai.com/bjdc/";
    private const string m_OddsUrl = "http://live.aicai.com/xiyaou/odds!getOddsTrack.htm?betId={0}&companyId={1}";
    private const string m_OddsUrlFull = "http://live.aicai.com/xiyaou/odds!getOuzhi.htm?betId={0}&propId=0&start=0&size=9999";
    private Dictionary<int, string> m_Cache;

    public void Download(int count)
    {
      this.ResetGlobalVar();
      string period = this.RetrievePeriod();
      string currentPeriod = DataHandlerSina.GetCurrentPeriod();
      if (period != currentPeriod)
      {
        DataHandlerSina.UpdateCurrentPeriod(period);
        DataHandlerSina.CallingSQLJob("Transfer data");
      }
      int lastUpdatedSerial = DataHandlerSina.GetLastUpdatedSerial();
      List<GameInfo> list = this.RetrieveCurrentGame(period, currentPeriod, lastUpdatedSerial);
      foreach (GameInfo game in count <= 0 ? Enumerable.ToList<GameInfo>(Enumerable.Where<GameInfo>((IEnumerable<GameInfo>) list, (Func<GameInfo, bool>) (g => g.GameTime.AddHours(-8.0) < DateTime.Now))) : Enumerable.ToList<GameInfo>(Enumerable.Take<GameInfo>((IEnumerable<GameInfo>) list, count)))
        this.DownloadDetailOdds(game);
    }

    private void ResetGlobalVar()
    {
      this.m_UTCDate = DateTime.SpecifyKind(this.m_UTCDate, DateTimeKind.Utc);
      this.m_Cache = new Dictionary<int, string>();
    }

    public void Update(int period)
    {
      this.ResetGlobalVar();
      foreach (GameInfo game in Enumerable.ToList<GameInfo>(Enumerable.Where<GameInfo>((IEnumerable<GameInfo>) DataHandlerSina.GetGameInfosByPeriod(period.ToString(), "FootballSinaDaily.dbo.GameInfo_Daily"), (Func<GameInfo, bool>) (g => g.FinalScore.Length > 0))))
      {
        this.RetrieveOdds(game, true);
        this.DownloadDetailOdds(game);
      }
    }

    private string RetrievePeriod()
    {
      string str = string.Empty;
      string html = this.RetrieveText("http://trade.500.com/bjdc/");
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(html);
      HtmlNode elementbyId = htmlDocument.GetElementbyId("expect_select");
      if (elementbyId == null)
        return str;
      Match match = Regex.Match(elementbyId.ChildNodes[2].InnerText, "[\\d]+");
      if (match.Success)
        str = match.Value;
      return str;
    }

    private List<GameInfo> RetrieveCurrentGame(string period, string currentPeriod, int lastSerial)
    {
      string html = this.RetrieveText("http://www.aicai.com/bjdc/");
      List<GameInfo> gameInfosByPeriod = DataHandlerSina.GetGameInfosByPeriod(period, "FootballSinaDaily.dbo.GameInfo_Daily");
      List<GameInfo> list = new List<GameInfo>();
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(html);
      HtmlNode elementbyId = htmlDocument.GetElementbyId("template_dc_spffushi_result");
      if (elementbyId == null)
        return list;
      foreach (HtmlNode htmlNode1 in (IEnumerable<HtmlNode>) elementbyId.SelectNodes("table/tbody"))
      {
        if (string.Compare("jq_display4allstop", htmlNode1.GetAttributeValue("class", ""), true) == 0)
        {
          foreach (HtmlNode htmlNode2 in (IEnumerable<HtmlNode>) htmlNode1.SelectNodes("tr"))
          {
            string attributeValue = htmlNode2.GetAttributeValue("fixid", "");
            if (!string.IsNullOrEmpty(attributeValue))
            {
              GameInfo game = new GameInfo();
              game.Serial = Convert.ToInt32(htmlNode2.ChildNodes[1].InnerText.Trim());
              game.GameId = Convert.ToInt32(attributeValue);
              string str = htmlNode2.ChildNodes[3].InnerText.Trim();
              game.League = str;
              Match match = Regex.Match(htmlNode2.ChildNodes[5].GetAttributeValue("jq_value", ""), "[\\d]{4}-[\\d]{2}-[\\d]{2}[\\s][\\d]{2}:[\\d]{2}");
              if (match.Success)
                game.GameTime = Convert.ToDateTime(match.Value);
              game.Host = htmlNode2.ChildNodes[7].InnerText.Trim();
              game.Guest = htmlNode2.ChildNodes[11].InnerText.Trim();
              game.Periods = period;
              if (!string.IsNullOrEmpty(htmlNode2.GetAttributeValue("style", "")))
              {
                if (Enumerable.Any<GameInfo>((IEnumerable<GameInfo>) gameInfosByPeriod, (Func<GameInfo, bool>) (g => g.GameId == game.GameId)))
                  this.RetrieveOdds(game, true);
              }
              else
              {
                if (period != currentPeriod)
                  this.RetrieveOdds(game, false);
                list.Add(game);
              }
            }
          }
        }
      }
      return list;
    }

    private void RetrieveOdds(GameInfo game, bool updateGameInfo)
    {
      this.RetrieveOdds(game, updateGameInfo, false);
    }

    private void RetrieveOdds(GameInfo game, bool updateGameInfo, bool downloadHistory)
    {
      this.RetrieveOdds(game, (Dictionary<int, DateTime>) null, updateGameInfo, downloadHistory);
    }

    private List<int> RetrieveOdds(GameInfo game, Dictionary<int, DateTime> maxUpdateTimes, bool updateGameInfo, bool downloadHistory)
    {
      string str1 = this.RetrieveText(string.Format("http://live.aicai.com/xiyaou/odds!getOuzhi.htm?betId={0}&propId=0&start=0&size=9999", (object) game.GameId));
      List<int> list1 = new List<int>();
      List<int> list2 = new List<int>();
      List<OddsInfo> list3 = new List<OddsInfo>();
      object obj1 = JsonConvert.DeserializeObject(str1);
      // ISSUE: reference to a compiler-generated field
      if (DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (DownloadSinaDailyBll)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> func = DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Site9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> callSite = DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Site9;
      // ISSUE: reference to a compiler-generated field
      if (DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea == null)
      {
        // ISSUE: reference to a compiler-generated field
        DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Root", typeof (DownloadSinaDailyBll), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea.Target((CallSite) DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea, obj1);
      foreach (object obj3 in func((CallSite) callSite, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb == null)
        {
          // ISSUE: reference to a compiler-generated field
          DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, JProperty>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JProperty), typeof (DownloadSinaDailyBll)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        JProperty jproperty = DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb.Target((CallSite) DownloadSinaDailyBll.\u003CRetrieveOdds\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb, obj3);
        if (!(jproperty.Name != "result"))
        {
          string leagueId = string.Empty;
          foreach (JProperty data1 in (IEnumerable<JToken>) jproperty.Value)
          {
            if (data1.Name == "matchTime")
              game.GameTime = this.RetrieveUpdateTime(data1);
            else if (data1.Name == "leagueId")
              leagueId = data1.Value.ToString();
            else if (data1.Name == "hostRank")
              game.HostRank = data1.Value.ToString().Replace("\"", "");
            else if (data1.Name == "awayRank")
              game.GuestRank = data1.Value.ToString().Replace("\"", "");
            if (updateGameInfo || downloadHistory)
            {
              if (data1.Name == "hostHalfScore")
                game.HalfScore = data1.Value.ToString();
              else if (data1.Name == "hostScore")
                game.FinalScore = data1.Value.ToString();
              else if (data1.Name == "awayHalfScore")
              {
                GameInfo gameInfo = game;
                string str2 = gameInfo.HalfScore + "-" + data1.Value.ToString();
                gameInfo.HalfScore = str2;
              }
              else if (data1.Name == "awayScore")
              {
                GameInfo gameInfo = game;
                string str2 = gameInfo.FinalScore + "-" + data1.Value.ToString();
                gameInfo.FinalScore = str2;
              }
            }
            if (!(data1.Name != "europOddsList"))
            {
              if (updateGameInfo)
              {
                DataHandlerSina.UpdateDailyGameInfo(game, "FootballSinaDaily");
                return list1;
              }
              foreach (JToken jtoken in (IEnumerable<JToken>) data1.Value)
              {
                OddsInfo oddsInfo1 = new OddsInfo();
                OddsInfo oddsInfo2 = new OddsInfo();
                oddsInfo1.GameId = game.GameId;
                oddsInfo2.GameId = game.GameId;
                foreach (JProperty data2 in (IEnumerable<JToken>) jtoken)
                {
                  if (data2.Name == "createTime")
                    oddsInfo2.UpdateTime = this.RetrieveUpdateTime(data2);
                  else if (data2.Name == "drowOdds")
                    oddsInfo1.Tie = Convert.ToDecimal(data2.Value.ToString()) / new Decimal(10000);
                  else if (data2.Name == "loseOdds")
                    oddsInfo1.Lose = Convert.ToDecimal(data2.Value.ToString()) / new Decimal(10000);
                  else if (data2.Name == "winOdds")
                    oddsInfo1.Win = Convert.ToDecimal(data2.Value.ToString()) / new Decimal(10000);
                  else if (data2.Name == "companyId")
                  {
                    oddsInfo1.CompanyId = Convert.ToInt32(data2.Value.ToString());
                    oddsInfo2.CompanyId = oddsInfo1.CompanyId;
                    if (oddsInfo1.CompanyId != 28 && oddsInfo1.CompanyId != 31 && (oddsInfo1.CompanyId != 45 && oddsInfo1.CompanyId != 59) && oddsInfo1.CompanyId != 65)
                      list2.Add(oddsInfo1.CompanyId);
                  }
                  else if (data2.Name == "lastUpdateTime")
                    oddsInfo1.UpdateTime = this.RetrieveUpdateTime(data2);
                  else if (data2.Name == "firstDrowOdds")
                    oddsInfo2.Tie = Convert.ToDecimal(data2.Value.ToString()) / new Decimal(10000);
                  else if (data2.Name == "firstLoseOdds")
                    oddsInfo2.Lose = Convert.ToDecimal(data2.Value.ToString()) / new Decimal(10000);
                  else if (data2.Name == "firstWinOdds")
                    oddsInfo2.Win = Convert.ToDecimal(data2.Value.ToString()) / new Decimal(10000);
                }
                if (maxUpdateTimes != null && maxUpdateTimes.ContainsKey(oddsInfo1.CompanyId) && maxUpdateTimes[oddsInfo1.CompanyId] < oddsInfo1.UpdateTime)
                  list1.Add(oddsInfo1.CompanyId);
                if (oddsInfo2.CompanyId != 28 && oddsInfo2.CompanyId != 31 && (oddsInfo2.CompanyId != 45 && oddsInfo2.CompanyId != 59) && oddsInfo2.CompanyId != 65)
                  list3.Add(oddsInfo2);
              }
            }
          }
          if (list3.Count > 0 && maxUpdateTimes == null)
          {
            DataHandlerSina.SaveDailyGameInfo(game, leagueId, "FootballSinaDaily");
            DataHandler500.SaveOddsInfo((IEnumerable<OddsInfo>) list3, "FootballSinaDaily.dbo.OddsInfo_Daily");
          }
        }
      }
      if (list3.Count > 0 && maxUpdateTimes == null)
      {
        DataHandlerSina.SaveGameCompanyMap(game.GameId, string.Join<int>(",", (IEnumerable<int>) list2), "FootballSinaDaily");
        DataHandlerSina.UpdateDataReady(game.GameId, DataReady.YesSimpleNotDetail, "FootballSinaDaily");
      }
      return list1;
    }

    private void DownloadDetailOdds(GameInfo game)
    {
      Dictionary<int, DateTime> currentOddsUpdateTime = DataHandlerSina.GetCurrentOddsUpdateTime(game.GameId);
      System.IO.File.AppendAllText("C:\\Users\\dxiao1\\Desktop\\Data\\log_sina.txt", string.Format("{1} Current Game: {0}..........\r\n", (object) game.GameId, (object) DateTime.Now.ToString()));
      List<int> list = this.RetrieveOdds(game, currentOddsUpdateTime, false, false);
      this.DownloadDetailOdds(game, "FootballSinaDaily.dbo.OddsInfo_Daily", (IEnumerable<int>) list);
    }

    private void DownloadDetailOdds(GameInfo game, string tableName, IEnumerable<int> companies)
    {
      bool flag = false;
      List<string> list1 = new List<string>();
      foreach (int num in companies)
      {
        string url = string.Format("http://live.aicai.com/xiyaou/odds!getOddsTrack.htm?betId={0}&companyId={1}", (object) game.GameId, (object) num);
        string str = this.RetrieveText(url);
        if (str.Contains("fail"))
        {
          list1.Add(num.ToString());
          System.IO.File.AppendAllText("C:\\Users\\dxiao1\\Desktop\\Data\\log_sina.txt", string.Format("Getting Data Failure, url: {0}\r\n", (object) url));
        }
        else
        {
          List<OddsInfo> list2 = new List<OddsInfo>();
          object obj1 = JsonConvert.DeserializeObject(str);
          // ISSUE: reference to a compiler-generated field
          if (DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sited == null)
          {
            // ISSUE: reference to a compiler-generated field
            DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sited = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (DownloadSinaDailyBll)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, IEnumerable> func = DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sited.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, IEnumerable>> callSite = DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sited;
          // ISSUE: reference to a compiler-generated field
          if (DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee == null)
          {
            // ISSUE: reference to a compiler-generated field
            DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Root", typeof (DownloadSinaDailyBll), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee.Target((CallSite) DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee, obj1);
          foreach (object obj3 in func((CallSite) callSite, obj2))
          {
            // ISSUE: reference to a compiler-generated field
            if (DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef == null)
            {
              // ISSUE: reference to a compiler-generated field
              DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef = CallSite<Func<CallSite, object, JProperty>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JProperty), typeof (DownloadSinaDailyBll)));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            JProperty jproperty = DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef.Target((CallSite) DownloadSinaDailyBll.\u003CDownloadDetailOdds\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef, obj3);
            if (!(jproperty.Name != "result"))
            {
              foreach (JToken jtoken in (IEnumerable<JToken>) ((JProperty) jproperty.Value.First).Value)
              {
                OddsInfo oddsInfo = new OddsInfo();
                oddsInfo.CompanyId = Convert.ToInt32(num);
                oddsInfo.GameId = game.GameId;
                foreach (JProperty data in (IEnumerable<JToken>) jtoken)
                {
                  if (data.Name == "createTime")
                    oddsInfo.UpdateTime = this.RetrieveUpdateTime(data);
                  else if (data.Name == "drowOdds")
                    oddsInfo.Tie = Convert.ToDecimal(data.Value.ToString()) / new Decimal(10000);
                  else if (data.Name == "loseOdds")
                    oddsInfo.Lose = Convert.ToDecimal(data.Value.ToString()) / new Decimal(10000);
                  else if (data.Name == "winOdds")
                    oddsInfo.Win = Convert.ToDecimal(data.Value.ToString()) / new Decimal(10000);
                }
                list2.Add(oddsInfo);
              }
            }
          }
          if (list2.Count > 0)
          {
            DataHandler500.SaveOddsInfo((IEnumerable<OddsInfo>) list2, tableName);
          }
          else
          {
            flag = true;
            break;
          }
        }
      }
      if (flag)
        DataHandlerSina.UpdateDataReady(game.GameId, DataReady.YesSimpleNoDetail);
      else if (list1.Count > 0)
      {
        DataHandlerSina.UpdateFailureIds(game.GameId, string.Join(",", (IEnumerable<string>) list1));
        if (list1.Count != Enumerable.Count<int>(companies))
          return;
        System.IO.File.AppendAllText("C:\\Users\\dxiao1\\Desktop\\Data\\log_sina.txt", string.Format("-------Data is missing, id: {0}-------\r\n", (object) game.GameId));
      }
      else
        DataHandlerSina.UpdateDataReady(game.GameId, DataReady.YesAll);
    }

    private string RetrieveText(string url)
    {
      string str1;
      if (this.m_Cache.TryGetValue(url.GetHashCode(), out str1))
        return str1;
      string str2;
      try
      {
        str2 = Encoding.UTF8.GetString(new WebClient().DownloadData(url));
        if (!str2.Contains("当前页面歇菜了"))
        {
          if (!str2.Contains("失败"))
            goto label_6;
        }
        System.IO.File.AppendAllText("C:\\Users\\dxiao1\\Desktop\\Data\\log_sina.txt", "Sleeping...\r\n");
        Thread.Sleep(300000);
        str2 = this.RetrieveText(url);
      }
      catch
      {
        System.IO.File.AppendAllText("C:\\Users\\dxiao1\\Desktop\\Data\\log_sina.txt", "Sleeping...\r\n");
        Thread.Sleep(300000);
        str2 = this.RetrieveText(url);
      }
label_6:
      if (!string.IsNullOrEmpty(str2))
        this.m_Cache[url.GetHashCode()] = str2;
      return str2;
    }

    private DateTime RetrieveUpdateTime(JProperty data)
    {
      long num = 0L;
      foreach (JProperty jproperty in (IEnumerable<JToken>) data.Value)
      {
        if (!(jproperty.Name != "time"))
          num = long.Parse(jproperty.Value.ToString());
      }
      return this.m_UTCDate.AddMilliseconds((double) num).ToLocalTime();
    }
  }
}
