using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RestSharp;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class AbonosServices : IAbonoService
    {
        private readonly IConfiguration _configurations;

        public AbonosServices(IConfiguration configuration)
        {
            _configurations = configuration;
        }

        public async Task<ResponseApiAbonos> GetBytesAsync(string Factura, string Cedula)
        {
            var Api = Environment.GetEnvironmentVariable("API_ABONO") ?? _configurations.GetValue<string>("AppConfigurations:ApiAbono");
            string recurso = $"?factura={Factura}&cedula={Cedula}";

            RestClient Client = new RestClient(Api) { Timeout = -1 };
            RestRequest Request = new RestRequest(resource: recurso, method: Method.GET);
            var result = await Client.ExecuteGetAsync(Request);
            ResponseApiAbonos Response = JsonConvert.DeserializeObject<ResponseApiAbonos>(result.Content);
            return Response;
        }

        public void Publish(SmsAbono SmsAbono)
        {
            ConnectionFactory Factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? _configurations.GetValue<string>("RabbitMQ:HostName"),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? _configurations.GetValue<string>("RabbitMQ:UserName"),
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? _configurations.GetValue<string>("RabbitMQ:Password"),
                Port = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RABBITMQ_PORT")) ? Int32.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), CultureInfo.InvariantCulture) : _configurations.GetValue<int>("RabbitMQ:Port"),
                VirtualHost = "/",
            };

            using IConnection Connection = Factory.CreateConnection("ENVIO_ABONO_SMS_SEND");
            using IModel Channel = Connection.CreateModel();

            string QueueName = _configurations.GetValue<string>("RabbitMQ:QueueName");

            Channel.QueueDeclare(QueueName, false, false, false, null);
            string Message = JsonConvert.SerializeObject(SmsAbono);
            byte[] Body = Encoding.UTF8.GetBytes(Message);
            Channel.BasicPublish("", QueueName, null, Body);
        }
    }
}
