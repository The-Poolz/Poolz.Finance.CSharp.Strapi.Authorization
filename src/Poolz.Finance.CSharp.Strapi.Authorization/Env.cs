using EnvironmentManager.Attributes;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public enum Env
{
    [EnvironmentVariable(typeof(Uri),true)]
    GRAPHQL_URL
}
