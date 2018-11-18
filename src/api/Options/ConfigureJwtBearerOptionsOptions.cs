using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.Options {
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions> {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public ConfigureJwtBearerOptions(
            IConfiguration configuration,
            IServiceProvider serviceProvider
            ) {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public void Configure(string name, JwtBearerOptions options) {


            options.Audience = _configuration["Authentication:ClientId"];
            options.Authority = _configuration["Authentication:Authority"];
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidIssuers = new[] {
                    "https://login.microsoftonline.com/0fd336bb-894f-41aa-b464-78017e1c4aec/v2.0"
                }
            };
            options.Events = new JwtBearerEvents {
                OnTokenValidated = context => {

                    var identity = context.Principal.Identity as ClaimsIdentity;

                    // Uncomment to make the user an contributor
                    identity.AddClaim(new Claim(ClaimTypes.Role, "contributor"));

                    // Uncomment to make the user an administrator
                     identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));

                    return Task.CompletedTask;

                }
            };

        }

        public void Configure(JwtBearerOptions options) => Configure(JwtBearerDefaults.AuthenticationScheme, options);

    }
}
