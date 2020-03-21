using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http.Headers;
using Coursera.Comparers;
using Coursera.Structures;
using Wintellect.PowerCollections;

namespace Coursera.General
{
    public static class Vec
    {
        private class StatusPoint
        {
            public int SegmentId { get; set; }
            public Segment2d Segment { get; set; }

            public override string ToString()
            {
                return $"X:{Segment.A} Y:{Segment.B} ID: {SegmentId}";
            }
        }

        public class EventPoint : IEquatable<EventPoint>, IComparable<EventPoint>
        {
            public override string ToString()
            {
                if (EventType == Type.Intersection)
                    return $"Inter: {A} {B}";
                return $"Status {A}";
            }

            public enum Type
            {
                Start,
                End,
                Intersection
            }

            internal int A { get; }
            internal int B { get; }
            internal Type EventType { get; }
            public double X { get; }

            public EventPoint(int a, int b, double x, Type eventType)
            {
                A = a;
                B = b;
                EventType = eventType;
                X = x;
            }

            public bool Equals(EventPoint other)
            {
                return A.Equals(other.A);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is EventPoint && Equals((EventPoint) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = A.GetHashCode();
                    hashCode = (hashCode * 397) ^ X.GetHashCode();
                    hashCode = (hashCode * 397) ^ EventType.GetHashCode();
                    return hashCode;
                }
            }

            public int CompareTo(EventPoint other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                var result = X.CompareTo(other.X);

                if (result == 0)
                {
                    var next = other.EventType;
                    var prev = EventType;

                    if (prev > next)
                        return 1;
                    return -1;
                }

                return result;
            }
        }

        public class SweepLine
        {
            public static double CurrentPosition { get; set; }
        }

        public class Segments2dComparer : IComparer<Segment2d>
        {
            private readonly SweepLine _sweepLine;
            private readonly OriginComparer _comparer;

            public Segments2dComparer(SweepLine sweepLine, OriginComparer comparer)
            {
                _sweepLine = sweepLine;
                _comparer = comparer;
            }

            public int Compare(Segment2d x, Segment2d y)
            {
                var up = new Ray2d(new Vector2d(SweepLine.CurrentPosition, 0), Vector2d.UnitY);
                var down = new Ray2d(new Vector2d(SweepLine.CurrentPosition, 0), new Vector2d(0, -1));


                if (!up.RayWithSegment(x, out var segx))
                    segx = down.RayWithSegment(x, out var x2) ? x2 : Vector2d.NaN;

                if (!up.RayWithSegment(y, out var segy))
                    segy = down.RayWithSegment(y, out var y2) ? y2 : Vector2d.NaN;

                var xn = Vector2d.IsNaN(segx);
                var yn = Vector2d.IsNaN(segy);

                if (xn || yn)
                    return 0;

                return _comparer.Compare(segx, segy);
            }
        }

        public static SortedSet<Vector2d> SegmentIntersection(Segment2d[] segments)
        {
            var intersectionPoints = new SortedSet<Vector2d>(new OriginComparer());
            var originComparer = new OriginComparer();
            var sl = new SweepLine();
            var status2 = new SortedList<Segment2d, int>(new Segments2dComparer(sl, originComparer));

            var bag = new OrderedBag<EventPoint>();

            for (var i = 0; i < segments.Length; i++)
            {
                var minx = Math.Min(segments[i].A.X, segments[i].B.X);
                var maxx = Math.Max(segments[i].A.X, segments[i].B.X);

                bag.Add(new EventPoint(i, i, minx, EventPoint.Type.Start));
                bag.Add(new EventPoint(i, i, maxx, EventPoint.Type.End));
            }

            while (bag.Any())
            {
                var curEvent = bag.RemoveFirst();
                SweepLine.CurrentPosition = curEvent.X;

                if (curEvent.EventType == EventPoint.Type.Start)
                {
                    var evtSeg = segments[curEvent.A];
                    status2[evtSeg] = curEvent.A;

                    CalculateAndUpdate(status2, bag, intersectionPoints);
                }
                else if (curEvent.EventType == EventPoint.Type.End)
                {
                    CalculateAndUpdate(status2, bag, intersectionPoints);

                    var indexOfValue = status2.IndexOfValue(curEvent.A);
                    if(indexOfValue > 0)
                        status2.Remove(status2.Keys[indexOfValue]);
                }
            }

            return intersectionPoints;
        }

        private static void CalculateAndUpdate(SortedList<Segment2d, int> status, OrderedBag<EventPoint> eventPoints,
            SortedSet<Vector2d> intersections)
        {
            for (int i = 0; i < status.Keys.Count - 1; i++)
            {
                var segA = status.Keys[i];
                var segB = status.Keys[i + 1];

                Vector2d intersection;
                Segment2d segInter;

                if (segA.Intersection(segB, out intersection, out segInter))
                {
                    if (Vector2d.IsNaN(intersection))
                    {
                        intersections.Add(segA.A);
                        intersections.Add(segA.B);
                    }
                    else
                    {
                        intersections.Add(intersection);
                    }

                    var eventPoint = new EventPoint(
                        status.Values[i],
                        status.Values[i + 1],
                        0, EventPoint.Type.Intersection);

                    eventPoints.Add(eventPoint);
                }
            }
        }

        static int Next(List<StatusPoint> arr1, Segment2d target)
        {
            if (!arr1.Any()) return 0;

            var segComparer = new SegmentsOriginXComparer();
            var idx = arr1.FindIndex(s => segComparer.Compare(s.Segment, target) == 1);

            return idx == -1 ? arr1.Count() : idx;
        }

        public static double Sqr(double value)
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

        public static Vector2d Mid(Vector2d v0, Vector2d v1, double factor)
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
            double num = (v1.X * v2.X + v1.Y * v2.Y) /
                         Math.Sqrt((v1.X * v1.X + v1.Y * v1.Y) * (v2.X * v2.X + v2.Y * v2.Y));
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
}