namespace Core.Models.Loads;

public abstract class Load
{
    public Load(
        double loadForFirstGroup,
        double loadForSecondGroup)
    {
        LoadForFirstGroup = loadForFirstGroup;
        LoadForSecondGroup = loadForSecondGroup;
    }

    public Load()
    {
        LoadForFirstGroup = 0;
        LoadForSecondGroup = 0;
    }

    public virtual double LoadForFirstGroup { get; set; }
    public virtual double LoadForSecondGroup { get; set; }
}