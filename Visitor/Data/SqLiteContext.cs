using Microsoft.Data.Sqlite;
using NPoco;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Visitor.Entities;

namespace Visitor.Data
{
    public class SqLiteContext: Database, IDatabaseContext
    {
        private readonly IDatabase _database;

        public SqLiteContext(string connection) : base(new SQLiteConnection(connection), DatabaseType.SQLite)
        {
            var dbFactory = DatabaseFactory.Config(x =>
            {
                x.UsingDatabase(() => new Database(new SqliteConnection(connection)));
            });
            _database = dbFactory.Build(this);
        }

        public void CreateDatabase()
        {
            Console.WriteLine("Creating Database stuff");
            CreateDataBaseTables();
        }

        private void CreateDataBaseTables()
        {
            var cmd = _database.Connection.CreateCommand();

            cmd.CommandText = GetVisitorTableQuery();
            cmd.ExecuteNonQuery();
            cmd.CommandText = CreateBranchTableQuery();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void CreateAndSeedDatabase()
        {
            _database.Connection.Open();
            CreateDatabase();

            SeedVisitors();
            SeedBranches();
            _database.Connection.Close();
        }

        public static string GetVisitorTableQuery()
        {
            return @"CREATE TABLE IF NOT EXISTS VISITORS
                      (
                         id                     INTEGER PRIMARY KEY,
                         Name                   NVARCHAR(30),
                         Company                NVARCHAR(30),
                         Datetime_In            DATE,
                         Datetime_Out           DATE,
                         Ip_Address              NVARCHAR(10)
                      );";
        }

        public static string CreateBranchTableQuery() {
            return @"CREATE TABLE IF NOT EXISTS REF_BRANCH
                      (
                        BRANCH_NUMBER           INTEGER PRIMARY KEY,
                        BRANCH_NAME             NVARCHAR(20),
                        COMPANY_NUMBER          INTEGER,
                        STOCK_COMPANY_NUMBER    INTEGER,
                        AREA_CODE               NVARCHAR(2),
                        BRANCH_ADDRESS_1        NVARCHAR(30),
                        BRANCH_ADDRESS_2        NVARCHAR(30),
                        BRANCH_ADDRESS_3        NVARCHAR(30),
                        BRANCH_ADDRESS_4        NVARCHAR(30),
                        BRANCH_STATUS           NVARCHAR(1),
                        EFFECTIVE_DATE          DATE,
                        MANAGER_NAME            NVARCHAR(30),
                        TELEPHONE_NUMBER        NVARCHAR(15),
                        NUMBER_OF_TILLS         INTEGER,
                        EPOS_SOFTWARE_RELEASE   INTEGER,
                        WAGE_COST               INTEGER,
                        STAFF_DISCOUNT_PCENT    INTEGER,
                        COMMISSION_RATE         NVARCHAR(1),
                        BRANCH_TYPE_ID          INTEGER,
                        COUNTRY_CODE            NVARCHAR(3),
                        MARKET_TYPE_ID          INTEGER,
                        NUMBER_OF_MOBILE_TILLS  INTEGER
                      );";
        }

        void SeedVisitors()
        {
            List<VisitorEntity> visitors = new List<VisitorEntity>()
            {
                new VisitorEntity()
                {
                    Id = 1,
                    Name = "Tester",
                    Company = "M&Co",
                    Datetime_In = DateTime.Now,
                    Datetime_Out = null,
                    Ip_Address = "192.2.85.100"
                }
            };
            _database.InsertBulk(visitors);
        }
        void SeedBranches()
        {
            List<BranchEntity> branches = new List<BranchEntity>()
            {
                new BranchEntity()
                {
                    Branch_Number = 285,
                    Branch_Name = "PAISLEY",
                    Company_Number = 1,
                    Stock_Company_Number = 1,
                    Area_Code = "22",
                    Branch_Address_1 = "UNITS 67  55 CENTRAL WAY",
                    Branch_Address_2 = "THE PIAZZA PAISLEY",
                    Branch_Address_3 = "RENFREWSHIRE",
                    Branch_Address_4 = "PA1 1EN",
                    Branch_Status = 'O',
                    Effective_Date = DateTime.Parse("11-SEP-98"),
                    Manager_Name = "",
                    Telephone_Number = "01418491325",
                    Number_Of_Tills = 3,
                    Epos_Software_Release = 3,
                    Wage_Cost = 300,
                    Staff_Discount_Pcent = 30,
                    Commission_Rate = "B",
                    Branch_Type_Id = 0,
                    Country_Code = "GB",
                    Market_Type_Id = 3,
                    Number_Of_Mobile_Tills =1
                }
            };
            _database.InsertBulk(branches);
        }

    }
}