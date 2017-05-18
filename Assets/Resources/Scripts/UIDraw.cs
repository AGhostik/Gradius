using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDraw : MonoBehaviour {

    public Texture progres_border;
    public Texture progres_line;

    private static int scores = 0;
    private static int player_health = 0;
    private static int player_maxHealth = 0;
    private static float level_progress_percentage;

    private int old_scores;
    private int old_health;
    private int old_maxHealth;
    private string scoresStr = "Score: ";
    private string healthStr = "Health: ";
    private float playerHealthPercent;

    // Use this for initialization
    void Start () {        
	}

    // Update is called once per frame
    void Update () {
        if (scores != old_scores)
        {
            scoresStr = "Score: " + scores;
            old_scores = scores;
        }

        playerHealthPercentCalculate();
    }

    void OnGUI()
    {
        UIhelper drawer = new UIhelper(256, 144);
        
        drawer.texture(progres_border).DrawProgresbar(progres_line, 2, 2, level_progress_percentage);
        drawer.texture(progres_border).DrawProgresbar(progres_line, 35, 2, playerHealthPercent);
        drawer.label(scoresStr).Draw(2, 11, 5);
        drawer.label(healthStr).Draw(35, 11, 5);
    }

    private void playerHealthPercentCalculate()
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
