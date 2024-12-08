using Core.Models.Loads;

namespace Application.Services.FemBuilder;

public enum LoadGroup
{
    First = 0,
    Second = 1,
}

public static class LoadGroupExtensions
{
    public static double GetLoadGroupValue(this Load load, LoadGroup loadGroup)
    {
        return loadGroup switch
        {
            LoadGroup.First => -load.LoadForFirstGroup,
            LoadGroup.Second => -load.LoadForSecondGroup,
            _ => throw new ArgumentOutOfRangeException(nameof(loadGroup), loadGroup, null)
        };
    } 
}