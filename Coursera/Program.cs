using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Coursera
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>();
            string input = null;

            while (!string.IsNullOrEmpty(input = Console.In.ReadLine()))
            {
                list.Add(input);
            }

            if (!list.Any())
                Console.Error.WriteLine("CMD LINE ERROR!!!");
            //
            IProcessTask task1_1 = new GrahamsAlgo2_1();
            var result = task1_1.ProcessTask(list.ToArray());

            Output(result);
        }

        private static void Output(string[] results)
        {
            foreach (var result in results)
                Console.Out.WriteLine(result);
        }
    }


    public struct Polygon2d
    {
        private LinkedList<VectorI> _lst;

        public Polygon2d(List<VectorI> points)
        {
            _lst = new LinkedList<VectorI>();

            foreach (var vectorI in points) Add(vectorI);
        }

        public void Add(VectorI vector)
        {
            _lst.AddFirst(vector);
        }

        private LinkedList<VectorI> List => _lst;

        public static bool PointInPolygon(IList<VectorI> poly, VectorI pt)
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


        public Orientation CheckPointOrientation(VectorI pt)
        {
            var segments = Segments.ToArray();
            var onBorder = segments.Any(n => pt.CheckOrientation(n.A, n.B) == Orientation.ON_SEGMENT);

            if (onBorder)
                return Orientation.BORDER;
            else
            {
                var ray = new RayI(pt, new VectorI(1, 0));
                int count = 0;

                foreach (var segmentI in segments)
                {
                    VectorI intersection;
                    if (ray.RayWithSegment(segmentI, out intersection)) count++;
                }

                var result = PointInPolygon(_lst.ToArray(), pt);

                if (result)
                    return Orientation.INSIDE;
                return Orientation.OUTSIDE;
            }
        }

        private IEnumerable<SegmentI> Segments
        {
            get
            {
                var cur = _lst.First;

                while (cur != _lst.Last)
                {
                    if (cur.Next != null)
                    {
                        yield return new SegmentI(cur.Value, cur.Next.Value);
                        cur = cur.Next;
                    }
                }

                yield return new SegmentI(_lst.Last.Value, _lst.First.Value);
            }
        }
    }

    public struct VectorI : IComparable<VectorI>
    {
        private bool Equals(VectorI other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is VectorI other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public double X { get; }
        public double Y { get; }
        public static VectorI NaN => new VectorI(double.NaN, double.NaN);

        public VectorI(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static VectorI Mid(VectorI v0, VectorI v1, double factor)
        {
            return v0 * (1.0 - factor) + v1 * factor;
        }

        public VectorI Sub(VectorI v)
        {
            return new VectorI(this.X - v.X, this.Y - v.Y);
        }

        public static bool operator ==(VectorI left, VectorI right)
        {
            return Math.Abs(left.X - right.X) < double.Epsilon && Math.Abs(left.Y - right.Y) < double.Epsilon;
        }

        public static bool operator !=(VectorI left, VectorI right)
        {
            return !(left == right);
        }

        public static VectorI operator +(VectorI a, VectorI b)
        {
            return new VectorI(a.X + b.X, a.Y + b.Y);
        }

        public static VectorI operator -(VectorI left, VectorI right)
        {
            return new VectorI(left.X - right.X, left.Y - right.Y);
        }

        public static VectorI operator *(VectorI lef, double factor)
        {
            return new VectorI(lef.X * factor, lef.Y * factor);
        }

        public static VectorI operator *(double factor, VectorI rhs)
        {
            return new VectorI(factor * rhs.X, factor * rhs.Y);
        }

        public Orientation CheckOrientation(VectorI a, VectorI b)
        {
            var val = (b.Y - Y) * (a.X - b.X) - (b.X - X) * (a.Y - b.Y);

            if (Math.Abs(val) < double.Epsilon)
            {
                var minx = Math.Min(b.X, a.X);
                var maxx = Math.Max(b.X, a.X);
                var miny = Math.Min(b.Y, a.Y);
                var maxy = Math.Max(b.Y, a.Y);

                if (X >= minx && X <= maxx && Y >= miny && Y <= maxy)
                    return Orientation.ON_SEGMENT;
                return Orientation.ONLINE;
            }

            return val > 0 ? Orientation.LEFT : Orientation.RIGHT;
        }

        public int CompareTo(VectorI other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }
    }

    interface IProcessTask
    {
        string[] ProcessTask(string[] stdIn);
    }

    public class GrahamsAlgo2_1 : IProcessTask
    {
        private List<VectorI> Vertices(string verticesStr, int count)
        {
            return Parse.ParseIntCoordinates(verticesStr, count);
        }

        public string[] ProcessTask(string[] stdIn)
        {
            var count = Parse.ParseCount(stdIn[0]);
            var vertices = Parse.ParseIntCoordinates(stdIn[1], count);

            var result = Graham(vertices);
            return null;
        }

        private Polygon2d Graham(List<VectorI> vertices)
        {
            //check count
            var ordered = vertices.OrderBy(n => n).ToArray();
            var lu = new List<VectorI> {ordered[0], ordered[1]};


            for (int i = 2; i < ordered.Length; i++)
            {
                var p = ordered[i];
                lu.Add(p);

                while (lu.Count > 2
                       && lu[lu.Count - 3].CheckOrientation(lu[lu.Count - 2], lu[lu.Count - 1]) != Orientation.LEFT)
                    lu.RemoveAt(lu.Count - 2);
            }

            var poly = new Polygon2d(lu);

            return poly;

        }
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

        public static List<VectorI> ParseIntCoordinates(string str, int num)
        {
            var nums = ParseIntArrayString(str, num * 2);
            List<VectorI> vertices = new List<VectorI>();

            for (int i = 0; i < num; i++)
            {
                var x = nums[i * 2];
                var y = nums[i * 2 + 1];

                vertices.Add(new VectorI(x, y));
            }

            return vertices;
        }

        public static VectorI ParseVector(string vector)
        {
            var vec = ParseIntArrayString(vector, 2);

            return new VectorI(vec[0], vec[1]);
        }


        public static int ParseCount(string segCountStr)
            => Parse.ParseIntArrayString(segCountStr, 1)[0];
    }


    public struct SegmentI
    {
        public SegmentI(VectorI a, VectorI b)
        {
            A = a;
            B = b;
        }

        public VectorI Dir => B.Sub(A);

        public VectorI A { get; }
        public VectorI B { get; }


        public struct TriangleI
        {
            private readonly VectorI _a;
            private readonly VectorI _b;
            private readonly VectorI _c;

            public TriangleI(VectorI a, VectorI b, VectorI c)
            {
                _a = a;
                _b = b;
                _c = c;
            }

            public Orientation OrientatePoint(VectorI p)
            {
                var o1 = p.CheckOrientation(_a, _b);
                var o2 = p.CheckOrientation(_b, _c);
                var o3 = p.CheckOrientation(_c, _a);

                if (o1 == o2 && o2 == o3 && (o3 == Orientation.RIGHT || o3 == Orientation.LEFT))
                    return Orientation.INSIDE;
                else if (o1 == Orientation.ON_SEGMENT || o2 == Orientation.ON_SEGMENT || o3 == Orientation.ON_SEGMENT)
                    return Orientation.BORDER;

                return Orientation.OUTSIDE;
            }
        }
    }

    public struct RayI
    {
        public VectorI Point { get; }
        public VectorI Dir { get; }

        public RayI(VectorI point, VectorI dir)
        {
            Point = point;
            Dir = dir;
        }


        public bool RayWithSegment(SegmentI seg, out VectorI intersection)
        {
            VectorI segDir = seg.Dir;
            double det = Dir.X * -segDir.Y + Dir.Y * segDir.X;

            if (Math.Abs(det) > float.Epsilon)
            {
                double left = ((seg.A.X - Point.X) * -segDir.Y +
                               (seg.A.Y - Point.Y) * segDir.X) / det;
                double right = ((Point.X - seg.A.X) * -Dir.Y +
                                (Point.Y - seg.A.Y) * Dir.X) / -det;

                if (left >= 0.0 && right >= 0.0 && right <= 1.0)
                {
                    intersection = VectorI.Mid(Point, Point + Dir, left);
                    return true;
                }

                intersection = VectorI.NaN;
                return false;
            }

            intersection = VectorI.NaN;
            return false;
        }
    }
}