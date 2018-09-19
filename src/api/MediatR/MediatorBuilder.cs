using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DDDEastAnglia.Api.MediatR {
    public class MediatorBuilder {
        private IServiceCollection _services;

        public MediatorBuilder(IServiceCollection services) {
            _services = services;
        }

        public MediatorBuilder AddRequestResolver<TType>() where TType: class, IMediatorRequestResolver {

            // remove any existing registration
            var existing = _services.SingleOrDefault(x => x.ServiceType == typeof(IMediatorRequestResolver));
            if (existing != null) _services.Remove(existing);

            // add the new one
            _services.AddSingleton<IMediatorRequestResolver, TType>();

            return this;

        }

        public MediatorBuilder AddJsonOptions(Action<MediatorJsonOptions> configure) {
            _services.Configure(configure);
            return this;
        }
    }
}
