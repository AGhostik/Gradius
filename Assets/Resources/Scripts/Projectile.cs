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
    public bool onlyHit = false;

    private int direction;

    // Use this for initialization
    void Start () {
        if (!toRight)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (toRight)
        {
            if (col.gameObject.tag == "Enemy")
            {
                Die(true);
            }
        }
        else
        {
            if (col.gameObject.tag == "Player")
            {
                Die(true);
            }
        }
    }

    // Update is called once per frame
    void Update () {

        transform.position += new Vector3( (speed_plus / 10f + speed_mult * Time.deltaTime) * direction, 0, 0);

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Die(!onlyHit);
        }        
	}

    private void Die(bool eff)
    {
        if (explosion != null && eff)
        {
            Instantiate(explosion).transform.position = gameObject.transform.position;
        }
        Destroy(gameObject);
    }
}