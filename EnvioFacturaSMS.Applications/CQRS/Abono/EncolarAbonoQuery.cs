using EnvioFacturaSMS.Domain.Models;
using MediatR;

namespace EnvioFacturaSMS.Applications.CQRS.Abono
{
    public class EncolarAbonoQuery : SmsAbono, IRequest<bool> { }
}
