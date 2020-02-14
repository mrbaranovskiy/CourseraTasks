using System.Collections.Generic;

namespace Coursera
{
    public class IntersectionWithPoligon1_3 : IProcessTask
    {
        private Polygon2d ReadPolygon(string pl, int count)
        {
            var arr = Parse.ParseIntArrayString(pl, count * 2);
            var vertices = new List<VectorI>();

            for (int i = 0; i < arr.Length; i += 2)
            {
                var x = arr[i];
                var y = arr[i + 1];

                vertices.Add(new VectorI(x, y));
            }

            return new Polygon2d(vertices);
        }

        public string[] ProcessTask(string[] stdIn)
        {
            var segPtCnt = Parse.ParseCount(stdIn[0]);
            var polygon = ReadPolygon(stdIn[1], segPtCnt);
            var countOfPt = Parse.ParseCount(stdIn[2]);
            string[] result = new string[countOfPt];

            for (int i = 0; i < countOfPt; i++)
            {
                var vec = Parse.ParseVector(stdIn[i + 3]);
                var orientation = polygon.CheckPointOrientation(vec);
                result[i] = Parse.ParseOrientation(orientation);
            }

            return result;
        }
    }
}