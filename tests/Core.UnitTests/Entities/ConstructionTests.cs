using Core.Models;

namespace Core.UnitTests.Entities;

public class ConstructionTests
{
    private Construction _testConstruction = null!;

    [SetUp]
    public void Setup()
    {
        _testConstruction = new Construction
        {
            Height = 0.15D,
            Width = 0.05D,
            Length = 3D,
            Amount = 1
        };
    }

    [Test]
    public void ShrinkageInWidth_NormalData_RightAnswer()
    {
        var res = _testConstruction.ShrinkageInWidth * 1000;

        Assert.That(res, Is.EqualTo(2.2D).Within(0.01D));
    }

    [Test]
    public void ShrinkageInWidth_OutOfTable_RightAnswer()
    {
        var construction = new Construction
        {
            Height = 0.31D,
            Width = 0.31D,
            Length = 3D
        };

        var res = construction.ShrinkageInWidth;

        Assert.That(res, Is.EqualTo(0.0111D).Within(0.00001D));
    }

    [Test]
    public void ShrinkageInWidth_BeforeTheTable_RightAnswer()
    {
        var construction = new Construction
        {
            Height = 0.01D,
            Width = 0.01D,
            Length = 3D
        };

        var res = construction.ShrinkageInWidth;

        Assert.That(res, Is.EqualTo(0.0005D).Within(0.00001D));
    }

    [Test]
    public void ShrinkageInHeight_NormalData_RightAnswer()
    {
        var res = _testConstruction.ShrinkageInHeight * 1000;

        Assert.That(res, Is.EqualTo(5.9D).Within(0.01D));
    }

    [Test]
    public void ShrinkageInHeight_OutOfTable_RightAnswer()
    {
        var construction = new Construction
        {
            Height = 0.31D,
            Width = 0.31D,
            Length = 3D
        };

        var res = construction.ShrinkageInHeight;

        Assert.That(res, Is.EqualTo(0.0111D).Within(0.00001D));
    }

    [Test]
    public void ShrinkageInHeight_BeforeTheTable_RightAnswer()
    {
        var construction = new Construction
        {
            Height = 0.01D,
            Width = 0.01D,
            Length = 3D
        };

        var res = construction.ShrinkageInHeight;

        Assert.That(res, Is.EqualTo(0.0005D).Within(0.00001D));
    }


    [Test]
    public void EffectiveWidth_NormalData_RightAnswer()
    {
        var res = _testConstruction.EffectiveWidth * 1000;

        Assert.That(res, Is.EqualTo(47.8D).Within(0.01D));
    }

    [Test]
    public void EffectiveHeight_NormalData_RightAnswer()
    {
        var res = _testConstruction.EffectiveHeight * 1000;

        Assert.That(res, Is.EqualTo(144.1D).Within(0.01D));
    }

    [Test]
    public void CrossSectionArea_NormalData_RightAnswer()
    {
        var res = _testConstruction.CrossSectionArea * 1000 * 1000;

        Assert.That(res, Is.EqualTo(6888D).Within(0.1D));
    }

    [Test]
    public void PolarMomentOfInertia_NormalData_RightAnswer()
    {
        var res = _testConstruction.PolarMomentOfInertia * 1000 * 1000 * 1000 * 1000;

        Assert.That(res, Is.EqualTo(13230460D).Within(1D));
    }

    [Test]
    public void MomentOfInertiaY_NormalData_RightAnswer()
    {
        var res = _testConstruction.MomentOfInertiaY * 1000 * 1000 * 1000 * 1000;

        Assert.That(res, Is.EqualTo(11918966D).Within(1D));
    }

    [Test]
    public void MomentOfInertiaZ_NormalData_RightAnswer()
    {
        var res = _testConstruction.MomentOfInertiaZ * 1000 * 1000 * 1000 * 1000;

        Assert.That(res, Is.EqualTo(1311494D).Within(1D));
    }

    [Test]
    public void MomentOfResistanceY_NormalData_RightAnswer()
    {
        var res = _testConstruction.MomentOfResistanceY * 1000 * 1000 * 1000;

        Assert.That(res, Is.EqualTo(165426D).Within(1D));
    }

    [Test]
    public void MomentOfResistanceZ_NormalData_RightAnswer()
    {
        var res = _testConstruction.MomentOfResistanceZ * 1000 * 1000 * 1000;

        Assert.That(res, Is.EqualTo(54874D).Within(1D));
    }

    [Test]
    public void StaticMomentOfShearSectionY_NormalData_RightAnswer()
    {
        var res = _testConstruction.StaticMomentOfShearSectionY * 1000 * 1000 * 1000;

        Assert.That(res, Is.EqualTo(124069D).Within(1D));
    }

    [Test]
    public void StaticMomentOfShearSectionZ_NormalData_RightAnswer()
    {
        var res = _testConstruction.StaticMomentOfShearSectionZ * 1000 * 1000 * 1000;

        Assert.That(res, Is.EqualTo(41155D).Within(1D));
    }
}