using Coursera.General;

namespace Coursera.Structures
{
    public struct Triangle2d
    {
        private readonly Vector2d _a;
        private readonly Vector2d _b;
        private readonly Vector2d _c;

        public Triangle2d(Vector2d a, Vector2d b, Vector2d c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public Orientation OrientatePoint(Vector2d p)
        {
            var o1 = Vec.CheckOrientation(p, _a, _b);
            var o2 = Vec.CheckOrientation(p, _b, _c);
            var o3 = Vec.CheckOrientation(p, _c, _a);
            ;

            if (o1 == o2 && o2 == o3 && (o3 == Orientation.RIGHT || o3 == Orientation.LEFT))
                return Orientation.INSIDE;
            if (o1 == Orientation.ON_SEGMENT || o2 == Orientation.ON_SEGMENT || o3 == Orientation.ON_SEGMENT)
                return Orientation.BORDER;

            return Orientation.OUTSIDE;
        }
    }
}