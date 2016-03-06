using UnityEngine;
using System.Collections;
using System.Drawing;
using Assets.Scripts.bean;

public class Map : MonoBehaviour
{
    //game objects which are representing briks, water, stone and players
    public Transform brickPrefab;
    public Transform waterPrefab;
    public Transform stonePrefab;
    public Transform coinsPrefab;
    public Transform healthPackPrefab;
    public Transform[] perfabs;

    public object[][] currentMap;
    public static bool mapCreated=false;
    public Map() {
        currentMap = new object[10][];
    }

    public object getObjAt(int i, int j) {
        return currentMap[i];
    }
    void hidePlayer(string plName) {
        int playerNumber = int.Parse(plName.Replace("P",""));
        Transform perfab = perfabs[playerNumber];       //player's tank
        perfab.gameObject.SetActive(false);
    }
    void movePlayer(Player p){             //moves given player's tank to new position
        int playerNumber = p.getPlayerNumber();
        Transform perfab = perfabs[playerNumber];       //player's tank
        Point pos = p.CurrentP;

        perfab.TransformDirection(1, 0, 0);
        float step = 10 * Time.deltaTime;
        Vector3 newPointTank = new Vector3(pos.X * 2f, 0, pos.Y * 2f);
        perfab.transform.position = Vector3.MoveTowards(perfab.transform.position, newPointTank, step);

        //determine the direction should move.

        Vector3 targetDir = Vector3.left;
        if (p.Direction == 0)
        {
            targetDir = Vector3.back;
        }
        else if (p.Direction == 1)
        {
            targetDir = Vector3.right;
        }
        else if (p.Direction == 2)
        {
            targetDir = Vector3.forward;
        }

        Vector3 newDir = Vector3.RotateTowards(perfab.transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(perfab.transform.position, newDir, UnityEngine.Color.red);

        perfab.transform.rotation = Quaternion.LookRotation(newDir);
    }
    void Update()
    {
        if (navigator.gameInstance != null)
        {
            if (navigator.gameInstance.GameStarted == true)
            {
                foreach (Player p in navigator.gameInstance.getPlayerList()) {
                    movePlayer(p);
                    fire(p.Shot);
                }
                if (!mapCreated) {
                    //drawMap();
                    //mapCreated = true;
                }
            }
        }
    }

    private void fire(bool isFired) {
        Transform cPlayerPerfab = perfabs[navigator.gameInstance.PlayerSelf.getPlayerNumber()];
        TankShooter ts = cPlayerPerfab.GetComponent<TankShooter>();
        ts.fireRequest = isFired;
        if (ts.fireRequest) {
            Debug.Log("fired");
        }
    }
    // Create map using recieved map details.
    void updateCoins() {
        foreach (Point p in navigator.gameInstance.getBricksCoor())
        {
            Instantiate(coinsPrefab, new Vector3(p.X * 2f, 1, p.Y * 2f), Quaternion.identity);
        }
    }
    public void drawMap() {

        // create game objects

        foreach (Point p in navigator.gameInstance.getBricksCoor())
        {
            Instantiate(brickPrefab, new Vector3(p.X * 2f, 1, p.Y * 2f), Quaternion.identity); //copies of perfabs created in the given place
        }
        foreach (Point p in navigator.gameInstance.getStoneCoor())
        {
            Instantiate(stonePrefab, new Vector3(p.X * 2f, 0, p.Y * 2f), Quaternion.identity);
        }
        foreach (Point p in navigator.gameInstance.getWaterCoor())
        {
            Instantiate(waterPrefab, new Vector3(p.X * 2f, 0, p.Y * 2f), Quaternion.identity);
        }

        // hide inactive players

        string[] names = { "P0", "P1", "P2", "P3", "P4" };
        foreach (string plName in names) {
            if (navigator.gameInstance.getPlayerByName(plName) == null) {
                hidePlayer(plName);
            }
        }

        // focus camera and move current player to the scene

        Point pos = navigator.gameInstance.PlayerSelf.CurrentP;
        Transform cPlayerPerfab = perfabs[navigator.gameInstance.PlayerSelf.getPlayerNumber()];
        cPlayerPerfab.Translate(new Vector3(pos.X * 2f, 0, pos.Y * 2f));
        cPlayerPerfab.TransformDirection(0,0,1);
        SmoothFollow2.target = cPlayerPerfab;

    }

}
