namespace Coursera
{
    public class PointsAndVector1_1 : IProcessTask
    {
        private SegmentI ReadSegment(string segStr)
        {
            var segment = Parse.ParseIntArrayString(segStr, 4);
            var ax = segment[0];
            var ay = segment[1];
            var bx = segment[2];
            var by = segment[3];

            return new SegmentI(new VectorI(ax, ay), new VectorI(bx, by));
        }

        private VectorI ReadVector(string vector)
        {
            var vec = Parse.ParseIntArrayString(vector, 2);

            return new VectorI(vec[0], vec[1]);
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
                arr[i] = Parse.ParseOrientation(p.CheckOrientation(segment.A, segment.B));
            }

            return arr;
        }
    }
}