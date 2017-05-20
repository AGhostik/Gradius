using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDraw : MonoBehaviour {

    public Texture progres_border;
    public Texture progres_line;

    private string damage1Str = "Gun1 Damage: ";
    private string damage2Str = "Gun2 Damage: ";
    private string scoresStr = "Score: ";
    private string healthStr = "Health: ";

    private int playerHealth;
    private int playerMaxHealth;
    private int old_playerHealth;
    private int old_playerMaxHealth;

    private float level_progress;
    private float playerHealthPercent;

    private void OnEnable()
    {
        EventController.UpdatePlayerHealth += takePlayerHealth;
        EventController.UpdatePlayerMaxHealth += takePlayerMaxHealth;
        EventController.UpdateLevelProgress += takeLevelProgress;
        EventController.UpdateScores += takeScores;
    }

    private void OnDisable()
    {
        EventController.UpdatePlayerHealth -= takePlayerHealth;
        EventController.UpdatePlayerMaxHealth -= takePlayerMaxHealth;
        EventController.UpdateLevelProgress -= takeLevelProgress;
        EventController.UpdateScores -= takeScores;
    }

    void Start () {
	}

    void Update () {
        updateHealth();
    }

    void OnGUI()
    {
        UIhelper drawer = new UIhelper(256, 144);
        
        drawer.texture(progres_border).DrawProgresbar(progres_line, 2, 2, level_progress);
        drawer.texture(progres_border).DrawProgresbar(progres_line, 35, 2, playerHealthPercent);
        drawer.label(scoresStr).Draw(2, 11, 5);
        drawer.label(healthStr).Draw(35, 11, 5);
        //drawer.label(damage1Str).Draw(2, 130, 5);
        //drawer.label(damage2Str).Draw(2, 137, 5);
    }

    private void takeScores(int value)
    {
        scoresStr = "Score: " + value;
    }

    private void takeLevelProgress(float value)
    {
        level_progress = value;
    }

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