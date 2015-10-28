using UnityEngine;
using System.Collections;
using System.Drawing;

public class Map: MonoBehaviour
{
    public Transform brickPrefab;
    public Transform waterPrefab;
    public Transform stonePrefab;

    private int width=10;
    private int height = 10;
    public object[][] currentMap;

    public Map() {
        currentMap = new object[10][];
    }

    public object getObjAt(int i, int j) {
        return currentMap[i];
    }
    
    void Start()
    {

    }
    public void drawMap() {
        foreach (Point p in navigator.gameInstance.getBricksCoor())
        {
            Debug.Log(p.ToString());
            Instantiate(brickPrefab, new Vector3(p.X * 5, 0, p.Y * 5), Quaternion.identity);
        }
        foreach (Point p in navigator.gameInstance.getStoneCoor())
        {
            Instantiate(stonePrefab, new Vector3(p.X * 5, 0, p.Y * 5), Quaternion.identity);
        }
        foreach (Point p in navigator.gameInstance.getWaterCoor())
        {
            Instantiate(waterPrefab, new Vector3(p.X * 5, 0, p.Y * 5), Quaternion.identity);
        }
    }

}
