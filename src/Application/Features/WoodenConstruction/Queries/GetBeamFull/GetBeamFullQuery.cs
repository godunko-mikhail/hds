using Application.Services;
using AutoMapper;
using Core.Common.Enums;
using Core.Common.Interfaces;
using Core.Models;
using Core.Models.Loads;
using MediatR;
using Svg;

namespace Application.Features.WoodenConstruction.Queries.GetBeamFull;

public class GetBeamFullQuery : IRequest<FullBeamVm>
{
    /// <summary>
    /// Материал доски
    /// </summary>
    public WoodenMaterials Material { get; set; }
    /// <summary>
    /// Доска камерной сушки
    /// </summary>
    public bool DryWood { get; set; }
    /// <summary>
    /// Глубокая пропитка антипиренами
    /// </summary>
    public bool FlameRetardants { get; set; }
    /// <summary>
    /// Ширина доски. В метрах
    /// </summary>
    public double Width { get; set; }
    /// <summary>
    /// Высота доски. В метрах
    /// </summary>
    public double Height { get; set; }
    /// <summary>
    /// Длинна доски. В метрах
    /// </summary>
    public double Length { get; set; }
    /// <summary>
    /// Колличество досок
    /// </summary>
    public int Amount { get; set; }
    /// <summary>
    /// Тип эксплуатации доски
    /// </summary>
    public ExploitationsType Exploitation { get; set; }
    /// <summary>
    /// Срок службы
    /// </summary>
    public int LifeTime { get; set; }
    /// <summary>
    /// Установившаяся температура воздуха
    /// </summary>
    public int SteadyTemperature { get; set; }
    /// <summary>
    /// Режим нагружения
    /// </summary>
    public LoadingModes LoadingMode { get; set; }
    /// <summary>
    /// Опоры. Смещение от начала доски в метрах
    /// </summary>
    public IEnumerable<double> Supports { get; set; } = null!;
    /// <summary>
    /// Распределённые нагрузки
    /// </summary>
    public IEnumerable<DistributedLoad> DistributedLoads { get; set; } = null!;
    /// <summary>
    /// Сосредоточеные нагрузки
    /// </summary>
    public IEnumerable<ConcentratedLoad> ConcentratedLoads { get; set; } = null!;
}

public class GetBeamFullQueryHandler : IRequestHandler<GetBeamFullQuery, FullBeamVm>
{
    private readonly ILoadsCalculator<Beam> _loadsCalculator;
    private readonly DrawingService _drawingService;
    private readonly IMapper _mapper;

    public GetBeamFullQueryHandler(
        IMapper mapper,
        ILoadsCalculator<Beam> loadsCalculator,
        DrawingService drawingService)
    {
        _mapper = mapper;
        _loadsCalculator = loadsCalculator;
        _drawingService = drawingService;
    }

    public async Task<FullBeamVm> Handle(GetBeamFullQuery request, CancellationToken cancellationToken)
    {
        var beam = _mapper.Map<Beam>(request);
        var femFirst = await _loadsCalculator.GetFirstGroupOfLimitStates(beam);
        var femSecond = await _loadsCalculator.GetSecondGroupOfLimitStates(beam);
        
        var vm = _mapper.Map<FullBeamVm>(beam);
        
        vm.GraphDisplacementFirstGroup = _drawingService.DrawDisplacement(femFirst).GetXML();
        vm.GraphMomentsFirstGroup = _drawingService.DrawMoments(femFirst).GetXML();
        vm.GraphForcesFirstGroup = _drawingService.DrawForce(femFirst).GetXML();
        
        vm.GraphDisplacementSecondGroup = _drawingService.DrawDisplacement(femSecond).GetXML();
        vm.GraphMomentsSecondGroup = _drawingService.DrawMoments(femSecond).GetXML();
        vm.GraphForcesSecondGroup = _drawingService.DrawForce(femSecond).GetXML();
        
        return vm;
    }
}