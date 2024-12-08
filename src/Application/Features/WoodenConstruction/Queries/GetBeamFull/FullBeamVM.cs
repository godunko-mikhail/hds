using Core.Common.Enums;
using Core.Common.Interfaces;
using Core.Models;

namespace Application.Features.WoodenConstruction.Queries.GetBeamFull;

public class FullBeamVm
{
    //todo: remove source values, add vms реакции опор и сегменты
    public GeometricCharacteristics GeometricCharacteristics { get; set; } = null!;
    public PhysicalMechanicalCharacteristics PhysicalMechanicalCharacteristics { get; set; } = null!;
    
    public double MaCoefficient { get; set; }
    public double MbCoefficient { get; set; }
    public double MccCoefficient { get; set; }
    
    public SupportReaction[] SupportReactionsFirstGroup { get; set; } = null!;
    public SupportReaction[] SupportReactionsSecondGroup { get; set; } = null!;
    
    public string? GraphDisplacementFirstGroup { get; set; } 
    public string? GraphMomentsFirstGroup { get; set; } 
    public string? GraphForcesFirstGroup { get; set; } 
    public string? GraphDisplacementSecondGroup { get; set; } 
    public string? GraphMomentsSecondGroup { get; set; } 
    public string? GraphForcesSecondGroup { get; set; } 
}