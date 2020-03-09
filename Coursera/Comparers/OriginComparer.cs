using System;
using System.Collections.Generic;
using Coursera.Structures;

namespace Coursera.Comparers
{
    public class OriginComparer : IComparer<Vector2d>
    {
        public int Compare(Vector2d x, Vector2d y)
        {
            if (x.X > y.X)
                return 1;
            if (Math.Abs(x.X - y.X) < Double.Epsilon)
            {
                if (x.Y > y.Y)
                    return 1;
                if (Math.Abs(x.Y - y.Y) < Double.Epsilon)
                    return 0;
            }

            return -1;
        }
    }
}