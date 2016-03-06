using UnityEngine;
using System.Collections;
using Assets.Scripts.bean;
using System.Collections.Generic;

public class navigator : MonoBehaviour {
    //base class which contains game instance.

    public static Game gameInstance;
    public static string cPlayerName;
    public GameObject menu;             // Assigned in inspector
    private bool isShowing=true;        // Menu showing or not
    private byte viewMode = 0;          // 0 - 3D/Plan view, 1 - Plan/3D view, 2 - Plan view, 3 - 3D view
                                        // Listers called once per frame
    void changeView() {
        if (!gameInstance.GameStarted) return; //do nothing if game is not stated

        Camera cam3d=GameObject.Find("Main Camera").GetComponent<Camera>();
        Camera camPlan = GameObject.Find("planView").GetComponent<Camera>();
        Rect majorRect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
        Rect minorRect = new Rect(new Vector2(0.6f, 0.6f), new Vector2(0.4f, 0.4f));

        switch (viewMode) {
            case 0:        
                cam3d.rect = majorRect;camPlan.rect = minorRect;
                cam3d.depth = -1; camPlan.depth = 1;
                break;
            case 1:                
                cam3d.rect = minorRect; camPlan.rect = majorRect;
                cam3d.depth = 1; camPlan.depth = -1;
                break;
            case 2:              
                cam3d.rect = minorRect; camPlan.rect = majorRect;
                cam3d.depth = -1; camPlan.depth = 1;
                break;
            case 3:              
                cam3d.rect = majorRect; camPlan.rect = minorRect;
                cam3d.depth = 1; camPlan.depth = -1;
                break;
        }
    }
	void Update() {


        if (Input.GetKeyDown("a"))
        {
            AI b = new AI(gameInstance);
            b.generateMap();
        }

        if (Input.GetKeyDown("up"))
            debugCMD("UP#");

        if (Input.GetKeyDown("down"))
            debugCMD("DOWN#");

        if (Input.GetKeyDown("left"))
            debugCMD("LEFT#");

        if (Input.GetKeyDown("right"))
            debugCMD("RIGHT#");

        if (Input.GetKeyDown("space"))
            debugCMD("SHOOT#");

        if (Input.GetKeyDown("escape"))
        {
            isShowing = !isShowing;
            menu.SetActive(isShowing);
        }
        if (Input.GetKeyDown("c"))      //change view mode
        {

            if (viewMode > 2) viewMode = 0;
            else viewMode++;
            changeView();
        }
            if (Input.GetKeyDown("b"))      //echos brick details
        {
            Debug.Log("Bricks damage levels,\nPosition\tDamage\n");
            string text = "";
            foreach (BrickWall b in gameInstance.getBricks()) {
                text +=b.Pos.X + "," + b.Pos.Y + "\t" + b.DamageLevel+"\n";
            }
            Debug.Log(text);
        }

        if (Input.GetKeyDown("p"))      //echos player details
        {
            Debug.Log("Player details,\nName\tPosition{X, Y}\tDir\tCoins\tHealth\tPoints\n");
            string text = "";
            foreach (Player p in gameInstance.getPlayerList())
            {
                text += p.Name+"\t" + p.CurrentP.ToString() + "\t" + p.Direction + "\t" + p.Coins+"\t"+p.Health+"\t"+p.PointsEarned+"\n";
            }
            Debug.Log(text);
        }
    }
    
    public void loadGame() {
        gameInstance = new Game();
        gameInstance.startGame(false);
        isShowing = false;
        menu.SetActive(isShowing);
        Application.LoadLevel("play_game");
    }

    public void quitGame()
    {
        gameInstance.GameFinished = true;
        Application.Quit();
    }
    
    public void debugGame()
    {
        gameInstance = new Game();
        gameInstance.startGame(true);
        Application.LoadLevel("debug_game");
    }
    // send a string to game server
    public static void debugCMD(string cmd) {
        if (gameInstance!=null)
        {
            Debug.Log(cmd);
            gameInstance.GetClient.Send(cmd);
        }
        else {
            Debug.Log("Not send "+cmd);
        }

    }

    public void back()
    {
        Application.LoadLevel("main_menu");
    }
}
