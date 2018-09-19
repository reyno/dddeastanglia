using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR {
    public class MediatorOptions {
        public string RoutePrefix { get; set; } = "mediator";
        public string NamespacePrefix { get; set; }
        public string RequestNameSuffix { get; set; } = "Request";
    }
}
