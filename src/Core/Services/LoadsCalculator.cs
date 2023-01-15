﻿using Core.Common.Interfaces;
using Core.Entities.Loads;
using MathCore.Common.Base;
using MathCore.Common.Interfaces;
using MathCore.FemCalculator;
using MathCore.FemCalculator.Model;
using System.Reflection.Metadata.Ecma335;
using static Core.Helpers.Fem.FemNodeSetter;
namespace Core.Services;

public class LoadsCalculator<TObj> : ILoadsCalculator<TObj>
    where TObj : ILoadable, IPhysicMechanicalCharacteristic, IGeometricCharacteristic
{
    private readonly IFemCalculator _femCalculator;

    public LoadsCalculator(IFemCalculator femCalculator)
    {
        _femCalculator = femCalculator;
    }
    
    public async Task<FemModel> GetFirstGroupOfLimitStates(TObj model)
    {
        var baseDots = CreateBaseDots(model);
        
        var maxDots = CreateAdditionalDots(baseDots);

        maxDots.SetSupportsValues(model.Supports)
            .SetConcentratedLoadsValues(model.ConcentratedLoads
                .Select(l => (l.Offset, l.LoadForFirstGroup)))
            .SetDistributedLoadsValues(model.DistributedLoads
                .Select(l => (l.OffsetStart, l.OffsetEnd, l.LoadForFirstGroup)));
        
        var data = new FemModel(CreateSegments(model, maxDots.Count() - 1), maxDots);

        await _femCalculator.CalculateAsync(data);
        Console.WriteLine(data.ToString());
        return data;
    }
    
    public async Task<FemModel> GetSecondGroupOfLimitStates(TObj model)
    {
        var baseDots = CreateBaseDots(model);

        var maxDots = CreateAdditionalDots(baseDots);

        maxDots.SetSupportsValues(model.Supports)
            .SetConcentratedLoadsValues(model.ConcentratedLoads
                .Select(l => (l.Offset, l.LoadForSecondGroup)))
            .SetDistributedLoadsValues(model.DistributedLoads
                .Select(l => (l.OffsetStart, l.OffsetEnd, l.LoadForSecondGroup)));

        var data = new FemModel(CreateSegments(model, maxDots.Count() - 1), maxDots);

        await _femCalculator.CalculateAsync(data);
        return data;
    }

    private static IEnumerable<Node> CreateBaseDots(TObj model)
    {
        var nodes = new List<Node>
        {
            new Node(x: 0),
            new Node(x: model.Length)
        };
        nodes
            .AddRange(model.Supports
                 .Select(support => new Node(x: support)));

        nodes
            .AddRange(model.ConcentratedLoads
                .Select(load => new Node(x: load.Offset)));

        foreach (var load in model.DistributedLoads)
        {
            nodes.Add(new Node(x: load.OffsetStart));
            nodes.Add(new Node(x: load.OffsetStart));
        }

        return nodes;
    }
    private static IOrderedEnumerable<Node> CreateAdditionalDots(IEnumerable<Node> importantNodes, double step = 0.05)
    {
        var importantNodesList = importantNodes.OrderBy(n => n.Coordinate.X).ToList();
        if (importantNodesList.Count < 2) throw new ArgumentException("number of nodes < 2");
        
        var newNodes = new List<Node>((int)(importantNodesList.Max(n => n.Coordinate.X) / step));

        for (var i = 0; i < importantNodesList.Count - 1; i++)
        {
            newNodes.Add(importantNodesList[i]);
            var distance = importantNodesList[i + 1].Coordinate.X - importantNodesList[i].Coordinate.X;
            if (distance <= 0.01) continue;

            var numberOfSegments = (int)Math.Ceiling(distance / step);

            var segmentSize = numberOfSegments < 3 ?
                distance / 3 : 
                distance / (numberOfSegments);

            for (var j = 1; j <= numberOfSegments; j++)
                newNodes.Add(new Node(x: importantNodesList[i].Coordinate.X + segmentSize * j));
        }
        newNodes.Add(importantNodesList[^1]);
        //TODO: connect same dots
        return newNodes.OrderBy(n => n.Coordinate.X);
    }
    private static IEnumerable<Segment> CreateSegments(TObj model, int count)
    {
        var segments = new Segment[count];
        for (var i = 0; i < count; i++)
        {
            segments[i] = new Segment()
            {
                First = new SegmentEnd(node: i + 1,
                    isFlexible: new Vector6D<bool>(),
                    isFixed: new Vector6D<bool>(){ U = true }),

                Second = new SegmentEnd(node: i + 2,
                    isFlexible: new Vector6D<bool>(),
                    isFixed: new Vector6D<bool>(){ U = true }),

                ZDirection = new Point3D(0, 0, 1),
                StiffnessModulus = model.StiffnessModulus,
                ShearModulus = model.ShearModulusAverage,
                CrossSectionalArea = model.CrossSectionArea,
                ShearAreaY = model.CrossSectionArea * 5 / 6,
                ShearAreaZ = model.CrossSectionArea * 5 / 6,
                MomentOfInertiaX = model.PolarMomentOfInertia,
                MomentOfInertiaY = model.MomentOfInertiaY,
                MomentOfInertiaZ = model.MomentOfInertiaZ
            };

        }
        return segments;
    }
}