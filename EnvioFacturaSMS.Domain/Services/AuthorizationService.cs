using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Domain.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IGenericRepository<SessionToken> _sessionTokenRepository;
        public AuthorizationService(IGenericRepository<SessionToken> sessionTokenRepository)
        {
            _sessionTokenRepository = sessionTokenRepository;
        }

        public async Task<bool> IsAuthorized(string Token)
        {
            Expression<Func<SessionToken, bool>> Condition = Session => Session.Token.Trim().Equals(Token);
            SessionToken Session = await _sessionTokenRepository.Get(Condition);
            if (Session != null) return true;
            return false;
        }
    }
}
