using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization;

public interface IStrapiClient
{
    public Task<GraphQLAuthResponse> ReceiveAuthInformationAsync(EthereumAddress address, string resource);
}