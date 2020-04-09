using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Dwapi.Bot.Core.Domain.Common;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.Core.Utility;
using Dwapi.Bot.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;
        public static IServiceCollection Services;
        public static IServiceCollection ServicesOnly;
        public static string ConnectionString;
        public static string MpiConnectionString;
        public static IConfigurationRoot Configuration;

        [OneTimeSetUp]
        public void Init()
        {
            SqlMapper.AddTypeHandler<Guid>(new GuidTypeHandler());
            RegisterLicence();
            RemoveTestsFilesDbs();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var dir = $"{TestContext.CurrentContext.TestDirectory.HasToEndWith(@"/")}";

            var config = Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var mpiConnectionString = config.GetConnectionString("mpiConnection")
                .Replace("#dir#", dir);
            MpiConnectionString = mpiConnectionString.ToOsStyle();

            var connectionString = config.GetConnectionString("liveConnection")
                .Replace("#dir#", dir);
            ConnectionString = connectionString.ToOsStyle();
            var connection = new SqliteConnection(connectionString.Replace(".db", $"{DateTime.Now.Ticks}.db"));
            connection.Open();

            var services = new ServiceCollection().AddDbContext<BotContext>(x => x.UseSqlite(connection));

            services
                .AddTransient<BotContext>()
                .AddTransient<IJaroWinklerScorer, JaroWinklerScorer>()
                .AddTransient<IMasterPatientIndexReader>(s=>new MasterPatientIndexReader(new DataSourceInfo(DbType.SQLite,mpiConnectionString)))
                .AddTransient<IPatientIndexRepository, PatientIndexRepository>();

            Services = services;

            ServicesOnly = Services;
            ServiceProvider = Services.BuildServiceProvider();
        }

        public static void ClearDb()
        {
            var context = ServiceProvider.GetService<BotContext>();
            context.Database.EnsureCreated();
        }
        public static void SeedData(params IEnumerable<object>[] entities)
        {
            var context = ServiceProvider.GetService<BotContext>();
            foreach (IEnumerable<object> t in entities)
            {
                context.AddRange(t);
            }
            context.SaveChanges();
        }

        private void RegisterLicence()
        {
            DapperPlusManager.AddLicense("1755;700-ThePalladiumGroup", "2073303b-0cfc-fbb9-d45f-1723bb282a3c");
            if (!Z.Dapper.Plus.DapperPlusManager.ValidateLicense(out var licenseErrorMessage))
            {
                throw new Exception(licenseErrorMessage);
            }
        }

        private void RemoveTestsFilesDbs()
        {
            string[] keyFiles =
                { "dwapibot.db","mpi.db"};
            string[] keyDirs = { @"TestArtifacts/Database".ToOsStyle()};

            foreach (var keyDir in keyDirs)
            {
                DirectoryInfo di = new DirectoryInfo(keyDir);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (!keyFiles.Contains(file.Name))
                        file.Delete();
                }
            }
        }
    }
}
