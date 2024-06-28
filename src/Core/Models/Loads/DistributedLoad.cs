namespace Core.Models.Loads;

public class DistributedLoad : Load
{
    public DistributedLoad()
    {
        OffsetStart = 0;
        OffsetEnd = 0;
    }

    public DistributedLoad(double offsetStart, double offsetEnd, double loadForFirstGroup, double loadForSecondGroup)
        : base(loadForFirstGroup, loadForSecondGroup)
    {
        OffsetStart = offsetStart;
        OffsetEnd = offsetEnd;
    }

    public double OffsetStart { get; set; }
    public double OffsetEnd { get; set; }
}