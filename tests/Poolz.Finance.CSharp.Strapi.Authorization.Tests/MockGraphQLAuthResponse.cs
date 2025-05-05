namespace Poolz.Finance.CSharp.Strapi.Authorization.Tests;

internal static class MockGraphQLAuthResponse
{
    internal static string AdminResource = "AdminResource";
    internal static string Admin = "0x0000000000000000000000000000000000000001";
    internal static string UserRole = "Role1";
    internal static string ResourceRole1 = "Role1";
    internal static string ResourceRole2 = "Role2";

    internal static AuthUser User = new()
    {
        Wallet = "0x0000000000000000000000000000000000000002",
        RoleIDs = new List<AuthRole> { new() { Name = UserRole } }
    };

    internal static AuthResource Resource1 = new()
    {
        Name = "Resource1",
        RoleIDs = new List<AuthRole>
        {
            new() { Name = ResourceRole1 }
        }
    };

    internal static AuthResource Resource2 = new()
    {
        Name = "Resource2",
        RoleIDs = new List<AuthRole>
        {
            new() { Name = ResourceRole2 }
        }
    };

    internal static GraphQLAuthResponse Mock => new(
        AdminResource: new AuthAdministratorsResource
        {
            OnlyAdminResources = [new AuthResource { Name = AdminResource }]
        },
        Admins: [new AuthAdministrator { Wallet = Admin }],
        Users: [User],
        Resources: [Resource1, Resource2]
    );
}