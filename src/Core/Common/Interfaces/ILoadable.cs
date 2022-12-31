﻿using HDS.Core.Beam.Entities;
using static HDS.Core.Data;

namespace Core.Common.Interfaces
{
    public interface ILoadable
    {
        LoadingModes LoadingMode { get; set; }
        IEnumerable<double> Supports { get; set; }
        IEnumerable<DistributedLoad> DistributedLoads { get; set; }
        IEnumerable<ConcentratedLoad> ConcentratedLoads { get; set; }
    }
}
