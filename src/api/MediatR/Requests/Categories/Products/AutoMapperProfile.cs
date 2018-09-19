using AutoMapper;
using DDDEastAnglia.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR.Requests.Categories.Products {
    public class AutoMapperProfile: Profile {
        public AutoMapperProfile() {

            CreateMap<CreateRequest, Product>()
                .ForMember(target => target.Id, options => options.Ignore())
                .ForMember(target => target.Category, options => options.Ignore())
                ;
        }
    }
}
