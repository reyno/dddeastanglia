using AutoMapper;
using DDDEastAnglia.Api.MediatR;
using MediatR;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection {
    public static class MediatorDependencyInjectionExtensions {

        public static MediatorBuilder AddMediator(this IServiceCollection services) {

            services.AddAutoMapper(options => options.CreateMissingTypeMaps = true);
            services.AddMediatR();

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PipelineBehavior<,>));
            services.AddSingleton<IMediatorRequestResolver, MediatorRequestResolver>();
            services.AddSingleton(provider => new MediatorJsonOptions());
            services.AddGenericTypes(typeof(RequestValidator<>));

            return new MediatorBuilder(services);

        }

        public static MediatorBuilder AddMediator(this IServiceCollection services, Action<MediatorOptions> configure) {
            services.Configure(configure);
            return services.AddMediator();
        }

        private static void AddGenericTypes(this IServiceCollection services, Type type) {
            // find all types that derive from the generic type supplied
            var assembly = typeof(DependencyInjectionExtensions).Assembly;
            var types =
                from t in assembly.GetTypes()
                where t.IsClass
                 && t.BaseType != null
                 && t.BaseType.IsGenericType
                 && t.BaseType.GetGenericTypeDefinition() == type
                select t
                ;

            foreach (var implementationType in types)
                services.AddScoped(implementationType.BaseType, implementationType);
        }
    }
}
