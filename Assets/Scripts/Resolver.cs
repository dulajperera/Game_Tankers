using UnityEngine;
using System.Collections;
using System.Drawing;
using lk.ac.mrt.cse.pc11.bean;
using Assets.Scripts.bean;
using Assets.Scripts.utility;

public class Resolver {
    private string msg;
    private Game gameInstance;
    public Resolver(string m,Game game){
        this.msg = m;
        this.gameInstance = game;
    }
    public void parseMsg() {
        switch (msg.Trim()[0]) {
            case 'I': parseTypeI(); break;
            case 'G': parseTypeG(); break;
            case 'S': parseTypeS(); break;
        }
  
    }
    private void parseTypeS()
    {
        string[] playerdata = msg.Split(':', '#');

        for (int i = 1; i < playerdata.Length; i++)
        {
            if (playerdata.Length < 2) {
                continue;
            }

            string[] data = playerdata[i].Split(';');
            Player p = new Player(data[0]);
            p.StartP = util.PointByStr(data[1]);
            p.Direction = int.Parse(data[2]);

            gameInstance.addPlayer(p);
        }
        gameInstance.GameStarted = true;
    }
    private void parseTypeI()
    {
        string[] data=msg.Split(':','#');
        navigator.cPlayerName = data[1];
        string[] bricks = data[2].Split(';');
        string[] stones = data[3].Split(';');
        string[] water = data[4].Split(';');
        foreach (string s in bricks) {
            gameInstance.addBricks(util.PointByStr(s));
        }
        foreach (string s in stones)
        {
            gameInstance.addStones(util.PointByStr(s));
        }
        foreach (string s in water)
        {
            gameInstance.addWater(util.PointByStr(s));
        }
    }
    private void parseTypeG()
    {
        string[] data = msg.Split(':', '#');
        for (int i = 1; i < data.Length; i++)
        {
            if (data[i][0] == 'P')
            {
                string[] playerdata = data[i].Split(';');
                Player p = gameInstance.getPlayerByName(playerdata[0]);
                p.CurrentP = util.PointByStr(playerdata[1]);
                p.Direction = int.Parse(playerdata[2]);
                p.Shot = bool.Parse(playerdata[3]);
                p.Health = int.Parse(playerdata[4]);
                p.Coins = int.Parse(playerdata[5]);
                p.PointsEarned = int.Parse(playerdata[6]);

            }
            else if(i==data.Length-1) { //brick damages
                string[] playerdata = data[i].Split(';');
                foreach (string s in playerdata)
                {
                    string[] coor = s.Split(',');
                    int x = int.Parse(coor[0]);
                    int y = int.Parse(coor[1]);
                    int d = int.Parse(coor[2]);
                    gameInstance.getBrickByPoint(new Point(x, y)).DamageLevel = d;
                }
            }
        }
    }
}
