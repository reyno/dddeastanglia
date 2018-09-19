using AutoMapper;
using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Data.Entities;
using DDDEastAnglia.Api.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR.Requests.Categories.Products {

    public class CreateRequest : IRequest<ProductModel> {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateRequestValidator : RequestValidator<CreateRequest> {
        public CreateRequestValidator(Db db) {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MustAsync(async (request, title, cancellationToken) => {
                    // make sure this is not a duplicate product for this category
                    return !await db.Products.AnyAsync(x 
                        => x.Category.Id == request.CategoryId
                        && x.Title == title
                        );
                })
                .WithMessage(request => $"A product already exists in this cateogry with the title '{request.Title}'")
                ;
        }
    }

    public class CreateRequestHandler : IRequestHandler<CreateRequest, ProductModel> {
        private readonly Db _db;
        private readonly IMapper _mapper;

        public CreateRequestHandler(
            Db db,
            IMapper mapper
            ) {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductModel> Handle(CreateRequest request, CancellationToken cancellationToken) {

            var entity = _mapper.Map<Product>(request);

            // set the category on the new product
            entity.Category = new Category { Id = request.CategoryId };
            _db.Entry(entity.Category).State = EntityState.Unchanged;

            // save to the database
            _db.Products.Add(entity);
            await _db.SaveChangesAsync();

            var model = _mapper.Map<ProductModel>(entity);

            return model;

        }

    }

}
