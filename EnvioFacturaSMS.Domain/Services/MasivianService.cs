using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Domain.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class MasivianService : IMasivianService
    {
        private string User;
        private string Password;

        private readonly IConfiguration _configuration;
        private readonly IParametrosService _parametrosService;

        public MasivianService(IConfiguration configuration, IParametrosService parametrosService)
        {
            _configuration = configuration;
            _parametrosService = parametrosService;
        }

        public async Task<IRestResponse<MasivianResponse>> Send(short NumeroParametro, string Celular, string Url, string Mensaje)
        {
            //Credenciales
            Parametro Parametro = await _parametrosService.GetParametro(NumeroParametro);
            User = Parametro.Nombre.Trim();
            Password = Parametro.Valor.Trim();

            string ApiMasivian = Environment.GetEnvironmentVariable("API_MASIVIAN") ?? _configuration.GetValue<string>("AppConfigurations:ApiMasivian");

            RestClient Client = new RestClient(ApiMasivian) { Timeout = -1};
            RestRequest Request = new RestRequest(method: Method.POST);

            RequestMasivian RequestMasivian = new RequestMasivian
            {
                To = $"57{Celular}",
                Text = $"{Mensaje} SHORTURL",
                LongMessage = true,
                Url = Url,
                IsFlash = false,
                IsPremium = true
            };

            string Authorizacion = Base64Encode($"{User}:{Password}");
            Request.AddHeader("Authorization", $"Basic {Authorizacion}");
            Request.AddJsonBody(RequestMasivian);

            IRestResponse<MasivianResponse> MasivianResponse = await Client.ExecutePostAsync<MasivianResponse>(Request);
            return MasivianResponse;
        }

        public static string Base64Encode(string PlainText)
        {
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            string Base64 = Convert.ToBase64String(PlainTextBytes);
            return Base64;
        }
    }
}
