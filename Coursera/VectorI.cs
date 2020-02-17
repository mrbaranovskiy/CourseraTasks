using System;

namespace Coursera
{
//    public struct Vector2 : IComparable<Vector2>
//    {
//        public override string ToString()
//        {
//            return $"{X} {Y}";
//        }
//
//        public double X { get; }
//        public double Y { get; }
//        public static Vector2 NaN => new Vector2(double.NaN, double.NaN);
//
//        public Vector2(double x, double y)
//        {
//            X = x;
//            Y = y;
//        }
//
//        public static Vector2 Mid(Vector2 v0, Vector2 v1, double factor)
//        {
//            return v0 * (1.0 - factor) + v1 * factor;
//        }
//
//        public Vector2 Sub(Vector2 v)
//        {
//            return new Vector2(this.X - v.X, this.Y - v.Y);
//        }
//
//        public static bool operator ==(Vector2 left, Vector2 right)
//        {
//            return Math.Abs(left.X - right.X) < double.Epsilon && Math.Abs(left.Y - right.Y) < double.Epsilon;
//        }
//
//        public static bool operator !=(Vector2 left, Vector2 right)
//        {
//            return !(left == right);
//        }
//
//        public static Vector2 operator +(Vector2 a, Vector2 b)
//        {
//            return new Vector2(a.X + b.X, a.Y + b.Y);
//        }
//
//        public static Vector2 operator -(Vector2 left, Vector2 right)
//        {
//            return new Vector2(left.X - right.X, left.Y - right.Y);
//        }
//
//        public static Vector2 operator *(Vector2 lef, double factor)
//        {
//            return new Vector2(lef.X * factor, lef.Y * factor);
//        }
//
//        public static Vector2 operator *(double factor, Vector2 rhs)
//        {
//            return new Vector2(factor * rhs.X, factor * rhs.Y);
//        }
//
//        public bool CounterClockWise(Vector2 p, Vector2 q)
//        {
//            return ((p.X - X) * (q.Y - Y)) > ((p.Y - Y) * (q.X - X));
//        }
//
//        public Orientation CheckOrientation(Vector2 a, Vector2 b)
//        {
//            var val = (b.Y - Y) * (a.X - b.X) - (b.X - X) * (a.Y - b.Y);
//
////            if (Math.Abs(val) < double.Epsilon)
////            {
////                var minx = Math.Min(b.X, a.X);
////                var maxx = Math.Max(b.X, a.X);
////                var miny = Math.Min(b.Y, a.Y);
////                var maxy = Math.Max(b.Y, a.Y);
////
////                if (X >= minx && X <= maxx && Y >= miny && Y <= maxy)
////                    return Orientation.ON_SEGMENT;
////                return Orientation.ONLINE;
////            }
//
//            return val < 0 ? Orientation.RIGHT : Orientation.LEFT;
//        }
//
//        public int CompareTo(Vector2 other)
//        {
//            var xComparison = X.CompareTo(other.X);
//            if (xComparison != 0) return xComparison;
//            return Y.CompareTo(other.Y);
//        }
//    }
}