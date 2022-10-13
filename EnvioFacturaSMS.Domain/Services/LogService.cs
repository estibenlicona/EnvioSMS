using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class LogService : ILogService
    {
        private readonly IGenericRepository<LogSMS> _logRepository;

        public LogService(IGenericRepository<LogSMS> logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Save(LogSMS Log)
        {
            await _logRepository.Insert(Log);
        }
    }
}
