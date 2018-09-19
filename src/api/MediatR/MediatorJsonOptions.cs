using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DDDEastAnglia.Api.MediatR {
    public class MediatorJsonOptions {
        public JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}
