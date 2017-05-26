using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public GameObject enemies;
    public GameObject player;

	// Use this for initialization
	void Start () {
        if (enemies != null)
        {
            Instantiate(enemies);
        }
        if (player != null)
        {
            Instantiate(player);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
