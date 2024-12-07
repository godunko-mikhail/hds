using static Core.Data;

namespace Core.Models.Loads;

public class DistributedLoadV2 : DistributedLoad
{
    public DistributedLoadV2()
    {
        NormativeValue = 0;
        NormativeValueUm = Units.kgm;
        LoadAreaWidth = 0;
        ReliabilityCoefficient = 0;
        ReducingFactor = 0;
    }

    public override double LoadForFirstGroup
    {
        get
        {
            if (NormativeValueUm == Units.kgm) return ReliabilityCoefficient * NormativeValue;
            return ReliabilityCoefficient * NormativeValue * (LoadAreaWidth ?? 0);
        }
    }

    public override double LoadForSecondGroup
    {
        get
        {
            if (NormativeValueUm == Units.kgm) return ReducingFactor * NormativeValue;
            return ReducingFactor * NormativeValue * (LoadAreaWidth ?? 0);
        }
    }

    public double NormativeValue { get; set; }
    public Units NormativeValueUm { get; set; }
    public double? LoadAreaWidth { get; set; }
    /// <summary>
    /// Коэффициент запаса прочности / надёжности
    /// </summary>
    public double ReliabilityCoefficient { get; set; }
    /// <summary>
    /// Коэффициент уменьшения
    /// </summary>
    public double ReducingFactor { get; set; }
}
