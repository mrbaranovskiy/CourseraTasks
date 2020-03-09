using System;
using System.Collections.Generic;
using Coursera.General;
using Coursera.Structures;

namespace Coursera.Comparers
{
    public class PolarComparer : IComparer<Vector2d>
    {
        private readonly Vector2d _center;

        public PolarComparer(Vector2d center)
        {
            _center = center;
        }

        public int Compare(Vector2d x, Vector2d y)
        {
            var a = Vec.SignedAround(x, Vector2d.UnitX, _center);
            var b = Vec.SignedAround(y, Vector2d.UnitX, _center);

            if (a > b)
                return 1;
            if (Math.Abs(a - b) < Double.Epsilon)
                return 0;
            return -1;
        }
    }
}