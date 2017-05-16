// Decompiled with JetBrains decompiler
// Type: OddsDataLayer.ConstantSQL
// Assembly: OddsDataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1581F622-2CF6-40F6-9EC6-DEB450D7BE33
// Assembly location: D:\BaiduYunDownload\Data\OddsDataLayer.dll

namespace OddsDataLayer
{
  public class ConstantSQL
  {
    public const string GetLastOdds = "SELECT d.*\r\nFROM {2}.dbo.OddsInfo_Daily d\r\nLEFT JOIN (\r\n\tSELECT game_id, company_id, MAX(update_time) AS update_time\r\n\tFROM {2}.dbo.OddsInfo_Daily\r\n    WHERE game_id = {0}\r\n\tGROUP BY game_id, company_id\r\n\t) m ON d.game_id = m.game_id\r\n\tAND d.company_id = m.company_id\r\n\tAND d.update_time = m.update_time\r\nLEFT JOIN Data2014.dbo.CompanyInfo c ON d.company_id = c.company_id\r\nWHERE m.game_id = {0} {1}";
    public const string GetFirstOdds = "SELECT d.*\r\nFROM {2}.dbo.OddsInfo_Daily d\r\nLEFT JOIN (\r\n\tSELECT game_id, company_id, MIN(update_time) AS update_time\r\n\tFROM {2}.dbo.OddsInfo_Daily\r\n    WHERE game_id = {0}\r\n\tGROUP BY game_id, company_id\r\n\t) m ON d.game_id = m.game_id\r\n\tAND d.company_id = m.company_id\r\n\tAND d.update_time = m.update_time\r\nLEFT JOIN Data2014.dbo.CompanyInfo c ON d.company_id = c.company_id\r\nWHERE m.game_id = {0} {1}";
  }
}
