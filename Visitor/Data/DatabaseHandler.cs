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

        public async Task<bool> AddVisitor(VisitorModel visitor) {
            var parameters = new { FirstName = visitor.FirstName, LastName = visitor.LastName, Company = visitor.Company, ContactNumber = visitor.ContactNumber, Reason = visitor.Reason, Datetime_In = DateTime.Now.ToString("yyyyMMddHHmmss"), IP_ADDRESS = visitor.Ip_Address };
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
            var parameters = new { id = id, signOut = DateTime.Now.ToString("yyyyMMddHHmmss") };
            var sql = @"UPDATE VISITORS SET DATETIME_OUT=@signOut where id = @id;";
            try {
                await DapperCon.ExecuteAsync(sql, parameters);
                return true;
            } catch {
                return false;
            }
            
        }

        public async Task<List<VisitorModel>> GetVisitorsForStore(string ip_address)
        {
            var parameters = new { ip_address = ip_address };
            var sql = @"select * from VISITORS where Ip_Address = @ip_address;";
            try {
                return DapperCon.QueryAsync<VisitorModel>(sql, parameters).Result.ToList();
            } catch {
                throw;
            }
        }

        public async Task<List<VisitorModel>> GetVisitorsForStoreByBranchNum(int branchNumber)
        {
            var parameters = new { branchNumber = branchNumber };
            var sql = @"select * from VISITORS where BRANCH_NUMBER = @branchNumber;";
            try {
                return DapperCon.QueryAsync<VisitorModel>(sql, parameters).Result.ToList();
            } catch {
                throw;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
