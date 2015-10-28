using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Assets.Scripts.bean
{
    public class Player
    {
        public Player(string cName) {
            this.Name = cName;
        }

        public int Coins { get; set; }
        public Point CurrentP { get; set; }
        public int Direction { get; set; }
        public int Health { get; set; }
        public int Index { get; set; }
        public bool InvalidCell { get; set; }
        public bool IsAlive { get; set; }
        public string Name { get; set; }
        public int PointsEarned { get; set; }
        public bool Shot { get; set; }
        public Point StartP { get; set; }
        public DateTime UpdatedTime { get; set; }
    }

}