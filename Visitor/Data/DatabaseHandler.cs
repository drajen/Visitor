﻿using Dapper;
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

        public async void AddVisitor(VisitorModel visitor)
        {
            var sql = @"INSERT INTO VISITORS (NAME,COMPANY,DATETIME_IN, DATETIME_OUT, IP_ADDRESS) VALUES ('" + visitor.Name + "','" + visitor.Company + "','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',null,'" + visitor.Ip_Address + "');";
            await DapperCon.ExecuteAsync(sql);
        }

        public async void UpdateVisitor(int id)
        {
            var sql = @"UPDATE VISITORS SET DATETIME_OUT= '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' where id = " + id + ";";
            await DapperCon.ExecuteAsync(sql);
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
