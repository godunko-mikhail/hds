namespace Core.Models.Loads;

public class ConcentratedLoadV2 : ConcentratedLoad
{
    public ConcentratedLoadV2()
    {
        NormativeValue = 0;
        ReliabilityCoefficient = 0;
        ReducingFactor = 0;
    }

    public ConcentratedLoadV2(double offset, double normativeValue, double reliabilityCoefficient,
        double reducingFactor) :
        base(offset)
    {
        NormativeValue = normativeValue;
        ReliabilityCoefficient = reliabilityCoefficient;
        ReducingFactor = reducingFactor;
    }

    public override double LoadForFirstGroup => ReliabilityCoefficient * NormativeValue;
    public override double LoadForSecondGroup => ReducingFactor * NormativeValue;

    public double NormativeValue { get; set; }
    public double ReliabilityCoefficient { get; set; }
    public double ReducingFactor { get; set; }
}