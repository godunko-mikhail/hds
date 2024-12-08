namespace Core.Models;

public class PhysicalMechanicalCharacteristics
{
    /// <summary>
    ///     Нормативный модуль упругости при изгибе с обеспеченностью 0.95 <br />
    ///     Нормативное значение модуля упругости, 5-процентный квантиль B.3
    /// </summary>
    private double StiffnessModulus { get; set; }

    /// <summary>
    ///     Средний модуль упругости при изгибе <br />
    ///     Среднее значение модуля упругости при изгибе B.3
    /// </summary>
    private double StiffnessModulusAverage { get; set; }

    /// <summary>
    ///     Средний модуль сдвига <br />
    ///     Среднее значение модуля сдвига B.3
    /// </summary>
    private double ShearModulusAverage { get; set; }

    /// <summary>
    ///     Расчётное сопротивление изгибу <br />
    ///     Расчетное сопротивление , Rаи МПа, для сортов древесины "Таблица 3"
    /// </summary>
    private double BendingResistance { get; set; }

    /// <summary>
    ///     Расчётное сопротивление скалыванию при изгибе
    ///     RАск
    /// </summary>
    private double BendingShearResistance { get; set; }
}