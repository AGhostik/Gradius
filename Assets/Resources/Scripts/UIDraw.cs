using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class UIDraw : MonoBehaviour {

    #region inspector

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

    #endregion

    private int gun1_lvl;
    private int gun2_lvl;

    private int playerHealth;
    private int playerMaxHealth;    

    private bool increaseRed;
    private float healthRedColor_min;
    private float healthRedColor_max;
    private float healthRedColor;
    private float playerHealthPercent;

    #region strings

    private string fpsStr;
    private string healthStr;
    private string speedStr;
    private string scoresStr;

    private string gun1DmgStr;
    private string gun2DmgStr;

    private string gun1FrStr;
    private string gun2FrStr;

    #endregion

    //private float level_progress;

    private void Awake()
    {
        gun1_lvl = 0;
        gun2_lvl = 0;
        SetScores(0);
    }

    private void OnEnable()
    {
        EventController.UpdatePlayerHealth += SetPlayerHealth;
        EventController.UpdatePlayerMaxHealth += SetPlayerMaxHealth;        
        EventController.UpdatePlayerGun1Damage += SetGun1Damage;
        EventController.UpdatePlayerGun2Damage += SetGun2Damage;
        EventController.UpdatePlayerGun1Level += SetGun1Level;
        EventController.UpdatePlayerGun2Level += SetGun2Level;
        EventController.UpdatePlayerGun1Firerate += SetGun1Firerate;
        EventController.UpdatePlayerGun2Firerate += SetGun2Firerate;
        EventController.UpdatePlayerSpeed += SetSpeed;
        EventController.UpdateScores += SetScores;

        //EventController.UpdateLevelProgress += SetLevelProgress;
    }

    private void OnDisable()
    {
        EventController.UpdatePlayerHealth -= SetPlayerHealth;
        EventController.UpdatePlayerMaxHealth -= SetPlayerMaxHealth;        
        EventController.UpdatePlayerGun1Damage -= SetGun1Damage;
        EventController.UpdatePlayerGun2Damage -= SetGun2Damage;
        EventController.UpdatePlayerGun1Level -= SetGun1Level;
        EventController.UpdatePlayerGun2Level -= SetGun2Level;
        EventController.UpdatePlayerGun1Firerate -= SetGun1Firerate;
        EventController.UpdatePlayerGun2Firerate -= SetGun2Firerate;
        EventController.UpdatePlayerSpeed -= SetSpeed;
        EventController.UpdateScores -= SetScores;

        //EventController.UpdateLevelProgress -= SetLevelProgress;
    }

    void Start ()
    {
        healthRedColor_max = Health.color.r;
        healthRedColor = healthRedColor_max;
        healthRedColor_min = 0.5f;
        increaseRed = false;        
    }

    void Update () {
        HealthStrColorChange();
    }

    private void LateUpdate()
    {
        fpsStr = string.Format("{0:00.0} FPS", (1.0f / Time.deltaTime));
        FPS.text = fpsStr;
    }

    #region event subscribers
    //scores
    private void SetScores(int value)
    {
        scoresStr = string.Format("Scores:\n{0}", value);
        Scores.text = scoresStr;
    }

    //speed
    private void SetSpeed(float value)
    {
        speedStr = string.Format("{0:0.00} speed",value);
        Speed.text = speedStr;
    }

    //health
    private void SetPlayerHealth(int value)
    {
        playerHealth = value;
        RecheckHealth();
    }
    private void SetPlayerMaxHealth(int value)
    {
        playerMaxHealth = value;
        RecheckHealth();
    }
    private void RecheckHealth()
    {
        playerHealthPercent = ((float)playerHealth / playerMaxHealth * 100);
        healthStr = string.Format("{0} | {1} hp", playerMaxHealth, playerHealth);
        Health.text = healthStr;
    }

    //gun damage
    private void SetGun1Damage(int value)
    {
        gun1DmgStr = string.Format("{0} dmg", value);
        DamageG1.text = gun1DmgStr;
    }
    private void SetGun2Damage(int value)
    {
        gun2DmgStr = string.Format("{0} dmg", value);
        DamageG2.text = gun2DmgStr;
    }

    //gun firerate
    private void SetGun1Firerate(float value)
    {
        gun1FrStr = string.Format("{0:0.00} ps", (1.0f / value));
        FirerateG1.text = gun1FrStr;
    }
    private void SetGun2Firerate(float value)
    {
        gun2FrStr = string.Format("{0:0.00} ps", (1.0f / value));
        FirerateG2.text = gun2FrStr;
    }

    //gun level
    private void SetGun1Level()
    {
        gun1_lvl++;
        ImageG1.sprite = LevelsG1[gun1_lvl];
        ImageG1.rectTransform.sizeDelta = new Vector2(LevelsG1[gun1_lvl].rect.width * 2.2f, LevelsG1[gun1_lvl].rect.height * 2.2f);
    }
    private void SetGun2Level()
    {
        gun2_lvl++;
        ImageG2.sprite = LevelsG2[gun2_lvl];
        ImageG2.rectTransform.sizeDelta = new Vector2(LevelsG2[gun2_lvl].rect.width * 2.8f, LevelsG2[gun2_lvl].rect.height * 2.8f);
    }

    //level progress
    //private void SetLevelProgress(float value)
    //{
    //    level_progress = value;
    //}
    #endregion

    private void HealthStrColorChange()
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