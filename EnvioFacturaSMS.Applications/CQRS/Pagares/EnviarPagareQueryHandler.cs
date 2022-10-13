using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Applications.CQRS.Pagares
{
    public class EnviarPagareQueryHandler : IRequestHandler<EnviarPagareQuery, bool>
    {
        private readonly IPagareService _pagareService;
        private readonly IClienteService _clienteService;
        private readonly IParametrosService _parametrosService;
        private readonly IConfiguration _configuration;
        public readonly ILogService _logService;
        public readonly IMasivianService _masivianService;

        public EnviarPagareQueryHandler(IPagareService pagareService, IClienteService clienteService, IParametrosService parametrosService, ILogService logService, IMasivianService masivianService, IConfiguration configuration)
        {
            _pagareService = pagareService;
            _clienteService = clienteService;
            _parametrosService = parametrosService;
            _logService = logService;
            _masivianService = masivianService;
            _configuration = configuration;
        }

        public async Task<bool> Handle(EnviarPagareQuery request, CancellationToken cancellationToken)
        {
            string CelularPruebas = Environment.GetEnvironmentVariable("CELULAR_PRUEBAS") ?? _configuration.GetValue<string>("CelularPrueba");

            short NumeroParametroAuthMassivian = (Environment.GetEnvironmentVariable("PARAMETRO_AUTH_MASSIVIAN") != null) ?
                short.Parse(Environment.GetEnvironmentVariable("PARAMETRO_AUTH_MASSIVIAN")) : _configuration.GetValue<short>("ParametroAuthMassivian");

            short NumeroParametroMensaje = (Environment.GetEnvironmentVariable("PARAMETRO_MENSAJE") != null) ?
                short.Parse(Environment.GetEnvironmentVariable("PARAMETRO_MENSAJE")) : _configuration.GetValue<short>("ParametroMensaje");

            Parametro ParametroMensaje = await _parametrosService.GetParametro(NumeroParametroMensaje);

            List<Pagare> Pagares = await _pagareService.List(request.TimesTamp);
            Pagares = Pagares.GroupBy(x => x.Cedula, (key, g) => g.OrderByDescending(e => e.LogId).FirstOrDefault()).ToList();

            int Limit = (Environment.GetEnvironmentVariable("LIMIT_ENVIO") != null) ?
                int.Parse(Environment.GetEnvironmentVariable("LIMIT_ENVIO")) : _configuration.GetValue<int>("LimitEnvio");

            if (Limit > 0) Pagares = Pagares.Take(Limit).ToList();

            foreach (Pagare Pagare in Pagares)
            {
                Cliente Cliente = await _clienteService.Get(Pagare.Cedula);
                if (Cliente == null) {
                    Pagare.EnvioSms = 2;
                    await _pagareService.Edit(Pagare);
                    continue;
                }
                string Celular = (CelularPruebas == string.Empty || CelularPruebas == null) ? Cliente.Celular.Trim() : CelularPruebas;
                string Mensaje = ParametroMensaje.Valor.Trim().Replace("@nombre", Cliente.Nombre.Trim());
                IRestResponse<MasivianResponse> MasivianResponse = await _masivianService.Send(NumeroParametroAuthMassivian, Celular, Pagare.Url, Mensaje);
                Pagare.EnvioSms = 1;
                await _pagareService.Edit(Pagare);
                await SaveLog(MasivianResponse, Celular, Mensaje);
            }

            return true;
        }

        public async Task SaveLog(IRestResponse<MasivianResponse> MasivianResponse, string Celular, string Mensaje)
        {

            LogSMS Log = new LogSMS
            {
                Celular = Celular,
                Message = Mensaje,
                MessageTemplate = $"{MasivianResponse.StatusDescription} = {MasivianResponse.Content}",
                Proceso = "EnvioSMSPagare",
                TimeStamp = DateTime.Now
            };
            await _logService.Save(Log);
        }

    }
}
