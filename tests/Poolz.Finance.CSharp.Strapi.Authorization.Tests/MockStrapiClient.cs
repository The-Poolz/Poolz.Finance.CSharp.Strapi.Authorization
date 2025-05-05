using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization.Tests;

internal class MockStrapiClient : IStrapiClient
{
    public Task<GraphQLAuthResponse> ReceiveAuthInformationAsync(EthereumAddress address, string resource)
    {
        return Task.FromResult(MockGraphQLAuthResponse.Mock);
    }
}