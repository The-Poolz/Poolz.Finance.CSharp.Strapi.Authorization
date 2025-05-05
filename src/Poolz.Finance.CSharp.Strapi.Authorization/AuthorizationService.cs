using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public class AuthorizationService(IStrapiClient strapi) : IAuthorizationService
{
    public AuthorizationService() : this(new StrapiClient()) { }

    public async Task<bool> IsAuthorizedAsync(EthereumAddress address, string resource)
    {
        var authInfo = await strapi.ReceiveAuthInformationAsync(address, resource);

        if (IsAdmin(authInfo.Admins, address)) return true;
        if (IsAdminResource(authInfo.AdminResource, resource)) return false;

        var resourceEntry = authInfo.Resources.FirstOrDefault(r => r.Name.Equals(resource, StringComparison.Ordinal));
        var userEntry = authInfo.Users.FirstOrDefault(u => u.Wallet.Equals(address, StringComparison.Ordinal));
        if (resourceEntry == null || userEntry == null) return false;

        return IsAllowedResource(resourceEntry, userEntry);
    }

    internal static bool IsAdmin(IEnumerable<AuthAdministrator> admins, EthereumAddress address) => admins.Any(x => x.Wallet == address);
    internal static bool IsAdminResource(AuthAdministratorsResource adminResource, string resource) => adminResource.OnlyAdminResources.Any(x => x.Name == resource);
    internal static bool IsAllowedResource(AuthResource resource, AuthUser user)
    {
        var resourceRoles = resource.RoleIDs.Select(r => r.Name).ToHashSet(StringComparer.Ordinal);
        return user.RoleIDs.Any(r => resourceRoles.Contains(r.Name));
    }
}