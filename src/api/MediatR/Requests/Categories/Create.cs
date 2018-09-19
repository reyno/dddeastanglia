using AutoMapper;
using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Data.Entities;
using DDDEastAnglia.Api.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR.Requests.Categories {

    public class CreateRequest : IRequest<CategoryModel> {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateRequestHandler : IRequestHandler<CreateRequest, CategoryModel> {
        private readonly Db _db;
        private readonly IMapper _mapper;

        public CreateRequestHandler(
            Db db,
            IMapper mapper
            ) {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CategoryModel> Handle(CreateRequest request, CancellationToken cancellationToken) {

            var entity = _mapper.Map<Category>(request);

            _db.Categories.Add(entity);
            await _db.SaveChangesAsync();

            var model = _mapper.Map<CategoryModel>(entity);

            return model;

        }

    }

}
