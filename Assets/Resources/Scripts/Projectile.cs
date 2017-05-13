using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float damage = 5f;
    public float speed_plus = 1f;
    public float speed_mult = 1f;
    public float TTL = 2f;

    public GameObject explosion;

    // Use this for initialization
    void Start () {
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update () {

        transform.position += new Vector3(speed_plus / 10f + speed_mult * Time.deltaTime, 0, 0);

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Die();
        }        
	}

    private void Die()
    {
        Instantiate(explosion).transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}