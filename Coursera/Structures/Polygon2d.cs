using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coursera.Comparers;
using Coursera.General;

namespace Coursera.Structures
{
    public struct Polygon2d : IEnumerable<Vector2d>, ICollection<Vector2d>
    {
        private readonly List<Vector2d> _points;

        private LinkedList<Vector2d> _lst;

        private double CrossProduct(Vector2d a,
            Vector2d b, Vector2d c)
        {
            double aa = a.X - b.X;
            double bb = a.Y - b.Y;
            double cc = c.X - b.X;
            double dd = c.Y - b.Y;

            return aa * dd - bb * cc;
        }


        public Vector2d[] AsArray(IComparer<Vector2d> comparer = null)
        {
            if (comparer == null)
                return _lst.ToArray();

            var arr = _lst.ToArray();
            Array.Sort(arr, comparer);

            return arr;
        }

        public Polygon2d ConvexHull()
        {
            if (_points.Count < 3)
                return new Polygon2d(_points);

            var ordered = _points.ToArray();
            Array.Sort(ordered, new OriginComparer());

            var lu = new List<Vector2d> {ordered[0], ordered[1]};

            for (int i = 2; i < ordered.Length; i++)
            {
                var p = ordered[i];
                lu.Add(p);

                while (lu.Count > 2
                       && Vec.CheckOrientation(lu[lu.Count - 3], lu[lu.Count - 2], lu[lu.Count - 1]) !=
                       Orientation.RIGHT)
                    lu.RemoveAt(lu.Count - 2);
            }

            var limit = lu.Count + 1;

            for (var i = ordered.Length - 1; i >= 0; i--)
            {
                var p = ordered[i];
                lu.Add(p);

                while (lu.Count > limit
                       && Vec.CheckOrientation(lu[lu.Count - 3], lu[lu.Count - 2], lu[lu.Count - 1]) !=
                       Orientation.RIGHT)
                    lu.RemoveAt(lu.Count - 2);
            }

            lu.RemoveAt(lu.Count - 1);
            var poly = new Polygon2d(lu);

            return poly;
        }

        public Polygon2d(List<Vector2d> points)
        {
            _points = points;
            _lst = new LinkedList<Vector2d>();

            foreach (var Vector2 in points) Add(Vector2);
        }

        public bool IsConvex()
        {
            if (_points.Count < 4)
                return true;

            bool sign = false;
            int n = _points.Count;

            for (int i = 0; i < n; i++)
            {
                double dx1 = _points[(i + 2) % n].X - _points[(i + 1) % n].X;
                double dy1 = _points[(i + 2) % n].Y - _points[(i + 1) % n].Y;
                double dx2 = _points[i].X - _points[(i + 1) % n].X;
                double dy2 = _points[i].Y - _points[(i + 1) % n].Y;
                double zcrossproduct = dx1 * dy2 - dy1 * dx2;

                if (i == 0)
                    sign = zcrossproduct > 0;
                else if (sign != (zcrossproduct > 0))
                    return false;
            }

            return true;
        }


        public void Add(Vector2d vector)
        {
            _lst.AddFirst(vector);
        }

        public void Clear()
        {
            _lst.Clear();
        }

        public bool Contains(Vector2d item)
        {
            return _lst.Contains(item);
        }

        public void CopyTo(Vector2d[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Vector2d item)
        {
            return _lst.Remove(item);
        }

        public int Count => _lst.Count;
        public bool IsReadOnly => false;

        private LinkedList<Vector2d> List => _lst;

        public static bool PointInPolygon(IList<Vector2d> poly, Vector2d pt)
        {
            int i, j;
            bool result = false;
            for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++) // j runs one step behind i
            {
                var piy = poly[i].Y;
                var pix = poly[i].X;
                var pjy = poly[j].Y;
                var pjx = poly[j].X;

                if ((piy <= pt.Y && pt.Y < pjy || pjy <= pt.Y && pt.Y < piy) &&
                    Math.Abs(pjy - piy) > double.Epsilon
                    && pt.X < (pjx - pix) * (pt.Y - piy) / (pjy - piy) + pix)
                    result = !result;
            }

            return result;
        }

        public static Polygon2d Intersects(Polygon2d current, Polygon2d other)
        {
            var segments = current.Segments.ToList();
            segments.AddRange(other.Segments);

            var result = Vec.SegmentIntersection(segments.ToArray()).ToList();

            var currentArr = current.AsArray();
            var otherArr = other.AsArray();

//            foreach (var p in currentArr)
//            {
//
//                if (PointInPolygon(currentArr, p))
//                    result.Add(p);
//
//            }
//
//            foreach (var VARIABLE in o)
//            {
//
//            }

            if(!result.Any())
                return new Polygon2d();

            return new Polygon2d(result.ToList()).ConvexHull();
        }


        public Orientation CheckPointOrientation(Vector2d pt)
        {
            var segments = Segments.ToArray();
            var onBorder = segments.Any(n => Vec.CheckOrientation(pt, n.A, n.B) == Orientation.ON_SEGMENT);

            if (onBorder)
                return Orientation.BORDER;
            else
            {
                var ray = new Ray2d(pt, new Vector2d(1, 0));
                int count = 0;

                foreach (var segmentI in segments)
                {
                    Vector2d intersection;
                    if (ray.RayWithSegment(segmentI, out intersection)) count++;
                }

                var result = PointInPolygon(_lst.ToArray(), pt);

                if (result)
                    return Orientation.INSIDE;
                return Orientation.OUTSIDE;
            }
        }

        private IEnumerable<Segment2d> Segments
        {
            get
            {
                var cur = _lst.First;

                while (cur != _lst.Last)
                {
                    if (cur.Next != null)
                    {
                        yield return new Segment2d(cur.Value, cur.Next.Value);
                        cur = cur.Next;
                    }
                }

                yield return new Segment2d(_lst.Last.Value, _lst.First.Value);
            }
        }

        public IEnumerator<Vector2d> GetEnumerator()
        {
            return _lst.GetEnumerator();
        }

        public override string ToString()
        {
            if (List == null || List.Count == 0)
                return string.Empty;

            if (List.Count == 1)
                return $"{List.First.Value.X} {List.First.Value.Y}";
            return List.Select(v => $"{v.X} {v.Y}").Aggregate((m, n) => m + " " + n);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}