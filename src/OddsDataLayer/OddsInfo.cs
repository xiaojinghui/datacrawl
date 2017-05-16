// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.OddsInfo
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using System;

namespace OddsDataLayer
{
  public class OddsInfo
  {
    public int OddsId { get; set; }

    public int GameId { get; set; }

    public int CompanyId { get; set; }

    public Decimal Win { get; set; }

    public Decimal Tie { get; set; }

    public Decimal Lose { get; set; }

    public DateTime UpdateTime { get; set; }

    public Decimal WinKelly { get; set; }

    public Decimal TieKelly { get; set; }

    public Decimal LoseKelly { get; set; }

    public double WinVar { get; set; }

    public double TieVar { get; set; }

    public double LoseVar { get; set; }
  }
}
