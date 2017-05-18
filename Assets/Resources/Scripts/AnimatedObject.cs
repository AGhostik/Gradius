using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObject : MonoBehaviour {

    [Header("Animation")]
    public Sprite[] frames = new Sprite[1];

    private int current_frame = 0;
    private float timer_start;
    private float timer;
    private SpriteRenderer render;

    // Use this for initialization
    void Start () {
        render = gameObject.GetComponent<SpriteRenderer>();

        timer_start = 0.1f;
        timer = timer_start;
    }    

    private void Anim()
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
