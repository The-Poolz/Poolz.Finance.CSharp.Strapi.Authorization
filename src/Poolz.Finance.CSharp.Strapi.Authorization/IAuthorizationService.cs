using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public interface IAuthorizationService
{
    public Task<bool> IsAuthorizedAsync(EthereumAddress address, string resource);
}