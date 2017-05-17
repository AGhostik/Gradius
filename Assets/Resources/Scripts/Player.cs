using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destroyable {

    [Header("Stats")]
    [Range(0f,5f)]
    public float speed = 1.2f;     

    [Header("Animation")]
    public Sprite NormalSprite;
    public Sprite UpSprite;
    public Sprite DownSprite;

    private int old_health;
    private int old_maxHealth;
    private float inputX;
    private float inputY;
    private float deltaX = 0;
    private float deltaY = 0;
    private Vector3 pos_min;
    private Vector3 pos_max;
    private Camera main_cam;
    private SpriteRenderer render;
    private EventController.playerHealth sendHealth = UIDraw.SetPlayerHealth;
    private EventController.playerHealth sendMaxHealth = UIDraw.SetPlayerMaxHealth;

    // Use this for initialization
    void Start()
    {
        thisTransform = transform;
        main_cam = Camera.main;
        render = gameObject.GetComponent<SpriteRenderer>();

        health = max_health;
        old_health = health;
        old_maxHealth = max_health;

        sendHealth(health);
        sendMaxHealth(max_health);
    }
	
	// Update is called once per frame
	void Update () {
        //thisTransform.position += new Vector3(Time.deltaTime,0,0);

        if (health <= 0)
        {
            sendHealth(0);
            Die();
        }
        else
        {
            if (old_health != health)
            {
                sendHealth(health);
                old_health = health;
            }
            if (old_maxHealth != max_health)
            {
                sendMaxHealth(max_health);
                old_maxHealth = max_health;
            }
        }

        pos_min = main_cam.ScreenToWorldPoint(new Vector3 (0,0,0));
        pos_max = main_cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        inputX = Input.GetAxis("Horizontal") * 3;
        inputY = Input.GetAxis("Vertical") * 3;

        flyInput();
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

        changeSprite(deltaY);

        thisTransform.position += new Vector3(deltaX * speed, deltaY * speed, 0);
    }

    private void changeSprite(float vertical)
    {
        if (UpSprite != null && DownSprite != null)
        {
            if (vertical < -0.01f)
            {
                render.sprite = DownSprite;
            }
            else
            if (vertical > 0.01f)
            {
                render.sprite = UpSprite;
            }
            else
            {
                render.sprite = NormalSprite;
            }
        }
    }
}
