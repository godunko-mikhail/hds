namespace Core.Models;

public class GeometricCharacteristics
{
    /// <summary>
    /// Величина усушки доски по ширине
    /// </summary>
    public double ShrinkageInWidth { get; set; }
    /// <summary>
    /// Величина усушки доски по высоте
    /// </summary>
    public double ShrinkageInHeight { get; set; }
    /// <summary>
    /// Расчётная ширина сечения балки
    /// </summary>
    public double EffectiveWidth { get; set; }
    /// <summary>
    /// Расчётная высота сечения балки
    /// </summary>
    public double EffectiveHeight { get; set; }
    /// <summary>
    /// Площадь поперечного сечения балки
    /// </summary>
    public double CrossSectionArea { get; set; }
    /// <summary>
    /// Полярный момент инерции сечения
    /// </summary>
    public double PolarMomentOfInertia { get; set; }
    /// <summary>
    /// Осевой момент инерции сечения относительно оси Y
    /// </summary>
    public double MomentOfInertiaY { get; set; }
    /// <summary>
    /// Осевой момент инерции сечения относительно оси Z
    /// </summary>
    public double MomentOfInertiaZ { get; set; }
    /// <summary>
    /// Момент сопротивления сечения относительно оси Y
    /// </summary>
    public double MomentOfResistanceY { get; set; }
    /// <summary>
    /// Момент сопротивления сечения относительно оси Z
    /// </summary>
    public double MomentOfResistanceZ { get; set; }
    /// <summary>
    /// Статический момент площади сдвигаемого сечения относительно оси Y
    /// </summary>
    public double StaticMomentOfShearSectionY { get; set; }
    /// <summary>
    /// Статический момент площади сдвигаемого сечения относительно оси Z
    /// </summary>
    public double StaticMomentOfShearSectionZ { get; set; }
}