using MediatR;
using System;

namespace EnvioFacturaSMS.Applications.CQRS.Pagares
{
    public class EnviarPagareQuery : IRequest<bool>
    {
        public DateTime TimesTamp { get; set; }
    }
}
