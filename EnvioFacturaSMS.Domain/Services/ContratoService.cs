using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class ContratoService : IContratoService
    {
        private readonly IGenericRepository<Contrato> _contratoRepository;
        private readonly IGenericRepository<Tienda> _tiendaRepository;
        public ContratoService(IGenericRepository<Contrato> contratoRepository, IGenericRepository<Tienda> tiendaRepository)
        {
            _contratoRepository = contratoRepository;
            _tiendaRepository = tiendaRepository;
        }

        public async Task<string> Get(string NumeroContrato, string Cedula)
        {
            Expression<Func<Contrato, bool>> condition = Contrato => Contrato.NumeroContrato.Equals(NumeroContrato) && Contrato.Cedula.Equals(Cedula);
            Contrato Contrato = await _contratoRepository.Get(condition);
            return Contrato.Url;
        }

        public async Task<bool> Edit(Contrato Contrato)
        {
            return await _contratoRepository.Update(Contrato);
        }

        public async Task<List<Contrato>> List(DateTime TimesTamp)
        {
            Expression<Func<Tienda, bool>> tiendaCondition = Tienda => Tienda.IndicadorEnvio.Equals(1);
            List<Tienda> Tiendas = await _tiendaRepository.List(tiendaCondition);
                       
            List<Contrato> Contratos = new List<Contrato>();
            foreach (Tienda Tienda in Tiendas)
            {
                Expression<Func<Contrato, bool>> contratoCondition = Contrato => Contrato.Fecha > TimesTamp && Contrato.Url != null && Contrato.Url != string.Empty && Contrato.EnvioSms == 0 &&
                Contrato.NumeroContrato != null && Contrato.NumeroContrato.Trim() != string.Empty && 
                Contrato.TiendaId.Trim().Equals(Tienda.TiendaId.Trim());

                List<Contrato> ContratosTienda = await _contratoRepository.List(contratoCondition);

                Contratos.AddRange(ContratosTienda);

            }

            return Contratos;
        }
    }
}
