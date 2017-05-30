using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [Header("Move")]    
    public float axisMultiplier = 0.5f;
    public Vector3 axis = new Vector3(1, 0, 0);

    [Header("Time To Live")]
    public float TTL = 0.3f;

    [Header("Animation")]
    public Sprite[] explosion = new Sprite[3];

    private int currentFrame = 1;
    private float timer;
    private float timerStart; 
    private Transform thisTransform;
    private AudioSource effectAudio;
    private SpriteRenderer render;

    void Awake()
    {
        thisTransform = transform;
        render = GetComponent<SpriteRenderer>();
        effectAudio = GetComponent<AudioSource>();
        timerStart = TTL / explosion.Length;
    }

    private void OnEnable()
    {
        timer = timerStart;
        axis *= axisMultiplier;        
        currentFrame = 1;
        changeFrame();
    }
    
    void Update () {
        Die();
        thisTransform.position += axis * Time.deltaTime;        
    }

    private void OnDisable()
    {
        effectAudio.mute = false;
    }

    private void Die()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (currentFrame < explosion.Length)
            {
                changeFrame(currentFrame);
                currentFrame++;
                timer = timerStart;
            }
            else
            {
                gameObject.SetActive(false);
            }        
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        render.sprite = explosion[frame_number];
    }
}
