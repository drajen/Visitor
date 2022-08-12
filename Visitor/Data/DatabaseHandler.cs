using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Visitor.Models;

namespace Visitor.Data
{
    public class DatabaseHandler: IDisposable
    {
        private static SQLiteConnection _dbConnection;

        public IDbConnection DapperCon;

        public static IConfiguration Configuration { get; set; }

       

        private string GetConnectionString()
        {
            return Configuration.GetConnectionString("test");
        }

        public DatabaseHandler(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            if (hostEnvironment.IsDevelopment())
            {
                var con = GetConnectionString().ToString();
                Console.WriteLine(con);
                _dbConnection = new SQLiteConnection(con);
                DapperCon = _dbConnection;

            }
            else
            {
                
            }

            DapperCon.Open();
        }

        public async Task<bool> AddVisitor(VisitorModel visitor)
        {
            var parameters = new { FirstName = visitor.FirstName, LastName = visitor.LastName, Company = visitor.Company, ContactNumber = visitor.ContactNumber, Reason = visitor.Reason, Datetime_In = DateTime.Now.ToString("yyyyMMddHHmmss"), IP_ADDRESS = visitor.Ip_Address };
            var sql = @"INSERT INTO VISITORS (FIRSTNAME, LASTNAME, COMPANY, CONTACTNUMBER, REASON, DATETIME_IN, DATETIME_OUT, IP_ADDRESS) VALUES (@FirstName,@LastName,@Company,@ContactNumber,@Reason,@Datetime_In,null,@Ip_Address);";
            await DapperCon.ExecuteAsync(sql,parameters);
            return true;
        }

        public async Task<bool> UpdateVisitor(int id)
        {
            var sql = @"UPDATE VISITORS SET DATETIME_OUT= '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where id = " + id + ";";
            await DapperCon.ExecuteAsync(sql);
            return true;
        }

        public async Task<List<VisitorModel>> GetVisitorsForStore(string ipAddress)
        {
            var sql = @"select * from VISITORS where IP_ADDRESS = '" + ipAddress + "';";
            IEnumerable<VisitorModel> VisitorList = await DapperCon.QueryAsync<VisitorModel>(sql);
            List<VisitorModel> StoreVisitorList = VisitorList.ToList();
            return StoreVisitorList;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
