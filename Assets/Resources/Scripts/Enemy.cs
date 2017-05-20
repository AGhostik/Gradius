using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI
{
    Fan, Rugal, Garun, Fose, Dee
}

public class Enemy : Destroyable {

    [Header("Stats")]    
    [Range(-5f, 5f)]
    public float move_speed = 0;

    [Header("AI type")]
    public AI ai_current;
    public int score = 5;

    // Use this for initialization
    void Start () {        
        thisTransform = transform;
        health = max_health;

        if (ai_current == AI.Fan)
        {
            oneFrameTime = 0.05f;
        }
        Amination_OnStart();
    }
	
	// Update is called once per frame
	void Update () {   
        thisTransform.position += new Vector3(Time.deltaTime * move_speed, 0, 0);

        if (health <= 0)
        {
            Die();
        }

        timerAnimation();
    }

    void OnDisable()
    {
        EventController.addScore(score);
    }
}