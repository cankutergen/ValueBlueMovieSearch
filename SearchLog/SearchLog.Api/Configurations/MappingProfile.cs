using AutoMapper;
using SearchLog.Api.Models;
using SearchLog.Entities.Concrete;
using ValueBlue.Core.Entities.Concrete;

namespace SearchLog.Api.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AggregationResult, DailyUsageReport>()
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src._id))
                    .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<SearchLogRequestModel, SearchLogDocument>();
        }
    }
}
