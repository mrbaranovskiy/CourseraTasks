using System;
using Coursera.General;

namespace Coursera.Structures
{
    public struct Ray2d
    {
        public Vector2d Point { get; }
        public Vector2d Dir { get; }

        public Ray2d(Vector2d point, Vector2d dir)
        {
            Point = point;
            Dir = dir;
        }

        public bool RayWithSegment(Segment2d seg, out Vector2d intersection)
        {
            Vector2d segDir = seg.Dir;
            double det = Dir.X * -segDir.Y + Dir.Y * segDir.X;

            if (Math.Abs(det) > double.Epsilon)
            {
                double left = ((seg.A.X - Point.X) * -segDir.Y +
                               (seg.A.Y - Point.Y) * segDir.X) / det;
                double right = ((Point.X - seg.A.X) * -Dir.Y +
                                (Point.Y - seg.A.Y) * Dir.X) / -det;

                if (left >= 0.0 && right >= 0.0 && right <= 1.0)
                {
                    intersection = Vec.Mid(Point, Point + Dir, left);
                    return true;
                }

                intersection = Vector2d.NaN;
                return false;
            }

            intersection = Vector2d.NaN;
            return false;
        }
    }
}