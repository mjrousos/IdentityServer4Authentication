using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Authentication.Stores
{
    public class CustomClientStore : IClientStore
    {
        public static IEnumerable<Client> AllClients { get; } = new[]
        {
            new Client
            {
                ClientId = "myClient",
                ClientName = "My Custom Client",
                AccessTokenLifetime = 60 * 60 * 24,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false,
                AllowedScopes = new[]
                {
                    "officeOwner",
                    StandardScopes.Roles.Name,

                    // Allow these to be requested even though they don't do anything
                    StandardScopes.OpenId.Name,
                    StandardScopes.Email.Name,
                    StandardScopes.Profile.Name,
                }
            }
        };

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.FromResult(AllClients.FirstOrDefault(c => c.ClientId == clientId));
        }
    }
}
