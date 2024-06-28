using Application.Features.WoodenConstruction.Queries.GetBeamFull;
using AutoMapper;
using Core.Models;

namespace Application.Common.Mappings;

public class BeamMappingProfile : Profile
{
    public BeamMappingProfile()
    {
        CreateMap<GetBeamFullQuery, Beam>();
        CreateMap<Beam, FullBeamVm>();
    }
}