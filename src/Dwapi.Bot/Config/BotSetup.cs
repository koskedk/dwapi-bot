using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dwapi.Bot.Config
{
    public class BotSetup
    {
        public static void Initialize(IConfiguration config)
        {
            Log.Debug("Initializing Job server...");

            var connectionString = config.GetConnectionString("jobzConnection");
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<BotJobsContext>(options =>
                options.UseSqlServer(connectionString));

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<BotJobsContext>();
                    context.Database.EnsureCreated();
                    context.Manifests.Add(new Manifest("Dwapi.Bot", GetVersion()));
                    context.SaveChanges();
                    Log.Debug($"Enrolled Dwapi.Bot v({GetVersion()})");
                }
            }
        }

        private static string GetVersion()
        {
            var ver = typeof(BotSetup).Assembly.GetName().Version;

            if (null == ver)
                return "1.0.0.0";

            return $"{ver.Major}.{ver.Minor}.{ver.Build}{ver.Revision}";
        }
    }
}
