using Core.Common.Interfaces;
using Core.Models;
using Core.Services;
using MathCore.Common.Base;
using MathCore.Common.Interfaces;
using MathCore.FemCalculator;
using MathCore.FemCalculator.Model;

namespace Core.UnitTests;

public class LoadsCalculator
{
    private ILoadsCalculator<Beam> _calculator = null!;
    private Beam _normalBeam = null!;
    private FemModel _normalFem = null!;

    [SetUp]
    public void Setup()
    {
        _calculator = new LoadsCalculator<Beam>(new TestFemCalculator());
        _normalBeam = new Beam()
        {
            Length = 4,
            Supports = new List<double>()
            {
                0,
                2,
                3
            }
        }; ;
        _normalFem = new FemModel()
        {
            Nodes = new List<Node>()
            {
                new Node()
                {
                    Coordinate = new Point3D(0.1, 0, 0),
                    Displacement = new Vector6D<double>(){ Z = 0.2 }
                },
                new Node()
                {
                    Coordinate = new Point3D(0.2, 0, 0),
                    Displacement = new Vector6D<double>(){ Z = 0.3 }
                },
                new Node()
                {
                    Coordinate = new Point3D(0.3, 0, 0),
                    Displacement = new Vector6D<double>(){ Z = 0.4 }
                },
                new Node()
                {
                    Coordinate = new Point3D(2, 0, 0),
                    Displacement = new Vector6D<double>(){ Z = 0.5 }
                },
                new Node()
                {
                    Coordinate = new Point3D(2.5, 0, 0),
                    Displacement = new Vector6D<double>(){ Z = -1.0 }
                },
                new Node()
                {
                    Coordinate = new Point3D(4, 0, 0),
                    Displacement = new Vector6D<double>(){ Z = -2.0 }
                },
            }
        };
    }

    [Test]
    public void GetSegmentMaximums_NormalBeam_AbsoluteValue()
    {

        var res = _calculator.GetSegmentDisplacementMaximums(_normalBeam, _normalFem);

        Assert.That(res.Count(), Is.EqualTo(3));
        Assert.That(res.ElementAt(0).AbsoluteValue, Is.EqualTo(0.5).Within(0.0000001));
        Assert.That(res.ElementAt(1).AbsoluteValue, Is.EqualTo(-1.0).Within(0.0000001));
        Assert.That(res.ElementAt(2).AbsoluteValue, Is.EqualTo(-2.0).Within(0.0000001));
    }
    [Test]
    public void GetSegmentMaximums_NormalBeam_RelativeValue()
    {

        var res = _calculator.GetSegmentDisplacementMaximums(_normalBeam, _normalFem);

        Assert.That(res.Count(), Is.EqualTo(3));
        Assert.That(res.ElementAt(0).RelativeValue, Is.EqualTo(0.25).Within(0.0000001)); // 0.5 / 2 
        Assert.That(res.ElementAt(1).RelativeValue, Is.EqualTo(-1.0).Within(0.0000001)); // -1.0 / 1
        Assert.That(res.ElementAt(2).RelativeValue, Is.EqualTo(-2.0).Within(0.0000001)); // -2 / 1
    }
}

public class TestFemCalculator : IFemCalculator
{
    public Task CalculateAsync(FemModel model)
    {
        throw new NotImplementedException();
    }
}