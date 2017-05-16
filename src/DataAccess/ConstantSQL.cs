// Decompiled with JetBrains decompiler
// Type: DataAccess.ConstantSQL
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

namespace DataAccess
{
  public class ConstantSQL
  {
    public const string GetLastOdds = "SELECT d.*\r\nFROM FootballSina.dbo.OddsInfo_Daily d\r\nLEFT JOIN (\r\n\tSELECT game_id, company_id, MAX(update_time) AS update_time\r\n\tFROM FootballSina.dbo.OddsInfo_Daily\r\n\tGROUP BY game_id, company_id\r\n\t) m ON d.game_id = m.game_id\r\n\tAND d.company_id = m.company_id\r\n\tAND d.update_time = m.update_time\r\nLEFT JOIN FootballSina.dbo.CompanyInfo c ON d.company_id = c.company_id\r\nWHERE m.game_id = {0} AND c.is_leading IN ({1})";
    public const string GetFirstOdds = "SELECT d.*\r\nFROM FootballSina.dbo.OddsInfo_Daily d\r\nLEFT JOIN (\r\n\tSELECT game_id, company_id, MIN(update_time) AS update_time\r\n\tFROM FootballSina.dbo.OddsInfo_Daily\r\n\tGROUP BY game_id, company_id\r\n\t) m ON d.game_id = m.game_id\r\n\tAND d.company_id = m.company_id\r\n\tAND d.update_time = m.update_time\r\nLEFT JOIN FootballSina.dbo.CompanyInfo c ON d.company_id = c.company_id\r\nWHERE m.game_id = {0} AND c.is_leading IN ({1})";
  }
}
