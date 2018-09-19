using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace DDDEastAnglia.Api.Options {
    public class ConfigureAuthorizationOptions : IConfigureOptions<AuthorizationOptions> {

        public void Configure(AuthorizationOptions options) {

            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("contributor", policy => policy
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireClaim(ClaimTypes.Role, "contributor")
                );

            options.AddPolicy("admin", policy => policy
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireClaim(ClaimTypes.Role, "admin")
                );

        }

    }
}
