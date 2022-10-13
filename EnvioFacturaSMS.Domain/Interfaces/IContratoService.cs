using EnvioFacturaSMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IContratoService
    {
        Task<string> Get(string NumeroContrato, string Cedula);
        Task<bool> Edit(Contrato Contrato);
        Task<List<Contrato>> List(DateTime TimesTamp);
    }
}
