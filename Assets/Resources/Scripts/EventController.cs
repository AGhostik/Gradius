using System;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public delegate void voidMethod();
    public delegate void intMethod(int value);
    public delegate void floatMethod(float value);

    public static event voidMethod InputA = delegate { };
    public static event voidMethod InputB = delegate { };
    public static event voidMethod InputX = delegate { };
    public static event voidMethod InputY = delegate { };

    public static event floatMethod Input_Horizontal = delegate { };
    public static event floatMethod Input_Vertical = delegate { };

    public static event intMethod UpdateScores = delegate { };
    public static event intMethod UpdatePlayerHealth = delegate { };
    public static event intMethod UpdatePlayerMaxHealth = delegate { };
    public static event floatMethod UpdateLevelProgress = delegate { };
    public static event floatMethod UpdateCameraSpeed = delegate { };

    //var static
    private static int scores = 0;
    
    private static float level_progress = 0;

    private static float camera_speed = 0;

    private static int gun1_damage = 0;
    private static int gun2_damage = 0;
    private static float player_speed = 0;
    private static int player_max_health = 0;
    private static int player_current_health = 0;
    //end var static

    //var
    private int old_scores;

    private float old_level_progress;

    private float old_camera_speed;

    private int old_gun1_damage;
    private int old_gun2_damage;
    private float old_player_speed;
    private int old_player_max_health;
    private int old_player_current_health;

    private float input_H;
    private float input_V;
    //end var

    private void Update()
    {
        input_H = Input.GetAxis("Horizontal");
        input_V = Input.GetAxis("Vertical");

        if (Input.GetAxisRaw("Menu") != 0)
        {
            Application.Quit();
        }

        if (input_H != 0)
        {
            Input_Horizontal(input_H);
        }
        if (input_V != 0)
        {
            Input_Vertical(input_V);
        }

        if (Input.GetAxisRaw("A") != 0)
        {
            InputA();
        }
        if (Input.GetAxisRaw("B") != 0)
        {
            InputB();
        }
        if (Input.GetAxisRaw("X") != 0)
        {
            InputX();
        }
        if (Input.GetAxisRaw("Y") != 0)
        {
            InputY();
        }

        if (old_scores != scores)
        {
            old_scores = scores;
            UpdateScores(scores);            
        }

        if (old_player_current_health != player_current_health)
        {
            old_player_current_health = player_current_health;
            UpdatePlayerHealth(player_current_health);
        }

        if (old_player_max_health != player_max_health)
        {
            old_player_max_health = player_max_health;
            UpdatePlayerMaxHealth(player_max_health);
        }

        if (old_level_progress != level_progress)
        {
            old_level_progress = level_progress;
            UpdateLevelProgress(level_progress);
        }

        if (old_camera_speed != camera_speed)
        {
            old_camera_speed = camera_speed;
            UpdateCameraSpeed(camera_speed);
        }
    }

    public static void addScore(int value)
    {
        scores += value;
    }
    public static void addGun1Damage(int value)
    {
        gun1_damage += value;
    }
    public static void addGun2Damage(int value)
    {
        gun2_damage += value;
    }
    public static void addPlayerCurrentHealth(int value)
    {
        player_current_health += value;
    }
    public static void addPlayerMaxHealth(int value)
    {
        player_max_health += value;
    }
    public static void addPlayerSpeed(float value)
    {
        player_speed += value;
    }

    public static void setLevelProgress(float value)
    {
        level_progress = value;
    }

    public static void setCamSpeed(float value)
    {
        camera_speed = value;
    }
}