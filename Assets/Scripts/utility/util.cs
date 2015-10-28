using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Assets.Scripts.utility
{
    class util
    {
        public static Point PointByStr(string s) {
            string[] points = s.Split(',');
            return new Point(int.Parse(points[0]), int.Parse(points[1]));
        }
    }
}
