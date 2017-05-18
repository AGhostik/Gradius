using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDraw : MonoBehaviour {

    public Texture progres_border;
    public Texture progres_line;

    private static int scores = 0;
    private static int damage1;
    private static int damage2;
    private static int player_health = 0;
    private static int player_maxHealth = 0;
    private static float level_progress_percentage;

    private int old_scores;
    private int old_health;
    private int old_maxHealth;
    private int old_damage1;
    private int old_damage2;
    private string damage1Str = "Gun1 Damage: ";
    private string damage2Str = "Gun2 Damage: ";
    private string scoresStr = "Score: ";
    private string healthStr = "Health: ";
    private float playerHealthPercent;

    // Use this for initialization
    void Start () {
        damage1 = 5; //убрать в будущем!
        damage2 = 65;
	}

    // Update is called once per frame
    void Update () {
        scoresUpdate();
        damageUpdate();
        playerHealthUpdate();
    }

    void OnGUI()
    {
        UIhelper drawer = new UIhelper(256, 144);
        
        drawer.texture(progres_border).DrawProgresbar(progres_line, 2, 2, level_progress_percentage);
        drawer.texture(progres_border).DrawProgresbar(progres_line, 35, 2, playerHealthPercent);
        drawer.label(scoresStr).Draw(2, 11, 5);
        drawer.label(healthStr).Draw(35, 11, 5);
        drawer.label(damage1Str).Draw(2, 130, 5);
        drawer.label(damage2Str).Draw(2, 137, 5);
    }

    private void damageUpdate()
    {
        if (damage1 != old_damage1)
        {
            damage1Str = "Gun1 Damage: " + damage1;
            old_damage1 = damage1;
        }
        if (damage2 != old_damage2)
        {
            damage2Str = "Gun2 Damage: " + damage2;
            old_damage2 = damage2;
        }
    }

    private void scoresUpdate()
    {
        if (scores != old_scores)
        {
            scoresStr = "Score: " + scores;
            old_scores = scores;
        }
    }

    private void playerHealthUpdate()
    {
        if (old_health != player_health)
        {
            old_health = player_health;
            if (player_maxHealth != 0)
            {
                playerHealthPercent = (float)player_health / player_maxHealth * 100;
            }
            healthStr = "Health: " + player_health;
        }
        if (old_maxHealth != player_maxHealth)
        {
            old_maxHealth = player_maxHealth;
            if (player_maxHealth != 0)
            {
                playerHealthPercent = (float)player_health / player_maxHealth * 100;
            }
        }
    }

    public static void SetGun1Damage(int value) //Player
    {
        damage1 += value;
    }
    public static void SetGun2Damage(int value) //Player
    {
        damage2 += value;
    }

    public static void SetLevelProgress(float percent) //MoveCamera
    {
        level_progress_percentage = percent;
    }

    public static void SetPlayerHealth(int value) //Player
    {
        player_health = value;
    }

    public static void SetPlayerMaxHealth(int value) //Player
    {
        player_maxHealth = value;
    }

    public static void EnemyDied(int score_points = 0) //Enemy
    {
        scores += score_points;
    }
}
