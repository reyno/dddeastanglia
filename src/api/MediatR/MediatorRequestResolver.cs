using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR {

    public interface IMediatorRequestResolver {
        Type Resolve(string name);
    }

    public class MediatorRequestResolver : IMediatorRequestResolver {

        public Type Resolve(string name) {
            throw new NotImplementedException();
        }

    }
}
