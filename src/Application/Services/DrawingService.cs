using Core.Entities;
using MathCore.FemCalculator;
using Svg;
using System.Drawing;
using System.Drawing.Drawing2D;
using MathCore.Common.Base;
using MathCore.FemCalculator.Model;

namespace Application.Services;
public class DrawingService
{
    private const double ScaleDefault = 100;
    private const double ScaleDisplacement = ScaleDefault * 10_000;
    private const double OffsetX = 5;
    private const double SizeCoef = 0.2;
    
    public SvgDocument DrawDisplacement(FemModel fem)
    {
        var maxX = fem.Nodes.Select(v => v.Coordinate.X).Max();
        var maxY = fem.Nodes.Select(v => Math.Abs(v.Displacement.Z)).Max();
        var coef = (maxX * SizeCoef * ScaleDefault) / maxY;

        var svg = InitSvgDocument(maxX, maxY);
        
        var beamBase = DrawValues(fem.Nodes
                .Select(node => new KeyValuePair<double, double>(node.Coordinate.X * ScaleDefault, maxY * coef)),
            Color.Coral);
        
        var beamDisplacementZ = DrawValues(fem.Nodes
                .Select(node => new KeyValuePair<double, double>(node.Coordinate.X * ScaleDefault, - (node.Displacement.Z * coef) + maxY * coef)),
            Color.DarkGreen);

        svg.Children.Add(beamBase);
        svg.Children.Add(beamDisplacementZ);

        return svg;
    }

    public SvgDocument DrawMoments(FemModel fem)
    {
        var maxX = fem.Nodes.Select(v => v.Coordinate.X).Max();
        var maxY = fem.Segments
            .SelectMany(v => new double[2] { v.First.Force!.V, v.Second.Force!.V })
            .Select(v => Math.Abs(v))
            .Max();
        
        var coef = (maxX * SizeCoef * ScaleDefault) / maxY;
        
        var beamBase = DrawValues(fem.Nodes
                .Select(node => new KeyValuePair<double, double>(node.Coordinate.X * ScaleDefault, maxY * coef)),
            Color.Coral);
        
        var svg = InitSvgDocument(maxX, maxY);
        
        var points = new List<KeyValuePair<double, double>>();


        for (var i = 0; i < fem.Segments.Count; i++)
        {
            var segment = fem.Segments[i];
            var node = fem.Nodes[i];
            var forceL = segment.First.Force!.V;
            var forceR = -segment.Second.Force!.V;

            points.Add(new KeyValuePair<double, double>(node.Coordinate.X * ScaleDefault, forceL * coef + maxY * coef));
            points.Add(new KeyValuePair<double, double>(fem.Nodes[segment.Second.Node - 1].Coordinate.X * ScaleDefault, forceR * coef + maxY * coef));
        } 
        points.Add(new KeyValuePair<double, double>(
            fem.Nodes.Last().Coordinate.X * ScaleDefault,
            -fem.Segments.Last().Second.Force!.V * coef + maxY * coef));
        
        svg.Children.Add(beamBase);
        svg.Children.Add(DrawValues(points, Color.DarkOliveGreen));

        return svg;
    }

    public SvgDocument DrawForce(FemModel fem)
    {
        
        var maxX = fem.Nodes.Select(v => v.Coordinate.X).Max();
        var maxY = fem.Segments
            .SelectMany(v => new double[2] { v.First.Force!.Z, v.Second.Force!.Z })
            .Select(v => Math.Abs(v))
            .Max();
        
        var coef = (maxX * SizeCoef * ScaleDefault) / maxY;
        
        var beamBase = DrawValues(fem.Nodes
                .Select(node => new KeyValuePair<double, double>(node.Coordinate.X * ScaleDefault, maxY * coef)),
            Color.Coral);
        
        var svg = InitSvgDocument(maxX, maxY);
        
        var points = new List<KeyValuePair<double, double>>();


        for (var i = 0; i < fem.Segments.Count; i++)
        {
            var segment = fem.Segments[i];
            var node = fem.Nodes[i];
            var forceL = segment.First.Force!.Z;
            var forceR = -segment.Second.Force!.Z;

            points.Add(new KeyValuePair<double, double>(node.Coordinate.X * ScaleDefault, forceL * coef + maxY * coef));
            points.Add(new KeyValuePair<double, double>(fem.Nodes[segment.Second.Node - 1].Coordinate.X * ScaleDefault, forceR * coef + maxY * coef));
        } 
        points.Add(new KeyValuePair<double, double>(
            fem.Nodes.Last().Coordinate.X * ScaleDefault,
            -fem.Segments.Last().Second.Force!.Z * coef + maxY * coef));
        
        svg.Children.Add(beamBase);
        svg.Children.Add(DrawValues(points, Color.DarkOliveGreen));

        return svg;
    }

    private static SvgDocument InitSvgDocument(double maxX, double maxY)
    {
        var svg = new SvgDocument();
        var coef = (maxX * SizeCoef * ScaleDefault) / maxY;
        svg.Height = new SvgUnit((float)(maxY * 2 * coef));
        svg.Width = new SvgUnit((float)(maxX * ScaleDefault + OffsetX * 2));
        return svg;
    }
    private static SvgPolyline DrawValues(IEnumerable<KeyValuePair<double, double>> values, Color color)
    {
        var line = new SvgPolyline
        {
            Points = new SvgPointCollection(),
            Fill = SvgPaintServer.None,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 1f
        };
        foreach (var point in values)
        {
            line.Points.Add(new SvgUnit((float) (OffsetX + point.Key))); //x
            line.Points.Add(new SvgUnit((float) (point.Value))); //y
        }
        return line;
    }
}
