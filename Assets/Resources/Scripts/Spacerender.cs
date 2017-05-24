using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spacerender : MonoBehaviour {

    public float speed = 0.5f;

    private Renderer render;

	// Use this for initialization
	void Start () {
        render = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        render.material.mainTextureOffset = new Vector2(Time.time * -speed, 0);
    }
}
