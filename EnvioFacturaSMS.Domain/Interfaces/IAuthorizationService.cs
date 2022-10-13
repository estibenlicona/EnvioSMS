using System.Threading.Tasks;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> IsAuthorized(string Token);
    }
}
