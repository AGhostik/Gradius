using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [Header("Stats")]
    public float TTL = 0.3f;
    public float moveSpeed_multiplier = 4;

    [Header("Animation")]
    public Sprite[] explosion = new Sprite[3];

    private int current_frame = 1;
    private float timer_start;
    private float timer;
    private SpriteRenderer render;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        render = gameObject.GetComponent<SpriteRenderer>();
        timer_start = TTL / explosion.Length;
        timer = timer_start;
        changeFrame();
    }
	
	// Update is called once per frame
	void Update () {
        thisTransform.position += new Vector3(Time.deltaTime * moveSpeed_multiplier, 0, 0);
        Die();
    }

    private void Die()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (current_frame < explosion.Length)
            {
                changeFrame(current_frame);
                current_frame++;
                timer = timer_start;
            }
            else
            {
                Destroy(gameObject);
            }        
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        render.sprite = explosion[frame_number];
    }
}
