using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [Header("Stats")]
    public float moveSpeed_multiplier = 4;

    [Header("Animation")]
    public Sprite[] explosion = new Sprite[3];

    [Header("Direction")]
    public bool toRight = true;

    private int current_frame = 1;
    private const float timer_const = 0.1f;
    private float timer = timer_const;
    private int direction;
    private SpriteRenderer render;

    // Use this for initialization
    void Start () {
        render = gameObject.GetComponent<SpriteRenderer>();

        if (!toRight)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        changeFrame();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(Time.deltaTime * moveSpeed_multiplier * direction, 0, 0);
        Die();
    }

    private void Die()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (current_frame == explosion.Length - 1)
            {
                Destroy(gameObject);
            }
            changeFrame(current_frame);
            current_frame++;
            timer = timer_const;
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        render.sprite = explosion[frame_number];
    }
}
