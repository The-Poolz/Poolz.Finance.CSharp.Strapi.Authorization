using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public class AuthorizationService(StrapiClient strapi)
{
    public async Task<bool> IsAuthorizedAsync(EthereumAddress address, string resource)
    {
        var authInfo = await strapi.ReceiveAuthInformationAsync(address, resource);

        var isAdmin = authInfo.Admins.Any(x => x.Wallet == address);
        Console.WriteLine($"Is Admin: {isAdmin}");
        if (isAdmin) return true;

        var isAdminResource = authInfo.AdminResource.OnlyAdminResources.Any(x => x.Name == resource);
        Console.WriteLine($"Is Admin Resource: {isAdminResource}");
        if (isAdminResource) return false;

        var resourceRoles = authInfo.Resources.FirstOrDefault(x => x.Name == resource)?.RoleIDs.Select(r => r.Name).ToArray();
        if (resourceRoles == null)
        {
            Console.WriteLine("Resource not found.");
            return false;
        }

        Console.WriteLine($"Roles allowed for resource {resource}:");
        foreach (var res in resourceRoles)
        {
            Console.WriteLine(res);
        }

        var userRoles = authInfo.Users.FirstOrDefault(x => x.Wallet == address)?.RoleIDs.Select(r => r.Name).ToArray();
        if (userRoles == null)
        {
            Console.WriteLine("User not found.");
            return false;
        }

        Console.WriteLine($"User {address} roles:");
        foreach (var us in userRoles)
        {
            Console.WriteLine(us);
        }

        var isAuthorizedToCall = userRoles.Intersect(resourceRoles).Any();
        Console.WriteLine($"Is Authorized to call {isAuthorizedToCall}");
        return isAuthorizedToCall;
    }
}