using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Garun : Destroyable
{

    [Header("Move")]
    [Range(0f, 5f)]
    public float moveSpeed = 0;
    [Range(0f, 359f)]
    public float angle = 180;
    public float frequency = 3;
    public float magnitude = 1; 
    
    private float anglePerpendicular;
    private Rigidbody2D thisRigidbody;
    private Vector2 colliderSize;
    private Vector3 camPos_min;
    private Vector2 sinTransform;
    private Vector2 startPos;
    private Vector2 move;
    private Vector2 axis;

    // Use this for initialization
    void Start()
    {
        Destroyable_OnStart();
        thisRigidbody = GetComponent<Rigidbody2D>();
        startPos = thisTransform.position;
        anglePerpendicular = angle + 90;
        axis = axisAngle(angle);
        move = axis * moveSpeed;
        
        Amination_OnStart();

        camPos_min = EventController.getCamPos_TL();

        SceneObjectContainer.AddProjectileContainer(gameObject.tag);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        sinTransform = axisAngle(anglePerpendicular) * (Mathf.Sin(frequency * Vector3.Distance(startPos, thisTransform.position))) * magnitude;
        thisRigidbody.position += (sinTransform + move) * Time.deltaTime;
        timerAnimation();

        if (thisTransform.position.x < camPos_min.x)
        {
            thisRigidbody.position = startPos;
        }
    }    

    private Vector2 axisAngle(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(betha), Mathf.Sin(betha));
    }
}