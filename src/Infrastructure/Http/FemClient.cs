using System.Text;
using MathCore.Common.Base;
using MathCore.Common.Interfaces;
using MathCore.FemCalculator;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Infrastructure;

public class FemClient : IFemCalculator
{
    private readonly string _femUri;
    private readonly HttpClient _httpClient;

    public FemClient(HttpClient httpClient, IConfigureOptions<FemClientConfig> options)
    {
        var config = new FemClientConfig();
        options.Configure(config);

        _httpClient = httpClient;
        _femUri = config.Connection;
    }

    public async Task CalculateAsync(FemModel model)
    {
        var request = new StringContent(MapToJson(model), Encoding.UTF8, "application/json");
        var json = MapToJson(model);
        var response = await _httpClient.PostAsync(_femUri, request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<FemClientResponse.Root>(responseBody);
        MapFromJson(result!, model);
    }

    private static void MapFromJson(FemClientResponse.Root response, FemModel model)
    {
        for (var i = 0; i < model.Segments.Count; i++)
        {
            model.Segments[i].First.Displacement = new Vector6D<double>
            {
                Y = response.Beams[i].First.Displacement.Y,
                Z = response.Beams[i].First.Displacement.Z,
                U = response.Beams[i].First.Displacement.U,
                X = response.Beams[i].First.Displacement.X,
                V = response.Beams[i].First.Displacement.V,
                W = response.Beams[i].First.Displacement.W
            };
            model.Segments[i].First.Force = new Vector6D<double>
            {
                Y = response.Beams[i].First.Force.Y,
                Z = response.Beams[i].First.Force.Z,
                U = response.Beams[i].First.Force.U,
                V = response.Beams[i].First.Force.V,
                X = response.Beams[i].First.Force.X,
                W = response.Beams[i].First.Force.W
            };
            model.Segments[i].Second.Displacement = new Vector6D<double>
            {
                Y = response.Beams[i].Second.Displacement.Y,
                Z = response.Beams[i].Second.Displacement.Z,
                U = response.Beams[i].Second.Displacement.U,
                X = response.Beams[i].Second.Displacement.X,
                V = response.Beams[i].Second.Displacement.V,
                W = response.Beams[i].Second.Displacement.W
            };
            model.Segments[i].Second.Force = new Vector6D<double>
            {
                Y = response.Beams[i].Second.Force.Y,
                Z = response.Beams[i].Second.Force.Z,
                U = response.Beams[i].Second.Force.U,
                V = response.Beams[i].Second.Force.V,
                X = response.Beams[i].Second.Force.X,
                W = response.Beams[i].Second.Force.W
            };
        }

        for (var i = 0; i < model.Nodes.Count; i++)
            model.Nodes[i].Displacement = new Vector6D<double>
            {
                X = response.Nodes[i].Displacement.X,
                Y = response.Nodes[i].Displacement.Y,
                Z = response.Nodes[i].Displacement.Z,
                U = response.Nodes[i].Displacement.U,
                V = response.Nodes[i].Displacement.V,
                W = response.Nodes[i].Displacement.W
            };
    }

    private static string MapToJson(FemModel model)
    {
        return JsonConvert.SerializeObject(new FemClientRequest.Root
        {
            Nc = model.Nodes.Count,
            Bc = model.Segments.Count,
            Beams = model.Segments.Select(s => new FemClientRequest.Beam
            {
                First = new FemClientRequest.First
                {
                    Node = s.First.Node,
                    Fixed = new FemClientRequest.Fixed
                    {
                        X = s.First.IsFixed.X,
                        Y = s.First.IsFixed.Y,
                        Z = s.First.IsFixed.Z,
                        U = s.First.IsFixed.U,
                        V = s.First.IsFixed.V,
                        W = s.First.IsFixed.W
                    },
                    Flexible = new FemClientRequest.Flexible
                    {
                        X = s.First.IsFlexible.X,
                        Y = s.First.IsFlexible.Y,
                        Z = s.First.IsFlexible.Z,
                        U = s.First.IsFlexible.U,
                        V = s.First.IsFlexible.V,
                        W = s.First.IsFlexible.W
                    }
                },
                Second = new FemClientRequest.Second
                {
                    Node = s.Second.Node,
                    Fixed = new FemClientRequest.Fixed
                    {
                        X = s.Second.IsFixed.X,
                        Y = s.Second.IsFixed.Y,
                        Z = s.Second.IsFixed.Z,
                        U = s.Second.IsFixed.U,
                        V = s.Second.IsFixed.V,
                        W = s.Second.IsFixed.W
                    },
                    Flexible = new FemClientRequest.Flexible
                    {
                        X = s.Second.IsFlexible.X,
                        Y = s.Second.IsFlexible.Y,
                        Z = s.Second.IsFlexible.Z,
                        U = s.Second.IsFlexible.U,
                        V = s.Second.IsFlexible.V,
                        W = s.Second.IsFlexible.W
                    }
                },
                CrossSectionArea = s.CrossSectionalArea,
                MomentOfInertiaX = s.MomentOfInertiaX,
                MomentOfInertiaY = s.MomentOfInertiaY,
                MomentOfInertiaZ = s.MomentOfInertiaZ,
                ShearAreaY = s.ShearAreaY,
                ShearAreaZ = s.ShearAreaZ,
                StiffnessModulus = s.StiffnessModulus,
                ShearModulus = s.ShearModulus,
                ZDirection = new FemClientRequest.ZDirection
                {
                    X = s.ZDirection.X,
                    Y = s.ZDirection.Y,
                    Z = s.ZDirection.Z
                }
            }),
            Nodes = model.Nodes.Select(n => new FemClientRequest.Node
            {
                Coordinate = new FemClientRequest.Coordinate
                {
                    X = n.Coordinate.X,
                    Y = n.Coordinate.Y,
                    Z = n.Coordinate.Z
                },
                Support = new FemClientRequest.Support
                {
                    X = n.Support.X,
                    Y = n.Support.Y,
                    Z = n.Support.Z,
                    U = n.Support.U,
                    V = n.Support.V,
                    W = n.Support.W
                },
                Load = new FemClientRequest.Load
                {
                    X = n.Load.X,
                    Y = n.Load.Y,
                    Z = n.Load.Z,
                    U = n.Load.U,
                    V = n.Load.V,
                    W = n.Load.W
                }
            })
        });
    }
}