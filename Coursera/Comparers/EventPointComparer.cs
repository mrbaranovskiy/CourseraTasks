using System.Collections.Generic;
using Coursera.General;

namespace Coursera.Comparers
{
    public class EventPointComparer : IComparer<Vec.EventPoint>
    {
        private readonly bool _byIndex;

        public EventPointComparer(bool byIndex = false)
        {
            _byIndex = byIndex;
        }

        public int Compare(Vec.EventPoint a, Vec.EventPoint b)
        {
            return a.CompareTo(b);
        }
    }
}