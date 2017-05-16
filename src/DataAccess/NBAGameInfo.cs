// Decompiled with JetBrains decompiler
// Type: DataAccess.NBAGameInfo
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;

namespace DataAccess
{
  public class NBAGameInfo
  {
    public int GameId { get; set; }

    public int AwayId { get; set; }

    public int HomeId { get; set; }

    public DateTime GameTime { get; set; }

    public Decimal LetPointHandicap { get; set; }

    public Decimal ScoreHandicap { get; set; }

    public string HalfScore { get; set; }

    public string FinalScore { get; set; }
  }
}
