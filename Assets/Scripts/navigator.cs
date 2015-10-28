using UnityEngine;
using System.Collections;

public class navigator : MonoBehaviour {
    public static Game gameInstance;
    public static string cPlayerName;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
    
    public void loadGame() {
        gameInstance = new Game();
        gameInstance.startGame();
        Application.LoadLevel("game_field");
    }

    public void quitGame()
    {
        gameInstance.GameFinished = true;
        Application.Quit();
    }

    public void debugGame()
    {
        gameInstance = new Game();
        gameInstance.startGame();
        Application.LoadLevel("debug_game");
    }

    public void debugCMD(string cmd) {
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
