using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR.Requests.Categories.Products {
    public class DeleteRequest : IRequest {
        public int Id { get; set; }

    }

    public class DeleteRequestHandler : IRequestHandler<DeleteRequest> {

        public Task<Unit> Handle(DeleteRequest request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

    }


}
