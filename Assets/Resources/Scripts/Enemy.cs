using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI
{
    Fan, Rugal, Garun, Fose, Dee
}

public class Enemy : MonoBehaviour {

    [Header("Stats")]    
    [Range(-5f, 5f)]
    public float move_speed = 0;

    [Header("AI type")]
    public AI ai_current;

    [Header("Animation")]
    public Sprite[] frames = new Sprite[1];    

    private int current_frame = 0;
    private float timer_start;
    private float timer;
    private SpriteRenderer render;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        render = gameObject.GetComponent<SpriteRenderer>();

        if (ai_current == AI.Fan)
        {
            timer_start = 0.05f;
            timer = timer_start;
        }
    }
	
	// Update is called once per frame
	void Update () {   
        thisTransform.position += new Vector3(Time.deltaTime * move_speed, 0, 0);

        Fan();
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