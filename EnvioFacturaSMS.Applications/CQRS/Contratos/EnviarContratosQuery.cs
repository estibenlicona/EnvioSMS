using MediatR;
using System;

namespace EnvioFacturaSMS.Applications.CQRS.Contratos
{
    public class EnviarContratosQuery : IRequest<bool>
    {
        public DateTime TimesTamp { get; set; }
    }
}
