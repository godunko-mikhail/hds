using AutoMapper;
using Core.Common.Enums;
using Core.Common.Interfaces;
using Core.Models;
using Core.Models.Loads;
using MediatR;

namespace Application.Features.WoodenConstruction.Queries.GetBeamFull;

public class GetBeamFullQuery : IRequest<FullBeamVm>
{
    public WoodenMaterials Material { get; set; }
    public bool DryWood { get; set; }
    public bool FlameRetardants { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public int Amount { get; set; }
    public ExploitationsType Exploitation { get; set; }
    public int LifeTime { get; set; }
    public int SteadyTemperature { get; set; }
    public LoadingModes LoadingMode { get; set; }
    public IEnumerable<double> Supports { get; set; } = null!;
    public IEnumerable<DistributedLoad> DistributedLoads { get; set; } = null!;
    public IEnumerable<ConcentratedLoad> ConcentratedLoads { get; set; } = null!;
}

public class GetBeamFullQueryHandler : IRequestHandler<GetBeamFullQuery, FullBeamVm>
{
    private readonly ILoadsCalculator<Beam> _loadsCalculator;
    private readonly IMapper _mapper;

    public GetBeamFullQueryHandler(
        IMapper mapper,
        ILoadsCalculator<Beam> loadsCalculator)
    {
        _mapper = mapper;
        _loadsCalculator = loadsCalculator;
    }

    public async Task<FullBeamVm> Handle(GetBeamFullQuery request, CancellationToken cancellationToken)
    {
        var beam = _mapper.Map<Beam>(request);
        var tmp = await _loadsCalculator.GetFirstGroupOfLimitStates(beam);
        var tmp2 = await _loadsCalculator.GetSecondGroupOfLimitStates(beam);
        return _mapper.Map<FullBeamVm>(beam);
    }
}