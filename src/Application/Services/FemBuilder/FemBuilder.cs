using Core;
using Core.Models;
using Core.Models.Loads;
using MathCore.Common.Base;
using MathCore.FemCalculator;
using MathCore.FemCalculator.Model;

namespace Application.Services.FemBuilder;

public class FemBuilder
{
    public List<Node> Nodes { get; private init; } = null!;
    public List<Segment> Segments { get; private set; } = null!;
    public LoadGroup LoadGroup { get; private set; }
    public bool Filled { get; private set; }
    
    
    public static FemBuilder Create(LoadGroup loadGroup = LoadGroup.First)
    {
        return new FemBuilder()
        {
            Nodes = [],
            Segments = [],
            LoadGroup = loadGroup,
            Filled = false
        };
    }

    public FemBuilder AddInitialNodes(Beam beam)
    {
        Nodes.Add(new Node(0));
        Nodes.Add(new Node(beam.Length));
        Nodes.AddRange(beam.Supports.Select(v => new Node(v)));
        Nodes.AddRange(beam.ConcentratedLoads.Select(v => new Node(v.Offset)));
        Nodes.AddRange(beam.DistributedLoads.SelectMany<DistributedLoad, Node>(v => [new Node(v.OffsetStart), new Node(v.OffsetEnd)]));
        Nodes.Sort(Data.NodesXCoordinateComparison);
        return this;
    }
    /// <summary>
    /// Заполняет промежутки между важными точками с шагом step 
    /// </summary>
    /// <param name="step">Шаг расстановки точек. По умолчанию 5 см</param>
    /// <returns>this</returns>
    /// <exception cref="ArgumentException"></exception>
    public FemBuilder FillNodes(double step = 0.05)
    {
        Filled = true;
        
        if (Nodes.Count < 2) throw new ArgumentException("number of nodes < 2");

        var newNodes = new List<Node>(GetApproxFillSize(step));
        
        for (var i = 0; i < Nodes.Count - 1; i++)
        {
            var distance = Nodes[i + 1].Coordinate.X - Nodes[i].Coordinate.X;
            
            // 1cm = skip 
            if (distance <= 0.01) continue;

            var numberOfSegments = (int)Math.Ceiling(distance / step);

            var segmentSize = numberOfSegments < 3 ? 
                distance / 3 : 
                distance / numberOfSegments;
            
            newNodes.AddRange(Enumerable.Range(1, numberOfSegments)
                .Select(segIndex => new Node(Nodes[i].Coordinate.X + segmentSize * segIndex)));
        }

        Nodes.AddRange(newNodes);
        Nodes.Sort(Data.NodesXCoordinateComparison);

        for (var i = 0; i < Nodes.Count - 1; i++)
        {
            var node = Nodes[i];
            var node2 = Nodes[i + 1];
            
            if (node2.Coordinate.X - node.Coordinate.X < Data.FemTolerance * 2)
            {
                Nodes.RemoveAt(i+1);
            }
        }
        var globalDiff = 10d;
        for (var i = 0; i < Nodes.Count - 1; i++)
        {
            var node = Nodes[i];
            var node2 = Nodes[i + 1];

            var diff = node2.Coordinate.X - node.Coordinate.X;
            if (diff < globalDiff) globalDiff = diff;
        }
        // foreach (var node in Nodes)
        // {
        //     if(!res.Any(v => Math.Abs(v.Coordinate.X - node.Coordinate.X) < .00005)){
        //         res.Add(node);
        //     }
        // }
        
        return this;
    }

    /// <summary>
    /// Фиксирует точки опоры на FEM модели
    /// </summary>
    /// <param name="supports">координаты опор</param>
    /// <returns></returns>
    public FemBuilder SetSupportValues(IEnumerable<double> supports)
    {
        var firstSet = true;
        foreach (var support in supports)
        {
            var node = FindNodeByXCoordinate(support);
            if (firstSet)
            {
                node.Support.X = true;
                firstSet = false;
            }
            node.Support.Y = true;
            node.Support.Z = true;
        }
        return this;
    }

