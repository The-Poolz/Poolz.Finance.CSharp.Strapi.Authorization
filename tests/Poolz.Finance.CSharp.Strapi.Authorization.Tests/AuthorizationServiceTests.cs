using Xunit;
using FluentAssertions;
using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization.Tests;

public class AuthorizationServiceTests
{
    public class IsAuthorizedAsync
    {
        [Fact]
        internal void WhenSetEnvironmentVariable_ShouldReturnIt()
        {
            var testUrl = new Uri("https://www.test.com");

            Environment.SetEnvironmentVariable("GRAPHQL_URL", testUrl.ToString());
            var url = StrapiClient.GraphQLUrl;
            url.Should().Be(testUrl);
        }
        [Fact]
        internal async Task WhenAdministratorCall_ShouldReturnTrue()
        {
            var response = new GraphQLAuthResponse(
                AdminResource: new AuthAdministratorsResource(),
                Admins: [new AuthAdministrator()],
                Users: []
            );
            var strapi = new MockStrapiClient(response);
            var authorizationService = new AuthorizationService(strapi);

            var result = await authorizationService.IsAuthorizedAsync(
                address: EthereumAddress.ZeroAddress,
                resource: "mock resource"
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal async Task WhenUserCallAdministratorResource_ShouldReturnFalse()
        {
            var response = new GraphQLAuthResponse(
                AdminResource: new AuthAdministratorsResource
                {
                    OnlyAdminResources = [new()]
                },
                Admins: [],
                Users: []
            );
            var strapi = new MockStrapiClient(response);
            var authorizationService = new AuthorizationService(strapi);

            var result = await authorizationService.IsAuthorizedAsync(
                address: EthereumAddress.ZeroAddress,
                resource: "mock resource"
            );

            result.Should().BeFalse();
        }

        [Fact]
        internal async Task WhenAllowedResourceForUser_ShouldReturnTrue()
        {
            var response = new GraphQLAuthResponse(
                AdminResource: new AuthAdministratorsResource
                {
                    OnlyAdminResources = []
                },
                Admins: [],
                Users: [new AuthUser()]
            );
            var strapi = new MockStrapiClient(response);
            var authorizationService = new AuthorizationService(strapi);

            var result = await authorizationService.IsAuthorizedAsync(
                address: EthereumAddress.ZeroAddress,
                resource: "mock resource"
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal async Task WhenNotAllowedResourceForUser_ShouldReturnFalse()
        {
            var response = new GraphQLAuthResponse(
                AdminResource: new AuthAdministratorsResource
                {
                    OnlyAdminResources = []
                },
                Admins: [],
                Users: []
            );
            var strapi = new MockStrapiClient(response);
            var authorizationService = new AuthorizationService(strapi);

            var result = await authorizationService.IsAuthorizedAsync(
                address: EthereumAddress.ZeroAddress,
                resource: "mock resource"
            );

            result.Should().BeFalse();
        }
    }
    public class GraphQLAuthResponseTests
    {
        [Theory]
        [InlineData(0, 0, 0)] // all false
        [InlineData(1, 0, 0)] // admin only
        [InlineData(0, 1, 0)] // admin resource only
        [InlineData(0, 0, 1)] // user only
        [InlineData(1, 1, 0)] // admin + admin resource
        [InlineData(1, 0, 1)] // admin + user
        [InlineData(0, 1, 1)] // admin resource + user
        [InlineData(1, 1, 1)] // all true
        internal void ShouldEvaluatePermissionFlagsCorrectly(
            int adminCount,
            int adminResourceCount,
            int userCount)
        {
            var admins = Enumerable.Range(0, adminCount).Select(_ => new AuthAdministrator()).ToList();
            var adminResources = Enumerable.Range(0, adminResourceCount).Select(_ => new AuthResource()).ToList();
            var users = Enumerable.Range(0, userCount).Select(_ => new AuthUser()).ToList();

            var result = new GraphQLAuthResponse(
                AdminResource: new AuthAdministratorsResource { OnlyAdminResources = adminResources },
                Admins: admins,
                Users: users
            );

            result.IsAdmin.Should().Be(adminCount > 0);
            result.IsAdminResource.Should().Be(adminResourceCount > 0);
            result.IsUserAllowed.Should().Be(userCount > 0);
        }
    }
}