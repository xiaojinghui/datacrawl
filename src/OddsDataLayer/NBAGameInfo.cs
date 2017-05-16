// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.NBAGameInfo
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using System;

namespace OddsDataLayer
{
  public class NBAGameInfo
  {
    public int GameId { get; set; }

    public int AwayId { get; set; }

    public string AwayName { get; set; }

    public string AwayEnName { get; set; }

    public string AwayAbbr { get; set; }

    public int AwayLeague { get; set; }

    public int AwayPTS { get; set; }

    public int HomeId { get; set; }

    public string HomeName { get; set; }

    public string HomeEnName { get; set; }

    public string HomeAbbr { get; set; }

    public int HomeLeague { get; set; }

    public int HomePTS { get; set; }

    public string HalfScore { get; set; }

    public string FinalScore { get; set; }

    public DateTime GameTime { get; set; }

    public Decimal AsiaTape { get; set; }

    public int AsiaTapeResult { get; set; }

    public Decimal ScoreTape { get; set; }

    public int ScoreTapeResult { get; set; }
  }
}
