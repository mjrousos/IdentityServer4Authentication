using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Authentication.Stores
{
    public class CustomScopeStore : IScopeStore
    {
        public static IEnumerable<Scope> GetAllScopes()
        {
            // IdentityServer considers Roles an identity scope, so access_tokens
            // won't include it. This modifies the standard scope to function
            // as a resource scope.
            //
            // Another option here (if roles are needed in access_tokens), would
            // just be to create a new scope (or use already-existing custom one)
            // and include `new ScopeClaim(JwtClaimTypes.Role)` in the Claims.
            var accessTokenRoles = StandardScopes.Roles;
            accessTokenRoles.Type = ScopeType.Resource;

            return new[]
            {
                // These aren't needed because IdentityServer won't include their
                // claims in access tokens anyhow; they are considered ScopeType.Identity scopes,
                // so the claims only apply to identity tokens.
                //
                // I still include them just so that requests including them don't fail.
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                
                // Include our modified roles scope
                accessTokenRoles,

                // The custom 'officeOwner' scope will include claims
                // related to accessing resources restricted by office number.
                // Requesting the 'officeOwner' scope will cause access tokens
                // to include the 'office' claim (if one exists).
                new Scope
                {
                    Name = "officeOwner",
                    DisplayName = "Office Owner",
                    Type = ScopeType.Resource,

                    Claims = new[] {
                        new ScopeClaim("office")
                    }
                }
            };
        }

        public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(GetAllScopes().Where(s => scopeNames.Contains(s.Name)));
        }

        public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            return Task.FromResult(GetAllScopes());
        }
    }
}
