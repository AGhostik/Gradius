using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Garun : Destroyable
{

    [Header("Move")]
    [Range(0f, 5f)]
    public float move_speed = 0;
    [Range(0f, 359f)]
    public float angle = 180;
    public float frequency = 3;
    public float magnitude = 1; 

    [Header("Reward")]
    public int score = 20;

    private int old_frame;
    private float angle_perpend;
    private bool increase_collider = false;
    private BoxCollider2D enemy_box_collider;
    private Rigidbody2D thisRigidbody;
    private Vector2 collider_size;
    private Vector3 cam_pos_min;
    private Vector3 cam_pos_max;
    private Vector2 sin_transform;
    private Vector2 start_pos;
    private Vector2 move;
    private Vector2 axis;

    // Use this for initialization
    void Start()
    {
        Destroyable_OnStart();
        thisRigidbody = GetComponent<Rigidbody2D>();
        enemy_box_collider = GetComponent<BoxCollider2D>();
        old_frame = current_frame;
        start_pos = thisTransform.position;
        angle_perpend = angle + 90;
        axis = axisAngle(angle);
        move = axis * move_speed;
        
        Amination_OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        sin_transform = axisAngle(angle_perpend) * (Mathf.Sin(frequency * Vector3.Distance(start_pos, thisTransform.position))) * magnitude;
        thisRigidbody.position += (sin_transform + move) * Time.deltaTime;
        timerAnimation();

        if (old_frame != current_frame)
        {
            afterChangeFrame();
            old_frame = current_frame;
        }

        if (thisTransform.position.x < cam_pos_min.x)
        {
            thisRigidbody.position = new Vector2(cam_pos_max.x, thisTransform.position.y);
            start_pos = thisTransform.position;
        }
    }

    private void afterChangeFrame()
    {
        if (enemy_box_collider.size.y <= 0.13f)
        {
            increase_collider = true;
        }
        else
        if (enemy_box_collider.size.y >= 0.17f)
        {
            increase_collider = false;
        }

        if (increase_collider)
        {
            enemy_box_collider.size += new Vector2(0, 0.02f);
        }
        else
        {
            enemy_box_collider.size -= new Vector2(0, 0.02f);
        }
    }

    private void OnEnable()
    {
        EventController.UpdateCameraWorldPoint_TL += takeCamWP_TL;
        EventController.UpdateCameraWorldPoint_BR += takeCamWP_BR;
    }

    void OnDisable()
    {
        EventController.addScore(score);

        EventController.UpdateCameraWorldPoint_TL -= takeCamWP_TL;
        EventController.UpdateCameraWorldPoint_BR -= takeCamWP_BR;
    }

    private Vector2 axisAngle(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(betha), Mathf.Sin(betha));
    }

    private void takeCamWP_TL(Vector3 vector)
    {
        cam_pos_min = vector;
    }

    private void takeCamWP_BR(Vector3 vector)
    {
        cam_pos_max = vector;
    }
}