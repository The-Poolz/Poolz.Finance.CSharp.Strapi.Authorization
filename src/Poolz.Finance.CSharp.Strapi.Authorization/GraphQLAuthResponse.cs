using Newtonsoft.Json;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

[method: JsonConstructor]
public record GraphQLAuthResponse(
    [JsonProperty("authAdministratorsResource")] AuthAdministratorsResource AdminResource,
    [JsonProperty("authAdministrators")] IEnumerable<AuthAdministrator> Admins,
    [JsonProperty("authUsers")] IEnumerable<AuthUser> Users
);