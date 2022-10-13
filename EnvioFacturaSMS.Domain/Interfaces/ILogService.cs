using EnvioFacturaSMS.Domain.Entities;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface ILogService
    {
        Task Save(LogSMS Log);
    }
}
