using UnityEngine;
using UnityEngine.UI;

public class UIDraw : MonoBehaviour {

    public Text FPS;
    public Text Scores;
    public Text Speed;
    public Text Health;
    public Text Gun1;
    public Text Gun2;

    private string gun_damage1Str = string.Empty;
    private string gun_damage2Str = string.Empty;   
    private string player_speedStr = string.Empty;
    private string scoresStr = "Score: ";
    private string healthStr = "Health: ";

   // private int gun1_lvl = 0;
   // private int upgrade_lvl = 0; 
    private int gun1_dmg;
    private int gun2_dmg;
    private float gun1_fr;
    private float gun2_fr;
    private int playerHealth;
    private int playerMaxHealth;
    private int old_playerHealth = 0;
    private int old_playerMaxHealth = 0;

    private float fps;
    private float fpsMax = 0;
    private float fpsMin = 999;
    private string fpsStr;

    //private float level_progress;
    //private float playerHealthPercent;

    private void OnEnable()
    {
        EventController.UpdatePlayerHealth += takePlayerHealth;
        EventController.UpdatePlayerMaxHealth += takePlayerMaxHealth;
        EventController.UpdateLevelProgress += takeLevelProgress;
        EventController.UpdatePlayerGun1Damage += takeGun1Damage;
        EventController.UpdatePlayerGun2Damage += takeGun2Damage;
        EventController.UpdatePlayerGun1Level += takeGun1Level;
        EventController.UpdatePlayerGun1Firerate += takeGun1Firerate;
        EventController.UpdatePlayerGun2Firerate += takeGun2Firerate;
        EventController.UpdatePlayerSpeed += takeSpeed;
        EventController.UpdateScores += takeScores;
    }

    private void OnDisable()
    {
        EventController.UpdatePlayerHealth -= takePlayerHealth;
        EventController.UpdatePlayerMaxHealth -= takePlayerMaxHealth;
        EventController.UpdateLevelProgress -= takeLevelProgress;
        EventController.UpdatePlayerGun1Damage -= takeGun1Damage;
        EventController.UpdatePlayerGun2Damage -= takeGun2Damage;
        EventController.UpdatePlayerGun1Level -= takeGun1Level;
        EventController.UpdatePlayerGun1Firerate -= takeGun1Firerate;
        EventController.UpdatePlayerGun2Firerate -= takeGun2Firerate;
        EventController.UpdatePlayerSpeed -= takeSpeed;
        EventController.UpdateScores -= takeScores;
    }

    void Start ()
    {
    }

    void Update () {
        updateHealth();

        //Temp
        fps = 1.0f / Time.deltaTime;
        if (fpsMax < fps) fpsMax = fps;
        if (fpsMin > fps) fpsMin = fps;
        fpsStr = "Fps: " + fps.ToString("0.") + "\nmax: " + fpsMax.ToString("0.") + "\nmin: " + fpsMin.ToString("0.");
        FPS.text = fpsStr;
    }

    //scores
    private void takeScores(int value)
    {
        scoresStr = "Score: " + value;
        Scores.text = scoresStr;
    }

    //speed
    private void takeSpeed(float value)
    {
        player_speedStr = "Speed: " + value.ToString();
        Speed.text = player_speedStr;
    }

    //gun damage
    private void takeGun1Damage(int value)
    {
        gun1_dmg = value;
        gun_damage1Str = "Dmg_1: " + gun1_dmg.ToString() + " / " + gun1_fr.ToString("0.00") + " per second";
        Gun1.text = gun_damage1Str;
    }
    private void takeGun2Damage(int value)
    {
        gun2_dmg = value;
        gun_damage2Str = "Dmg_2: " + gun2_dmg.ToString() + " / " + gun2_fr.ToString("0.00") + " per second";
        Gun2.text = gun_damage2Str;
    }

    //gun firerate
    private void takeGun1Firerate(float value)
    {
        gun1_fr = value;
        gun_damage1Str = "Dmg_1: " + gun1_dmg.ToString() + " / " + gun1_fr.ToString("0.00") + " per second";
        Gun1.text = gun_damage1Str;
    }
    private void takeGun2Firerate(float value)
    {
        gun2_fr = value;
        gun_damage2Str = "Dmg_2: " + gun2_dmg.ToString() + " / " + gun2_fr.ToString("0.00") + " per second";
        Gun2.text = gun_damage2Str;
    }

    //gun level
    private void takeGun1Level(int value)
    {
        //gun1_lvl = value;
    }

    //level progress
    private void takeLevelProgress(float value)
    {
        //level_progress = value;
    }

    //health
    private void takePlayerHealth(int value)
    {
        playerHealth = value;
    }
    private void takePlayerMaxHealth(int value)
    {
        playerMaxHealth = value;
    }
    private void calculateHealthPercent()
    {
        //playerHealthPercent = (float)playerHealth / playerMaxHealth * 100;
        healthStr = "Health: " + playerHealth + " / " + playerMaxHealth;
        Health.text = healthStr;
    }
    private void updateHealth()
    {
        if ((old_playerHealth != playerHealth) && (old_playerMaxHealth != playerMaxHealth))
        {
            calculateHealthPercent();
        }
        else
        if (old_playerHealth != playerHealth)
        {
            calculateHealthPercent();
        }
        else
        if (old_playerMaxHealth != playerMaxHealth)
        {
            calculateHealthPercent();
        }
    }
}