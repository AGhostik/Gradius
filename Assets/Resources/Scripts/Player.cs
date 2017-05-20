using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destroyable {

    [Header("Stats")]
    [Range(0f,5f)]
    public float speed = 1.2f;

    private int old_health;
    private int old_maxHealth;
    private float inputX;
    private float inputY;
    private float deltaX = 0;
    private float deltaY = 0;
    private Vector3 pos_min;
    private Vector3 pos_max;
    private Camera main_cam;

    // Use this for initialization
    void Start()
    {
        Amination_OnStart();
        thisTransform = transform;
        main_cam = Camera.main;

        health = max_health;
        old_health = health;
        old_maxHealth = max_health;

        EventController.addPlayerCurrentHealth(health);
        EventController.addPlayerMaxHealth(max_health);
    }
	
	// Update is called once per frame
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

        pos_min = main_cam.ScreenToWorldPoint(new Vector3 (0,0,0));
        pos_max = main_cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        inputX = Input.GetAxis("Horizontal") * 3;
        inputY = Input.GetAxis("Vertical") * 3;

        flyInput();
    }

    private void OnEnable()
    {
        EventController.Input_Horizontal += takeInput_H;
        EventController.Input_Vertical += takeInput_V;
    }

    private void OnDisable()
    {
        EventController.Input_Horizontal -= takeInput_H;
        EventController.Input_Vertical -= takeInput_V;
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
