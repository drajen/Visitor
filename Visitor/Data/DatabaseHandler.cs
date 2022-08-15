using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Visitor.Entities;
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

        public async Task<bool> AddVisitor(VisitorModel visitor) {
            var parameters = new { FirstName = visitor.FirstName, LastName = visitor.LastName, Company = visitor.Company, ContactNumber = visitor.ContactNumber, Reason = visitor.Reason, Datetime_In = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), IP_ADDRESS = visitor.Ip_Address };
            var sql = @"INSERT INTO VISITORS (FIRSTNAME, LASTNAME, COMPANY, CONTACTNUMBER, REASON, DATETIME_IN, DATETIME_OUT, IP_ADDRESS) VALUES (@FirstName,@LastName,@Company,@ContactNumber,@Reason,@Datetime_In,null,@Ip_Address);";
            try {
                await DapperCon.ExecuteAsync(sql, parameters);
                return true;
            } catch  {
                return false;
            }
        }

        public async Task<bool> UpdateVisitor(int id)
        {
            var parameters = new { id = id, signOut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            var sql = @"UPDATE VISITORS SET DATETIME_OUT=@signOut where id = @id;";
            try {
                await DapperCon.ExecuteAsync(sql, parameters);
                return true;
            } catch {
                return false;
            }
            
        }

        public async Task<List<VisitorModel>> GetVisitorsForStore(string ipAddress, DateTime? date = null)
        {
            if (date == null)
                date = DateTime.Now;
            var parameters = new { ip = ipAddress, startDate = $"{date.Value.ToString("yyyy-MM-dd")} 00:00:00", endDate = $"{date.Value.ToString("yyyy-MM-dd")} 23:59:59" };
            var sql = @"select * from VISITORS where IP_ADDRESS = @ip and DATETIME_IN between @startDate and @endDate;";
            try {
                return DapperCon.QueryAsync<VisitorModel>(sql, parameters).Result.ToList();
            } catch {
                throw;
            }
        }

        public async Task<List<VisitorModel>> GetVisitorsForStoreByBranchNum(int branchNumber, DateTime? date = null) {

            if (date == null)
                date = DateTime.Now;
            var secondOctet = branchNumber % 100;
            var firstOctet = (branchNumber - secondOctet) / 100;
            var ipString = $"192.{firstOctet}.{secondOctet}.%";
            var parameters = new { ipString = ipString, startDate = $"{date.Value.ToString("yyyy-MM-dd")} 00:00:00", endDate = $"{date.Value.ToString("yyyy-MM-dd")} 23:59:59" };
            var sql = @"select * from VISITORS where IP_ADDRESS like @ipString and DATETIME_IN between @startDate and @endDate;";

            try {
                return DapperCon.QueryAsync<VisitorModel>(sql, parameters).Result.ToList();
            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<List<BranchModel>> GetBranches()
        {
            var sql = @"select * from REF_BRANCH order by BRANCH_NAME";
            try {
                return DapperCon.QueryAsync<BranchEntity>(sql).Result
                    .Select(x => new BranchModel { Number = x.Branch_Number, Name = x.Branch_Name })
                    .ToList();
            } catch {
                throw;
            }
        }

        public void Dispose() {
            DapperCon.Dispose();
        }
    }
}