    /// <summary>
    /// Устанавливает значения нагрузок для сосредоточенных нагрузок
    /// </summary>
    /// <param name="concentratedLoads"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public FemBuilder SetConcentratedLoads(IEnumerable<ConcentratedLoad> concentratedLoads)
    {
        foreach (var load in concentratedLoads)
        {
            var node = FindNodeByXCoordinate(load.Offset);

            // Z looks up, so load is negative
            node.Load.Z += load.GetLoadGroupValue(LoadGroup);
        }

        return this;
    }
    
    /// <summary>
    /// Устанавливает значения нагрузок и моментов для равномерно распределённых нагрузок
    /// </summary>
    /// <param name="distributedLoads"></param>
    /// <returns></returns>
    public FemBuilder SetDistributedLoads(IEnumerable<DistributedLoad> distributedLoads)
    {
        GuardFilled();
        foreach (var load in distributedLoads)
        {
            var nodesBetweenLoad = Nodes.Where(node => node.Coordinate.X >= load.OffsetStart - Data.FemTolerance &&
                                                       node.Coordinate.X <= load.OffsetEnd + Data.FemTolerance)
                .OrderBy(node => node.Coordinate.X)
                .ToArray();

            for (var i = 0; i < nodesBetweenLoad.Length - 1; i++)
            {
                var leftNode = nodesBetweenLoad[i];
                var rightNode = nodesBetweenLoad[i + 1];
                var l = rightNode.Coordinate.X - leftNode.Coordinate.X;
                
                var force = load.GetLoadGroupValue(LoadGroup) * l / 2;
                var moment = load.GetLoadGroupValue(LoadGroup) * l * l / 12;

                leftNode.Load.Z += force;
                rightNode.Load.Z += force;
                
                leftNode.Load.V += moment;
                rightNode.Load.V += moment;
            }
        }
        
        return this;
    }

    public FemBuilder CreateSegments(Beam beam)
    {
        GuardFilled();
        var count = Nodes.Count - 1;
        var segments = new Segment[count];
        for (var i = 0; i < count; i++)
            segments[i] = new Segment
            {
                First = new SegmentEnd(i + 1,
                    new AxisProperties(),
                    new AxisProperties() { U = true }),

                Second = new SegmentEnd(i + 2,
                    new AxisProperties(),
                    new AxisProperties() { U = true }),

                ZDirection = new Point3D(0, 0, 1),
                StiffnessModulus = beam.StiffnessModulus,
                ShearModulus = beam.ShearModulusAverage,
                CrossSectionalArea = beam.CrossSectionArea,
                ShearAreaY = beam.CrossSectionArea * 5 / 6,
                ShearAreaZ = beam.CrossSectionArea * 5 / 6,
                MomentOfInertiaX = beam.PolarMomentOfInertia,
                MomentOfInertiaY = beam.MomentOfInertiaY,
                MomentOfInertiaZ = beam.MomentOfInertiaZ
            };
        
        Segments = [..segments];
        return this;
    }
    public FemModel Build()
    {
        GuardFilled();
        return new FemModel(Segments, Nodes);
    }

    public FemBuilder SetLoadGroup(LoadGroup loadGroup)
    {
        LoadGroup = loadGroup;
        return this;
    }

    private void GuardFilled()
    {
        if (!Filled) throw new InvalidOperationException("Fill model with nodes before this action");
    }
    private Node FindNodeByXCoordinate(double coordinate)
    {
        // 0.001 м = 0.1 см = 1 мм
        return Nodes.First(n => Math.Abs(n.Coordinate.X - coordinate) < Data.FemTolerance);
    }
    
    private int GetApproxFillSize(double step)
    {
        // Примерное значение. Обычное количество исходных точек от 5 до 20.
        // В результате обычно около 100 точек. Лучше преаллоцировать чем list будет делать это сам
        return (int) (Nodes.Max(n => n.Coordinate.X) / step) + 5;
    }
    
    public FemBuilder Clone()
    {
        return new FemBuilder()
        {
            Nodes = Nodes.Select(v => v.Clone()).ToList(),
            Segments = Segments.Select(v => v.Clone()).ToList(),
            LoadGroup = LoadGroup,
            Filled = Filled
        };
    }
}