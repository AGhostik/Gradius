using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObject : MonoBehaviour {

    [Header("Timer")]
    public float oneFrameTime = 0.1f;

    [Header("Sprites")]
    public Sprite[] frames = new Sprite[1];

    protected float timer;
    protected int current_frame = 0; 
    
    private SpriteRenderer render;
    
    /// <summary>
    /// Put in OnStart();
    /// </summary>
    protected void Amination_OnStart () {
        render = gameObject.GetComponent<SpriteRenderer>();        
        timer = oneFrameTime;
    }    

    protected void timerAnimation()
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
            timer = oneFrameTime;
        }
    }

    protected void changeFrame(int frame_number = 0)
    {
        if (render != null)
        {
            render.sprite = frames[frame_number];
        }
    }
}
