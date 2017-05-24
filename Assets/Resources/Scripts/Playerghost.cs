using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerghost : MonoBehaviour {

    public Transform player;
    private Transform thisTransform;

	// Use this for initialization
	void Start () {
        thisTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            Destroy(gameObject);
        }
        else
        {
            thisTransform.position = player.transform.position;
        }
	}
}
