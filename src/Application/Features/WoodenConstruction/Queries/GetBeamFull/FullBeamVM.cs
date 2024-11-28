using Core.Common.Enums;

namespace Application.Features.WoodenConstruction.Queries.GetBeamFull;

public class FullBeamVm
{
    public ExploitationsType Exploitation { get; set; }
    public WoodenMaterials Material { get; set; }
    public bool DryWood { get; set; }
    public bool FlameRetardants { get; set; }
    public int Amount { get; set; }
    public int LifeTime { get; set; }
    public int SteadyTemperature { get; set; }
    public double StiffnessModulus { get; set; }
    public double StiffnessModulusAverage { get; set; }
    public double ShearModulusAverage { get; set; }
    public double BendingResistance { get; set; }
    public double BendingShearResistance { get; set; }
    public double MaCoefficient { get; set; }
    public double MbCoefficient { get; set; }
    public double MccCoefficient { get; set; }
    public double ShrinkageInWidth { get; set; }
    public double ShrinkageInHeight { get; set; }
    public double EffectiveWidth { get; set; }
    public double EffectiveHeight { get; set; }
    public double CrossSectionArea { get; set; }
    public double PolarMomentOfInertia { get; set; }
    public double MomentOfInertiaY { get; set; }
    public double MomentOfInertiaZ { get; set; }
    public double MomentOfResistanceY { get; set; }
    public double MomentOfResistanceZ { get; set; }
    public double StaticMomentOfShearSectionY { get; set; }
    public double StaticMomentOfShearSectionZ { get; set; }
    
    
    public string? GraphDisplacementFirstGroup { get; set; } 
    public string? GraphMomentsFirstGroup { get; set; } 
    public string? GraphForcesFirstGroup { get; set; } 
    public string? GraphDisplacementSecondGroup { get; set; } 
    public string? GraphMomentsSecondGroup { get; set; } 
    public string? GraphForcesSecondGroup { get; set; } 
}