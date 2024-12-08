namespace MathCore.Common.Base;

public class Point3D : Point2D, IEquatable<Point3D>
{
    public Point3D(double x, double y, double z) : base(x, y)
    {
        Z = z;
    }

    public Point3D()
    {
    }

    public double Z { get; set; }

    public bool Equals(Point3D? other)
    {
        if (other == null) return false;
        return X == other.X &&
               Y == other.Y &&
               Z == other.Z;
    }

    public Point3D Clone()
    {
        return new Point3D(X, Y, Z);
    }
}