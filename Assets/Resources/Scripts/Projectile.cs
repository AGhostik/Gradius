using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float damage = 5f;
    public float speed_plus = 1f;
    public float speed_mult = 1f;
    public float TTL = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.position += new Vector3(speed_plus / 10f + speed_mult * Time.deltaTime, 0, 0);

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
	}
}
