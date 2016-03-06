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
            this.StartP = new Point(0, 0);
            this.CurrentP = new Point(0, 0);
        }

        public int Coins { get; set; }
        public Point CurrentP { get; set; }
        public int Direction { get; set; }
        private byte direction;                 // 0 - ^, 1 - >, 2 - v, 3 - <
        public int Health { get; set; }
        public int Index { get; set; }
        public bool InvalidCell { get; set; }
        public bool IsAlive { get; set; }
        public string Name { get; set; }
        public int PointsEarned { get; set; }
        public bool Shot { get; set; }

        public Point StartP { get; set; }
        public DateTime UpdatedTime { get; set; }

        public void setShot(bool shot) {
            Shot = shot;
        }
        public int getPlayerNumber() {
           return  int.Parse(Name.Replace("P", ""));
        }
    }

}