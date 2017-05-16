// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.OddsHandler
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OddsDataLayer
{
  public class OddsHandler
  {
    private string _GameId;
    private string _CompId;
    private bool _IsDaily;
    private List<UIOddsInfo> _companyOdds;
    private List<UIOddsInfo> _calculateOdds;

    public List<UIOddsInfo> CompanyOdds
    {
      get
      {
        return this._companyOdds;
      }
    }

    public List<UIOddsInfo> CalculateOdds
    {
      get
      {
        return this._calculateOdds;
      }
    }

    public OddsHandler(string gameId, string compId, bool isDaily)
    {
      this._GameId = gameId;
      this._CompId = compId;
      this._IsDaily = isDaily;
    }

    public string GetJsonString(bool needAppendix)
    {
      string limittedCompanyList = this.RetrieveCompanyLimitted(this._CompId);
      this._companyOdds = new List<UIOddsInfo>();
      this._calculateOdds = new List<UIOddsInfo>();
      Dictionary<int, Company> allCompanyInfos = new DataHandler(this._IsDaily).GetAllCompanyInfos();
      string appendix1 = needAppendix ? "--初值" : "";
      OddsDataLayer.CalculateOdds calculateOdds = new OddsDataLayer.CalculateOdds(this._GameId, this._IsDaily);
      calculateOdds.Calculate(CalculateEnum.First, limittedCompanyList);
      this.RetrieveUIOddsInfo(calculateOdds.OddsList, allCompanyInfos, appendix1, CalculateEnum.First);
      string appendix2 = needAppendix ? "--终值" : "";
      calculateOdds.Calculate(CalculateEnum.Last, limittedCompanyList);
      this.RetrieveUIOddsInfo(calculateOdds.OddsList, allCompanyInfos, appendix2, CalculateEnum.Last);
      return string.Format("{{\"{0}\":{1},\"{2}\":{3}}}", (object) "compOdds", (object) JsonConvert.SerializeObject((object) Enumerable.OrderBy<UIOddsInfo, string>((IEnumerable<UIOddsInfo>) this._companyOdds, (Func<UIOddsInfo, string>) (o => o.Name))), (object) "calcOdds", (object) JsonConvert.SerializeObject((object) Enumerable.OrderBy<UIOddsInfo, string>((IEnumerable<UIOddsInfo>) this._calculateOdds, (Func<UIOddsInfo, string>) (o => o.Name))));
    }

    private string RetrieveCompanyLimitted(string compId)
    {
      string str = string.Empty;
      switch (compId)
      {
        case "0":
          str = " AND is_leading = 1";
          goto case "2";
        case "1":
          str = " AND is_leading IN (0, 1)";
          goto case "2";
        case "2":
          return str;
        default:
          str = string.Format(" AND c.company_id IN (SELECT company_id FROM FootballSina.dbo.CustomCompanyList WHERE id = {0})", (object) compId);
          goto case "2";
      }
    }

    private void RetrieveUIOddsInfo(List<OddsInfo> oddsList, Dictionary<int, Company> allExistCompanies, string appendix, CalculateEnum oddsType)
    {
      List<UIOddsInfo> list = Enumerable.ToList<UIOddsInfo>(Enumerable.Select(Enumerable.ThenBy(Enumerable.OrderByDescending(Enumerable.Join((IEnumerable<OddsInfo>) oddsList, (IEnumerable<KeyValuePair<int, Company>>) allExistCompanies, (Func<OddsInfo, int>) (odds => odds.CompanyId), (Func<KeyValuePair<int, Company>, int>) (company => company.Key), (odds, company) =>
      {
        var fAnonymousType2 = new
        {
          odds = odds,
          company = company
        };
        return fAnonymousType2;
      }), param0 => param0.company.Value.IsLeading), param0 => param0.company.Value.Name), param0 => new UIOddsInfo()
      {
        Name = param0.company.Value.Name,
        WinVar = param0.odds.WinVar * 100.0,
        TieVar = param0.odds.TieVar * 100.0,
        LoseVar = param0.odds.LoseVar * 100.0,
        Win = param0.odds.Win,
        Tie = param0.odds.Tie,
        Lose = param0.odds.Lose,
        WinKelly = param0.odds.WinKelly * new Decimal(100),
        TieKelly = param0.odds.TieKelly * new Decimal(100),
        LoseKelly = param0.odds.LoseKelly * new Decimal(100),
        UpdateTime = param0.odds.UpdateTime,
        OddsType = oddsType
      }));
      UIOddsInfo uiOddsInfo1 = new UIOddsInfo()
      {
        Name = "最大值" + appendix,
        WinVar = Enumerable.Max<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.WinVar)),
        TieVar = Enumerable.Max<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.TieVar)),
        LoseVar = Enumerable.Max<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.LoseVar)),
        WinKelly = Enumerable.Max<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.WinKelly)),
        TieKelly = Enumerable.Max<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.TieKelly)),
        LoseKelly = Enumerable.Max<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.LoseKelly))
      };
      UIOddsInfo uiOddsInfo2 = new UIOddsInfo()
      {
        Name = "平均值" + appendix,
        WinVar = Enumerable.Average<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.WinVar)),
        TieVar = Enumerable.Average<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.TieVar)),
        LoseVar = Enumerable.Average<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.LoseVar)),
        WinKelly = Enumerable.Average<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.WinKelly)),
        TieKelly = Enumerable.Average<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.TieKelly)),
        LoseKelly = Enumerable.Average<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.LoseKelly))
      };
      UIOddsInfo uiOddsInfo3 = new UIOddsInfo()
      {
        Name = "最小值" + appendix,
        WinVar = Enumerable.Min<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.WinVar)),
        TieVar = Enumerable.Min<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.TieVar)),
        LoseVar = Enumerable.Min<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, double>) (o => o.LoseVar)),
        WinKelly = Enumerable.Min<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.WinKelly)),
        TieKelly = Enumerable.Min<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.TieKelly)),
        LoseKelly = Enumerable.Min<UIOddsInfo>((IEnumerable<UIOddsInfo>) list, (Func<UIOddsInfo, Decimal>) (o => o.LoseKelly))
      };
      this._companyOdds.AddRange((IEnumerable<UIOddsInfo>) list);
      this._calculateOdds.Add(uiOddsInfo2);
      this._calculateOdds.Add(uiOddsInfo1);
      this._calculateOdds.Add(uiOddsInfo3);
    }
  }
}
