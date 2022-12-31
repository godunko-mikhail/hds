﻿using MathCore.Common.Base;

namespace MathCore.FemCalculator
{
    public class Beam
    {
        public BeamEnd First { get; set; }
        public BeamEnd Second { get; set; }
        public Point3D ZDirection { get; set; }
        public double StiffnessModulus { get; set; }
        public double ShearModulus { get; set; }
        public double CrossSectionalArea { get; set; }
        public double ShearAreaY { get; set; }
        public double ShearAreaZ { get; set; }
        public double MomentOfInertiaX { get; set; }
        public double MomentOfInertiaY { get; set; }
        public double MomentOfInertiaZ { get; set; }

        public Beam()
        {
            First = new BeamEnd();
            Second = new BeamEnd();
            ZDirection = new Point3D();
        }
    }
}
