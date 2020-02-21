using System.Numerics;

namespace Coursera
{
    public class PointsAndVector1_1 : IProcessTask
    {
        private Segment2d ReadSegment(string segStr)
        {
            var segment = Parse.ParseIntArrayString(segStr, 4);
            var ax = segment[0];
            var ay = segment[1];
            var bx = segment[2];
            var by = segment[3];

            return new Segment2d(new Vector2d(ax, ay), new Vector2d(bx, by));
        }

        private Vector2d ReadVector(string vector)
        {
            var vec = Parse.ParseIntArrayString(vector, 2);

            return new Vector2d(vec[0], vec[1]);
        }

        private int ReadSegmentsCount(string segCountStr)
            => Parse.ParseIntArrayString(segCountStr, 1)[0];

        public string[] ProcessTask(string[] stdIn)
        {
            var segment = ReadSegment(stdIn[0]);
            var segCount = ReadSegmentsCount(stdIn[1]);


            var arr = new string[segCount];

            for (int i = 0; i < segCount; i++)
            {
                var p = ReadVector(stdIn[i + 2]);
                arr[i] = Parse.ParseOrientation(Vec.CheckOrientation(p,segment.A, segment.B));
            }

            return arr;
        }
    }
}