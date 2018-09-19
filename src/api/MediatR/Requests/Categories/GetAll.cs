using AutoMapper;
using DDDEastAnglia.Api.Data;
using DDDEastAnglia.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR.Requests.Categories {

    public class GetAllRequest : IRequest<IEnumerable<CategoryModel>> { }

    public class GetAllRequestHandler : IRequestHandler<GetAllRequest, IEnumerable<CategoryModel>> {

        private readonly Db _db;
        private readonly IMapper _mapper;

        public GetAllRequestHandler(
            Db db,
            IMapper mapper
            ) {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryModel>> Handle(GetAllRequest request, CancellationToken cancellationToken) {

            // get the entities from the database
            var entities = await _db.Categories.ToListAsync();

            // convert to models automatically
            var models = entities.Select(_mapper.Map<CategoryModel>);


            return models;

        }

    }

}
