using Application.Features.WoodenConstruction.Queries.GetBeamFull;
using AutoMapper;
using Core.Models;

namespace Application.Common.Mappings;

public class BeamMappingProfile : Profile
{
    public BeamMappingProfile()
    {
        CreateMap<GetBeamFullQuery, Beam>();
        CreateMap<Beam, FullBeamVm>()
            .ForPath(v => v.GeometricCharacteristics, v => v
                .MapFrom(beam => beam))
            .ForPath(v => v.PhysicalMechanicalCharacteristics, v => v
                .MapFrom(beam => beam));
        CreateMap<Beam, GeometricCharacteristics>();
        CreateMap<Beam, PhysicalMechanicalCharacteristics>();
    }
}