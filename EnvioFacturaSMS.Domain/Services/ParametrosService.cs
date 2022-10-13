using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class ParametrosService : IParametrosService
    {
        private readonly IGenericRepository<Parametro> _parametroRepository;
        public ParametrosService(IGenericRepository<Parametro>  parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        public async Task<Parametro> GetParametro(short Numero)
        {
            Expression<Func<Parametro, bool>> condition = x => x.Numero.Equals(Numero);
            Parametro parametro = await _parametroRepository.Get(condition);
            return parametro;
        }
    }
}
