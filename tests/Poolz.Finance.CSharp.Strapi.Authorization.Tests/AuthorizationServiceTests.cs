using Xunit;
using FluentAssertions;
using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization.Tests;

public class AuthorizationServiceTests
{
    public class IsAuthorizedAsync
    {
        [Fact]
        internal async Task WhenAdministratorCall_ShouldReturnTrue()
        {
            var response = new GraphQLAuthResponse(
                AdminResource: new AuthAdministratorsResource(),
                Admins: [new AuthAdministrator ()],
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
                    OnlyAdminResources = new List<AuthResource> { new() }
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
                    OnlyAdminResources = new List<AuthResource>()
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
                    OnlyAdminResources = new List<AuthResource>()
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

    public class IsAdmin
    {
        [Fact]
        internal void WhenAdministratorCall_ShouldReturnTrue()
        {
            var result = AuthorizationService.IsAdmin(
                admins: [new AuthAdministrator()]
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal void WhenNonAdministratorCall_ShouldReturnFalse()
        {
            var result = AuthorizationService.IsAdmin(
                admins: []
            );

            result.Should().BeFalse();
        }
    }

    public class IsAdminResource
    {
        [Fact]
        internal void WhenAdministratorResource_ShouldReturnTrue()
        {
            var result = AuthorizationService.IsAdminResource(
                adminResource: new AuthAdministratorsResource
                {
                    OnlyAdminResources = new List<AuthResource> { new() }
                }
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal void WhenNotAdministratorResource_ShouldReturnFalse()
        {
            var result = AuthorizationService.IsAdminResource(
                adminResource: new AuthAdministratorsResource
                {
                    OnlyAdminResources = new List<AuthResource>()
                }
            );

            result.Should().BeFalse();
        }
    }

    public class IsAllowedResource
    {
        [Fact]
        internal void WhenAuthorizedCall_ShouldReturnTrue()
        {
            var result = AuthorizationService.IsAllowedResource(
                users: [new AuthUser()]
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal void WhenNotAuthorizedCall_ShouldReturnFalse()
        {
            var result = AuthorizationService.IsAllowedResource(
                users: []
            );

            result.Should().BeFalse();
        }
    }
}