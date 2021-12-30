using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using HangfireSerilogEx.Hangfire.Filters;
using HangfireSerilogEx.Hangfire.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireSerilogEx.Hangfire.Config
{
    public class HangfireConfig
    {
        public static void HangFireServices(IServiceCollection services, string connectionString)
        {
            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(connectionString,
                    new SqlServerStorageOptions
                    {
                        SchemaName = "Exemplo",
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }).UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
            });

            services.AddHangfireServer();
        }

        public static void HangfireConfigure(IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                //Configura o dashboar como somente leitura
                IsReadOnlyFunc = (DashboardContext context) => true,

                //Adiciona o filtro de autorização
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            RecurringJob.AddOrUpdate<BuscarHoraAtual>("BuscarHoraAtual", x => x.Run(), Cron.Minutely());
        }
    }
}
