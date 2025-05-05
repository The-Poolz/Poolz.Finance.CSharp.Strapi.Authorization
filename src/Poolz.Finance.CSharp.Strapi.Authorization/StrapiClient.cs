using GraphQL;
using GraphQL.Client.Http;
using Net.Web3.EthereumWallet;
using GraphQL.Client.Serializer.Newtonsoft;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public class StrapiClient(string apiUrl = StrapiClient.ApiUrl) : IStrapiClient
{
    public const string ApiUrl = "https://data.poolz.finance/graphql";

    private readonly GraphQLHttpClient _client = new(
        new GraphQLHttpClientOptions { EndPoint = new Uri(apiUrl) },
        new NewtonsoftJsonSerializer()
    );

    public async Task<GraphQLAuthResponse> ReceiveAuthInformationAsync(EthereumAddress address, string resource)
    {
        var adminResourcesFilter = new GraphQlQueryParameter<AuthResourceFiltersInput>("adminResourcesFilter", new AuthResourceFiltersInput
        {
            Name = new StringFilterInput { Eq = resource }
        });
        var adminsFilter = new GraphQlQueryParameter<AuthAdministratorFiltersInput>("adminsFilter", new AuthAdministratorFiltersInput
        {
            Wallet = new StringFilterInput { Eq = address.Address }
        });
        var resourcesFilter = new GraphQlQueryParameter<AuthResourceFiltersInput>("resourcesFilter", new AuthResourceFiltersInput
        {
            Name = new StringFilterInput { Eq = resource }
        });
        var usersFilter = new GraphQlQueryParameter<AuthUserFiltersInput>("usersFilter", new AuthUserFiltersInput
        {
            Wallet = new StringFilterInput { Eq = address.Address }
        });

        var queryBuilder = new QueryQueryBuilder()
            .WithAuthAdministratorsResource(
                new AuthAdministratorsResourceQueryBuilder().WithOnlyAdminResources(
                    new AuthResourceQueryBuilder().WithName(),
                    resourcesFilter
                )
            )
            .WithAuthAdministrators(
                new AuthAdministratorQueryBuilder().WithWallet(),
                adminsFilter
            )
            .WithAuthResources(
                new AuthResourceQueryBuilder().WithName().WithRoleIDs(
                    new AuthRoleQueryBuilder().WithName()
                ),
                adminResourcesFilter
            )
            .WithAuthUsers(
                new AuthUserQueryBuilder().WithWallet().WithRoleIDs(
                    new AuthRoleQueryBuilder().WithName()
                ),
                usersFilter
            )
            .WithParameter(resourcesFilter)
            .WithParameter(adminsFilter)
            .WithParameter(usersFilter)
            .WithParameter(adminResourcesFilter);

        var response = await _client.SendQueryAsync<GraphQLAuthResponse>(new GraphQLRequest
        {
            Query = queryBuilder.Build()
        });

        return response.Data;
    }
}