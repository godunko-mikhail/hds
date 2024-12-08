using MathCore.Common.Base;

namespace MathCore.FemCalculator.Model;

public class Node
{
    public Node()
    {
        Coordinate = new Point3D();
        Support = new AxisProperties();
        Load = new Vector6D<double>();
    }

    public Node(Point3D coordinate)
    {
        Coordinate = coordinate;
        Support = new AxisProperties();
        Load = new Vector6D<double>();
    }

    public Node(double x)
    {
        Coordinate = new Point3D(x, 0, 0);
        Support = new AxisProperties();
        Load = new Vector6D<double>();
    }
    
    public Point3D Coordinate { get; set; }
    public AxisProperties Support;
    public Vector6D<double> Load;

    public Vector6D<double> Displacement { get; set; }

    public Node Clone()
    {
        return new Node(Coordinate)
        {
            Support = Support,
            Load = Load,
            Displacement = Displacement,
        };
    }
}