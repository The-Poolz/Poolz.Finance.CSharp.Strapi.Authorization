using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public class AuthorizationService(IStrapiClient strapi) : IAuthorizationService
{
    public async Task<bool> IsAuthorizedAsync(EthereumAddress address, string resource)
    {
        var auth = await strapi.ReceiveAuthInformationAsync(address, resource);
        return auth.IsAdmin ||
            (!auth.IsAdminResource && auth.IsUserAllowed);
    }
}