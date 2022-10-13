using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IGenericRepository<Cliente> _clienteRepository;
        public ClienteService(IGenericRepository<Cliente> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public async Task<Cliente> Get(string Cedula)
        {
            Expression<Func<Cliente, bool>> expression = cliente => cliente.Cedula.Trim().Equals(Cedula);
            Cliente cliente = await _clienteRepository.Get(expression);
            return cliente;
        }
    }
}
