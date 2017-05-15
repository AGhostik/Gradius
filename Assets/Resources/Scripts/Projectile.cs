using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Stats")]
    public int damage = 5;
    public float speed_plus = 1f;
    public float speed_mult = 1f;
    public float TTL = 2f;

    [Header("Direction")]
    public bool toRight = true;

    [Header("Animation after die")]
    public GameObject explosion;

    private int direction;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        if (!toRight)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
	}

    // Update is called once per frame
    void Update () {

        thisTransform.position += new Vector3( (speed_plus / 10f + speed_mult * Time.deltaTime) * direction, 0, 0);

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Die(false);
        }        
	}

    public void Die(bool eff = true)
    {
        if (explosion != null && eff)
        {
            Instantiate(explosion).transform.position = thisTransform.position;
        }
        Destroy(gameObject);
    }
}