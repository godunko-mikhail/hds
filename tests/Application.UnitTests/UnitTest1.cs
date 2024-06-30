using System.Xml;
using Application.Services;
using MathCore.FemCalculator;

namespace Application.UnitTests;

public class Tests
{
    private readonly FemModel _femModel = FemInit.FemModel;
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var svc = new DrawingService();
        
        var res = svc.DrawDisplacement(_femModel);
        using var sw = new StringWriter();
        using var xml = new XmlTextWriter(sw);
        res.Write(xml);
        
        File.WriteAllText(@"C:\Users\Happy\d.svg", sw.ToString());
        Assert.Pass();
    }
    [Test]
    public void Test1F()
    {
        var svc = new DrawingService();
        
        var res = svc.DrawForce(_femModel);
        using var sw = new StringWriter();
        using var xml = new XmlTextWriter(sw);
        res.Write(xml);
        
        File.WriteAllText(@"C:\Users\Happy\f.svg", sw.ToString());
        Assert.Pass();
    }
    [Test]
    public void Test1M()
    {
        var svc = new DrawingService();
        
        var res = svc.DrawMoments(_femModel);
        using var sw = new StringWriter();
        using var xml = new XmlTextWriter(sw);
        res.Write(xml);
        
        File.WriteAllText(@"C:\Users\Happy\m.svg", sw.ToString());
        Assert.Pass();
    }
}