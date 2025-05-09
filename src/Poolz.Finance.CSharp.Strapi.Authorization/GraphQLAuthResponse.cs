using Newtonsoft.Json;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

[method: JsonConstructor]
public class GraphQLAuthResponse(
    [JsonProperty("authAdministratorsResource")] AuthAdministratorsResource AdminResource,
    [JsonProperty("authAdministrators")] IEnumerable<AuthAdministrator> Admins,
    [JsonProperty("authUsers")] IEnumerable<AuthUser> Users
)
{
    public bool IsAdmin => Admins.Any();
    public bool IsUserAllowed => Users.Any();
    public bool IsAdminResource => AdminResource.OnlyAdminResources.Count != 0; // CA1827 vs CA1860 , IEnumerable vs ICollection
    public bool IsAllowed => IsAdmin || (!IsAdminResource && IsUserAllowed);
}
