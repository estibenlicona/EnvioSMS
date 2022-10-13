using EnvioFacturaSMS.Domain.Entities;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IParametrosService
    {
        Task<Parametro> GetParametro(short NumeroParametro);
    }
}
