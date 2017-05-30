using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
    public delegate void voidMethod();
    public delegate void intMethod(int value);
    public delegate void floatMethod(float value);
    public delegate void vector3Method(Vector3 vector);
    public delegate void gameObjectMethod(GameObject obj);

    public static event voidMethod InputA = delegate { };
    public static event voidMethod InputB = delegate { };
    public static event voidMethod InputX = delegate { };
    public static event voidMethod InputY = delegate { };

    public static event floatMethod Input_Horizontal = delegate { };
    public static event floatMethod Input_Vertical = delegate { };

    public static event intMethod UpdatePlayerHealth = delegate { };
    public static event intMethod UpdatePlayerMaxHealth = delegate { };
    public static event intMethod UpdatePlayerGun1Damage = delegate { };
    public static event intMethod UpdatePlayerGun2Damage = delegate { };
    public static event intMethod UpdatePlayerGun1Level = delegate { };
    public static event intMethod UpdatePlayerGun2Level = delegate { };
    public static event floatMethod UpdatePlayerGun1Firerate = delegate { };
    public static event floatMethod UpdatePlayerGun2Firerate = delegate { };
    public static event floatMethod UpdatePlayerSpeed = delegate { };

    public static event gameObjectMethod PlayerGotHit = delegate { };

    public static event intMethod UpdateScores = delegate { };
    public static event floatMethod UpdateLevelProgress = delegate { };
    
    public static Camera mainCamera;

    //var static
    private static List<GameObject> playerHit = new List<GameObject>();

    private static int scores = 0;
    
    private static float level_progress = 0;

    private static int gun1_damage = 0;
    private static int gun2_damage = 0;
    private static float gun1_firerate = 0;
    private static float gun2_firerate = 0;
    private static int gun1_level = 0;
    private static int gun2_level = 0;
    private static float player_speed = 0;
    private static int player_max_health = 0;
    private static int player_current_health = 0;

    private static Vector3 camera_worldPoint_TL;
    private static Vector3 camera_worldPoint_BR;
    //end var static

    //var
    private int old_scores = -1;

    private float old_level_progress = -1;

    private int old_gun1_damage = -1;
    private int old_gun2_damage = -1;
    private float old_gun1_firerate = -1;
    private float old_gun2_firerate = -1;
    private int old_gun1_level = -1;
    private int old_gun2_level = -1;
    private float old_player_speed = -1;
    private int old_player_max_health = -1;
    private int old_player_current_health = -1;


    private float input_H;
    private float input_V;
    private float old_input_H;
    private float old_input_V;
    //end var

    private void Awake()
    {
        mainCamera = Camera.main;
        camera_worldPoint_TL = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        camera_worldPoint_BR = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    private void Update()
    {
        input_H = Input.GetAxis("Horizontal");
        input_V = Input.GetAxis("Vertical");

        if (Input.GetAxisRaw("Menu") != 0)
        {
            SceneManager.LoadScene(0);
        }

        if (input_H != old_input_H)
        {
            Input_Horizontal(input_H);
            old_input_H = input_H;
        }
        if (input_V != old_input_V)
        {
            Input_Vertical(input_V);
            old_input_V = input_V;
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
        // scores
        if (old_scores != scores)
        {
            old_scores = scores;
            UpdateScores(scores);            
        }

        // player health
        if (old_player_current_health != player_current_health)
        {
            old_player_current_health = player_current_health;
            UpdatePlayerHealth(player_current_health);
        }

        // player MAX health
        if (old_player_max_health != player_max_health)
        {
            old_player_max_health = player_max_health;
            UpdatePlayerMaxHealth(player_max_health);
        }

        // gun damage
        if (old_gun1_damage != gun1_damage)
        {
            old_gun1_damage = gun1_damage;
            UpdatePlayerGun1Damage(gun1_damage);
        }
        if (old_gun2_damage != gun2_damage)
        {
            old_gun2_damage = gun2_damage;
            UpdatePlayerGun2Damage(gun2_damage);
        }

        // gun firerate
        if (old_gun1_firerate != gun1_firerate)
        {
            old_gun1_firerate = gun1_firerate;
            UpdatePlayerGun1Firerate(gun1_firerate);
        }
        if (old_gun2_firerate != gun2_firerate)
        {
            old_gun2_firerate = gun2_firerate;
            UpdatePlayerGun2Firerate(gun2_firerate);
        }

        // gun level
        if (old_gun1_level != gun1_level)
        {
            old_gun1_level = gun1_level;
            UpdatePlayerGun1Level(gun1_level);
        }
        if (old_gun2_level != gun2_level)
        {
            old_gun2_level = gun2_level;
            UpdatePlayerGun2Level(gun2_level);
        }

        // player speed
        if (old_player_speed != player_speed)
        {
            old_player_speed = player_speed;
            UpdatePlayerSpeed(player_speed);
        }

        // Level progress
        if (old_level_progress != level_progress)
        {
            old_level_progress = level_progress;
            UpdateLevelProgress(level_progress);
        }

        //player hit
        if (playerHit.Count > 0)
        {
            foreach (GameObject obj in playerHit)
            {
                if (obj != null)
                    PlayerGotHit(obj);
            }
            playerHit.Clear();
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

    public static void playerGun1LevelUP()
    {
        gun1_level++;
    }
    public static void playerGun2LevelUP()
    {
        gun2_level++;
    }

    public static void setGun1Damage(int value)
    {
        gun1_damage = value;
    }
    public static void setGun2Damage(int value)
    {
        gun2_damage = value;
    }

    public static void setGun1Firerate(float value)
    {
        gun1_firerate = value;
    }
    public static void setGun2Firerate(float value)
    {
        gun2_firerate = value;
    }

    public static void setLevelProgress(float value)
    {
        level_progress = value;
    }

    public static void playerHitAdd(GameObject obj)
    {
        playerHit.Add(obj);
    }

    public static Vector3 getCamPos_TL()
    {
        return camera_worldPoint_TL;
    }
    public static Vector3 getCamPos_BR()
    {
        return camera_worldPoint_BR;
    }

    public static void cleanAll()
    {
        playerHit.Clear();
        scores = 0;
        level_progress = 0;
        gun1_damage = 0;
        gun2_damage = 0;
        gun1_firerate = 0;
        gun2_firerate = 0;
        gun1_level = 0;
        gun2_level = 0;
        player_speed = 0;
        player_max_health = 0;
        player_current_health = 0;
}
}