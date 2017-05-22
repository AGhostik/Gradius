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

    [Header("Sound")]
    public AudioClip Sound;
    [Range(0f, 5f)]
    public float volume = 1;

    private int current_frame = 1;
    private float timer_start;
    private float timer;
    private SpriteRenderer render;
    private AudioSource aud;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        render = GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();
        timer_start = TTL / explosion.Length;
        timer = timer_start;
        changeFrame();
    }    

    // Update is called once per frame
    void Update () {
        Die();
        thisTransform.position += axis * axisMultiplier * Time.deltaTime;        
    }

    private void playSound()
    {
        if (Sound != null && volume > 0)
        {
            aud.PlayOneShot(Sound, volume);
        }
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
