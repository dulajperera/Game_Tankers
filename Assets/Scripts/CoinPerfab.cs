using UnityEngine;
using System.Collections;
using System.Drawing;

public class CoinPerfab : MonoBehaviour {
    public int lifetime = 0;
    public Point position;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 50 * Time.deltaTime,0);
    }

    void onDestroy() {
        navigator.gameInstance.removeCoin(position);
    }
}
