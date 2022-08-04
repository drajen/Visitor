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

            cmd.Dispose();
        }

        public void CreateAndSeedDatabase()
        {
            _database.Connection.Open();
            CreateDatabase();

            SeedVisitors();
  
            _database.Connection.Close();
        }

        public static string GetVisitorTableQuery()
        {
            return @"CREATE TABLE IF NOT EXISTS VISITORS
                      (
                         id                     INTEGER PRIMARY KEY,
                         FirstName              NVARCHAR(30),
                         LastName               NVARCHAR(30),
                         Company                NVARCHAR(30),
                         ContactNumber          NVARCHAR(30),
                         Reason                 NVARCHAR(500),
                         Datetime_In            DATE,
                         Datetime_Out           DATE,
                         Ip_Address             NVARCHAR(10)
                      );";
        }
        void SeedVisitors()
        {
            List<VisitorEntity> visitors = new List<VisitorEntity>()
            {
                new VisitorEntity()
                {
                    Id = 1,
                    FirstName = "Dharrish",
                    LastName = "Rajendram",
                    Company = "M&Co",
                    ContactNumber = "07555975986",
                    Reason = "Shopping",
                    Datetime_In = DateTime.Now,
                    Datetime_Out = null,
                    Ip_Address = "1.1.1.1"

                }


            };
            _database.InsertBulk(visitors);
        }


    }
}
