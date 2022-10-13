using Autofac;
using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Domain.Services;
using EnvioFacturaSMS.Infraestructure.Context;
using EnvioFacturaSMS.Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Globalization;
using System.Reflection;

namespace EnvioFacturaSMS.ConceptsTest.Extensions
{
    public static class DependencyInjection
    {

        public static IServiceCollection LoadExtensions(this IServiceCollection services, IConfiguration JsonConfig)
        {
            services.AddMediatR(Assembly.Load("EnvioFacturaSMS.Applications"), Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.Load("EnvioFacturaSMS.Domain"), Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.Load("EnvioFacturaSMS.Infraestructure"), Assembly.GetExecutingAssembly());
            var sqlServerConnection = Environment.GetEnvironmentVariable("DB_SERVER") ?? JsonConfig.GetConnectionString("SqlConection");
            services.AddDbContext<PrivadaContext>(x => x.UseSqlServer(sqlServerConnection), ServiceLifetime.Transient);
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IAzureService), typeof(AzureService));
            services.AddTransient(typeof(IAbonoService), typeof(AbonosServices));
            services.AddTransient(typeof(ILogService), typeof(LogService));
            services.AddTransient(typeof(IMasivianService), typeof(MasivianService));
            services.AddTransient(typeof(IParametrosService), typeof(ParametrosService));

            string HostRabbit = JsonConfig.GetValue<string>("RabbitMQ:HostName");
            string UserRabbit = JsonConfig.GetValue<string>("RabbitMQ:UserName");
            string PasswordRabbit = JsonConfig.GetValue<string>("RabbitMQ:Password");
            int PortRabbit = JsonConfig.GetValue<int>("RabbitMQ:Port");

            ConnectionFactory Factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? HostRabbit,
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? UserRabbit,
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? PasswordRabbit,
                Port = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RABBITMQ_PORT")) 
                    ? Int32.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), CultureInfo.InvariantCulture) : PortRabbit,
                VirtualHost = "/",
                DispatchConsumersAsync = true
            };

            services.AddSingleton(lmbd => Factory.CreateConnection("AGAVAL_SMS_ABONO_RECEIVE"));

            return services;
        }

        public static ContainerBuilder LoadExtensions(this ContainerBuilder builder, IConfiguration JsonConfig)
        {
            builder.Register(c => JsonConfig).As<IConfiguration>();

            return builder;
        }
    }
}
