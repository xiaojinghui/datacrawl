// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.GraphHelper
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace OddsDataLayer
{
  public class GraphHelper
  {
    private double[] _OddsSplitPoint = new double[19]
    {
      0.5,
      1.0,
      1.5,
      2.0,
      2.5,
      3.0,
      3.5,
      4.0,
      5.0,
      6.0,
      7.0,
      8.0,
      9.0,
      10.0,
      12.0,
      15.0,
      18.0,
      24.0,
      30.0
    };
    private int _calcCount = 0;
    private List<Tuple<DateTime, double, double, double, int>> _avgOdds = new List<Tuple<DateTime, double, double, double, int>>();
    private List<Tuple<DateTime, double, double, double, int>> _minOdds = new List<Tuple<DateTime, double, double, double, int>>();
    private int _GameId;
    private string _OddsTableName;
    private DateTime _GameTime;
    private List<DateTime> _SplitDates;

    public List<Tuple<DateTime, double, double, double, int>> OddsList
    {
      get
      {
        return this._avgOdds;
      }
    }

    public List<Tuple<DateTime, double, double, double, int>> OddsMinList
    {
      get
      {
        return this._minOdds;
      }
    }

    public GraphHelper(string gameId)
      : this(gameId, "OddsInfo")
    {
    }

    public GraphHelper(string gameId, string oddsTableName)
      : this(gameId, oddsTableName, DateTime.MinValue)
    {
    }

    public GraphHelper(string gameId, string oddsTableName, DateTime gameTime)
    {
      this._GameId = Convert.ToInt32(gameId);
      this._OddsTableName = oddsTableName;
      this._GameTime = gameTime;
      if (!(this._GameTime != DateTime.MinValue))
        return;
      this._SplitDates = new List<DateTime>(this._OddsSplitPoint.Length + 1);
      foreach (double num in Enumerable.Reverse<double>((IEnumerable<double>) this._OddsSplitPoint))
        this._SplitDates.Add(this._GameTime.AddHours(-num));
      this._SplitDates.Add(this._GameTime);
    }

    public void CollectData(int cmpSet, bool isDaily)
    {
    //  this.Reset();
    //  DataHandler dataHandler = new DataHandler(isDaily);
    //  Stack<OddsInfo> oddsInfos = cmpSet != 0 
				//? (cmpSet != 1 ? (cmpSet != 2 ? dataHandler.GetCustomOddsInfoByGame(this._GameId, this._OddsTableName, cmpSet) : dataHandler.GetNotExchangeOddsInfoByGame(this._GameId, this._OddsTableName)) : dataHandler.GetMainOddsInfoByGame(this._GameId, this._OddsTableName)) 
				//: dataHandler.GetAllOddsInfoByGame(this._GameId, this._OddsTableName);
    //  if (oddsInfos == null || oddsInfos.Count == 0)
    //    return;
    //  if (this._GameTime != DateTime.MinValue)
    //  {
    //    IOrderedEnumerable<IGrouping<\u003C\u003Ef__AnonymousType0<int, int>, OddsInfo>> orderedEnumerable = Enumerable.OrderBy<IGrouping<\u003C\u003Ef__AnonymousType0<int, int>, OddsInfo>, int>(Enumerable.Where<IGrouping<\u003C\u003Ef__AnonymousType0<int, int>, OddsInfo>>(Enumerable.GroupBy((IEnumerable<OddsInfo>) oddsInfos, odds =>
    //    {
    //      var fAnonymousType0 = new
    //      {
    //        CompanyId = odds.CompanyId,
    //        TimeRange = this.CheckTimeRange(odds.UpdateTime)
    //      };
    //      return fAnonymousType0;
    //    }), og => og.Key.TimeRange > -1), og => og.Key.TimeRange);
    //    Dictionary<int, OddsPair> oddsPairDic = new Dictionary<int, OddsPair>();
    //    foreach (IGrouping<\u003C\u003Ef__AnonymousType0<int, int>, OddsInfo> grouping in orderedEnumerable)
    //    {
    //      OddsPair oddsPair;
    //      if (!oddsPairDic.TryGetValue(grouping.Key.TimeRange, out oddsPair))
    //      {
    //        oddsPair = new OddsPair();
    //        oddsPair.TimeRange = grouping.Key.TimeRange;
    //        oddsPairDic.Add(grouping.Key.TimeRange, oddsPair);
    //      }
    //      oddsPair.First.Add(Enumerable.FirstOrDefault<OddsInfo>((IEnumerable<OddsInfo>) grouping));
    //      oddsPair.Last.Add(Enumerable.LastOrDefault<OddsInfo>((IEnumerable<OddsInfo>) grouping));
    //    }
    //    this.RetrieveGraphedOdds(oddsPairDic);
    //  }
    //  else
    //    this.RetrieveGraphedOdds(oddsInfos);
    }

    public void Reset()
    {
      this._calcCount = 0;
      this._avgOdds.Clear();
    }

    private void RetrieveGraphedOdds(Stack<OddsInfo> oddsInfos)
    {
      Dictionary<int, OddsInfo> dictionary = new Dictionary<int, OddsInfo>();
      List<int> list = new List<int>();
      while (oddsInfos.Count > 0)
      {
        OddsInfo oddsInfo1 = oddsInfos.Pop();
        if (!dictionary.ContainsKey(oddsInfo1.CompanyId))
          dictionary.Add(oddsInfo1.CompanyId, oddsInfo1);
        else
          dictionary[oddsInfo1.CompanyId] = oddsInfo1;
        if (oddsInfos.Count > 0)
        {
          OddsInfo oddsInfo2 = oddsInfos.Peek();
          if (oddsInfo1.CompanyId != oddsInfo2.CompanyId && !list.Contains(oddsInfo1.CompanyId))
            list.Add(oddsInfo1.CompanyId);
          if (!(oddsInfo1.UpdateTime == oddsInfo2.UpdateTime) && list.Count > 10)
          {
            this.Calculate(oddsInfo1.UpdateTime, (IEnumerable<OddsInfo>) dictionary.Values);
            list.Clear();
          }
        }
        else
          this.Calculate(oddsInfo1.UpdateTime, (IEnumerable<OddsInfo>) dictionary.Values);
      }
    }

    private void RetrieveGraphedOdds(Dictionary<int, OddsPair> oddsPairDic)
    {
      Dictionary<int, OddsPair> dictionary = new Dictionary<int, OddsPair>();
      for (int index = 0; index < this._SplitDates.Count; ++index)
      {
        OddsPair oddsPair;
        if (!oddsPairDic.TryGetValue(index, out oddsPair))
        {
          oddsPair = index != 0 ? this.GetPreviousPair(oddsPairDic, index) : this.GetNextPair(oddsPairDic, index);
          if (oddsPair != null)
          {
            oddsPair.TimeRange = index;
            oddsPairDic[index] = oddsPair;
          }
        }
      }
      List<int> list1 = new List<int>();
      foreach (KeyValuePair<int, OddsPair> keyValuePair in oddsPairDic)
      {
        if (keyValuePair.Key == 0 || list1.Count == 0)
        {
          list1 = Enumerable.ToList<int>(Enumerable.Select<OddsInfo, int>((IEnumerable<OddsInfo>) keyValuePair.Value.Last, (Func<OddsInfo, int>) (o => o.CompanyId)));
          this.Calculate(this._SplitDates[keyValuePair.Key], (IEnumerable<OddsInfo>) keyValuePair.Value.Last, keyValuePair.Key + 1);
        }
        else
        {
          Enumerable.ToList<OddsInfo>(Enumerable.Except<OddsInfo>((IEnumerable<OddsInfo>) keyValuePair.Value.Last, (IEnumerable<OddsInfo>) keyValuePair.Value.First));
          List<int> list2 = Enumerable.ToList<int>(Enumerable.Select<OddsInfo, int>((IEnumerable<OddsInfo>) keyValuePair.Value.Last, (Func<OddsInfo, int>) (o => o.CompanyId)));
          List<int> diffComps = Enumerable.ToList<int>(Enumerable.Except<int>((IEnumerable<int>) list1, (IEnumerable<int>) list2));
          if (diffComps.Count > 0)
          {
            OddsPair previousPair = this.GetPreviousPair(oddsPairDic, keyValuePair.Key);
            if (previousPair != null)
            {
              IEnumerable<OddsInfo> collection = Enumerable.Where<OddsInfo>((IEnumerable<OddsInfo>) previousPair.Last, (Func<OddsInfo, bool>) (o => diffComps.Contains(o.CompanyId)));
              keyValuePair.Value.Last.AddRange(collection);
              keyValuePair.Value.First.AddRange(collection);
            }
            else
              continue;
          }
          this.Calculate(this._SplitDates[keyValuePair.Key], (IEnumerable<OddsInfo>) keyValuePair.Value.Last, keyValuePair.Key + 1);
          List<int> list3 = Enumerable.ToList<int>(Enumerable.Except<int>((IEnumerable<int>) list2, (IEnumerable<int>) list1));
          list1.AddRange((IEnumerable<int>) list3);
        }
      }
    }

    private void Calculate(DateTime time, IEnumerable<OddsInfo> toCalcList)
    {
      ++this._calcCount;
      this.Calculate(time, toCalcList, this._calcCount);
    }

    private void Calculate(DateTime time, IEnumerable<OddsInfo> toCalcList, int index)
    {
      Decimal num1 = new Decimal(0);
      Decimal num2 = new Decimal(0);
      Decimal num3 = new Decimal(0);
      foreach (OddsInfo oddsInfo in toCalcList)
      {
        Decimal num4 = oddsInfo.Tie * oddsInfo.Lose;
        Decimal num5 = oddsInfo.Win * oddsInfo.Lose;
        Decimal num6 = oddsInfo.Win * oddsInfo.Tie;
        num1 += num4 * new Decimal(100) / (num4 + num5 + num6);
        num2 += num5 * new Decimal(100) / (num4 + num5 + num6);
        num3 += num6 * new Decimal(100) / (num4 + num5 + num6);
      }
      int num7 = Enumerable.Count<OddsInfo>(toCalcList);
      Decimal num8 = num1 / (Decimal) num7;
      Decimal num9 = num2 / (Decimal) num7;
      Decimal num10 = num3 / (Decimal) num7;
      foreach (OddsInfo oddsInfo in toCalcList)
      {
        oddsInfo.WinKelly = oddsInfo.Win * num8 / new Decimal(100);
        oddsInfo.TieKelly = oddsInfo.Tie * num9 / new Decimal(100);
        oddsInfo.LoseKelly = oddsInfo.Lose * num10 / new Decimal(100);
      }
      Decimal num11 = Enumerable.Average<OddsInfo>(toCalcList, (Func<OddsInfo, Decimal>) (o => o.WinKelly));
      Decimal num12 = Enumerable.Average<OddsInfo>(toCalcList, (Func<OddsInfo, Decimal>) (o => o.TieKelly));
      Decimal num13 = Enumerable.Average<OddsInfo>(toCalcList, (Func<OddsInfo, Decimal>) (o => o.LoseKelly));
      foreach (OddsInfo oddsInfo in toCalcList)
      {
        oddsInfo.WinVar = Math.Pow((double) (oddsInfo.WinKelly - num11), 2.0) * 100.0;
        oddsInfo.TieVar = Math.Pow((double) (oddsInfo.TieKelly - num12), 2.0) * 100.0;
        oddsInfo.LoseVar = Math.Pow((double) (oddsInfo.LoseKelly - num13), 2.0) * 100.0;
      }
      double num14 = Enumerable.Average<OddsInfo>(toCalcList, (Func<OddsInfo, double>) (o => o.WinVar));
      double num15 = Enumerable.Average<OddsInfo>(toCalcList, (Func<OddsInfo, double>) (o => o.TieVar));
      double num16 = Enumerable.Average<OddsInfo>(toCalcList, (Func<OddsInfo, double>) (o => o.LoseVar));
      this._avgOdds.Add(Tuple.Create<DateTime, double, double, double, int>(time, num14 * 100.0, num15 * 100.0, num16 * 100.0, index));
      double num17 = Enumerable.Min<OddsInfo>(toCalcList, (Func<OddsInfo, double>) (o => o.WinVar));
      double num18 = Enumerable.Min<OddsInfo>(toCalcList, (Func<OddsInfo, double>) (o => o.TieVar));
      double num19 = Enumerable.Min<OddsInfo>(toCalcList, (Func<OddsInfo, double>) (o => o.LoseVar));
      this._minOdds.Add(Tuple.Create<DateTime, double, double, double, int>(time, num17 * 100.0, num18 * 100.0, num19 * 100.0, index));
    }

    private int CheckTimeRange(DateTime toCheck)
    {
      return this._SplitDates.FindIndex((Predicate<DateTime>) (d => d > toCheck));
    }

    private OddsPair GetPreviousPair(Dictionary<int, OddsPair> oddsPairDic, int currentKey)
    {
      OddsPair oddsPair = (OddsPair) null;
      if (currentKey - 1 < 0 || oddsPairDic.TryGetValue(currentKey - 1, out oddsPair))
        return oddsPair;
      return this.GetPreviousPair(oddsPairDic, currentKey - 1);
    }

    private OddsPair GetNextPair(Dictionary<int, OddsPair> oddsPairDic, int currentKey)
    {
      OddsPair oddsPair = (OddsPair) null;
      if (currentKey >= oddsPairDic.Count || oddsPairDic.TryGetValue(currentKey + 1, out oddsPair))
        return oddsPair;
      return this.GetNextPair(oddsPairDic, currentKey + 1);
    }
  }
}
