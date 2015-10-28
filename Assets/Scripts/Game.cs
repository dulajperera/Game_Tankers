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

    private  volatile List<Player> playerList;
    private static volatile List<string> playerIPList;
    private static volatile List<int> playerPort;
   
    //private volatile List<CoinPile> coinPileList;
    //private volatile List<CoinPile> availableCoinPileList;
    //private volatile List<CoinPile> disappearCoinPileList;
    private static int coinPilesToDisclose = -1;
    private static int nextCoinPileSend = 0;
    private static volatile List<LifePack> lifePackList;
    private static volatile List<LifePack> availableLifePackList;
    private static volatile List<LifePack> disappearLifePackList;
    private static int lifePacksToDisclose = -1;
    private static int nextLifePackSend = 0;

    //private static volatile List<CoinPile> plunderCoinPileList;

    private  List<Point> obstacles=new List<Point>();
    private  List<Point> brickLocations=new List<Point>();
    private  List<BrickWall> brickWalls= new List<BrickWall>();
    private  List<Point> water= new List<Point>();
    private  List<Bullet> activeBullets= new List<Bullet>();

    private int maxPlayerNumber;
    private int minPlayerNumber = 1;
    private int mapSize;
    private string mapDetails;
    private int obstaclePenalty;

    private bool gameStarted = false;
    private bool gameFinished = false;
    private Client client;

    private byte direction; // 0 - ^, 1 - >, 2 - v, 3 - <
    private Player playerSelf;
    private Point playerPos=new Point(0,0);

    public List<Point> getWaterCoor() {
        return this.water;
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
    public  void changeDirection(byte d) {
        direction = d;
    }
    public  Point PlayerPos
    {
        get { return playerSelf.CurrentP; }
        set { playerSelf.CurrentP = value; }
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
    public void startGame() {
        if (client == null)
        {
            GameFinished = false;
            client = new Client();

            new Thread(client.Listen).Start();
            navigator.gameInstance = this;
            Debug.Log("Game started");
        }
    }

    public static void Resolve(object stateInfo)
    {
        string x = (string)stateInfo;
        Debug.Log(x);

    }

    // Use this for initialization
}
