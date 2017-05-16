// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.CalculateOdds
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace OddsDataLayer
{
  public class CalculateOdds
  {
    private double _winAvgVar = double.NaN;
    private double _tieAvgVar = double.NaN;
    private double _loseAvgVar = double.NaN;
    private bool _isDaily = false;
    private List<OddsInfo> _oddsList = new List<OddsInfo>();
    private string _GameId;

    public double WinAvgVar
    {
      get
      {
        return this._winAvgVar;
      }
    }

    public double TieAvgVar
    {
      get
      {
        return this._tieAvgVar;
      }
    }

    public double LoseAvgVar
    {
      get
      {
        return this._loseAvgVar;
      }
    }

    public List<OddsInfo> OddsList
    {
      get
      {
        return this._oddsList;
      }
    }

    public CalculateOdds(string gameId, bool isDaily)
    {
      this._GameId = gameId;
      this._winAvgVar = double.NaN;
      this._tieAvgVar = double.NaN;
      this._loseAvgVar = double.NaN;
      this._oddsList = new List<OddsInfo>();
      this._isDaily = isDaily;
    }

    public void Calculate(CalculateEnum calc, string limittedCompanyList)
    {
      string sql = string.Empty;
      if (calc == CalculateEnum.First)
        sql = string.Format("SELECT d.*\r\nFROM {2}.dbo.OddsInfo_Daily d\r\nLEFT JOIN (\r\n\tSELECT game_id, company_id, MIN(update_time) AS update_time\r\n\tFROM {2}.dbo.OddsInfo_Daily\r\n    WHERE game_id = {0}\r\n\tGROUP BY game_id, company_id\r\n\t) m ON d.game_id = m.game_id\r\n\tAND d.company_id = m.company_id\r\n\tAND d.update_time = m.update_time\r\nLEFT JOIN Data2014.dbo.CompanyInfo c ON d.company_id = c.company_id\r\nWHERE m.game_id = {0} {1}", (object) this._GameId, (object) limittedCompanyList, this._isDaily ? (object) "FootballSinaDaily" : (object) "Data2014");
      else if (calc == CalculateEnum.Last)
        sql = string.Format("SELECT d.*\r\nFROM {2}.dbo.OddsInfo_Daily d\r\nLEFT JOIN (\r\n\tSELECT game_id, company_id, MAX(update_time) AS update_time\r\n\tFROM {2}.dbo.OddsInfo_Daily\r\n    WHERE game_id = {0}\r\n\tGROUP BY game_id, company_id\r\n\t) m ON d.game_id = m.game_id\r\n\tAND d.company_id = m.company_id\r\n\tAND d.update_time = m.update_time\r\nLEFT JOIN Data2014.dbo.CompanyInfo c ON d.company_id = c.company_id\r\nWHERE m.game_id = {0} {1}", (object) this._GameId, (object) limittedCompanyList, this._isDaily ? (object) "FootballSinaDaily" : (object) "Data2014");
      if (string.IsNullOrEmpty(sql))
        return;
      this._oddsList = Enumerable.ToList<OddsInfo>((IEnumerable<OddsInfo>) new DataHandler(this._isDaily).GetOddsInfo(sql));
      this.Calculate(this._oddsList);
    }

    public void Calculate(List<OddsInfo> oddsList)
    {
      Decimal num1 = new Decimal(0);
      Decimal num2 = new Decimal(0);
      Decimal num3 = new Decimal(0);
      foreach (OddsInfo oddsInfo in oddsList)
      {
        Decimal num4 = oddsInfo.Tie * oddsInfo.Lose;
        Decimal num5 = oddsInfo.Win * oddsInfo.Lose;
        Decimal num6 = oddsInfo.Win * oddsInfo.Tie;
        num1 += num4 * new Decimal(100) / (num4 + num5 + num6);
        num2 += num5 * new Decimal(100) / (num4 + num5 + num6);
        num3 += num6 * new Decimal(100) / (num4 + num5 + num6);
      }
      Decimal num7 = num1 / (Decimal) this._oddsList.Count;
      Decimal num8 = num2 / (Decimal) this._oddsList.Count;
      Decimal num9 = num3 / (Decimal) this._oddsList.Count;
      foreach (OddsInfo oddsInfo in this._oddsList)
      {
        oddsInfo.WinKelly = oddsInfo.Win * num7 / new Decimal(100);
        oddsInfo.TieKelly = oddsInfo.Tie * num8 / new Decimal(100);
        oddsInfo.LoseKelly = oddsInfo.Lose * num9 / new Decimal(100);
      }
      Decimal num10 = Enumerable.Average<OddsInfo>((IEnumerable<OddsInfo>) this._oddsList, (Func<OddsInfo, Decimal>) (o => o.WinKelly));
      Decimal num11 = Enumerable.Average<OddsInfo>((IEnumerable<OddsInfo>) this._oddsList, (Func<OddsInfo, Decimal>) (o => o.TieKelly));
      Decimal num12 = Enumerable.Average<OddsInfo>((IEnumerable<OddsInfo>) this._oddsList, (Func<OddsInfo, Decimal>) (o => o.LoseKelly));
      foreach (OddsInfo oddsInfo in this._oddsList)
      {
        oddsInfo.WinVar = Math.Pow((double) (oddsInfo.WinKelly - num10), 2.0) * 100.0;
        oddsInfo.TieVar = Math.Pow((double) (oddsInfo.TieKelly - num11), 2.0) * 100.0;
        oddsInfo.LoseVar = Math.Pow((double) (oddsInfo.LoseKelly - num12), 2.0) * 100.0;
      }
      this._winAvgVar = Enumerable.Average<OddsInfo>((IEnumerable<OddsInfo>) this._oddsList, (Func<OddsInfo, double>) (o => o.WinVar));
      this._tieAvgVar = Enumerable.Average<OddsInfo>((IEnumerable<OddsInfo>) this._oddsList, (Func<OddsInfo, double>) (o => o.TieVar));
      this._loseAvgVar = Enumerable.Average<OddsInfo>((IEnumerable<OddsInfo>) this._oddsList, (Func<OddsInfo, double>) (o => o.LoseVar));
    }
  }
}
