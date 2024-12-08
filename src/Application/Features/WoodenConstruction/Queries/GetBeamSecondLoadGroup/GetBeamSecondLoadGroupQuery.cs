using Application.Services.FemBuilder;
using Core.Common.Enums;
using Core.Common.Interfaces;
using Core.Models;
using Core.Models.Loads;
using MathCore.Common.Interfaces;
using MediatR;

namespace Application.Features.WoodenConstruction.Queries.GetBeamSecondLoadGroup;

public class GetBeamSecondLoadGroupQuery : IRequest<SecondLoadGroupQueryVm>
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
    
    /// <summary>
    /// Распределённые нагрузки
    /// </summary>
    public IEnumerable<DistributedLoadV2> DistributedLoadsV2 { get; set; } = null!;
    /// <summary>
    /// Сосредоточеные нагрузки
    /// </summary>
    public IEnumerable<ConcentratedLoadV2> ConcentratedLoadsV2 { get; set; } = null!;
}

public class GetBeamSecondLoadGroupQueryHandler : IRequestHandler<GetBeamSecondLoadGroupQuery, SecondLoadGroupQueryVm>
{
    private readonly ILoadsCalculator<Beam> _loadsCalculator;
    private readonly IFemCalculator _femCalculator;

    public GetBeamSecondLoadGroupQueryHandler(ILoadsCalculator<Beam> loadsCalculator, IFemCalculator femCalculator)
    {
        _loadsCalculator = loadsCalculator;
        _femCalculator = femCalculator;
    }

    public async Task<SecondLoadGroupQueryVm> Handle(GetBeamSecondLoadGroupQuery request, CancellationToken cancellationToken)
    {
        var beam = new Beam
        {
            Material = request.Material,
            Amount = request.Amount,
            DryWood = request.DryWood,
            FlameRetardants = request.FlameRetardants,
            Width = request.Width,
            Height = request.Height,
            Length = request.Length,
            Exploitation = request.Exploitation,
            LifeTime = request.LifeTime,
            SteadyTemperature = request.SteadyTemperature,
            LoadingMode = request.LoadingMode,
            Supports = request.Supports,
            DistributedLoads = request.DistributedLoads.Union(request.DistributedLoadsV2),
            ConcentratedLoads = request.ConcentratedLoads.Union(request.ConcentratedLoadsV2),
        };
        
        var model = FemBuilder.Create(LoadGroup.Second)
            .AddInitialNodes(beam)
            .FillNodes()
            .SetSupportValues(beam.Supports)
            .SetConcentratedLoads(beam.ConcentratedLoads)
            .SetDistributedLoads(beam.DistributedLoads)
            .CreateSegments(beam)
            .Build();
        
        await _femCalculator.CalculateAsync(model);
        
        
        
        throw new NotImplementedException();
    }
}