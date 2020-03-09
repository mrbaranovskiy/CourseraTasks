using System;
using System.Collections.Generic;
using System.Linq;
using Coursera.Structures;

namespace Coursera.General
{
    public static class Parse
    {
        public static string ParseOrientation(Orientation o)
        {
            switch (o)
            {
                case Orientation.LEFT:
                    return "LEFT";
                case Orientation.RIGHT:
                    return "RIGHT";
                case Orientation.ONLINE:
                    return "ON_LINE";
                case Orientation.ON_SEGMENT:
                    return "ON_SEGMENT";
                case Orientation.INSIDE:
                    return "INSIDE";
                case Orientation.OUTSIDE:
                    return "OUTSIDE";
                case Orientation.BORDER:
                    return "BORDER";
                default:
                    throw new ArgumentOutOfRangeException(nameof(o), o, null);
            }
        }

        public static int[] ParseIntArrayString(string str, int num)
        {
            var arr = str.Split(' ').Select(n => int.Parse(n.Trim())).ToArray();

            if (arr.Count() != num)
                throw new Exception("Parse exception");

            return arr;
        }

        public static List<Vector2d> ParseIntCoordinates(string str, int num)
        {
            var nums = ParseIntArrayString(str, num * 2);
            List<Vector2d> vertices = new List<Vector2d>();

            for (int i = 0; i < num; i++)
            {
                var x = nums[i * 2];
                var y = nums[i * 2 + 1];

                vertices.Add(new Vector2d(x, y));
            }

            return vertices;
        }

        public static Vector2d ParseVector(string vector)
        {
            var vec = ParseIntArrayString(vector, 2);

            return new Vector2d(vec[0], vec[1]);
        }


        public static int ParseCount(string segCountStr)
            => Parse.ParseIntArrayString(segCountStr, 1)[0];
    }
}