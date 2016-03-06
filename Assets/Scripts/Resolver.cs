using UnityEngine;
using System.Drawing;
using Assets.Scripts.bean;
using Assets.Scripts.utility;

public class Resolver
{
    //this class is used to parse server replies.

    private string msg;
    private Game gameInstance;
    public Resolver()
    {

    }

    public void parseMsg(string m, Game game)
    {
        this.msg = m;
        this.gameInstance = game;
        switch (msg.Trim().Substring(0,2))
        {
            case "I:": parseTypeI(); break;  // initial msg
            case "G:": parseTypeG(); break;  // game world updates
            case "S:": parseTypeS(); break;  // game start msg
            case "C:": parseTypeC(); break;  // coin appears
            case "L:": parseTypeL(); break;  // lifepack appears
        }

        // server's acknoledgement replies.

        if (msg.Contains("OBSTACLES"))
        {
            foundObstacle();
        }
        else if (msg.Contains("CELL_OCCUPIED"))
        {
            cellOccupied();
        }

        else if (msg.Contains("DEAD"))
        {
            foundDead();
        }

        else if (msg.Contains("TOO_QUICK"))
        {
            foundTooQuick();
        }
        else if (msg.Contains("INVALID_CELL"))
        {
            invalidCell();
        }
        else if (msg.Contains("GAME_NOT_STARTED_YET"))
        {
            notStarted();
        }

        else if (msg.Contains("GAME_HAS_FINISHED"))
        {
            foundGameFinished();
        }
        else if (msg.Contains("NOT_A_VALID_CONTESTANT"))
        {
            invalidContestant();
        } else if (msg.Contains("PLAYERS_FULL")) {
            Debug.Log("You can't join. Maximum number of players added");
        }
        else if (msg.Contains("ALREADY_ADDED#"))
        {
            Debug.Log("You can't join again. You have already added to the game");
        }
        else if (msg.Contains("GAME_ALREADY_STARTED#"))
        {
            Debug.Log("You can't join. Game already started.");
        }
        else if (msg.Contains("GAME_FINISHED#"))
        {
            Debug.Log("Game finished!");
        }


    }

    //methods to show different actions
    public void foundObstacle()
    {
        Debug.Log("Obstacle Found! Can't move");
    }

    public void cellOccupied()
    {
        Debug.Log("Already Occupied cell");
    }

    public void invalidCell()
    {
        Debug.Log("Invalid Cell");
    }
    public void foundDead()
    {
        Debug.Log("Dead!");
    }

    public void foundTooQuick()
    {
        Debug.Log("Too quick movement! Be cool.");

    }

    public void foundGameFinished()
    {
        Debug.Log(msg.Contains("Game already finished!"));
    }

    public void notStarted()
    {
        Debug.Log(msg.Contains("Not started yet"));
    }

    public void invalidContestant()
    {
        Debug.Log(msg.Contains("Game finished"));
    }

    private void parseTypeS()
    {
        string[] playerdata = msg.Split(':', '#');

        Debug.Log("Players in the contenst,");

        for (int i = 1; i < playerdata.Length; i++)
        {
            string[] data = playerdata[i].Split(';');

            if (data.Length < 2)
            {
                continue;
            }
               
            Player p = new Player(data[0]);

            p.StartP = util.PointByStr(data[1]);
            p.CurrentP = util.PointByStr(data[1]);
            p.Direction = int.Parse(data[2]);
            gameInstance.addPlayer(p);

            Debug.Log(p.Name);

            navigator.gameInstance.GameStarted = true;
        }

        // assign current active player instance

        gameInstance.PlayerSelf = gameInstance.getPlayerByName(navigator.cPlayerName);
        gameInstance.GameStarted = true;
    }

    private void parseTypeI()
    {
        string[] data = msg.Split(':', '#');
        navigator.cPlayerName = data[1];

        Debug.Log("current player: " + navigator.cPlayerName);

        string[] bricks = data[2].Split(';');
        string[] stones = data[3].Split(';');
        string[] water = data[4].Split(';');

        foreach (string s in bricks)
        {
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

    private void parseTypeC()
    {
       
        string[] coins = msg.Split(':', '#');
        string[] position = coins[1].Split(',');
        string lifeTime = coins[2];
        int value = int.Parse(coins[3]);

        Point p = new Point(int.Parse(position[0]), int.Parse(position[1]));
        gameInstance.addCoin(p, int.Parse(lifeTime));
        Debug.Log("Coin appears at " + p.ToString() + ". It will be there for " + lifeTime.ToString() + " time. Coin value is " + value);
    }
    private void parseTypeL()
    {

        string[] life = msg.Split(':', '#');
        string[] position = life[1].Split(',');
        string lifeTime = life[2];

        Point p = new Point(int.Parse(position[0]), int.Parse(position[1]));
        gameInstance.addLifePack(p, int.Parse(lifeTime));

        Debug.Log("Life pack appears at " + p.ToString() + ". It will be there for " + lifeTime.ToString() + " time.");
    }
    private void parseTypeG()
    {
        string[] data = msg.Split(':', '#');
        for (int i = 1; i < 6; i++)
        {
            if (data[i][0] == 'P')
            {
                string[] playerdata = data[i].Split(';');
                Player p = gameInstance.getPlayerByName(playerdata[0]);
                p.CurrentP = util.PointByStr(playerdata[1]);
                p.Direction = int.Parse(playerdata[2]);
                p.setShot(int.Parse(playerdata[3])==1? true :false);
                p.Health = int.Parse(playerdata[4]);
                p.Coins = int.Parse(playerdata[5]);
                p.PointsEarned = int.Parse(playerdata[6]);

            }
            else
            { 
                //brick damages
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
