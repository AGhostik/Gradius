using UnityEngine;

public class AnimatedObject : MonoBehaviour {

    [Header("Timer")]
    public float oneFrameTime = 0.1f;

    [Header("Sprites")]
    public Sprite[] frames = new Sprite[1];

    protected float timer;
    protected int currentFrame = 0;
    protected int framesLength;
    
    protected SpriteRenderer render;
    
    protected virtual void Awake () {
        render = GetComponent<SpriteRenderer>();        
        timer = oneFrameTime;
        framesLength = frames.Length;
    }    
    
    protected void TimerAnimation()
    {        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (currentFrame < frames.Length - 1)
            {
                currentFrame++;
            }
            else
            {
                currentFrame = 0;
            }
            ChangeFrame(currentFrame);
            timer = oneFrameTime;
        }
    }

    protected void ChangeFrame(int frame_number = 0)
    {
        if (render != null)
        {
            render.sprite = frames[frame_number];
        }
    }
}
