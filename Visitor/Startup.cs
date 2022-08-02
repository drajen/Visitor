using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Visitor.Data;
using Visitor.Services;

namespace Visitor
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        private IHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();

            if (HostEnvironment.IsDevelopment())
            {
                Console.WriteLine($"{DateTime.Now.ToString()} Development Mode");
                string testDbPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "storeVisitor.db3");
                File.Delete(testDbPath);

                var testDbDataSource = $"DataSource={testDbPath}";
                Console.WriteLine(testDbDataSource);
                var createdDatabase = new SqLiteContext($"{testDbDataSource};New=True");
                createdDatabase.CreateAndSeedDatabase();

                services.AddScoped<IDatabaseContext, SqLiteContext>(provider =>
                {
                    return new SqLiteContext(testDbDataSource);
                });
                  
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToString()} Production Mode");
                /*    services.AddScoped<IDatabaseContext, OracleContext>(provider =>
                    {
                        var oracleContext = new OracleContext(Configuration.GetConnectionString("OracleConnection"));
                        return oracleContext;
                    });
                    services.AddScoped<IUnitOfWork, UnitOfWork>();*/
            }
            services.AddSingleton<DatabaseHandler>();
            services.AddScoped<IIpTools, IpTools>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
