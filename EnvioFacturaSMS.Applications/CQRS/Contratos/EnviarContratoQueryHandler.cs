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

namespace EnvioFacturaSMS.Applications.CQRS.Contratos
{
    public class EnviarContratoQueryHandler : IRequestHandler<EnviarContratosQuery, bool>
    {
        private readonly IContratoService _contratoService;
        private readonly IClienteService _clienteService;
        private readonly ILogService _logService;
        private readonly IMasivianService _masivianService;
        private readonly IConfiguration _configuration;
        private readonly IParametrosService _parametrosService;

        public EnviarContratoQueryHandler(IContratoService contratoService, IClienteService clienteService, ILogService logService, IMasivianService masivianService, IConfiguration configuration, IParametrosService parametrosService)
        {
            _logService = logService;
            _masivianService = masivianService;
            _clienteService = clienteService;
            _contratoService = contratoService;
            _configuration = configuration;
            _parametrosService = parametrosService;
        }

        public async Task<bool> Handle(EnviarContratosQuery request, CancellationToken cancellationToken)
        {
            string CelularPruebas = Environment.GetEnvironmentVariable("CELULAR_PRUEBAS") ?? _configuration.GetValue<string>("CelularPrueba");
            
            short NumeroParametroAuthMassivian = (Environment.GetEnvironmentVariable("PARAMETRO_AUTH_MASSIVIAN") != null) ? 
                short.Parse(Environment.GetEnvironmentVariable("PARAMETRO_AUTH_MASSIVIAN")) : _configuration.GetValue<short>("ParametroAuthMassivian");
            
            short NumeroParametroMensaje = (Environment.GetEnvironmentVariable("PARAMETRO_MENSAJE") != null) ?
                short.Parse(Environment.GetEnvironmentVariable("PARAMETRO_MENSAJE")) : _configuration.GetValue<short>("ParametroMensaje");

            Parametro ParametroMensaje = await _parametrosService.GetParametro(NumeroParametroMensaje);
            List<Contrato> Contratos = await _contratoService.List(request.TimesTamp);
            Contratos = Contratos.GroupBy(x => x.Cedula, (key, g) => g.OrderByDescending(e => e.LogId).FirstOrDefault()).ToList();

            int Limit = (Environment.GetEnvironmentVariable("LIMIT_ENVIO") != null) ?
                int.Parse(Environment.GetEnvironmentVariable("LIMIT_ENVIO")) : _configuration.GetValue<int>("LimitEnvio");
            
            if (Limit > 0) Contratos = Contratos.Take(Limit).ToList();

            foreach (Contrato Contrato in Contratos)
            {
                Cliente Cliente = await _clienteService.Get(Contrato.Cedula);
                string Celular = (CelularPruebas == string.Empty || CelularPruebas == null) ? Cliente.Celular.Trim() : CelularPruebas;
                if (Cliente == null)
                {
                    Contrato.EnvioSms = 2;
                    await _contratoService.Edit(Contrato);
                    continue;
                }
                string Mensaje = ParametroMensaje.Valor.Trim().Replace("@nombre", Cliente.Nombre.Trim());
                IRestResponse<MasivianResponse> MasivianResponse = await _masivianService.Send(NumeroParametroAuthMassivian, Celular, Contrato.Url, Mensaje);
                Contrato.EnvioSms = 1;
                await _contratoService.Edit(Contrato);
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
                Proceso = "EnvioSMScontrato",
                TimeStamp = DateTime.Now
            };
            await _logService.Save(Log);
        }
    }
}
