using System.Numerics;

namespace Coursera
{
    public class PointsAndTriangle1_2 : IProcessTask
    {
        private SegmentI.TriangleI ReadTriangle(string str)
        {
            var tr = Parse.ParseIntArrayString(str, 6);

            var a = new Vector2(tr[0], tr[1]);
            var b = new Vector2(tr[2], tr[3]);
            var c = new Vector2(tr[4], tr[5]);

            return new SegmentI.TriangleI(a, b, c);
        }

        public string[] ProcessTask(string[] stdIn)
        {
            var triangle = ReadTriangle(stdIn[0]);
            var count = Parse.ParseCount(stdIn[1]);
            var arr = new string[count];
            for (int i = 0; i < count; i++)
            {
                var line = stdIn[i + 2];
                var p = Parse.ParseVector(line);

                arr[i] = Parse.ParseOrientation(triangle.OrientatePoint(p));
            }

            return arr;
        }
    }
}