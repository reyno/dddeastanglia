using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DDDEastAnglia.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddMediator();

            services.AddCors();
            services.AddDbContext<Db>(options => options.UseInMemoryDatabase("temp"));

            // Setup options configuration for auth
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddSingleton<IConfigureOptions<AuthorizationOptions>, ConfigureAuthorizationOptions>();

            // Setup auth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddAuthorization();
            services.AddAuthorizationPolicyEvaluator();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }

            app.UseCors(config => config
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials()
            );

            app.UseHttpsRedirection();

            app.UseMediator();

        }
    }
}
