using EnvioFacturaSMS.Domain.Models;
using RestSharp;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IMasivianService
    {
        Task<IRestResponse<MasivianResponse>> Send(short NumeroParametro, string Celular, string Url, string Mensaje);
    }
}
