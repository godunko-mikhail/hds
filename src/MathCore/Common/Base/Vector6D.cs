using System.Numerics;

namespace MathCore.Common.Base;

public struct Vector6D<TValue>
    where TValue : struct, INumber<TValue>
{
    public TValue X { get; set; }
    public TValue Y { get; set; }
    public TValue Z { get; set; }
    public TValue U { get; set; }
    public TValue V { get; set; }
    public TValue W { get; set; }

    public static Vector6D<TValue> operator +(Vector6D<TValue> left, Vector6D<TValue> right)
    {
        return new Vector6D<TValue>()
        {
            X = left.X + right.X,
            Y = left.Y + right.Y,
            Z = left.Z + right.Z,
            U = left.U + right.U,
            V = left.V + right.V,
            W = left.W + right.W,
        };
    }
}

public struct AxisProperties
{
    public bool X { get; set; }
    public bool Y { get; set; }
    public bool Z { get; set; }
    public bool U { get; set; }
    public bool V { get; set; }
    public bool W { get; set; }
}