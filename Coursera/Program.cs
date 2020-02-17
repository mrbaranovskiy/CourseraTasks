using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Coursera
{
    class Program
    {
        static void Main(string[] args)
        {
            //var a = Vec.AngleSigned(new Vector2(3, 3), new Vector2(4, 3));
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

    public class IsConvexAlgo_2_0 : IProcessTask
    {
        public string[] ProcessTask(string[] stdIn)
        {
            throw new NotImplementedException();
        }
    }

    public class LowerTangent_2_3 : IProcessTask
    {
        public string[] ProcessTask(string[] stdIn)
        {
            var countV = Parse.ParseCount(stdIn[0]);
            var vertices = Parse.ParseIntCoordinates(stdIn[1], countV);
            var countI = Parse.ParseCount(stdIn[2]);

            var convex = new Polygon2d(vertices).ConvexHull();

            var results = new string[countI];
            return null;
        }
    }

    public class GrahamsAlgo2_1 : IProcessTask
    {
        private List<Vector2> Vertices(string verticesStr, int count)
        {
            return Parse.ParseIntCoordinates(verticesStr, count);
        }

        public string[] ProcessTask(string[] stdIn)
        {
            var count = Parse.ParseCount(stdIn[0]);
            var vertices = Parse.ParseIntCoordinates(stdIn[1], count);
            var result = new string[2];
            if (count == 1)
            {
                result[0] = "0";
                result[1] = vertices.First().ToString();
                return result;

            }

            var poly = new Polygon2d(vertices);


            result[0] = poly.Count.ToString();
            var hull = poly.ConvexHull();
            //result[1] = poly.ConvexHull().;

            return null;
        }
    }

    #region STUFF

    public struct Polygon2d : IEnumerable<Vector2>, ICollection<Vector2>
    {
        private readonly List<Vector2> _points;

        private LinkedList<Vector2> _lst;

        private double CrossProduct(Vector2 a,
            Vector2 b, Vector2 c)
        {
            double aa = a.X - b.X;
            double bb = a.Y - b.Y;
            double cc = c.X - b.X;
            double dd = c.Y - b.Y;

            return aa * dd - bb * cc;
        }


        public Polygon2d ConvexHull()
        {
            if (_points.Count < 3)
                return new Polygon2d(_points);
            //check count
            var ordered = _points.OrderBy(n => n).ToArray();
            var lu = new List<Vector2> {ordered[0], ordered[1]};


            for (int i = 2; i < ordered.Length; i++)
            {
                var p = ordered[i];
                lu.Add(p);

                while (lu.Count > 2
                       && Vec.CheckOrientation(lu[lu.Count - 3],lu[lu.Count - 2], lu[lu.Count - 1]) != Orientation.RIGHT)
                    lu.RemoveAt(lu.Count - 2);
            }

            var limit = lu.Count + 1;

            for (var i = ordered.Length - 3; i >= 0; i--)
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

        public Polygon2d(List<Vector2> points)
        {
            _points = points;
            _lst = new LinkedList<Vector2>();

            foreach (var Vector2 in points) Add(Vector2);
        }

        public bool IsConvex()
        {
            int q, l;
            int count = _points.Count;

            bool right = false;
            bool left = false;

            for (int i = 0; i < count; i++)
            {
                q = (i + 1) % count;
                l = (q + 1) % count;

                double dir = CrossProduct(_points[i], _points[q], _points[l]);
                if (dir < 0) right = true;
                else if (dir > 0) left = true;
                if (right && left) return false;
            }

            return true;
        }


        public void Add(Vector2 vector)
        {
            _lst.AddFirst(vector);
        }

        public void Clear()
        {
            _lst.Clear();
        }

        public bool Contains(Vector2 item)
        {
            return _lst.Contains(item);
        }

        public void CopyTo(Vector2[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Vector2 item)
        {
            return _lst.Remove(item);
        }

        public int Count => _lst.Count;
        public bool IsReadOnly => false;

        private LinkedList<Vector2> List => _lst;

        public static bool PointInPolygon(IList<Vector2> poly, Vector2 pt)
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


        public Orientation CheckPointOrientation(Vector2 pt)
        {
            var segments = Segments.ToArray();
            var onBorder = segments.Any(n => Vec.CheckOrientation(pt,n.A, n.B) == Orientation.ON_SEGMENT);

            if (onBorder)
                return Orientation.BORDER;
            else
            {
                var ray = new RayI(pt, new Vector2(1, 0));
                int count = 0;

                foreach (var segmentI in segments)
                {
                    Vector2? intersection;
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

        public IEnumerator<Vector2> GetEnumerator()
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

        public static List<Vector2> ParseIntCoordinates(string str, int num)
        {
            var nums = ParseIntArrayString(str, num * 2);
            List<Vector2> vertices = new List<Vector2>();

            for (int i = 0; i < num; i++)
            {
                var x = nums[i * 2];
                var y = nums[i * 2 + 1];

                vertices.Add(new Vector2(x, y));
            }

            return vertices;
        }

        public static Vector2 ParseVector(string vector)
        {
            var vec = ParseIntArrayString(vector, 2);

            return new Vector2(vec[0], vec[1]);
        }


        public static int ParseCount(string segCountStr)
            => Parse.ParseIntArrayString(segCountStr, 1)[0];
    }

    public struct SegmentI
    {
        public SegmentI(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        public Vector2 Dir => B - A;

        public Vector2 A { get; }
        public Vector2 B { get; }


        public struct TriangleI
        {
            private readonly Vector2 _a;
            private readonly Vector2 _b;
            private readonly Vector2 _c;

            public TriangleI(Vector2 a, Vector2 b, Vector2 c)
            {
                _a = a;
                _b = b;
                _c = c;
            }

            public Orientation OrientatePoint(Vector2 p)
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
    }

    public struct RayI
    {
        public Vector2 Point { get; }
        public Vector2 Dir { get; }

        public RayI(Vector2 point, Vector2 dir)
        {
            Point = point;
            Dir = dir;
        }


        public bool RayWithSegment(SegmentI seg, out Vector2? intersection)
        {
            Vector2 segDir = seg.Dir;
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
        public static Orientation CheckOrientation(Vector2 q, Vector2 a, Vector2 b)
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

        public static Vector2 Mid(Vector2 v0, Vector2 v1, float factor)
        {
            return v0 * (1f - factor) + v1 * factor;
        }

        public static double Angle2X(Vector2 v)
        {
            return Math.Atan2(v.Y, v.X);
        }

        public static double AngleSigned(Vector2 v1, Vector2 v2)
        {
            double num = Math.Acos(AngleCos(v1, v2));
            if (-v1.Y * v2.X + v1.X * v2.Y < 0.0)
                num = -num;
            return num;
        }

        public static double AngleCos(Vector2 v1, Vector2 v2)
        {
            double num = (v1.X * v2.X + v1.Y * v2.Y) / Math.Sqrt((v1.X * v1.X + v1.Y * v1.Y) * (v2.X * v2.X + v2.Y * v2.Y));
            if (num < -1.0)
                return -1.0;
            if (num > 1.0)
                return 1.0;
            return num;
        }

        public static double Angle0To2Pi(Vector2 v1, Vector2 v2)
        {
            double num = AngleShortest(v1, v2);
            if (-v1.Y * v2.X + v1.X * v2.Y < 0.0)
                num = 2.0 * Math.PI - num;
            return num;
        }

        public static double AngleShortest(Vector2 v1, Vector2 v2)
        {
            return Math.Acos(AngleCos(v1, v2));
        }


    }

    #endregion
}