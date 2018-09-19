using DDDEastAnglia.Api.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection {
    public static class DependencyInjectionExtensions {

        public static void AddRequestValidators(this IServiceCollection services) {

            services.AddGenericTypes(typeof(RequestValidator<>));

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
