using AutoMapper;
using DDDEastAnglia.Api.Data.Entities;

namespace DDDEastAnglia.Api.MediatR.Requests.Categories {
    public class AutoMapperProfile : Profile {
        public AutoMapperProfile() {

            CreateMap<CreateRequest, Category>()
                .ForMember(target => target.Id, options => options.Ignore())
                .ForMember(target => target.Products, options => options.Ignore())
                ;

        }
    }
}
