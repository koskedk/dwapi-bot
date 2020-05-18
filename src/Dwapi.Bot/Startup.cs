using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.Infrastructure;
using Dwapi.Bot.Infrastructure.Data;
using Dwapi.Bot.SharedKernel.Common;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Z.Dapper.Plus;

namespace Dwapi.Bot
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment,IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var connectionString = Configuration["ConnectionStrings:botConnection"];
            var mpiConnectionString = Configuration["ConnectionStrings:mpiConnection"];


            services.AddDbContext<BotContext>(o => o.UseSqlServer(
                    connectionString,
                    x => x.MigrationsAssembly(typeof(BotContext).GetTypeInfo().Assembly.GetName().Name)
                )
            );

            services.AddTransient<IJaroWinklerScorer, JaroWinklerScorer>();
            services.AddTransient<IMasterPatientIndexReader>(s =>
                new MasterPatientIndexReader(new DataSourceInfo(DbType.MsSQL, mpiConnectionString)));
            services.AddScoped<IMatchConfigRepository, MatchConfigRepository>();
            services.AddScoped<ISubjectIndexRepository, SubjectIndexRepository>();
            services.AddMediatR(typeof(RefreshIndex).Assembly, typeof(IndexRefreshed).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(
                builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<SubjectIndexProfile>();
                }
            );

            try
            {
                DapperPlusManager.AddLicense("1755;700-ThePalladiumGroup", "2073303b-0cfc-fbb9-d45f-1723bb282a3c");
                if (!DapperPlusManager.ValidateLicense(out var licenseErrorMessage))
                {
                    throw new Exception(licenseErrorMessage);
                }
            }
            catch (Exception e)
            {
                Log.Debug($"{e}");
                throw;
            }

            EnsureMigrationOfContext<BotContext>(serviceProvider);
            Log.Debug(@"initializing Database [Complete]");
            Log.Debug(
                @"---------------------------------------------------------------------------------------------------");
            Log.Debug
            (@"
                                          _____                      _
                                         |  __ \                    (_)
                                         | |  | |_      ____ _ _ __  _
                                         | |  | \ \ /\ / / _` | '_ \| |
                                         | |__| |\ V  V / (_| | |_) | |
                                         |_____/  \_/\_/ \__,_| .__/|_|
                                                              | |
                                                              |_|
");
            Log.Debug(
                @"---------------------------------------------------------------------------------------------------");
            Log.Debug("Dwapi.Bot started !");
        }

          public static void EnsureMigrationOfContext<T>(IServiceProvider app) where T : BotContext
          {
              var contextName = typeof(T).Name;
              Log.Debug($"initializing Database context: {contextName}");
              var context = app.GetService<T>();
              try
              {
                  context.Database.Migrate();
                  context.EnsureSeeded();
                  Log.Debug($"initializing Database context: {contextName} [OK]");
              }
              catch (Exception e)
              {

                  Log.Fatal($"initializing Database context: {contextName} Error");
                  Log.Fatal($"{e}");
              }

          }
    }
}
