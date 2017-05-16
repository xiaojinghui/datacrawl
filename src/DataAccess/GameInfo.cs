// Decompiled with JetBrains decompiler
// Type: DataAccess.GameInfo
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;

namespace DataAccess
{
  public class GameInfo
  {
    public int GameId { get; set; }

    public DateTime GameTime { get; set; }

    public string League { get; set; }

    public string Host { get; set; }

    public string Guest { get; set; }

    public string HostRank { get; set; }

    public string GuestRank { get; set; }

    public string HalfScore { get; set; }

    public string FinalScore { get; set; }

    public string OddsUrl { get; set; }

    public int DataReady { get; set; }

    public string Periods { get; set; }

    public int Serial { get; set; }

    public Decimal WinAvg { get; set; }

    public Decimal TieAvg { get; set; }

    public Decimal LoseAvg { get; set; }

    public Decimal AsiaTape { get; set; }

    public string AsiaTapeZh { get; set; }

    public int AsiaTapeResult { get; set; }

    public Decimal ScoreTape { get; set; }

    public string ScoreTapeZh { get; set; }

    public int ScoreTapeResult { get; set; }

    public string CompanyList { get; set; }
  }
}
