using lk.ac.mrt.cse.pc11.bean;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Assets.Scripts.bean
{   
    //Coicider merging with the parent
    public class LifePack
    {
        Point pos;
        int seconds;
        public int Seconds
        {
            get { return seconds; }
            set { seconds = value; }
        }
        public Point Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public LifePack(Point position,int sec)
        {
            pos = position;
            seconds = sec;
        }
    }
}
