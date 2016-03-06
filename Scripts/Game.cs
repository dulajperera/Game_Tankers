using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using lk.ac.mrt.cse.pc11.bean;
using lk.ac.mrt.cse.pc11.util;
using System.Threading;
using Assets.Scripts.bean;



public class Game {
    #region Variables
    // Lists to store game data.
    private List<Player> playerList=new List<Player>();
    private List<Point> obstacles=new List<Point>();
    private List<Point> brickLocations=new List<Point>();
    private List<BrickWall> brickWalls= new List<BrickWall>();
    private List<Coin> coins = new List<Coin>();
    private List<LifePack> lifePacks = new List<LifePack>();
    private List<Point> water= new List<Point>();

    private bool gameStarted = false;
    private bool gameFinished = false;
    private Client client;

    Resolver r;
    private Player playerSelf;              // player instance of current game

    private AI ai;

    public List<LifePack> getLifePacks() {
        return lifePacks;
    }

    public List<Coin> getCoins()
    {

        return coins;
    }

    public List<Point> getWater()
    {
        return water;
    }
    public List<Point> getStones()
    {
        return obstacles;
    }


    //getters and setters for variables.
    public List<Point> getWaterCoor() {
        return this.water;
    }
    public List<Player> getPlayerList() {
        return this.playerList;
    }
    public List<Point> getStoneCoor()
    {
        return this.obstacles;
    }
    public List<Point> getBricksCoor()
    {
        return this.brickLocations;
    }
    public List<BrickWall> getBricks()
    {
        return this.brickWalls;
    }
    public void addWater(Point loc) {
        water.Add(loc);
    }
    public void addCoin(Point loc,int sec)
    {
        Coin c = new Coin(loc, sec);
        coins.Add(c);
        navigator.map.addCoin(c);
    }

    public void removeCoin(Point p)
    {
        coins.RemoveAll(x => x.Pos == p);
    }

    public void removeLifePack(Point p) {
        lifePacks.RemoveAll(x => x.Pos == p);
    }
    public void addLifePack(Point loc, int sec)
    {
        LifePack lp = new LifePack(loc, sec);
        lifePacks.Add(lp);
        navigator.map.addLifePack(lp);
    }
    public void addStones(Point loc) {
        obstacles.Add(loc);
    }
    public void addBricks(Point loc) {
        brickLocations.Add(loc);
        brickWalls.Add(new BrickWall(loc));
    }
    public BrickWall getBrickByPoint(Point p) {
        foreach (BrickWall b in brickWalls) {
            if (b.Pos == p) return b;
        }
        return null;
    }
    public  void addPlayer(Player p) {
        playerList.Add(p);
    }
    public  Player getPlayerByName(string name)
    {
        Player player=null;
        foreach (Player p in playerList) {
            if (p.Name.ToLower().Equals(name.ToLower())) {
                player = p;
                break;
            }
        }
        return player;
    }
    #endregion

    public  Point PlayerPos
    {
        get { return playerSelf.CurrentP; }
        set {

            playerSelf.CurrentP = value;

        }
    }
    public  Player PlayerSelf
    {
        get { return playerSelf; }
        set { playerSelf = value; }
    }
    public bool GameStarted
    {
        get { return gameStarted; }
        set { gameStarted = value; }
    }
    public bool GameFinished
    {
        get { return gameFinished; }
        set { gameFinished = value; }
    }
    public Client GetClient
    {
        get { return client; }
    }
    public void endGame()
    {
        gameFinished = true;
    }
    public void startGame(bool isDebug) {
        if (client == null)
        {
            GameFinished = false;
            client = new Client();
            ai = new AI(this);

            new Thread(client.Listen).Start();
            //new Thread(ai.goNextPosition).Start();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ai.goNextPosition), (object)"");


            navigator.gameInstance = this;
            Debug.Log("Game started");
            if (!isDebug) {
                //GetClient.Send("JOIN#");
            }
        }
    }

    public void Resolve(object stateInfo)
    {
        string x = (string)stateInfo;
        if (this.r == null) {
           this.r = new Resolver();
        }
        //Debug.Log(x);           //echos server reply.
        r.parseMsg(x, navigator.gameInstance);           //sends reply to parser.
    }

}
