using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Coursera.General;
using Coursera.Structures;

namespace Coursera
{
    class Program
    {
        static void Main(string[] args)
        {
            var seg1 = new Segment2d(new Vector2d(-2, -1),  new Vector2d(2, 3));
            var seg2 = new Segment2d(new Vector2d(2, 3),  new Vector2d(3, -2));
            var seg3 = new Segment2d(new Vector2d(3, -2),  new Vector2d(-2, -1));

            var arr = new[] {seg1, seg2, seg3};

            var result1 = Vec.SegmentIntersection(arr);

            var list = new List<string>();
            string input = null;

            while (!string.IsNullOrEmpty(input = Console.In.ReadLine())) list.Add(input);

            if (!list.Any())
                Console.Error.WriteLine("CMD LINE ERROR!!!");

            IProcessTask task1_1 = new PolygonsIntersection();
            var result = task1_1.ProcessTask(list.ToArray());

            Output(result);
        }

        private static void Output(string[] results)
        {
            foreach (var result in results)
                Console.Out.WriteLine(result);
        }
    }

    public class PolygonsIntersection : IProcessTask
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