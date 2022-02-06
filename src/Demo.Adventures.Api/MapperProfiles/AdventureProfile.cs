using AutoMapper;
using Demo.Adventures.Api.Contracts.Adventures;
using Demo.Adventures.Api.Contracts.Games;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Api.MapperProfiles
{
    internal sealed class AdventureProfile : Profile
    {
        public AdventureProfile()
        {
            CreateMap<Adventure, AdventureDto>();
            CreateMap<Step, StepDto>();
            CreateMap<Option, OptionDto>();
            CreateMap<GameStep, GameStepDto>();
        }
    }
}