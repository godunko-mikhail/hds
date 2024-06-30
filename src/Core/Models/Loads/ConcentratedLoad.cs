namespace Core.Models.Loads;

public class ConcentratedLoad : Load
{
    public ConcentratedLoad()
    {
        Offset = 0;
    }

    public ConcentratedLoad(double offset)
    {
        Offset = offset;
    }

    public ConcentratedLoad(double offset, double loadForFirstGroup, double loadForSecondGroup)
        : base(loadForFirstGroup, loadForSecondGroup)
    {
        Offset = offset;
    }

    public double Offset { get; set; }
}