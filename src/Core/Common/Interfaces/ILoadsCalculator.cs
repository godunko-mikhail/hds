using MathCore.FemCalculator;
using MathCore.FemCalculator.Model;

namespace Core.Common.Interfaces;

public interface ILoadsCalculator<in TObj>
    where TObj : ILoadable, IPhysicMechanicalCharacteristic, IGeometricCharacteristic
{
    IEnumerable<SegmentDisplacementMaximum> GetSegmentDisplacementMaximums(TObj model, FemModel fem);
    ForceMaximum GetForceMaximum(TObj model, FemModel fem);
    
    SupportReaction[] GetSupportReactions(TObj model, FemModel fem);
}
public record SegmentDisplacementMaximum(Node Node, double AbsoluteValue, double RelativeValue);
public record ForceMaximum(double MaxForce, double MaxTangentialStress, double LoadingCoefficient);

public record struct SupportReaction(double Force, double Weight);