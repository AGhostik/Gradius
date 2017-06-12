using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
#region delegate
    public delegate void voidMethod();
    public delegate void intMethod(int value);
    public delegate void floatMethod(float value);
    public delegate void vector3Method(Vector3 vector);
    public delegate void gameObjectMethod(GameObject obj);
    #endregion

#region event
    public static event intMethod UpdateScores = delegate { };
    public static event intMethod UpdatePlayerHealth = delegate { };
    public static event intMethod UpdatePlayerMaxHealth = delegate { };
    public static event intMethod UpdatePlayerGun1Damage = delegate { };
    public static event intMethod UpdatePlayerGun2Damage = delegate { };    

    public static event voidMethod InputA = delegate { };
    public static event voidMethod InputB = delegate { };
    public static event voidMethod InputX = delegate { };
    public static event voidMethod InputY = delegate { };

    public static event voidMethod UpdatePlayerGun1Level = delegate { };
    public static event voidMethod UpdatePlayerGun2Level = delegate { };

    public static event floatMethod Input_Vertical = delegate { };
    public static event floatMethod Input_Horizontal = delegate { };
    public static event floatMethod UpdatePlayerSpeed = delegate { };
    public static event floatMethod UpdateLevelProgress = delegate { };
    public static event floatMethod UpdatePlayerGun1Firerate = delegate { };
    public static event floatMethod UpdatePlayerGun2Firerate = delegate { }; 

    public static event gameObjectMethod PlayerGotHit = delegate { };
    #endregion

#region var static
    public static Camera mainCamera;
    private static Vector3 camera_worldPoint_TL;
    private static Vector3 camera_worldPoint_BR;
    #endregion

#region var
    private float input_H;
    private float input_V;
    private float old_input_H;
    private float old_input_V;
#endregion

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
            Global.levelIndex = 0;
            SceneManager.LoadScene(1);
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
        
    }

    public static void AddScore(int value)
    {
        Global.scores += value;
        UpdateScores(Global.scores);
    }
    public static void SetPlayerCurrentHealth(int value)
    {
        UpdatePlayerHealth(value);
    }
    public static void SetPlayerMaxHealth(int value)
    {
        UpdatePlayerMaxHealth(value);
    }
    public static void SetPlayerSpeed(float value)
    {
        UpdatePlayerSpeed(value);
    }

    public static void PlayerGun1LevelUP()
    {
        UpdatePlayerGun1Level();
    }
    public static void PlayerGun2LevelUP()
    {
        UpdatePlayerGun2Level();
    }

    public static void SetGun1Damage(int value)
    {
        UpdatePlayerGun1Damage(value);
    }
    public static void SetGun2Damage(int value)
    {
        UpdatePlayerGun2Damage(value);
    }

    public static void SetGun1Firerate(float value)
    {
        UpdatePlayerGun1Firerate(value);
    }
    public static void SetGun2Firerate(float value)
    {
        UpdatePlayerGun2Firerate(value);
    }

    public static void SetLevelProgress(float value)
    {
        UpdateLevelProgress(value);
    }

    public static void PlayerHitAdd(GameObject obj)
    {
        PlayerGotHit(obj);
    }

    /// <summary>
    /// Top Left point
    /// </summary>
    public static Vector3 GetCamPos_TL()
    {
        return camera_worldPoint_TL;
    }
    /// <summary>
    /// Bottom Right point
    /// </summary>
    public static Vector3 GetCamPos_BR()
    {
        return camera_worldPoint_BR;
    }
}