using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI
{
    Fan, Rugal, Garun, Fose, Dee
}

public class Enemy : MonoBehaviour {

    [Header("Stats")]
    public int health = 100;
    [Range(-5f, 5f)]
    public float move_speed = 0;

    [Header("AI type")]
    public AI ai_current;
    public bool isShot = false;

    [Header("Animation")]
    public Sprite[] frames = new Sprite[1];
    public GameObject explosion;

    [Header("Weapons")]
    public GameObject Gun;

    [Header("Ammunition")]
    public GameObject Bullet;

    [Header("Rate of fire")]
    public float firerate1 = 1;

    private float firerate1_timer = 1;

    private int current_frame = 0;
    private float timer_start;
    private float timer;
    private SpriteRenderer render;

    // Use this for initialization
    void Start () {
        render = gameObject.GetComponent<SpriteRenderer>();

        if (ai_current == AI.Fan)
        {
            timer_start = 0.05f;
            timer = timer_start;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            Die();
        }

        transform.position += new Vector3(Time.deltaTime * move_speed, 0, 0);
        //gameObject.transform.Rotate(Vector3.forward);

        Fan();

        if (isShot)
        {
            Shot();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            health -= col.gameObject.GetComponent<Projectile>().damage;
        }
    }

    private void Shot()
    {
        if (Gun != null && Bullet != null)
        {
            if (firerate1_timer >= firerate1)
            {
                GameObject bullet = Instantiate(Bullet);
                bullet.transform.position = Gun.transform.position;
                bullet.transform.SetParent(Gun.transform);
                firerate1_timer = 0;
            }
            if (firerate1_timer < firerate1) firerate1_timer += Time.deltaTime;
        }
    }

    private void Die()
    {
        if (explosion != null)
        {
            Instantiate(explosion).transform.position = gameObject.transform.position;
        }
        Destroy(gameObject);
    }

    private void Fan()
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
            timer = timer_start;
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        render.sprite = frames[frame_number];
    }
}