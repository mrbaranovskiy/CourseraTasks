using System;
using Coursera.General;
using Coursera.Structures;

namespace Coursera.Operations
{
    public class SegmentsIndersection31 : IProcessTask
    {
        public string[] ProcessTask(string[] stdIn)
        {
            var a = Parse.ParseIntCoordinates(stdIn[0], 2);
            var b = Parse.ParseIntCoordinates(stdIn[1], 2);

            var results = new string[1];

            var ab = new Segment2d(a[0], a[1]);
            var cd = new Segment2d(b[0], b[1]);

            Vector2d inter;
            Segment2d output;
            var result = ab.Intersection(cd, out inter, out output);

            if (result)
            {
                if (Vector2d.IsNaN(inter))
                    results[0] = "A common segment of non-zero length.";
                else
                    results[0] = $"The intersection point is ({Math.Round(inter.X)}" +
                                 $", {Math.Round(inter.Y)}).";
            }
            else
            {
                if (!Vector2d.IsNaN(inter))
                {
                    results[0] = "A common segment of non-zero length.";
                }
                else
                {
                    results[0] = "No common points.";
                }
            }

            return results;
        }
    }
}