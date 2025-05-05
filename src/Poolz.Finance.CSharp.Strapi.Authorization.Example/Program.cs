namespace Poolz.Finance.CSharp.Strapi.Authorization.Example;

internal class Program
{
    private static async Task Main()
    {
        var authService = new AuthorizationService(new StrapiClient());

        await authService.IsAuthorizedAsync("0x", "resource");
    }
}