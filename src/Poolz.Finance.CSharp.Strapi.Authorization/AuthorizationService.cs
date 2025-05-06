using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public class AuthorizationService(IStrapiClient strapi) : IAuthorizationService
{
    public AuthorizationService() : this(new StrapiClient()) { }

    public async Task<bool> IsAuthorizedAsync(EthereumAddress address, string resource)
    {
        var authInfo = await strapi.ReceiveAuthInformationAsync(address, resource);
        return IsAdmin(authInfo.Admins) || (!IsAdminResource(authInfo.AdminResource) && IsAllowedResource(authInfo.Users));
    }

    internal static bool IsAdmin(IEnumerable<AuthAdministrator> admins) => admins.Any();
    internal static bool IsAdminResource(AuthAdministratorsResource adminResource) => adminResource.OnlyAdminResources.Any();
    internal static bool IsAllowedResource(IEnumerable<AuthUser> users) => users.Any();
}