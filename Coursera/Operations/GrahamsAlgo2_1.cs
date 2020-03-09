using System.Collections.Generic;
using Coursera.General;
using Coursera.Structures;

namespace Coursera.Operations
{
    public class GrahamsAlgo2_1 : IProcessTask
    {
        private List<Vector2d> Vertices(string verticesStr, int count)
        {
            return Parse.ParseIntCoordinates(verticesStr, count);
        }

        public string[] ProcessTask(string[] stdIn)
        {
            var count = Parse.ParseCount(stdIn[0]);
            var vertices = Parse.ParseIntCoordinates(stdIn[1], count);
            //var inputCount = Parse.ParseCount(stdIn[2]);
            var result = new string[1];

            var poly = new Polygon2d(vertices);

            var isConvex = poly.ConvexHull().IsConvex();

            result[0] = isConvex ? "CONVEX" : "NON_CONVEX";

            //result[0] = hull.Count.ToString();
            //result[1] = hull.ToString();

//            for (int i = 0; i < inputCount; i++)
//            {
//                var vec = Parse.ParseVector(stdIn[i + 3]);
//                var array = hull.AsArray(new PolarComparer(vec));
//
//                var min = array[0];
//                var max = array[array.Length - 1];
//
//                if (vec.CheckLeftRight(min, max) == Orientation.LEFT)
//                {
//                    Vec.Swap(ref min, ref max);
//                    Console.WriteLine("Test");
//                }
//
//                result[i] = $"{min.X} {min.Y} {max.X} {max.Y}";
//            }

            //result[1] = poly.ConvexHull().;

            return result;
        }
    }
}