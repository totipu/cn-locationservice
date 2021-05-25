using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace StatlerWaldorfCorp.LocationService {
    public class Startup
    {
        public static string[] Args {get; set;} = new string[] {};
        private ILogger logger;
        private ILoggerFactory loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;

            this.loggerFactory = loggerFactory;
            this.loggerFactory.AddConsole(LogLevel.Information);
            this.loggerFactory.AddDebug();

            this.logger = this.loggerFactory.CreateLogger("StartUp");
        }

        /*public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional:true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);               

            Configuration = builder.Build();

            this.loggerFactory = loggerFactory;
            this.loggerFactory.AddConsole(LogLevel.Information);
            this.loggerFactory.AddDebug();

            this.logger = this.loggerFactory.CreateLogger("Startup");
        }*/

        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var transient = true;
            if (Configuration.GetSection("transient") != null) {
                transient = Boolean.Parse(Configuration.GetSection("transient").Value);
            }

            if (transient) {
                logger.LogInformation("Using transient location record repository.");
                services.AddScoped<ILocationRecordRepository, InMemoryLocationRecordRepository>();
            } else {            
                // string connectionString = Configuration.GetSection("postgres:cstr").Value;

                string pgsqlServer = Environment.GetEnvironmentVariable("PGSQL_SERVER"); // "totipulocationsvcdb.postgres.database.azure.com";
                string databaseName = Environment.GetEnvironmentVariable("PGSQL_DATABASE"); // "locationService";
                string userId = Environment.GetEnvironmentVariable("PGSQL_USERID"); // "totipu@totipulocationsvcdb";
                string password = Environment.GetEnvironmentVariable("PGSQL_PASSWORD"); // "industrija2!";

                string connectionString = $"Host={pgsqlServer};Port=5432;Database={databaseName};Username={userId};Password={password}";

                services.AddEntityFrameworkNpgsql()
                    .AddDbContext<LocationDbContext> (options => options.UseNpgsql(connectionString));

                logger.LogInformation("Using '{0}' for DB connection string.", connectionString);

                services.AddScoped<ILocationRecordRepository, LocationRecordRepository>();
            }
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}