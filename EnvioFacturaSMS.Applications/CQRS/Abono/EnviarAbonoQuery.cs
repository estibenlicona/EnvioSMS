using EnvioFacturaSMS.Domain.Models;
using MediatR;
using RestSharp;

namespace EnvioFacturaSMS.Applications.Abono
{
    public class EnviarAbonoQuery : IRequest<IRestResponse<MasivianResponse>>
    {
        public string NumeroDocumento { get; set; }
        public string Cedula { get; set; }
        public string Celular { get; set; }
        public string Mensaje { get; set; }
        public short NumeroParametro { get; set; }
    }
}
