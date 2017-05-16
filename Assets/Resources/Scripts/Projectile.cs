using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Stats")]
    public int damage = 5;
    public float range = 10;
    public float speed_plus = 1;
    public float speed_mult = 1;
    public float TTL = 5;
    public int pierce_count = 0;

    [Header("Direction")]
    public bool toRight = true;

    [Header("Animation")]
    public GameObject pierceExplosion;
    public GameObject dieExplosion;
    
    private float delta_speed;
    private Vector3 startPos;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        startPos = thisTransform.position;
        delta_speed = ((speed_plus + 1) * speed_mult) * 2;

        if (!toRight)
        {
            delta_speed *= -1;
        }
	}

    // Update is called once per frame
    void Update () {

        thisTransform.position += new Vector3( delta_speed * Time.deltaTime, 0, 0);

        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Die(false);
        }

        rangeCheck();
	}

    public void checkDie()
    {
        if (pierce_count > 0)
        {
            pierce_count--;
            instantianeExplosion(pierceExplosion);
        }
        else
        {
            Die();
        }
    }

    private void Die(bool eff = true)
    {
        if (eff)
        {
            instantianeExplosion(dieExplosion);
        }
        Destroy(gameObject);
    }

    private void instantianeExplosion(GameObject expl)
    {
        if (expl != null)
        {
            Instantiate(dieExplosion).transform.position = thisTransform.position;
        }
    }

    private void rangeCheck()
    {
        if (toRight)
        {
            if (thisTransform.position.x - startPos.x >= range)
            {
                Die();
            }
        }
        else
        {
            if (startPos.x - thisTransform.position.x >= range)
            {
                Die();
            }
        }
    }
}