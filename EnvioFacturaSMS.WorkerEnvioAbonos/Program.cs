using Autofac;
using Autofac.Extensions.DependencyInjection;
using EnvioFacturaSMS.Applications.Abono;
using EnvioFacturaSMS.ConceptsTest.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.ConceptsTest
{
    public class Program
    {
        public static void Main()
        {

            var services = new ServiceCollection();
            var JsonConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false).Build();

            Serilog.Events.LogEventLevel logEventLevel = (Serilog.Events.LogEventLevel)int.Parse("4");
            Log.Logger = new LoggerConfiguration().WriteTo.Console(Serilog.Events.LogEventLevel.Information).CreateLogger();

            DependencyInjection.LoadExtensions(services, JsonConfig);

            services.AddSingleton(c => Log.Logger);
            services.AddSingleton(c => JsonConfig);
            services.AddLogging(loggingBuilder =>
                  loggingBuilder.AddSerilog(dispose: true)
            );

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.Register(c => JsonConfig).As<IConfiguration>();
            var Container = builder.Build();
            using (var scope = Container.BeginLifetimeScope())
            {

                var BrokerConnection = scope.Resolve<IConnection>() ?? throw new Exception("No broker connection");
                    
                var QueueName = JsonConfig.GetValue<string>("RabbitMQ:QueueName");
                ManualResetEvent QuitEvent = new ManualResetEvent(false);
                Console.CancelKeyPress += (sender, eArgs) =>
                {
                    QuitEvent.Set();
                    eArgs.Cancel = true;
                };

                using (var channel = BrokerConnection.CreateModel())
                {
                    channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);
                    channel.ConfirmSelect();
                    AsyncEventingBasicConsumer Consumer = new AsyncEventingBasicConsumer(channel);

                    Consumer.Received += async (sender, @event) =>
                    {
                        await Task.Run(async () =>
                        {
                            string Message = Encoding.UTF8.GetString(@event.Body.ToArray());
                            EnviarAbonoQuery SmsAbono = JsonConvert.DeserializeObject<EnviarAbonoQuery>(Message);
                            var mediator = scope.Resolve<IMediator>();
                            await mediator.Send(SmsAbono);

                        }).ContinueWith(task => {
                            if (task.IsFaulted)
                            {
                                Log.Logger.Error($"Error: {task.Exception.Message}, Message: {Encoding.UTF8.GetString(@event.Body.ToArray())}");
                                channel.BasicNack(@event.DeliveryTag, false, false);
                            }
                            else
                            {
                                channel.BasicAck(@event.DeliveryTag, false);
                            }
                        });
                    };
                    channel.BasicConsume(queue: QueueName, autoAck: false, consumer: Consumer);
                    QuitEvent.WaitOne();
                }
                
            }

        }
    }
}
