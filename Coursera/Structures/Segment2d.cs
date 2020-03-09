using System;
using System.Collections.Generic;
using System.Linq;
using Coursera.Comparers;
using Coursera.General;

namespace Coursera.Structures
{
    public struct Segment2d : IEqualityComparer<Segment2d>
    {
        public Segment2d(Vector2d a, Vector2d b)
        {
            A = a;
            B = b;
        }

        public Segment2d SwapToY()
        {
            return A.Y < B.Y ? new Segment2d(B, A) : this;
        }

        private static Segment2d NaN = new Segment2d(Vector2d.NaN, Vector2d.NaN);

        public bool IsNaN => Vector2d.IsNaN(A) || Vector2d.IsNaN(B);

        public Vector2d Dir => B - A;

        public Vector2d A { get; }
        public Vector2d B { get; }

        public bool Intersection(Segment2d segment, out Vector2d intersection, out Segment2d seg)
        {
            var x1 = A.X;
            var y1 = A.Y;
            var x2 = B.X;
            var y2 = B.Y;
            var x3 = segment.A.X;
            var y3 = segment.A.Y;
            var x4 = segment.B.X;
            var y4 = segment.B.Y;

            var denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (Math.Abs(denom) > float.Epsilon)
            {
                var t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denom;
                var u = ((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / -denom;

                if (Vec.InOpenRange(t, 0, 1) && Vec.InOpenRange(u, 0, 1))
                {
                    intersection = new Vector2d(
                        x1 + t * (x2 - x1),
                        y1 + t * (y2 - y1)
                    );

                    seg = NaN;
                    return true;
                }
            }
            else
            {
                var onSegment = new SortedSet<Vector2d>(new OriginComparer());

                var o1 = Vec.CheckOrientation(segment.A, A, B);
                var o2 = Vec.CheckOrientation(segment.B, A, B);
                var o3 = Vec.CheckOrientation(A, segment.A, segment.B);
                var o4 = Vec.CheckOrientation(B, segment.A, segment.B);

                //shit code
                if (o1 == Orientation.ON_SEGMENT) onSegment.Add(segment.A);
                if (o2 == Orientation.ON_SEGMENT) onSegment.Add(segment.B);
                if (o3 == Orientation.ON_SEGMENT) onSegment.Add(A);
                if (o4 == Orientation.ON_SEGMENT) onSegment.Add(B);

                var or = new[] {o1, o2, o3, o4};

                if (o1 == o2 && o2 == o3 & o3 == o4 && o4 == Orientation.ONLINE)
                {
                    intersection = Vector2d.NaN;
                    seg = NaN;
                    return false;
                }

                if (o1 == o2 && o2 == o3 & o3 == o4 && o4 == Orientation.ON_SEGMENT)
                {
                    intersection = Vector2d.NaN;
                    seg = segment;
                    return true;
                }

                if (or.Any(n => n == Orientation.LEFT || n == Orientation.RIGHT))
                {
                    intersection = Vector2d.NaN;
                    seg = NaN;
                    return false;
                }


                if (onSegment.Count == 1)
                {
                    intersection = onSegment.First();
                    seg = NaN;
                    return true;
                }

                if (onSegment.Count > 1)
                {
                    var arr = onSegment.ToArray();
                    double minDist = double.MaxValue; // don't need
                    var a = Vector2d.NaN;
                    var b = Vector2d.NaN;

                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        var cur = (i + 1) % onSegment.Count;
                        var next = (cur + 1) % onSegment.Count;

                        //govno code
                        var sqrDist = arr[cur].SqrDist(arr[next]);

                        if (sqrDist < minDist)
                        {
                            minDist = sqrDist;
                            a = arr[cur];
                            b = arr[next];
                        }
                    }

                    intersection = Vector2d.NaN;
                    seg = new Segment2d(a, b);
                    return true;
                }
            }

            intersection = Vector2d.NaN;
            seg = NaN;
            return false;
        }

        public static bool operator ==(Segment2d x, Segment2d y)
        {
            if (x.IsNaN || y.IsNaN) return false;
            return x.A == y.A && x.B == y.B || x.A == y.B && x.B == y.A;
        }

        public static bool operator !=(Segment2d left, Segment2d right)
        {
            return !(left == right);
        }

        public bool Equals(Segment2d x, Segment2d y)
        {
            if (x.Equals(y)) return true;
            if (x.IsNaN || y.IsNaN) return false;

            return x == y;
        }

        public int GetHashCode(Segment2d obj)
        {
            var a = obj.A.X.GetHashCode();
            var b = obj.A.X.GetHashCode();
            var c = obj.A.X.GetHashCode();
            var d = obj.A.X.GetHashCode();

            return unchecked(a * b * c * d);
        }
    }
}