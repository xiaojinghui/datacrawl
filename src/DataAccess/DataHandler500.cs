// Decompiled with JetBrains decompiler
// Type: DataAccess.DataHandler500
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
  public class DataHandler500
  {
    private const string _OddsTableModel = "\r\nCREATE TABLE [dbo].[{0}](\r\n\t[odds_id] [int] IDENTITY (1, 1) NOT NULL,\r\n\t[game_id] [int] NOT NULL,\r\n\t[company_id] [int] NOT NULL,\r\n\t[win] [numeric](18, 3) NULL,\r\n\t[tie] [numeric](18, 3) NULL,\r\n\t[lose] [numeric](18, 3) NULL,\r\n\t[update_time] [datetime] NOT NULL,\r\n CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED \r\n(\r\n\t[odds_id] ASC\r\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n";
    private const string _OddsTableModel2 = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U')) \r\nDROP TABLE {0};";

    public static void CreateOddsTables()
    {
      string cmdText = "select distinct odds_table from LeagueInfo";
      List<string> list = new List<string>();
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, cmdText, new SqlParameter[0]))
      {
        while (dataReader.Read())
          list.Add(dataReader.GetString(0));
      }
      foreach (string str in list)
        SqlServerHelper.ExecuteNonQuery(CommandType.Text, string.Format("IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U')) \r\nDROP TABLE {0};", (object) str.Trim()), new SqlParameter[0]);
      foreach (string str in list)
        SqlServerHelper.ExecuteNonQuery(CommandType.Text, string.Format("\r\nCREATE TABLE [dbo].[{0}](\r\n\t[odds_id] [int] IDENTITY (1, 1) NOT NULL,\r\n\t[game_id] [int] NOT NULL,\r\n\t[company_id] [int] NOT NULL,\r\n\t[win] [numeric](18, 3) NULL,\r\n\t[tie] [numeric](18, 3) NULL,\r\n\t[lose] [numeric](18, 3) NULL,\r\n\t[update_time] [datetime] NOT NULL,\r\n CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED \r\n(\r\n\t[odds_id] ASC\r\n)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n", (object) str.Trim()), new SqlParameter[0]);
    }

    public static void SaveLeagueInfo(string area, string country, List<string> leagues)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in leagues)
        stringBuilder.AppendFormat("INSERT INTO LeagueInfo (name, country, area, odds_table) \r\nSELECT '{0}', '{1}', '{2}', '' WHERE NOT EXISTS (SELECT 1 FROM LeagueInfo WHERE name='{0}');", (object) str, (object) country, (object) area);
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), new SqlParameter[0]);
    }

    public static void SaveCompanyInfo(string id, string name, string country, int isLeading)
    {
      string cmdText = "INSERT INTO CompanyInfo (company_id, name, country, is_leading) \r\nSELECT @company_id, @name, @country, @is_leading WHERE NOT EXISTS (SELECT 1 FROM CompanyInfo WHERE company_id=@company_id);";
      SqlParameter[] sqlParameterArray = new SqlParameter[4];
      sqlParameterArray[0] = new SqlParameter("@company_id", (object) id);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@name", (object) name);
      sqlParameterArray[1].DbType = DbType.AnsiString;
      sqlParameterArray[2] = new SqlParameter("@country", (object) country);
      sqlParameterArray[2].DbType = DbType.AnsiString;
      sqlParameterArray[3] = new SqlParameter("@is_leading", (object) isLeading);
      sqlParameterArray[3].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveGameInfo(GameInfo game, string period, int leagueId)
    {
      string cmdText = "SET NOCOUNT ON;\r\nINSERT INTO GameInfo (id, league_id, league_name, periods, serial, game_time, host, host_rank, guest, guest_rank, score_half, score_final) \r\nSELECT @id, @league_id, @league_name, @periods, @serial, @game_time, @host, @host_rank, @guest, @guest_rank, @score_half, @score_final \r\nWHERE NOT EXISTS (SELECT 1 FROM GameInfo WHERE id=@id);\r\nSET NOCOUNT OFF;";
      SqlParameter[] sqlParameterArray = new SqlParameter[12];
      sqlParameterArray[0] = new SqlParameter("@id", (object) game.GameId);
      sqlParameterArray[0].DbType = DbType.Int32;
      sqlParameterArray[1] = new SqlParameter("@league_id", (object) leagueId);
      sqlParameterArray[1].DbType = DbType.Int32;
      sqlParameterArray[2] = new SqlParameter("@game_time", (object) game.GameTime);
      sqlParameterArray[2].DbType = DbType.DateTime;
      sqlParameterArray[3] = new SqlParameter("@host", (object) game.Host);
      sqlParameterArray[3].DbType = DbType.AnsiString;
      sqlParameterArray[4] = new SqlParameter("@host_rank", string.IsNullOrEmpty(game.HostRank) ? (object) "" : (object) game.HostRank);
      sqlParameterArray[4].DbType = DbType.AnsiString;
      sqlParameterArray[5] = new SqlParameter("@guest", (object) game.Guest);
      sqlParameterArray[5].DbType = DbType.AnsiString;
      sqlParameterArray[6] = new SqlParameter("@guest_rank", string.IsNullOrEmpty(game.GuestRank) ? (object) "" : (object) game.GuestRank);
      sqlParameterArray[6].DbType = DbType.AnsiString;
      sqlParameterArray[7] = new SqlParameter("@score_half", string.IsNullOrEmpty(game.HalfScore) ? (object) "" : (object) game.HalfScore);
      sqlParameterArray[7].DbType = DbType.AnsiString;
      sqlParameterArray[8] = new SqlParameter("@score_final", string.IsNullOrEmpty(game.FinalScore) ? (object) "" : (object) game.FinalScore);
      sqlParameterArray[8].DbType = DbType.AnsiString;
      sqlParameterArray[9] = new SqlParameter("@league_name", (object) game.League);
      sqlParameterArray[9].DbType = DbType.AnsiString;
      sqlParameterArray[10] = new SqlParameter("@periods", (object) period);
      sqlParameterArray[10].DbType = DbType.Int32;
      sqlParameterArray[11] = new SqlParameter("@serial", (object) game.Serial);
      sqlParameterArray[11].DbType = DbType.Int32;
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, cmdText, sqlParameterArray);
    }

    public static void SaveOddsInfo(IEnumerable<OddsInfo> oddsInfo, string tableName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (OddsInfo oddsInfo1 in oddsInfo)
        stringBuilder.AppendFormat("INSERT INTO {6} (game_Id, company_id, win, tie, lose, update_time) SELECT {0}, {1}, {2}, {3}, {4}, '{5}'\r\nWHERE NOT EXISTS (SELECT 1 FROM {6} WHERE game_Id={0} and company_id={1} and update_time='{5}');", (object) oddsInfo1.GameId, (object) oddsInfo1.CompanyId, (object) oddsInfo1.Win, (object) oddsInfo1.Tie, (object) oddsInfo1.Lose, (object) oddsInfo1.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"), (object) tableName);
      SqlServerHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), new SqlParameter[0]);
    }

    public static List<LeagueInfo> GetAllLeagues()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, string.Format("select * from LeagueInfo order by Name"), new SqlParameter[0]))
      {
        List<LeagueInfo> list = new List<LeagueInfo>();
        while (dataReader.Read())
          list.Add(new LeagueInfo()
          {
            LeagueId = dataReader.GetInt32(0),
            Name = dataReader.GetString(1),
            Country = dataReader.GetString(2),
            Area = dataReader.GetString(3),
            OddsTable = dataReader.GetString(4),
            Level = dataReader.GetInt32(5),
            DataCollect = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetString(6)
          });
        return list;
      }
    }

    public static Dictionary<int, Company> GetAllCompanyInfos()
    {
      using (IDataReader dataReader = (IDataReader) SqlServerHelper.ExecuteReader(CommandType.Text, "select * from CompanyInfo", new SqlParameter[0]))
      {
        Dictionary<int, Company> dictionary = new Dictionary<int, Company>();
        while (dataReader.Read())
        {
          Company company = new Company();
          company.Id = dataReader.GetInt32(0);
          company.Name = dataReader.GetString(1);
          company.Country = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
          company.IsLeading = dataReader.GetInt32(4);
          dictionary[company.Id] = company;
        }
        return dictionary;
      }
    }
  }
}
