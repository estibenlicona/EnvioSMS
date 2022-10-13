using EnvioFacturaSMS.Domain.Models;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IAbonoService
    {
        Task<ResponseApiAbonos> GetBytesAsync(string Factura, string Cedula);
        void Publish(SmsAbono SmsAbono);
    }
}
