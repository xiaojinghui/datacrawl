// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.DataViewer
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using Newtonsoft.Json;
using System.Collections.Specialized;

namespace OddsDataLayer
{
  public class DataViewer
  {
    private NameValueCollection _Collection;

    public DataViewer(NameValueCollection collection)
    {
      this._Collection = collection;
    }

    public string GetData(bool isDaily)
    {
      string str1 = this._Collection["type"].Trim();
      string period = this._Collection["period"];
      string League = this._Collection["league"];
      string str2 = string.Empty;
      DataHandler dataHandler = new DataHandler(isDaily);
      switch (str1)
      {
        case "periods":
          str2 = JsonConvert.SerializeObject((object) dataHandler.GetAllPeriods());
          break;
        case "leagues":
          str2 = JsonConvert.SerializeObject((object) dataHandler.GetAllLeaguesByPeriod(period));
          break;
        case "games":
          str2 = JsonConvert.SerializeObject(string.IsNullOrEmpty(League) || !(League != "null") || !(League != "undefined") ? (object) dataHandler.GetGameInfosByPeriod(period) : (object) dataHandler.GetGameInfosByPeriodAndLeague(period, League));
          break;
      }
      return str2;
    }
  }
}
