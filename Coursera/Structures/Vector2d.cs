using System;
using Coursera.General;

namespace Coursera.Structures
{
    public struct Vector2d : IComparable<Vector2d>
    {
        public bool Equals(Vector2d other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector2d && Equals((Vector2d) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

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

        public double X { get; }
        public double Y { get; }
        public static Vector2d NaN => new Vector2d(float.NaN, float.NaN);
        public static Vector2d UnitX => new Vector2d(1, 0);
        public static Vector2d UnitY => new Vector2d(0, 1);

        public static bool IsNaN(Vector2d vec)
        {
            return double.IsNaN(vec.X) || double.IsNaN(vec.Y);
        }

        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2d Mid(Vector2d v0, Vector2d v1, double factor)
        {
            return v0 * (1.0f - factor) + v1 * factor;
        }

        public Vector2d Sub(Vector2d v)
        {
            return new Vector2d(this.X - v.X, this.Y - v.Y);
        }

        public static bool operator ==(Vector2d left, Vector2d right)
        {
            return Math.Abs(left.X - right.X) < double.Epsilon && Math.Abs(left.Y - right.Y) < double.Epsilon;
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

        public static Vector2d operator *(Vector2d lef, double factor)
        {
            return new Vector2d(lef.X * factor, lef.Y * factor);
        }

        public static Vector2d operator *(double factor, Vector2d rhs)
        {
            return new Vector2d(factor * rhs.X, factor * rhs.Y);
        }

        public bool CounterClockWise(Vector2d p, Vector2d q)
        {
            return ((p.X - X) * (q.Y - Y)) > ((p.Y - Y) * (q.X - X));
        }

        public bool OnLine(Vector2d q, Vector2d p)
        {
            return OnLine(new Vector2d(X, Y), q, p);
        }

        public static bool OnLine(Vector2d q, Vector2d p, Vector2d l)
        {
            return Math.Abs(((p.X - q.X) * (l.Y - q.Y)) - ((p.Y - q.Y) * (l.X - q.X))) < float.Epsilon;
        }

        public bool CW(Vector2d a, Vector2d b)
        {
            var val = (b.Y - Y) * (a.X - b.X) - (b.X - X) * (a.Y - b.Y);

            return val < 0;
        }

        public int CompareTo(Vector2d other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }
    }
}