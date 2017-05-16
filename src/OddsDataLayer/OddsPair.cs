// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.OddsPair
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using System.Collections.Generic;

namespace OddsDataLayer
{
  public class OddsPair
  {
    public int TimeRange { get; set; }

    public List<OddsInfo> First { get; set; }

    public List<OddsInfo> Last { get; set; }

    public OddsPair()
    {
      this.First = new List<OddsInfo>();
      this.Last = new List<OddsInfo>();
    }
  }
}
