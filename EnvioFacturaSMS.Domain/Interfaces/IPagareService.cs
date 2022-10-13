using EnvioFacturaSMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IPagareService
    {
        Task<Pagare> Get(string NumeroPagare, string Cedula);
        Task<bool> Edit(Pagare Pagare);
        Task<List<Pagare>> List(DateTime TimesTamp);
    }
}
