using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDraw : MonoBehaviour {

    public Texture progres_border;
    public Texture progres_line;

    public Texture speedIcon;
    public List<Texture> gun1Icon;
    public Texture gun2Icon;
    public Texture upgrade1Icon;
    public Texture upgrade2Icon;

    private string gun_damage1Str = string.Empty;
    private string gun_damage2Str = string.Empty;   
    private string player_speedStr = string.Empty;
    private string scoresStr = "Score: ";
    private string healthStr = "Health: ";

    private int gun1_lvl = 0;
   // private int upgrade_lvl = 0; 
    private int gun1_dmg;
    private int gun2_dmg;
    private float gun1_fr;
    private float gun2_fr;
    private int playerHealth;
    private int playerMaxHealth;
    private int old_playerHealth = 0;
    private int old_playerMaxHealth = 0;

    private float level_progress;
    private float playerHealthPercent;

    private UIhelper drawer;

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
        drawer = new UIhelper(256, 144);
    }

    void Update () {
        updateHealth();
    }

    void OnGUI()
    {
        drawer.texture(progres_border).DrawProgresbar(progres_line, 2, 2, level_progress);
        drawer.texture(progres_border).DrawProgresbar(progres_line, 35, 2, playerHealthPercent);
        drawer.label(scoresStr).Draw(2, 11, 5);
        drawer.label(healthStr).Draw(35, 11, 5);

        /*
        drawer.texture(speedIcon).DrawAndResize(2, 109, 14, 7);
        drawer.texture(gun1Icon[gun1_lvl]).DrawAndResize(5, 116, 7, 4);
        drawer.texture(gun2Icon).DrawAndResize(5, 123, 7, 4);
        drawer.texture(upgrade1Icon).DrawAndResize(2, 130, 14, 7);
        drawer.texture(upgrade2Icon).DrawAndResize(2, 137, 14, 7);
        */
        drawer.label(player_speedStr).Draw(2, 109, 5);
        drawer.label(gun_damage1Str).Draw(2, 116, 5);
        drawer.label(gun_damage2Str).Draw(2, 123, 5);
    }

    //scores
    private void takeScores(int value)
    {
        scoresStr = "Score: " + value;
    }

    //speed
    private void takeSpeed(float value)
    {
        player_speedStr = "Speed: " + value.ToString();
    }

    //gun damage
    private void takeGun1Damage(int value)
    {
        gun1_dmg = value;
        gun_damage1Str = "Dmg_1: " + gun1_dmg.ToString() + " / " + gun1_fr.ToString("0.00") + " per second";
    }
    private void takeGun2Damage(int value)
    {
        gun2_dmg = value;
        gun_damage2Str = "Dmg_2: " + gun2_dmg.ToString() + " / " + gun2_fr.ToString("0.00") + " per second";
    }

    //gun firerate
    private void takeGun1Firerate(float value)
    {
        gun1_fr = value;
        gun_damage1Str = "Dmg_1: " + gun1_dmg.ToString() + " / " + gun1_fr.ToString("0.00") + " per second";
    }
    private void takeGun2Firerate(float value)
    {
        gun2_fr = value;
        gun_damage2Str = "Dmg_2: " + gun2_dmg.ToString() + " / " + gun2_fr.ToString("0.00") + " per second";
    }

    //gun level
    private void takeGun1Level(int value)
    {
        gun1_lvl = value;
    }

    //level progress
    private void takeLevelProgress(float value)
    {
        level_progress = value;
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
        playerHealthPercent = (float)playerHealth / playerMaxHealth * 100;
        healthStr = "Health: " + playerHealth;
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