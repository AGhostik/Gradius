using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destroyable {

    [Header("Stats")]
    [Range(0f,5f)]
    public float speed = 1.2f;
    public float maxSpeed = 3;

    private float inputX;
    private float inputY;
    private float deltaX = 0;
    private float deltaY = 0;
    private Vector3 pos_min;
    private Vector3 pos_max;
    private Camera main_cam;
    
    void Start()
    {
        Amination_OnStart();  
        main_cam = Camera.main;        
        Destroyable_OnStart();        

        EventController.addPlayerCurrentHealth(health);
        EventController.addPlayerMaxHealth(max_health);
        EventController.addPlayerSpeed(speed);
    }
	void Update ()
    {
        if (old_health != health)
        {
            EventController.addPlayerCurrentHealth(health - old_health);            
            old_health = health;            
        }
        if (old_maxHealth != max_health)
        {
            EventController.addPlayerMaxHealth(max_health - old_maxHealth);
            old_maxHealth = max_health;
        }

        if (health <= 0)
        {
            Die();
        }

        inputX = Input.GetAxis("Horizontal") * 3;
        inputY = Input.GetAxis("Vertical") * 3;

        flyInput();
    }

    private void OnEnable()
    {
        EventController.Input_Horizontal += takeInput_H;
        EventController.Input_Vertical += takeInput_V;
        EventController.UpdateCameraWorldPoint_TL += takeCamWP_TL;
        EventController.UpdateCameraWorldPoint_BR += takeCamWP_BR;
    }

    private void OnDisable()
    {
        EventController.Input_Horizontal -= takeInput_H;
        EventController.Input_Vertical -= takeInput_V;
        EventController.UpdateCameraWorldPoint_TL -= takeCamWP_TL;
        EventController.UpdateCameraWorldPoint_BR -= takeCamWP_BR;
    }

    private void takeCamWP_TL(Vector3 vector)
    {
        pos_min = vector;
    }

    private void takeCamWP_BR(Vector3 vector)
    {
        pos_max = vector;
    }

    private void takeInput_H(float value)
    {
        inputX = value;
    }

    private void takeInput_V(float value)
    {
        inputY = value;
    }

    private void flyInput()
    {
        deltaX = 0;
        deltaY = 0;

        if (inputX < 0)
        {
            if (thisTransform.position.x > pos_min.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        } else
        {
            if (thisTransform.position.x < pos_max.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        }

        if (inputY < 0)
        {
            if (thisTransform.position.y > pos_min.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }
        else
        {
            if (thisTransform.position.y < pos_max.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }

        PlayerAnimation(deltaY);

        thisTransform.position += new Vector3(deltaX * speed, deltaY * speed, 0);
    }

    private void PlayerAnimation(float vertical)
    {
        if (frames.Length >= 3)
        {
            if (vertical < -0.01f)
            {
                changeFrame(2); //DownSprite
            }
            else
            if (vertical > 0.01f)
            {
                changeFrame(0); //UpSprite
            }
            else
            {
                changeFrame(1); //NormalSprite
            }
        }
    }
}
