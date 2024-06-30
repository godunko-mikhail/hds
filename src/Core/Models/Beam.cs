using Core.Common.Enums;
using Core.Common.Interfaces;
using Core.Models.Loads;

namespace Core.Entities;

public class Beam : WoodenConstruction, ILoadable
{
    public Beam()
    {
    }

    public Beam(WoodenConstruction data, ILoadable loads)
    {
        Material = data.Material;
        DryWood = data.DryWood;
        FlameRetardants = data.FlameRetardants;
        Width = data.Width;
        Height = data.Height;
        Length = data.Length;
        Amount = data.Amount;
        LifeTime = data.LifeTime;
        SteadyTemperature = data.SteadyTemperature;
        LoadingMode = loads.LoadingMode;
        Supports = loads.Supports;
        DistributedLoads = loads.DistributedLoads;
        ConcentratedLoads = loads.ConcentratedLoads;
    }

    public LoadingModes LoadingMode { get; set; }
    public IEnumerable<double> Supports { get; set; }
    public IEnumerable<DistributedLoad> DistributedLoads { get; set; }
    public IEnumerable<ConcentratedLoad> ConcentratedLoads { get; set; }

    private record class ShrinkageValue(double Size, double Value);
}