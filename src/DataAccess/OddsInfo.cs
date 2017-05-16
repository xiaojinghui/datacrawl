// Decompiled with JetBrains decompiler
// Type: DataAccess.OddsInfo
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;

namespace DataAccess
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
