using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Coursera.Comparers;
using Coursera.General;
using Coursera.Structures;

namespace Coursera
{
    class Program
    {
        static void Main(string[] args)
        {
            var seg1 = new Segment2d(new Vector2d(-5, -4),  new Vector2d(-5, 4));
            var seg2 = new Segment2d(new Vector2d(-6, -3),  new Vector2d(-2, 1));
            var seg3 = new Segment2d(new Vector2d(-6, 3),  new Vector2d(-4, 1));
            var seg4 = new Segment2d(new Vector2d(-3, -2),  new Vector2d(-3, 4));
            var seg5 = new Segment2d(new Vector2d(-4, 3),  new Vector2d(1, 3));

            var arr = new[] {seg1, seg2, seg3, seg4, seg5};

            var result1 = Vec.SegmentIntersection(arr);

            var list = new List<string>();
            string input = null;

            while (!string.IsNullOrEmpty(input = Console.In.ReadLine())) list.Add(input);

            if (!list.Any())
                Console.Error.WriteLine("CMD LINE ERROR!!!");

            IProcessTask task1_1 = new PolygonsIntersection_3_2();
            var result = task1_1.ProcessTask(list.ToArray());

            Output(result);
        }

        private static void Output(string[] results)
        {
            foreach (var result in results)
                Console.Out.WriteLine(result);
        }
    }
    
    public class PolygonsIntersection_3_2 : IProcessTask
    {
        public string[] ProcessTask(string[] stdIn)
        {
            var pna = Parse.ParseCount(stdIn[0]);
            var verticesA = Parse.ParseIntCoordinates(stdIn[1], pna);
            var pnb = Parse.ParseCount(stdIn[2]);
            var verticesB = Parse.ParseIntCoordinates(stdIn[3], pnb);
            
            var polyginA = new Polygon2d(verticesA);
            var polyginB = new Polygon2d(verticesB);

            var arr = polyginA.Intersect(polyginB).ToArray();

            //var arr = intersection.ToArray();
            Array.Sort(arr, new OriginComparer());
            Array.Sort(arr, new PolarComparer(arr[0]));

            //10 4
            //10 6
            //6 10
            //4 10
            //0 6
            //0 4
            //4 0
            //6 0
            return null;
        }
    }


    public class SegmentsIndersection_3_1 : IProcessTask
    {
        public string[] ProcessTask(string[] stdIn)
        {
            var c1 = Parse.ParseCount(stdIn[0]);
            var pts1 = Parse.ParseIntCoordinates(stdIn[1], c1);

            var c2 = Parse.ParseCount(stdIn[2]);
            var pts2 = Parse.ParseIntCoordinates(stdIn[3], c2);

            var p1 = new Polygon2d(pts1);
            var p2 = new Polygon2d(pts2);

            string[] results = new string[2];

            var hull = Polygon2d.Intersects(p1, p2);

            results[0] = hull.Count.ToString();
            results[1] = hull.Select(n => $"{n.X.ToString()} {n.Y.ToString()}").Aggregate((n, m) => m + ' ' + n);

            return results;
        }
    }


    #region STUFF

    #endregion
}