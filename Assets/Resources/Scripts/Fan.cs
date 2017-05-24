using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : Destroyable
{

    [Header("Move")]
    [Range(0f, 5f)]
    public float move_speed = 0;
    [Range(0f, 359f)]
    public float angle = 180;

    [Header("Reward")]
    public int score = 5;

    private int old_frame;
    private float angle_perpend;
    private Vector3 cam_pos_min;
    private Vector3 cam_pos_max;
    private Vector2 sin_transform;
    private Vector2 move;
    private Vector2 axis;
    private Rigidbody2D thisRigidbody;

    // Use this for initialization
    void Start()
    {
        Destroyable_OnStart();
        thisRigidbody = GetComponent<Rigidbody2D>();
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

        thisRigidbody.position += move * Time.deltaTime;
        timerAnimation();

        if (thisTransform.position.x < cam_pos_min.x)
        {
            thisRigidbody.position = new Vector2(cam_pos_max.x, thisTransform.position.y);
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