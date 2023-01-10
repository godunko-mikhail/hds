﻿using FluentValidation;

namespace Application.WoodenConstruction.Queries.GetBeamFull;

public class GetBeamFullQueryValidator : AbstractValidator<GetBeamFullQuery>
{
    public GetBeamFullQueryValidator()
    {
        RuleFor(v => v.Length)
            .GreaterThanOrEqualTo(300)
            .LessThanOrEqualTo(12000);

        RuleFor(v => v.Width)
            .GreaterThan(19)
            .LessThan(400);

        RuleFor(v => v.Height)
            .GreaterThan(19)
            .LessThan(400);

        RuleFor(v => v.Amount)
            .GreaterThan(0);

        RuleFor(v => v.LifeTime)
            .GreaterThan(50);

        RuleForEach(v => v.Supports)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(12000);

        RuleFor(v => v.SteadyTemperature)
            .GreaterThan(-60)
            .LessThan(60);

        RuleForEach(v => v.DistributedLoads).ChildRules(v =>
        {
            v.RuleFor(load => load.OffsetStart)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12000);
            v.RuleFor(load => load.OffsetEnd)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12000);
        });
        RuleForEach(v => v.ConcentratedLoads).ChildRules(v =>
        {
            v.RuleFor(load => load.Offset)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(12000);
        });
    }
}