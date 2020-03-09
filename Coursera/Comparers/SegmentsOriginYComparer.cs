using System.Collections.Generic;
using Coursera.Structures;

namespace Coursera.Comparers
{
    public class SegmentsOriginYComparer : IComparer<Segment2d>
    {
        public int Compare(Segment2d a, Segment2d b)
        {
            if (a.A.Y > b.A.Y)
                return 1;
            return a.A.X < a.B.X ? 1 : -1;
        }
    }
}