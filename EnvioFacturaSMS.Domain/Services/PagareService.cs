using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class PagareService : IPagareService
    {

        private readonly IGenericRepository<Pagare> _pagareRepository;
        private readonly IGenericRepository<Tienda> _tiendaRepository;

        public PagareService(IGenericRepository<Pagare> pagareRepository, IGenericRepository<Tienda> tiendaRepository)
        {
            _pagareRepository = pagareRepository;
            _tiendaRepository = tiendaRepository;
        }

        public async Task<Pagare> Get(string NumeroPagare, string Cedula)
        {
            Expression<Func<Pagare, bool>> condition = 
                Pagare => Pagare.NumeroPagare.Trim().Equals(NumeroPagare) && Pagare.Cedula.Trim().Equals(Cedula);
            Pagare Pagare = await _pagareRepository.Get(condition);
            return Pagare;
        }

        public async Task<bool> Edit(Pagare Pagare)
        {
            return await _pagareRepository.Update(Pagare);
        }

        public async Task<List<Pagare>> List(DateTime TimesTamp)
        {
            Expression<Func<Tienda, bool>> tiendaCondition = Tienda => Tienda.IndicadorEnvio.Equals(1);
            List<Tienda> Tiendas = await _tiendaRepository.List(tiendaCondition);

            List<Pagare> Pagares = new List<Pagare>();
            foreach (Tienda Tienda in Tiendas)
            {
                Expression<Func<Pagare, bool>> condition = Pagare => Pagare.Fecha > TimesTamp && Pagare.Url != null && Pagare.Url != string.Empty && Pagare.EnvioSms == 0 &&
                  Pagare.NumeroPagare != null && Pagare.NumeroPagare.Trim() != string.Empty &&
                  Pagare.TiendaId.Trim().Equals(Tienda.TiendaId.Trim());

                List<Pagare> PagaresTienda = await _pagareRepository.List(condition);
                Pagares.AddRange(PagaresTienda);
            }
                
            return Pagares;
        }
    }
}
