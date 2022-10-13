using EnvioFacturaSMS.Applications.CQRS.Contratos;
using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Domain.Services;
using EnvioFacturaSMS.Infraestructure.Context;
using EnvioFacturaSMS.Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.CronJobContratos
{
    public class Program
    {
        public static async Task Main()
        {
            var Container = CreateHostBuilder().Build();
            using (var scope = Container.Services.CreateScope())
            {
                var _logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    EnviarContratosQuery SmsContratos = new EnviarContratosQuery();

                    var _config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    SmsContratos.TimesTamp = (Environment.GetEnvironmentVariable("FECHA_ENVIO") != null) ? DateTime.Parse(Environment.GetEnvironmentVariable("FECHA_ENVIO")) : _config.GetValue<DateTime>("FechaEnvio");

                    var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await _mediator.Send(SmsContratos);
                    _logger.LogInformation($"{DateTime.Now.ToString("yyyy-m-d HH:mm:s")}: Proceso finalizado");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{DateTime.Now.ToString("yyyy-m-d HH:mm:s")}: {ex.Message}");
                    throw;
                }
            }

        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    var JsonConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", false).Build();
                    services.AddSingleton(c => JsonConfig);

                    services.AddMediatR(Assembly.Load("EnvioFacturaSMS.Applications"), Assembly.GetExecutingAssembly());
                    services.AddMediatR(Assembly.Load("EnvioFacturaSMS.Domain"), Assembly.GetExecutingAssembly());
                    services.AddMediatR(Assembly.Load("EnvioFacturaSMS.Infraestructure"), Assembly.GetExecutingAssembly());

                    var sqlServerConnection = Environment.GetEnvironmentVariable("DB_SERVER") ?? JsonConfig.GetConnectionString("SqlConection");
                    services.AddDbContext<PrivadaContext>(x => x.UseSqlServer(sqlServerConnection), ServiceLifetime.Transient);

                    services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                    services.AddTransient<IContratoService, ContratoService>();
                    services.AddTransient<ILogService, LogService>();
                    services.AddTransient<IMasivianService, MasivianService>();
                    services.AddTransient<IParametrosService, ParametrosService>();
                    services.AddTransient<IClienteService, ClienteService>();

                    int logLevelEnv = JsonConfig.GetValue<int>("LogLevel");
                    if (Environment.GetEnvironmentVariable("LOG_LEVEL") != null) logLevelEnv = int.Parse(Environment.GetEnvironmentVariable("LOG_LEVEL"));
                    Log.Logger = new LoggerConfiguration().WriteTo.Console((Serilog.Events.LogEventLevel)logLevelEnv).CreateLogger();
                });
    }
}
