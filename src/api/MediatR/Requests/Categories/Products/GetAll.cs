using AutoMapper;
using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR.Requests.Categories.Products {

    public class GetAllRequest : IRequest<IEnumerable<ProductModel>> {
        public int CategoryId { get; set; }
    }

    public class GetAllRequestValidator : RequestValidator<GetAllRequest> {

        public GetAllRequestValidator(Db db) {
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .MustAsync(async (categoryId, cancellationToken) => {
                    return await db.Categories.AnyAsync(x => x.Id == categoryId);
                });
        }

    }
    public class GetAllRequestHandler : IRequestHandler<GetAllRequest, IEnumerable<ProductModel>> {
        private readonly Db _db;
        private readonly IMapper _mapper;

        public GetAllRequestHandler(
            Db db,
            IMapper mapper
            ){
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> Handle(GetAllRequest request, CancellationToken cancellationToken) {

            var entities = await _db
                .Products
                .Where(x => x.Category.Id == request.CategoryId)
                .ToListAsync();

            return entities.Select(_mapper.Map<ProductModel>);

        }

    }
}
