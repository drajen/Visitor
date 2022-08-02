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
            var parameters = new { FirstName = visitor.Name, Company = visitor.Company, SignIn = DateTime.Now.ToString("yyyyMMddHHmmss"), IP=visitor.Ip_Address };
            var sql = @"INSERT INTO VISITORS (NAME,COMPANY,DATETIME_IN, DATETIME_OUT, IP_ADDRESS) VALUES (@FirstName,@Company,@SignIn,null,@IP);";
            try {
                await DapperCon.ExecuteAsync(sql, parameters);
                return true;
            } catch  {
                throw;
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
                throw;
            }
            
        }

        public async Task<List<VisitorModel>> GetVisitorsForStore(int branchNumber)
        {
            var parameters = new { branchNumber = branchNumber };
            var sql = @"select * from VISITORS where BRANCH_NUMBER = @branchNumber;";
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
