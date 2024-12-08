using MathCore.Common.Base;

namespace MathCore.FemCalculator.Model;

public struct SegmentEnd
{
    public SegmentEnd(int node, AxisProperties isFlexible, AxisProperties isFixed)
    {
        Node = node;
        IsFlexible = isFlexible;
        IsFixed = isFixed;
    }

    public SegmentEnd()
    {
        IsFlexible = new AxisProperties();
        IsFixed = new AxisProperties();
    }

    public int Node { get; set; }

    public AxisProperties IsFlexible { get; set; }
    public AxisProperties IsFixed { get; set; }

    public Vector6D<double> Displacement { get; set; }
    public Vector6D<double> Force { get; set; }
}