namespace Poolz.Finance.CSharp.Strapi.Authorization.Example
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var authService = new AuthorizationService(new StrapiClient());

            await authService.IsAuthorizedAsync("0x", "resource");
        }
    }
}
