using AutoMapper;
using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.MediatR;
using DDDEastAnglia.Api.MediatR.Requests.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDEastAnglia.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            // Add AutoMapper and MediatR
            services.AddAutoMapper(options => options.CreateMissingTypeMaps = true);
            services.AddMediatR();

            // Add pipeline behavior using open generics
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PipelineBehavior<,>));

            // Add my validator to DI
            services.AddScoped<RequestValidator<CreateRequest>, CreateRequestValidator>();

            services.AddCors();
            services.AddDbContext<Db>(options => options.UseInMemoryDatabase("temp"));
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
            app.UseMvc();
        }
    }
}
