﻿using MathCore.FemCalculator;
using MathCore.FemCalculator.Model;

namespace Core.Common.Interfaces;

public interface ILoadsCalculator<in TObj>
    where TObj : ILoadable, IPhysicMechanicalCharacteristic, IGeometricCharacteristic
{
    Task<FemModel> GetFirstGroupOfLimitStates(TObj model);
    Task<FemModel> GetSecondGroupOfLimitStates(TObj model);
    IEnumerable<SegmentMaximum> GetSegmentMaximums(TObj model, FemModel fem);
}

public record SegmentMaximum(Node Node, double AbsoluteValue, double RelativeValue);