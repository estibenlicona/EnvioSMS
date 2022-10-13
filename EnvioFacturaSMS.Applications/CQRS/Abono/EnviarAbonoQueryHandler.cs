using EnvioFacturaSMS.Applications.Excepetions;
using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using EnvioFacturaSMS.Domain.Models;
using MediatR;
using RestSharp;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Applications.Abono
{
    public class EnviarAbonoQueryHandler : IRequestHandler<EnviarAbonoQuery, IRestResponse<MasivianResponse>>
    {
        public readonly IAbonoService _abonoService;
        public readonly IAzureService _azureService;
        public readonly ILogService _logService;
        public readonly IMasivianService _masivianService;

        public EnviarAbonoQueryHandler(
            IAbonoService abonoService,
            IAzureService azureService,
            ILogService logService,
            IMasivianService masivianService
        )
        {
            _abonoService = abonoService;
            _azureService = azureService;
            _logService = logService;
            _masivianService = masivianService;
        }

        public async Task<IRestResponse<MasivianResponse>> Handle(EnviarAbonoQuery request, CancellationToken cancellationToken)
        {
            ResponseApiAbonos ResponseApiAbonos = await _abonoService.GetBytesAsync(request.NumeroDocumento, request.Cedula);
            if (ResponseApiAbonos == null) throw new ApiAbonoException("Archivo PDF no encontrado");
            string UrlAzure = _azureService.Upload(ResponseApiAbonos.Factura, request.NumeroDocumento);
            IRestResponse<MasivianResponse> MasivianResponse = await _masivianService.Send(request.NumeroParametro, request.Celular, UrlAzure, request.Mensaje);
            if(MasivianResponse.StatusCode != HttpStatusCode.OK) throw new AzureStorageException("Error cargando el Archivo PDF al AzureStorage");
            SaveLog(MasivianResponse, request);
            return MasivianResponse;
        }

        public async void SaveLog(IRestResponse<MasivianResponse> MasivianResponse, EnviarAbonoQuery request)
        {
            LogSMS Log = new LogSMS
            {
                Celular = request.Celular,
                Message = request.Mensaje,
                MessageTemplate = $"{MasivianResponse.StatusDescription} = {MasivianResponse.Content}",
                Proceso = "EnvioSMSAbono",
                TimeStamp = DateTime.Now
            };
            await _logService.Save(Log);
        }
    }
}
