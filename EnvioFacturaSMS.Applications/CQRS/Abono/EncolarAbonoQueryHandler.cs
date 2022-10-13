using EnvioFacturaSMS.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Applications.CQRS.Abono
{
    public class EncolarAbonoQueryHandler : IRequestHandler<EncolarAbonoQuery, bool>
    {
        public readonly IAbonoService _abonoService;
        public EncolarAbonoQueryHandler(IAbonoService abonoService)
        {
            _abonoService = abonoService;
        }

        public async Task<bool> Handle(EncolarAbonoQuery request, CancellationToken cancellationToken)
        {
            _abonoService.Publish(request);
            await Task.CompletedTask;
            return true;
        }
    }
}
