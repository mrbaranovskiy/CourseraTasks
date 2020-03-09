using System.Collections.Generic;
using Coursera.Structures;

namespace Coursera.Comparers
{
    public class SegmentsOriginXComparer : IComparer<Segment2d>
    {
        public int Compare(Segment2d a, Segment2d b)
        {
            if (a.A.X < b.A.X)
                return 1;
            return a.A.Y > a.B.Y
                ? 1
                : -1;
        }
    }
}