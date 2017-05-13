using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Stats")]
    public float health = 100f;

    [Header("Animation")]
    public Sprite[] frames = new Sprite[1];

    private int current_frame = 0;
    private const float timer_const = 0.05f;
    private float timer = timer_const;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        anima();
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            health -= col.gameObject.GetComponent<Projectile>().damage;
        }
    }

    private void anima()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (current_frame < frames.Length - 1)
            {
                current_frame++;
            }
            else
            {
                current_frame = 0;
            }
            changeFrame(current_frame);
            timer = timer_const;
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = frames[frame_number];
    }
}
