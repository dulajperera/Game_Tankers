using UnityEngine;
using System.Collections;

public class GameTerminator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onDestroy() {
        navigator.gameInstance.GameFinished = true;
    }
}
