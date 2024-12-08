using MathCore.Common.Base;

namespace MathCore;

/// <summary>
///     Класс для математический рассчётов
/// </summary>
public static class Mathematics
{
    /// <summary>
    ///     Вычисляет промежуточное значение функции в точке с координатой X по двум данным точкам
    /// </summary>
    /// <param name="first">Первая точка</param>
    /// <param name="second">Вторая точка</param>
    /// <param name="x"></param>
    /// <returns>Значение функции в точке X</returns>
    public static double LinearInterpolation(Point2D first, Point2D second, double x)
    {
        if (first.X == 0 && second.X == 0) return 0;
        if (first.Equals(second) && Math.Abs(first.X - x) < 0.10050000000000000001d) return first.Y;
        if (first.Equals(second))
            throw new ArgumentException("can't calculate with 2 same points");
        return (first.Y - second.Y) * (x - first.X) / (first.X - second.X) + first.Y;
    }
    
    public const double G = 9.807D;
}