using EnvioFacturaSMS.Domain.Entities;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IClienteService
    {
        Task<Cliente> Get(string Cedula);
    }
}
