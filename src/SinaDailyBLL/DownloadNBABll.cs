// Decompiled with JetBrains decompiler
// Type: SinaDailyBLL.DownloadNBABll
// Assembly: SinaDailyBLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 780ED807-CE3C-455E-8D24-8A4F56965874
// Assembly location: D:\BaiduYunDownload\Data\SinaDailyBLL.dll

using HtmlAgilityPack;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OddsDataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace SinaDailyBLL
{
  public class DownloadNBABll
  {
    private WebClient m_WebClient;

    public DownloadNBABll()
    {
      this.m_WebClient = new WebClient();
    }

    public void DownloadNBAStats(string date)
    {
      string html = this.RetrieveText(string.Format("http://www.nba.com/gameline/{0}/", (object) date), Encoding.UTF8);
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(html);
      HtmlNode elementbyId = htmlDocument.GetElementbyId("nbaSSOuter");
      if (elementbyId == null)
        return;
      string str1 = string.Empty;
      foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) elementbyId.ChildNodes)
      {
        Match match = Regex.Match(htmlNode.GetAttributeValue("id", ""), "[\\d]+");
        if (match.Success)
        {
          string str2 = this.RetrieveText(string.Format("http://stats.nba.com/stats/boxscore?GameID={0}&RangeType=0&StartPeriod=0&EndPeriod=0&StartRange=0&EndRange=0", (object) match.Value), Encoding.UTF8);
          int gameId = Convert.ToInt32(match.Value);
          int gameType = gameId.ToString().StartsWith("2") ? 2 : 3;
          object obj1 = JsonConvert.DeserializeObject(str2);
          // ISSUE: reference to a compiler-generated field
          if (DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (DownloadNBABll)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, IEnumerable> func = DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, IEnumerable>> callSite = DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
          // ISSUE: reference to a compiler-generated field
          if (DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Root", typeof (DownloadNBABll), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, obj1);
          foreach (object obj3 in func((CallSite) callSite, obj2))
          {
            // ISSUE: reference to a compiler-generated field
            if (DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, JProperty>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JProperty), typeof (DownloadNBABll)));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            JProperty root = DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site3.Target((CallSite) DownloadNBABll.\u003CDownloadNBAStats\u003Eo__SiteContainer0.\u003C\u003Ep__Site3, obj3);
            if (!(root.Name != "resultSets"))
            {
              str1 = this.ReadGameSummary(match.Value, gameType, root);
              if (str1 == "3")
              {
                this.ReadTeamStats(match.Value, root);
                this.ReadLineScore(gameId, root);
              }
            }
          }
        }
      }
      if (str1 != "3")
      {
        string str2 = date.Insert(6, "-").Insert(4, "-");
        this.DownloadOddsFrom500(str2);
        this.DownloadTodayOddsFromSina(DateTime.Parse(str2));
        System.IO.File.WriteAllText("C:\\Users\\dxiao1\\Desktop\\Data\\date.txt", str2);
      }
      else
        DataAccess.DataHandlerNBA.UpdateGameResults();
    }

    public void DownloadNBAStats2(string date)
    {
      string str1 = date.Insert(6, "-").Insert(4, "-");
      DateTime gameTime = Convert.ToDateTime(str1);
      if (DataAccess.DataHandlerNBA.GetGameInfosByDate(gameTime.AddDays(1.0)).Rows.Count == 0)
        this.InitialGameInfo("214");
      DataTable gameInfosByDate = DataAccess.DataHandlerNBA.GetGameInfosByDate(gameTime);
      string str2 = string.Empty;
      foreach (DataRow dataRow in (InternalDataCollectionBase) gameInfosByDate.Rows)
      {
        int gameId = Convert.ToInt32(dataRow["id"]);
        string str3 = this.RetrieveText(string.Format("http://stats.nba.com/stats/boxscore?GameID={0}&RangeType=0&StartPeriod=0&EndPeriod=0&StartRange=0&EndRange=0", (object) gameId.ToString("D10")), Encoding.UTF8);
        int gameType = gameId.ToString().StartsWith("2") ? 2 : 3;
        object obj1 = JsonConvert.DeserializeObject(str3);
        // ISSUE: reference to a compiler-generated field
        if (DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (DownloadNBABll)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, IEnumerable> func = DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, IEnumerable>> callSite = DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site5;
        // ISSUE: reference to a compiler-generated field
        if (DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Root", typeof (DownloadNBABll), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site6.Target((CallSite) DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site6, obj1);
        foreach (object obj3 in func((CallSite) callSite, obj2))
        {
          // ISSUE: reference to a compiler-generated field
          if (DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site7 = CallSite<Func<CallSite, object, JProperty>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JProperty), typeof (DownloadNBABll)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          JProperty root = DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site7.Target((CallSite) DownloadNBABll.\u003CDownloadNBAStats2\u003Eo__SiteContainer4.\u003C\u003Ep__Site7, obj3);
          if (!(root.Name != "resultSets"))
          {
            str2 = this.ReadGameSummary(gameId.ToString(), gameType, root);
            if (str2 == "3")
            {
              this.ReadTeamStats(gameId.ToString(), root);
              this.ReadLineScore(gameId, root);
            }
          }
        }
      }
      if (str2 != "3")
      {
        this.DownloadOddsFrom500(str1);
        this.DownloadTodayOddsFromSina(DateTime.Parse(str1));
        System.IO.File.WriteAllText("C:\\Users\\dxiao1\\Desktop\\Data\\date.txt", str1);
      }
      else
        DataAccess.DataHandlerNBA.UpdateGameResults();
      if (DataAccess.DataHandlerNBA.IsPredictFinished(str1))
        return;
      NBAGameStatsHelper nbaGameStatsHelper = new NBAGameStatsHelper();
      nbaGameStatsHelper.SaveFourFactorPerGameToDB(str1);
      nbaGameStatsHelper.SaveFourFactorPerTeamToDB(str1);
      nbaGameStatsHelper.SaveNewPredictionToDB(str1);
    }

    public void UpdateNBADetailScore(int year)
    {
      foreach (int gameId in DataAccess.DataHandlerNBA.GetNoDetailScoreGame(year))
      {
        object obj1 = JsonConvert.DeserializeObject(this.RetrieveText(string.Format("http://stats.nba.com/stats/boxscore?GameID={0}&RangeType=0&StartPeriod=0&EndPeriod=0&StartRange=0&EndRange=0", (object) gameId.ToString("D10")), Encoding.UTF8));
        // ISSUE: reference to a compiler-generated field
        if (DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (DownloadNBABll)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, IEnumerable> func = DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Site9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, IEnumerable>> callSite = DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Site9;
        // ISSUE: reference to a compiler-generated field
        if (DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea == null)
        {
          // ISSUE: reference to a compiler-generated field
          DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Root", typeof (DownloadNBABll), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea.Target((CallSite) DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Sitea, obj1);
        foreach (object obj3 in func((CallSite) callSite, obj2))
        {
          // ISSUE: reference to a compiler-generated field
          if (DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb == null)
          {
            // ISSUE: reference to a compiler-generated field
            DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, JProperty>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JProperty), typeof (DownloadNBABll)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          JProperty root = DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb.Target((CallSite) DownloadNBABll.\u003CUpdateNBADetailScore\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb, obj3);
          if (!(root.Name != "resultSets"))
            this.ReadLineScore(gameId, root);
        }
      }
    }

    public void InitialGameInfo(string season)
    {
      int maxGameId = DataAccess.DataHandlerNBA.GetMaxGameId(season);
      for (int index = 1; index < 20; ++index)
        this.DownloadGameStat(maxGameId + index);
    }

    public void UpdateMissedGameInfo()
    {
      foreach (DataRow dataRow in (InternalDataCollectionBase) DataAccess.DataHandlerNBA.GetMissedGameInfo().Rows)
        this.DownloadGameStat(Convert.ToInt32(dataRow["id"]));
    }

    private void DownloadGameStat(int gameId)
    {
      string str = this.RetrieveText(string.Format("http://stats.nba.com/stats/boxscore?GameID={0}&RangeType=0&StartPeriod=0&EndPeriod=0&StartRange=0&EndRange=0", (object) gameId.ToString("D10")), Encoding.UTF8);
      int gameType = gameId.ToString().StartsWith("2") ? 2 : 3;
      object obj1 = JsonConvert.DeserializeObject(str);
      // ISSUE: reference to a compiler-generated field
      if (DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sited == null)
      {
        // ISSUE: reference to a compiler-generated field
        DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sited = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (DownloadNBABll)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> func = DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sited.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> callSite = DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sited;
      // ISSUE: reference to a compiler-generated field
      if (DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee == null)
      {
        // ISSUE: reference to a compiler-generated field
        DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Root", typeof (DownloadNBABll), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee.Target((CallSite) DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitee, obj1);
      foreach (object obj3 in func((CallSite) callSite, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef == null)
        {
          // ISSUE: reference to a compiler-generated field
          DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef = CallSite<Func<CallSite, object, JProperty>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JProperty), typeof (DownloadNBABll)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        JProperty root = DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef.Target((CallSite) DownloadNBABll.\u003CDownloadGameStat\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef, obj3);
        if (!(root.Name != "resultSets") && this.ReadGameSummary(gameId.ToString(), gameType, root) == "3")
        {
          this.ReadTeamStats(gameId.ToString(), root);
          this.ReadLineScore(gameId, root);
        }
      }
    }

    private string ReadGameSummary(string gameId, int gameType, JProperty root)
    {
      string str = string.Empty;
      foreach (JProperty jproperty in (IEnumerable<JToken>) ((JArray) root.Value)[0])
      {
        if (!(jproperty.Name != "rowSet"))
        {
          foreach (JToken jtoken in (IEnumerable<JToken>) jproperty.Value)
          {
            str = jtoken[(object) 3].ToString();
            string homeId = jtoken[(object) 6].ToString();
            string awayId = jtoken[(object) 7].ToString();
            string gameTime = jtoken[(object) 0].ToString().Replace("\"", "");
            DataAccess.DataHandlerNBA.SaveGameInfo(gameId, awayId, homeId, gameTime, string.Empty, string.Empty, string.Empty, string.Empty, -1, -1, gameType);
          }
        }
      }
      return str;
    }

    private void ReadTeamStats(string gameId, JProperty root)
    {
      foreach (JProperty jproperty in (IEnumerable<JToken>) ((JArray) root.Value)[5])
      {
        if (!(jproperty.Name != "rowSet"))
        {
          int num = 0;
          foreach (JToken data in (IEnumerable<JToken>) jproperty.Value)
            DataAccess.DataHandlerNBA.SaveGameStats(gameId, data[(object) 1].ToString(), data, num++);
        }
      }
    }

    private void ReadLineScore(int gameId, JProperty root)
    {
      foreach (JProperty jproperty in (IEnumerable<JToken>) ((JArray) root.Value)[1])
      {
        if (!(jproperty.Name != "rowSet"))
        {
          foreach (JToken jtoken in (IEnumerable<JToken>) jproperty.Value)
          {
            int Q1 = Convert.ToInt32(jtoken[(object) 7].ToString());
            int Q2 = Convert.ToInt32(jtoken[(object) 8].ToString());
            int Q3 = Convert.ToInt32(jtoken[(object) 9].ToString());
            int Q4 = Convert.ToInt32(jtoken[(object) 10].ToString());
            int OT1 = Convert.ToInt32(jtoken[(object) 11].ToString());
            int OT2 = 0;
            int OT3 = 0;
            int OT4 = 0;
            if (OT1 > 0)
              OT2 = Convert.ToInt32(jtoken[(object) 12].ToString());
            if (OT2 > 0)
              OT3 = Convert.ToInt32(jtoken[(object) 13].ToString());
            if (OT3 > 0)
              OT4 = Convert.ToInt32(jtoken[(object) 14].ToString());
            string teamId = jtoken[(object) 3].ToString();
            DataAccess.DataHandlerNBA.UpdateGameScore(gameId, teamId, Q1, Q2, Q3, Q4, OT1, OT2, OT3, OT4);
          }
        }
      }
    }

    public void DownloadOddsFrom500(string gameTime)
    {
      string html1 = this.RetrieveText(string.Format("http://trade.500.com/jclq/index.php?playid=313&date={0}", (object) gameTime), Encoding.GetEncoding("GB2312"));
      HtmlDocument htmlDocument1 = new HtmlDocument();
      htmlDocument1.LoadHtml(html1);
      HtmlNode elementbyId1 = htmlDocument1.GetElementbyId("d_" + gameTime);
      if (elementbyId1 == null)
        return;
      foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) elementbyId1.ChildNodes[3].ChildNodes)
      {
        if (!(htmlNode.Name != "tr") && !(htmlNode.GetAttributeValue("lg", "") != "NBA"))
        {
          string attributeValue1 = htmlNode.GetAttributeValue("rf", "");
          string attributeValue2 = htmlNode.GetAttributeValue("yszf", "");
          MatchCollection matchCollection = Regex.Matches(htmlNode.InnerHtml, "http://liansai\\.500\\.com/lq/215/team/([\\d]+)/");
          string gameId = string.Empty;
          if (matchCollection.Count > 0)
          {
            string awayId = matchCollection[0].Groups[1].Value;
            string homeId = matchCollection[1].Groups[1].Value;
            gameId = DataAccess.DataHandlerNBA.GetGameIdBy500TeamId(gameTime, awayId, homeId);
            DataAccess.DataHandlerNBA.Update500Odds(gameId, attributeValue1, attributeValue2);
          }
          Match match = Regex.Match(htmlNode.InnerHtml, "http://odds\\.500\\.com/lq/shuju.php\\?id=([\\d]+)&r=1");
          if (match.Success)
          {
            string fId = match.Groups[1].Value;
            this.Download500DetailOdds(fId, gameId, "293", "威廉希尔", false);
            this.Download500DetailOdds(fId, gameId, "293", "威廉希尔", true);
          }
        }
      }
      string html2 = this.RetrieveText(string.Format("http://trade.500.com/jclq/index.php?playid=275&date={0}", (object) gameTime), Encoding.GetEncoding("GB2312"));
      HtmlDocument htmlDocument2 = new HtmlDocument();
      htmlDocument2.LoadHtml(html2);
      HtmlNode elementbyId2 = htmlDocument2.GetElementbyId("d_" + gameTime);
      if (elementbyId2 == null)
        return;
      foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) elementbyId2.ChildNodes[3].ChildNodes)
      {
        if (!(htmlNode.Name != "tr") && !(htmlNode.GetAttributeValue("lg", "") != "NBA"))
        {
          htmlNode.GetAttributeValue("zid", "");
          MatchCollection matchCollection = Regex.Matches(htmlNode.InnerHtml, "http://liansai\\.500\\.com/lq/215/team/([\\d]+)/");
          string gameId = string.Empty;
          if (matchCollection.Count > 0)
          {
            string awayId = matchCollection[0].Groups[1].Value;
            string homeId = matchCollection[1].Groups[1].Value;
            gameId = DataAccess.DataHandlerNBA.GetGameIdBy500TeamId(gameTime, awayId, homeId);
          }
          Match match = Regex.Match(htmlNode.InnerHtml, "http://zx\\.500\\.com/common/jjzs_tubiao\\.php\\?lot=2&id=[\\d]+&gg=2&play=2&lhash=[\\w]+");
          if (match.Success)
          {
            this.Download500DetailOdds2(match.Value, gameId, false);
            this.Download500DetailOdds2(match.Value.Replace("play=2", "play=4"), gameId, true);
          }
        }
      }
    }

    private void Download500DetailOdds(string fId, string gameId, string compId, string compName, bool isScore)
    {
      string str = isScore ? "zongfenajax" : "rangfenajax";
      foreach (XContainer xcontainer in XElement.Parse("<root>" + this.RetrieveText(string.Format("http://odds.500.com/lq/inc/{2}.php?_=1416803194913&id={1}&folder=00059&fid={0}", (object) fId, (object) compId, (object) str), Encoding.UTF8) + "</root>").Elements())
      {
        List<XElement> list = Enumerable.ToList<XElement>(xcontainer.Elements());
        if (isScore)
          DataAccess.DataHandlerNBA.SaveScoreHandicap(gameId, compName, list[3].Value, Convert.ToDecimal(list[0].Value), list[1].Value, Convert.ToDecimal(list[2].Value));
        else
          DataAccess.DataHandlerNBA.SaveAsiaHandicap(gameId, compName, list[3].Value, Convert.ToDecimal(list[0].Value), list[1].Value, Convert.ToDecimal(list[2].Value));
      }
    }

    private void Download500DetailOdds2(string url, string gameId, bool isScore)
    {
      string str = this.RetrieveText(url, Encoding.GetEncoding("GB2312"));
      int startIndex = str.IndexOf("<table");
      int num = str.IndexOf("</table>");
      foreach (XContainer xcontainer in Enumerable.Skip<XElement>(XElement.Parse(str.Substring(startIndex, num - startIndex + 8)).Elements(), 1))
      {
        List<XElement> list = Enumerable.ToList<XElement>(xcontainer.Elements());
        if (isScore)
          DataAccess.DataHandlerNBA.SaveScoreHandicap(gameId, "竞彩", list[3].Value, Convert.ToDecimal(list[0].Value), list[1].Value, Convert.ToDecimal(list[2].Value));
        else
          DataAccess.DataHandlerNBA.SaveAsiaHandicap(gameId, "竞彩", list[3].Value, Convert.ToDecimal(list[0].Value), list[1].Value, Convert.ToDecimal(list[2].Value));
      }
    }

    public void DownloadTodayOddsFromSina(DateTime gameTime)
    {
      string html = this.RetrieveText("http://www.aicai.com/pages/lottery/jclc/index_guding_sf.shtml", Encoding.UTF8);
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(html);
      string idFormat = "jq_gudingsf_match_" + gameTime.ToString("yyMMdd") + "{0}_tr";
      this.DownloadSinaOddsByGame(document, idFormat, 301, 330, gameTime.ToString("yyyy-MM-dd"));
    }

    public void DownloadHistroyOddsFromSina(string gameTime)
    {
      string html = this.RetrieveText(string.Format("http://www.aicai.com/lottery/jcReport!lcMatchResult.jhtml?lotteryType=4061&matchNames=&startMatchTime={0}&endMatchTime={0}", (object) gameTime), Encoding.UTF8);
      HtmlDocument document = new HtmlDocument();
      document.LoadHtml(html);
      this.DownloadSinaOddsByGame(document, "tra{0}", 0, 30, Convert.ToDateTime(gameTime).AddDays(-1.0).ToString("yyyy-MM-dd"));
    }

    private void DownloadSinaOddsByGame(HtmlDocument document, string idFormat, int startIndex, int endIndex, string gameTime)
    {
      for (HtmlNode elementbyId = document.GetElementbyId(string.Format(idFormat, (object) startIndex++)); startIndex < endIndex; elementbyId = document.GetElementbyId(string.Format(idFormat, (object) startIndex++)))
      {
        if (elementbyId != null && elementbyId.ChildNodes[1].InnerText.Trim() == "NBA")
        {
          string awayName = elementbyId.ChildNodes[7].InnerText.Trim();
          string homeName = elementbyId.ChildNodes[9].InnerText.Trim();
          string gameIdByName = DataAccess.DataHandlerNBA.GetGameIdByName(gameTime, awayName, homeName);
          Match match = Regex.Match(elementbyId.InnerHtml, "http://live\\.aicai\\.com/lc/xyo_([\\d]+)\\.html");
          if (match.Success)
          {
            this.DownloadSinaDetailOdds(gameIdByName, match.Groups[1].Value, 2, "Bet365", false);
            this.DownloadSinaDetailOdds(gameIdByName, match.Groups[1].Value, 33, "澳门", false);
            this.DownloadSinaDetailOdds(gameIdByName, match.Groups[1].Value, 5, "Bet365", true);
            this.DownloadSinaDetailOdds(gameIdByName, match.Groups[1].Value, 34, "澳门", true);
          }
        }
      }
    }

	/*
    private void DownloadSinaDetailOdds(string gameId, string sinaGameId, int companyId, string companyName, bool isScore)
    {
      string str1 = this.RetrieveText(!isScore ? string.Format("http://live.aicai.com/lc/xyo/bkletscore!getOddsDetail.htm?betId={0}&companyId={1}", (object) sinaGameId, (object) companyId) : string.Format("http://live.aicai.com/lc/xyo/bkscore!getOddsDetail.htm?betId={0}&companyId={1}", (object) sinaGameId, (object) companyId), Encoding.UTF8);
      int num = isScore ? 1 : -1;
      object obj1 = JsonConvert.DeserializeObject(str1);
      // ISSUE: reference to a compiler-generated field
      if (DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site11 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (DownloadNBABll)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> func = DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> callSite = DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site11;
      // ISSUE: reference to a compiler-generated field
      if (DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Root", typeof (DownloadNBABll), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site12.Target((CallSite) DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site12, obj1);
      foreach (object obj3 in func((CallSite) callSite, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site13 = CallSite<Func<CallSite, object, JProperty>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (JProperty), typeof (DownloadNBABll)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        JProperty jproperty1 = DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site13.Target((CallSite) DownloadNBABll.\u003CDownloadSinaDetailOdds\u003Eo__SiteContainer10.\u003C\u003Ep__Site13, obj3);
        if (!(jproperty1.Name != "result"))
        {
          foreach (JProperty jproperty2 in (IEnumerable<JToken>) jproperty1.Value)
          {
            if (!(jproperty2.Name != "oddsDetailList"))
            {
              foreach (JToken jtoken in (IEnumerable<JToken>) jproperty2.Value)
              {
                string str2 = jtoken[(object) "winOdds"].ToString().Replace("\"", "");
                string str3 = jtoken[(object) "loseOdds"].ToString().Replace("\"", "");
                string handicap = (Convert.ToDecimal(jtoken[(object) "tape"].ToString().Replace("\"", "")) * (Decimal) num / new Decimal(10)).ToString();
                string date = jtoken[(object) "createTime"].ToString().Replace("\"", "");
                if (isScore)
                  DataAccess.DataHandlerNBA.SaveScoreHandicap(gameId, companyName, date, Convert.ToDecimal(str3) / new Decimal(10000), handicap, Convert.ToDecimal(str2) / new Decimal(10000));
                else
                  DataAccess.DataHandlerNBA.SaveAsiaHandicap(gameId, companyName, date, Convert.ToDecimal(str3) / new Decimal(10000), handicap, Convert.ToDecimal(str2) / new Decimal(10000));
              }
            }
          }
        }
      }
    }
*/
    private string RetrieveText(string url, Encoding encoding)
    {
      string str;
      try
      {
        byte[] bytes = this.m_WebClient.DownloadData(url);
        str = encoding.GetString(bytes);
        if (str.Contains("当前页面歇菜了"))
        {
          System.IO.File.AppendAllText("log_nba.txt", "Sleeping...\r\n");
          Thread.Sleep(300000);
          str = this.RetrieveText(url, encoding);
        }
      }
      catch
      {
        System.IO.File.AppendAllText("log_nba.txt", "Sleeping...\r\n");
        Thread.Sleep(300000);
        str = this.RetrieveText(url, encoding);
      }
      return str;
    }
  }
}
