using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR {

    public interface IMediatorRequestResolver {
        Type Resolve(string name);
    }

    public class MediatorRequestResolver : IMediatorRequestResolver {
        private readonly MediatorOptions _mediatorOptions;
        private readonly IEnumerable<TypeInfo> _requestTypes;

        public MediatorRequestResolver(
            IOptions<MediatorOptions> mediatorOptionsAccessor
            ) {

            _mediatorOptions = mediatorOptionsAccessor.Value;

            _requestTypes = GetType().Assembly.DefinedTypes.Where(type =>
                typeof(IBaseRequest).IsAssignableFrom(type)
            );

        }

        public Type Resolve(string name) {

            var fullname = $"{_mediatorOptions.NamespacePrefix}.{name}{_mediatorOptions.RequestNameSuffix}";

            return _requestTypes.SingleOrDefault(x => x.FullName.EndsWith(fullname, StringComparison.OrdinalIgnoreCase));            

        }

    }
}
