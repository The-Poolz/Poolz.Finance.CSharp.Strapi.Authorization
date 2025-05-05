using Xunit;
using FluentAssertions;
using Net.Web3.EthereumWallet;

namespace Poolz.Finance.CSharp.Strapi.Authorization.Tests;

public class AuthorizationServiceTests
{
    public class IsAuthorizedAsync
    {
        private static readonly IStrapiClient _strapi = new MockStrapiClient();
        private readonly AuthorizationService _authorizationService = new(_strapi);

        [Fact]
        internal async Task WhenAdministratorCall_ShouldReturnTrue()
        {
            var result = await _authorizationService.IsAuthorizedAsync(
                address: MockGraphQLAuthResponse.Admin,
                resource: MockGraphQLAuthResponse.Resource1.Name
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal async Task WhenUserCallAdministratorResource_ShouldReturnFalse()
        {
            var result = await _authorizationService.IsAuthorizedAsync(
                address: MockGraphQLAuthResponse.User.Wallet,
                resource: MockGraphQLAuthResponse.AdminResource
            );

            result.Should().BeFalse();
        }

        [Fact]
        internal async Task WhenResourceNotFound_ShouldReturnFalse()
        {
            var result = await _authorizationService.IsAuthorizedAsync(
                address: MockGraphQLAuthResponse.User.Wallet,
                resource: "another one resource"
            );

            result.Should().BeFalse();
        }

        [Fact]
        internal async Task WhenUserNotFound_ShouldReturnFalse()
        {
            var result = await _authorizationService.IsAuthorizedAsync(
                address: EthereumAddress.ZeroAddress,
                resource: MockGraphQLAuthResponse.Resource1.Name
            );

            result.Should().BeFalse();
        }

        [Fact]
        internal async Task WhenAllowedResourceForUser_ShouldReturnTrue()
        {
            var result = await _authorizationService.IsAuthorizedAsync(
                address: MockGraphQLAuthResponse.User.Wallet,
                resource: MockGraphQLAuthResponse.Resource1.Name
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal async Task WhenNotAllowedResourceForUser_ShouldReturnFalse()
        {
            var result = await _authorizationService.IsAuthorizedAsync(
                address: MockGraphQLAuthResponse.User.Wallet,
                resource: MockGraphQLAuthResponse.Resource2.Name
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
                admins: [new AuthAdministrator { Wallet = MockGraphQLAuthResponse.Admin }],
                address: MockGraphQLAuthResponse.Admin
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal void WhenNonAdministratorCall_ShouldReturnFalse()
        {
            var result = AuthorizationService.IsAdmin(
                admins: [new AuthAdministrator { Wallet = MockGraphQLAuthResponse.Admin }],
                address: MockGraphQLAuthResponse.User.Wallet
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
                    OnlyAdminResources = new List<AuthResource>
                    {
                        new() { Name = MockGraphQLAuthResponse.AdminResource }
                    }
                },
                resource: MockGraphQLAuthResponse.AdminResource
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal void WhenNotAdministratorResource_ShouldReturnFalse()
        {
            var result = AuthorizationService.IsAdminResource(
                adminResource: new AuthAdministratorsResource
                {
                    OnlyAdminResources = new List<AuthResource>
                    {
                        new() { Name = MockGraphQLAuthResponse.AdminResource }
                    }
                },
                resource: "another one resource"
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
                resource: MockGraphQLAuthResponse.Resource1,
                user: MockGraphQLAuthResponse.User
            );

            result.Should().BeTrue();
        }

        [Fact]
        internal void WhenNotAuthorizedCall_ShouldReturnTrue()
        {
            var result = AuthorizationService.IsAllowedResource(
                resource: MockGraphQLAuthResponse.Resource1,
                user: new AuthUser
                {
                    Wallet = MockGraphQLAuthResponse.User.Wallet,
                    RoleIDs = new List<AuthRole> { new() { Name = "another one role" } }
                }
            );

            result.Should().BeFalse();
        }
    }
}