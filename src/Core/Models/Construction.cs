using Core.Common.Interfaces;
using MathCore.Common.Base;
using static Core.Data;
using static MathCore.Mathematics;

namespace Core.Models;

public class Construction : IGeometricCharacteristic
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public int Amount { get; set; }
    public double ShrinkageInWidth => GetShrinkage(Width);
    public double ShrinkageInHeight => GetShrinkage(Height);
    public double EffectiveWidth => (Width - ShrinkageInWidth) * Amount;
    public double EffectiveHeight => (Height - ShrinkageInHeight) * Amount;
    public double CrossSectionArea => EffectiveWidth * EffectiveHeight;

    public double PolarMomentOfInertia => EffectiveWidth * EffectiveHeight *
        (EffectiveWidth * EffectiveWidth + EffectiveHeight * EffectiveHeight) / 12;

    public double MomentOfInertiaY => EffectiveWidth * EffectiveHeight * EffectiveHeight * EffectiveHeight / 12;
    public double MomentOfInertiaZ => EffectiveWidth * EffectiveWidth * EffectiveWidth * EffectiveHeight / 12;
    public double MomentOfResistanceY => EffectiveWidth * EffectiveHeight * EffectiveHeight / 6;
    public double MomentOfResistanceZ => EffectiveWidth * EffectiveWidth * EffectiveHeight / 6;
    public double StaticMomentOfShearSectionY => EffectiveWidth * EffectiveHeight * EffectiveHeight / 8;
    public double StaticMomentOfShearSectionZ => EffectiveWidth * EffectiveWidth * EffectiveHeight / 8;

    private static double GetShrinkage(double thickness)
    {
        var point1 =
            ShrinkageValues.FirstOrDefault(v => v.Length > thickness) ??
            ShrinkageValues.ElementAt(new Index(2, true));

        var point2 =
            ShrinkageValues.LastOrDefault(v => v.Length <= thickness) ??
            ShrinkageValues.ElementAt(1);

        return LinearInterpolation(
            new Point2D(point1.Length, point1.Shrinkage),
            new Point2D(point2.Length, point2.Shrinkage),
            thickness);
    }
}