using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;

namespace Coursera
{
    class Program
    {
        static void Main(string[] args)
        {
//            Segment2d seg = new Segment2d(new Vector2d(-5,0), new Vector2d(0,0));
//            Segment2d seg2 = new Segment2d(new Vector2d(0,0), new Vector2d(2, 0) );
//
//            Segment2d segOut;
//            Vector2d vec;
//            var intersection = seg.Intersection(seg2, out vec, out segOut);

            var list = new List<string>();
            string input = null;

            while (!string.IsNullOrEmpty(input = Console.In.ReadLine()))
            {
                list.Add(input);
            }

            if (!list.Any())
                Console.Error.WriteLine("CMD LINE ERROR!!!");
            //
            IProcessTask task1_1 = new PolygonsIntersection();
            var result = task1_1.ProcessTask(list.ToArray());

            Output(result);
        }

        private static void Output(string[] results)
        {
            foreach (var result in results)
                Console.Out.WriteLine(result);
        }
    }

    public class PolygonsIntersection : IProcessTask
    {
        public string[] ProcessTask(string[] stdIn)
        {
            var c1 = Parse.ParseCount(stdIn[0]);
            var pts1 = Parse.ParseIntCoordinates(stdIn[1], c1);

            var c2 = Parse.ParseCount(stdIn[2]);
            var pts2 = Parse.ParseIntCoordinates(stdIn[3], c2);

            var p1 = new Polygon2d(pts1);
            var p2 = new Polygon2d(pts2);

            var hull = Polygon2d.Intersects(p1, p2);


            return null;
        }
    }


