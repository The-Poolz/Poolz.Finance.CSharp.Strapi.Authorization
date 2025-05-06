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
        var usersFilter = new GraphQlQueryParameter<AuthUserFiltersInput>("usersFilter", new AuthUserFiltersInput
        {
            Wallet = new StringFilterInput { Eq = address.Address },
            RoleIDs = new AuthRoleFiltersInput
            {
                ResourceIDs = new AuthResourceFiltersInput
                {
                    Name = new StringFilterInput { Eq = resource }
                }
            }
        });

        var queryBuilder = new QueryQueryBuilder()
            .WithAuthAdministratorsResource(
                new AuthAdministratorsResourceQueryBuilder().WithOnlyAdminResources(
                    new AuthResourceQueryBuilder().WithDocumentId(),
                    adminResourcesFilter
                )
            )
            .WithAuthAdministrators(
                new AuthAdministratorQueryBuilder().WithDocumentId(),
                adminsFilter
            )
            .WithAuthUsers(
                new AuthUserQueryBuilder().WithDocumentId(),
                usersFilter
            )
            .WithParameter(adminResourcesFilter)
            .WithParameter(adminsFilter)
            .WithParameter(usersFilter);

        var response = await _client.SendQueryAsync<GraphQLAuthResponse>(new GraphQLRequest
        {
            Query = queryBuilder.Build()
        });

        return response.Data;
    }
}