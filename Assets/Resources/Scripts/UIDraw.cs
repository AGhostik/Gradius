using UnityEngine;
using UnityEngine.UI;

public class UIDraw : MonoBehaviour {

    [Header("Main")]
    public Text FPS;
    public Text Scores;

    [Header("VicViper")]
    public Text Speed;
    public Text Health;

    [Header("Gun 1")]
    public Image ImageG1;
    public Text DamageG1;
    public Text FirerateG1;
    public Sprite[] LevelsG1;

    [Header("Gun 2")]
    public Image ImageG2;
    public Text DamageG2;
    public Text FirerateG2;
    public Sprite[] LevelsG2;

    private int gun1_lvl = 0;
    private int gun2_lvl = 0;
    //private int upgrade_lvl = 0; 
    private int gun1_dmg;
    private int gun2_dmg;
    private float gun1_fr;
    private float gun2_fr;
    private int playerHealth;
    private int playerMaxHealth;    
    private int old_playerHealth = 0;
    private int old_playerMaxHealth = 0;

    private bool increaseRed;
    private float healthRedColor_min;
    private float healthRedColor_max;
    private float healthRedColor;
    private float playerHealthPercent;

    private float fps;
    private string fpsStr;

    //private float level_progress;

    private void OnEnable()
    {
        EventController.UpdatePlayerHealth += takePlayerHealth;
        EventController.UpdatePlayerMaxHealth += takePlayerMaxHealth;        
        EventController.UpdatePlayerGun1Damage += takeGun1Damage;
        EventController.UpdatePlayerGun2Damage += takeGun2Damage;
        EventController.UpdatePlayerGun1Level += takeGun1Level;
        EventController.UpdatePlayerGun2Level += takeGun2Level;
        EventController.UpdatePlayerGun1Firerate += takeGun1Firerate;
        EventController.UpdatePlayerGun2Firerate += takeGun2Firerate;
        EventController.UpdatePlayerSpeed += takeSpeed;
        EventController.UpdateScores += takeScores;

        //EventController.UpdateLevelProgress += takeLevelProgress;
    }

    private void OnDisable()
    {
        EventController.UpdatePlayerHealth -= takePlayerHealth;
        EventController.UpdatePlayerMaxHealth -= takePlayerMaxHealth;        
        EventController.UpdatePlayerGun1Damage -= takeGun1Damage;
        EventController.UpdatePlayerGun2Damage -= takeGun2Damage;
        EventController.UpdatePlayerGun1Level -= takeGun1Level;
        EventController.UpdatePlayerGun2Level -= takeGun2Level;
        EventController.UpdatePlayerGun1Firerate -= takeGun1Firerate;
        EventController.UpdatePlayerGun2Firerate -= takeGun2Firerate;
        EventController.UpdatePlayerSpeed -= takeSpeed;
        EventController.UpdateScores -= takeScores;

        //EventController.UpdateLevelProgress -= takeLevelProgress;
    }

    void Start ()
    {
        healthRedColor_max = Health.color.r;
        healthRedColor = healthRedColor_max;
        healthRedColor_min = 0.5f;
        increaseRed = false;        
    }

    void Update () {
        updateHealth();

        healthStrColorChange();
        
        fps = 1.0f / Time.deltaTime;
        fpsStr = fps.ToString("0.") + " FPS";
        FPS.text = fpsStr;
    }

    //scores
    private void takeScores(int value)
    {
        Scores.text = "Scores:\n" + value;
    }

    //speed
    private void takeSpeed(float value)
    {
        Speed.text = value.ToString() + " speed";
    }

    //gun damage
    private void takeGun1Damage(int value)
    {
        DamageG1.text = value.ToString() + " dmg";
    }
    private void takeGun2Damage(int value)
    {
        DamageG2.text = value.ToString() + " dmg";
    }

    //gun firerate
    private void takeGun1Firerate(float value)
    {
        FirerateG1.text = (1.0f / value).ToString("0.00") + " ps";
    }
    private void takeGun2Firerate(float value)
    {
        FirerateG2.text = (1.0f / value).ToString("0.00") + " ps";
    }

    //gun level
    private void takeGun1Level(int value)
    {
        gun1_lvl = value;
        ImageG1.sprite = LevelsG1[gun1_lvl];
        ImageG1.rectTransform.sizeDelta = new Vector2(LevelsG1[gun1_lvl].rect.width * 2.2f, LevelsG1[gun1_lvl].rect.height * 2.2f);
    }
    private void takeGun2Level(int value)
    {
        gun2_lvl = value;
        ImageG2.sprite = LevelsG2[gun2_lvl];
        ImageG2.rectTransform.sizeDelta = new Vector2(LevelsG2[gun2_lvl].rect.width * 2.8f, LevelsG2[gun2_lvl].rect.height * 2.8f);
    }

    //level progress
    /*
    private void takeLevelProgress(float value)
    {
        level_progress = value;
    }
    */

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
        playerHealthPercent = ((float)playerHealth / playerMaxHealth * 100);
        Health.text = playerMaxHealth + " / " + playerHealth + " hp";
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

    private void healthStrColorChange()
    {
        if (playerHealthPercent <= 30 && playerHealthPercent > 0)
        {
            Health.color = new Color(healthRedColor, 0, 0, 1);

            if (increaseRed && healthRedColor < healthRedColor_max)
            {
                healthRedColor += Time.deltaTime / 2;
            }
            else
            {
                increaseRed = false;
            }

            if (!increaseRed && healthRedColor > healthRedColor_min)
            {
                healthRedColor -= Time.deltaTime / 2;
            }
            else
            {
                increaseRed = true;
            }
        }
        else
        {
            Health.color = new Color(healthRedColor_max, 0, 0, 1);
        }
    }
}