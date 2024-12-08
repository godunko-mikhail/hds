using Core.Common.Interfaces;
using MathCore;
using MathCore.Common.Base;
using MathCore.Common.Interfaces;
using MathCore.FemCalculator;
using MathCore.FemCalculator.Model;

namespace Core.Services;

public class LoadsCalculator<TObj> : ILoadsCalculator<TObj>
    where TObj : ILoadable, IPhysicMechanicalCharacteristic, IGeometricCharacteristic
{
    private readonly IFemCalculator _femCalculator;

    public LoadsCalculator(IFemCalculator femCalculator)
    {
        _femCalculator = femCalculator;
    }


    public IEnumerable<SegmentDisplacementMaximum> GetSegmentDisplacementMaximums(TObj model, FemModel fem)
    {
        var baseDots = GetSupportWithConsolesCoordinates(model);
        var maxNodes = new SegmentDisplacementMaximum[baseDots.Count - 1];
        
        for (var i = 0; i < baseDots.Count - 1; i++)
        {
            var leftDot = baseDots[i];
            var rightDot = baseDots[i + 1];
            var offset = rightDot - leftDot;

            var nodesInside = fem.Nodes.Where(n => n.Coordinate.X >= leftDot && n.Coordinate.X <= rightDot);

            var node = nodesInside.MaxBy(x => Math.Abs(x.Displacement.Z))!;

            maxNodes[i] = new SegmentDisplacementMaximum(
                node, 
                node.Displacement.Z, 
                node.Displacement.Z / offset);
        }
        return maxNodes;
    }

    
    /// <summary>
    /// Расчёт нормального напряжения в расчётном сечении - τ
    /// </summary>
    /// <param name="model"></param>
    /// <param name="fem"></param>
    /// <returns>τ</returns>
    public ForceMaximum GetForceMaximum(TObj model, FemModel fem)
    {
        var maxSegment = fem.Segments
            .SelectMany(s => new[] { s.First, s.Second })
            .MaxBy(s => s.Force.Z);
        
        var stress = GetTangentialStress(
            maxSegment.Force!.Z,
            model.StaticMomentOfShearSectionY,
            model.MomentOfInertiaY,
            model.EffectiveWidth);

        return new ForceMaximum(
            maxSegment.Force.Z,
            stress,
            stress / model.BendingShearResistance);
    }

    public SupportReaction[] GetSupportReactions(TObj model, FemModel fem)
    {
        var supports = new SupportReaction[model.Supports.Count()];

        for (var i = 0; i < model.Supports.Count(); i++)
        {
            var node = fem.FindNodeByXCoordinate(model.Supports.ElementAt(i));
            var segmentsForce = fem.Segments
                .SelectMany<Segment, SegmentEnd>(v => [v.First, v.Second])
                .Where(v => v.Node == fem.Nodes.IndexOf(node) + 1)
                .OrderBy(v => v.Node)
                .Sum(v => v.Force.Z);

            var force = segmentsForce - node.Load.Z;
            
            supports[i] = new SupportReaction(force, force / Mathematics.G);
        }

        return supports;
    }

    private static List<double> GetSupportWithConsolesCoordinates(TObj model)
    {
        var dots = new List<double>();
        dots.AddRange(model.Supports);

        if (!dots.Any(d => d < 0.0000001)) dots.Add(0);
        if (!(dots.Any(d => Math.Abs(d - model.Length) < 0.0000001))) dots.Add(model.Length);

        return dots;
    }
    
    /// <summary>
    /// Расчёт Касательного напряжение
    /// </summary>
    /// <param name="force">Q - расчётная поперечная сила</param>
    /// <param name="staticMomentOfShearSection">Sбр - статический момент брутто сдвигаемой части относительно нейтральной оси </param>
    /// <param name="momentOfInertia">Iy момент инерции брутто поперечного сечения элемента относительно нейтральной оси</param>
    /// <param name="width">bрас - расчётная ширина сечения элемента</param>
    /// <returns>касательное напряжение</returns>
    private static double GetTangentialStress(double force, double staticMomentOfShearSection, double momentOfInertia, double width)
    {
        return force * staticMomentOfShearSection / momentOfInertia * width;
    }
}