    #region STUFF

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
            if(comparer == null)
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
                       && Vec.CheckOrientation(lu[lu.Count - 3],lu[lu.Count - 2], lu[lu.Count - 1]) != Orientation.RIGHT)
                    lu.RemoveAt(lu.Count - 2);
            }

            var limit = lu.Count + 1;

            for (var i = ordered.Length - 1; i >= 0; i--)
            {
                var p = ordered[i];
                lu.Add(p);

                while (lu.Count > limit
                       && Vec.CheckOrientation(lu[lu.Count - 3],lu[lu.Count - 2], lu[lu.Count - 1]) != Orientation.RIGHT)
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

            for(int i = 0; i < n; i++)
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
            var hullPts = new SortedSet<Vector2d>();

            foreach (var currentSegment in current.Segments)
            {
                foreach (var otherSegment in other.Segments)
                {
                    Vector2d inter;
                    Segment2d segment;
                    var result = currentSegment.Intersection(otherSegment, out inter, out segment);

                    if (result)
                    {
                        //If segments intersected on point
                        if (!Vector2d.IsNaN(inter))
                            hullPts.Add(inter);
                        else if(!segment.IsNaN)
                        {
                            hullPts.Add(segment.A);
                            hullPts.Add(segment.B);
                        }
                    }
                }
            }

            foreach (var currentPoint in current._points)
            {
                if (PointInPolygon(other._points, currentPoint))
                    hullPts.Add(currentPoint);
            }

            foreach (var otherPoint in other._points)
            {
                if (PointInPolygon(current._points, otherPoint))
                    hullPts.Add(otherPoint);
            }

            //remove degenerated
            var hull = new Polygon2d(hullPts.ToList()).ConvexHull();

            for (int i = 1; i < hull.Count - 2; i++)
            {
                var q = (i + 1) % hull.Count;
                var p = (q + 1) % hull.Count;
                var l = (p + 1) % hull.Count;

                var pts = hull._points;

                if (pts[q].OnLine(pts[p], pts[l]))
                {
                    hull.Remove(pts[p]);
                }
            }

            return hull;
        }


        public Orientation CheckPointOrientation(Vector2d pt)
        {
            var segments = Segments.ToArray();
            var onBorder = segments.Any(n => Vec.CheckOrientation(pt,n.A, n.B) == Orientation.ON_SEGMENT);

            if (onBorder)
                return Orientation.BORDER;
            else
            {
                var ray = new RayI(pt, new Vector2d(1, 0));
                int count = 0;

                foreach (var segmentI in segments)
                {
                    Vector2d? intersection;
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

    interface IProcessTask
    {
        string[] ProcessTask(string[] stdIn);
    }

    public enum Orientation
    {
        LEFT,
        RIGHT,
        ONLINE,
        ON_SEGMENT,
        INSIDE,
        OUTSIDE,
        BORDER
    }

    public static class Parse
    {
        public static string ParseOrientation(Orientation o)
        {
            switch (o)
            {
                case Orientation.LEFT:
                    return "LEFT";
                case Orientation.RIGHT:
                    return "RIGHT";
                case Orientation.ONLINE:
                    return "ON_LINE";
                case Orientation.ON_SEGMENT:
                    return "ON_SEGMENT";
                case Orientation.INSIDE:
                    return "INSIDE";
                case Orientation.OUTSIDE:
                    return "OUTSIDE";
                case Orientation.BORDER:
                    return "BORDER";
                default:
                    throw new ArgumentOutOfRangeException(nameof(o), o, null);
            }
        }

        public static int[] ParseIntArrayString(string str, int num)
        {
            var arr = str.Split(' ').Select(n => int.Parse(n.Trim())).ToArray();

            if (arr.Count() != num)
                throw new Exception("Parse exception");

            return arr;
        }

        public static List<Vector2d> ParseIntCoordinates(string str, int num)
        {
            var nums = ParseIntArrayString(str, num * 2);
            List<Vector2d> vertices = new List<Vector2d>();

            for (int i = 0; i < num; i++)
            {
                var x = nums[i * 2];
                var y = nums[i * 2 + 1];

                vertices.Add(new Vector2d(x, y));
            }

            return vertices;
        }

        public static Vector2d ParseVector(string vector)
        {
            var vec = ParseIntArrayString(vector, 2);

            return new Vector2d(vec[0], vec[1]);
        }


        public static int ParseCount(string segCountStr)
            => Parse.ParseIntArrayString(segCountStr, 1)[0];
    }

    public struct Segment2d
    {
        public Segment2d(Vector2d a, Vector2d b)
        {
            A = a;
            B = b;
        }

        public static Segment2d NaN = new Segment2d(Vector2d.NaN, Vector2d.NaN);

        public bool IsNaN => Vector2d.IsNaN(A) || Vector2d.IsNaN(B);

        public Vector2d Dir => B - A;

        public Vector2d A { get; }
        public Vector2d B { get; }

        public bool Intersection(Segment2d segment, out Vector2d intersection, out Segment2d seg)
        {
            var x1 = A.X; var y1 = A.Y;
            var x2 = B.X; var y2 = B.Y;
            var x3 = segment.A.X; var y3 = segment.A.Y;
            var x4 = segment.B.X; var y4 = segment.B.Y;

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

                    seg = new Segment2d();
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
                if(o1 == Orientation.ON_SEGMENT) onSegment.Add(segment.A);
                if(o2 == Orientation.ON_SEGMENT) onSegment.Add(segment.B);
                if(o3 == Orientation.ON_SEGMENT) onSegment.Add(A);
                if(o4 == Orientation.ON_SEGMENT) onSegment.Add(B);

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
                    double minDist = double.MaxValue;
                    var a = Vector2d.NaN;
                    var b = Vector2d.NaN;

                    for (int i = 0; i < arr.Length - 1 ; i++)
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
                    seg = new Segment2d(a,b);
                    return true;
                }

            }

            intersection = Vector2d.NaN; seg = NaN;
            return false;

        }
    }

    public struct TriangleI
    {
        private readonly Vector2d _a;
        private readonly Vector2d _b;
        private readonly Vector2d _c;

        public TriangleI(Vector2d a, Vector2d b, Vector2d c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public Orientation OrientatePoint(Vector2d p)
        {
            var o1 = Vec.CheckOrientation(p,_a, _b);
            var o2 = Vec.CheckOrientation(p,_b, _c);
            var o3 = Vec.CheckOrientation(p,_c, _a);;

            if (o1 == o2 && o2 == o3 && (o3 == Orientation.RIGHT || o3 == Orientation.LEFT))
                return Orientation.INSIDE;
            if (o1 == Orientation.ON_SEGMENT || o2 == Orientation.ON_SEGMENT || o3 == Orientation.ON_SEGMENT)
                return Orientation.BORDER;

            return Orientation.OUTSIDE;
        }


    }

    public struct RayI
    {
        public Vector2d Point { get; }
        public Vector2d Dir { get; }

        public RayI(Vector2d point, Vector2d dir)
        {
            Point = point;
            Dir = dir;
        }


        public bool RayWithSegment(Segment2d seg, out Vector2d? intersection)
        {
            Vector2d segDir = seg.Dir;
            float det = Dir.X * -segDir.Y + Dir.Y * segDir.X;

            if (Math.Abs(det) > float.Epsilon)
            {
                float left = ((seg.A.X - Point.X) * -segDir.Y +
                               (seg.A.Y - Point.Y) * segDir.X) / det;
                float right = ((Point.X - seg.A.X) * -Dir.Y +
                                (Point.Y - seg.A.Y) * Dir.X) / -det;

                if (left >= 0.0 && right >= 0.0 && right <= 1.0)
                {
                    intersection = Vec.Mid(Point, Point + Dir, left);
                    return true;
                }

                intersection = null;
                return false;
            }

            intersection = null;
            return false;
        }
    }

    public static class Vec
    {

        public static float Sqr(float value)
        {
            return value * value;
        }

        public static bool InOpenRange<T>(T num, T lower, T upper) where T
            : IComparable
        {
            if (lower.CompareTo(upper) == 1)
                throw new Exception("Go v sraku exception");

            if (num.CompareTo(lower) > -1)
                return num.CompareTo(upper) < 1;
            return false;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = b;
            b = a;
            a = temp;
        }

        public static Orientation CheckOrientation(Vector2d q, Vector2d a, Vector2d b)
        {
            var val = (b.Y - q.Y) * (a.X - b.X) - (b.X - q.X) * (a.Y - b.Y);

            if (Math.Abs(val) < double.Epsilon)
            {
                var minx = Math.Min(b.X, a.X);
                var maxx = Math.Max(b.X, a.X);
                var miny = Math.Min(b.Y, a.Y);
                var maxy = Math.Max(b.Y, a.Y);

                if (q.X >= minx && q.X <= maxx && q.Y >= miny && q.Y <= maxy)
                    return Orientation.ON_SEGMENT;
                return Orientation.ONLINE;
            }

            return val < 0 ? Orientation.RIGHT : Orientation.LEFT;
        }

        public static double SignedAround(Vector2d a, Vector2d b, Vector2d around)
        {
            var ax = -around.X + a.X;
            var bx = -around.X + b.X;
            var ay = -around.Y + a.Y;
            var by = -around.Y + b.Y;

            return AngleSigned(new Vector2d(ax, ay), new Vector2d(bx, by));
        }

        public static Vector2d Mid(Vector2d v0, Vector2d v1, float factor)
        {
            return v0 * (1f - factor) + v1 * factor;
        }

        public static double AngleToX(Vector2d v)
        {
            return Math.Atan2(v.Y, v.X);
        }

        public static double AngleSigned(Vector2d v1, Vector2d v2)
        {
            double num = Math.Acos(AngleCos(v1, v2));
            if (-v1.Y * v2.X + v1.X * v2.Y < 0.0)
                num = -num;
            return num;
        }

        public static double AngleCos(Vector2d v1, Vector2d v2)
        {
            double num = (v1.X * v2.X + v1.Y * v2.Y) / Math.Sqrt((v1.X * v1.X + v1.Y * v1.Y) * (v2.X * v2.X + v2.Y * v2.Y));
            if (num < -1.0)
                return -1.0;
            if (num > 1.0)
                return 1.0;
            return num;
        }

        public static double Angle0To2Pi(Vector2d v1, Vector2d v2)
        {
            double num = AngleShortest(v1, v2);
            if (-v1.Y * v2.X + v1.X * v2.Y < 0.0)
                num = 2.0 * Math.PI - num;
            return num;
        }

        public static double AngleShortest(Vector2d v1, Vector2d v2) => Math.Acos(AngleCos(v1, v2));
    }
    
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

     public struct Vector2d : IComparable<Vector2d>
    {
        public override string ToString()
        {
            return $"{X} {Y}";
        }

        public double Dist(Vector2d v)
        {
            return Math.Sqrt(Vec.Sqr(X - v.X) + Vec.Sqr(Y - v.Y));
        }

        public double SqrDist(Vector2d v)
        {
            return Vec.Sqr(this.X - v.X) + Vec.Sqr(this.Y - v.Y);
        }

        public float X { get; }
        public float Y { get; }
        public static Vector2d NaN => new Vector2d(float.NaN, float.NaN);
        public static Vector2d UnitX => new Vector2d(1,0);

        public static bool IsNaN(Vector2d vec)
        {
            return float.IsNaN(vec.X) || float.IsNaN(vec.Y);
        }

        public Vector2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2d Mid(Vector2d v0, Vector2d v1, float factor)
        {
            return v0 * (1.0f - factor) + v1 * factor;
        }

        public Vector2d Sub(Vector2d v)
        {
            return new Vector2d(this.X - v.X, this.Y - v.Y);
        }

        public static bool operator ==(Vector2d left, Vector2d right)
        {
            return Math.Abs(left.X - right.X) < double.Epsilon && Math.Abs(left.Y - right.Y) < float.Epsilon;
        }

        public static bool operator !=(Vector2d left, Vector2d right)
        {
            return !(left == right);
        }

        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2d operator -(Vector2d left, Vector2d right)
        {
            return new Vector2d(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2d operator *(Vector2d lef, float factor)
        {
            return new Vector2d(lef.X * factor, lef.Y * factor);
        }

        public static Vector2d operator *(float factor, Vector2d rhs)
        {
            return new Vector2d(factor * rhs.X, factor * rhs.Y);
        }

        public bool CounterClockWise(Vector2d p, Vector2d q)
        {
            return ((p.X - X) * (q.Y - Y)) > ((p.Y - Y) * (q.X - X));
        }

        public bool OnLine(Vector2d q, Vector2d p)
        {
            return OnLine(new Vector2d(X, Y), q, p );
        }

        public static bool OnLine(Vector2d q, Vector2d p, Vector2d l)
        {
            return Math.Abs(((p.X - q.X) * (l.Y - q.Y)) - ((p.Y - q.Y) * (l.X - q.X))) < float.Epsilon;
        }

        public Orientation CheckLeftRight(Vector2d a, Vector2d b)
        {
            var val = (b.Y - Y) * (a.X - b.X) - (b.X - X) * (a.Y - b.Y);

//            if (Math.Abs(val) < double.Epsilon)
//            {
//                var minx = Math.Min(b.X, a.X);
//                var maxx = Math.Max(b.X, a.X);
//                var miny = Math.Min(b.Y, a.Y);
//                var maxy = Math.Max(b.Y, a.Y);
//
//                if (X >= minx && X <= maxx && Y >= miny && Y <= maxy)
//                    return Orientation.ON_SEGMENT;
//                return Orientation.ONLINE;
//            }

            return val < 0 ? Orientation.RIGHT : Orientation.LEFT;
        }

        public int CompareTo(Vector2d other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }
    }

    #endregion
}