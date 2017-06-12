using UnityEngine;

public class Player : Destroyable {

    [Header("Stats")]
    [Range(0f,5f)]
    public float speed = 1.2f;
    public float maxSpeed = 3;

    [Header("Upgrade Instantiate")]
    public GameObject companionLevel_1;
    public GameObject companionLevel_2;

    [Header("Sound")]
    public AudioClip powerUpSound;
    [Range(0f, 5f)]
    public float volumePowUp = 1;
    public AudioClip enemyImpactSound;
    [Range(0f, 5f)]
    public float volumeEnemyImpact = 1;

    private int playerUpgradeLvl;

    private float inputX;
    private float inputY;
    private float deltaX;
    private float deltaY;

    private Gun playerGun_1;
    private Gun playerGun_2;
    private Vector3 camPos_min;
    private Vector3 camPos_max;    
    private AudioSource playerAudio;
    private GameObject companionLevel_1_cached;
    private GameObject companionLevel_2_cached;

    private Rigidbody2D thisRigidbody;

    protected override void Awake()
    {
        base.Awake();

        thisRigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        playerGun_1 = thisTransform.GetChild(0).gameObject.GetComponent<Gun>();
        playerGun_2 = thisTransform.GetChild(1).gameObject.GetComponent<Gun>();

        thisTransform.parent = SceneObjectContainer.Player.transform;
        SceneObjectContainer.AddProjectileContainer(gameObject.tag);
    }
    
    private void OnEnable()
    {
        EventController.Input_Horizontal += TakeInput_H;
        EventController.Input_Vertical += TakeInput_V;
        EventController.PlayerGotHit += TakeHit;
        
        if (companionLevel_1 != null)
        {
            companionLevel_1_cached = Instantiate(companionLevel_1);
            companionLevel_1_cached.GetComponent<Playerghost>().player = thisTransform;
            companionLevel_1_cached.transform.parent = SceneObjectContainer.Player.transform;
            companionLevel_1_cached.SetActive(false);
        }
        if (companionLevel_2 != null)
        {
            companionLevel_2_cached = Instantiate(companionLevel_2);
            companionLevel_2_cached.GetComponent<Companion>().ParentTransform = thisTransform;
            companionLevel_2_cached.transform.parent = SceneObjectContainer.Player.transform;
            playerGun_1.Companion = companionLevel_2_cached;
            playerGun_2.Companion = companionLevel_2_cached;
            companionLevel_2_cached.SetActive(false);
        }        
    }

    protected void Start()
    {
        EventContriller_setGunDamage();

        EventController.SetGun1Firerate(playerGun_1.firerate);
        EventController.SetGun2Firerate(playerGun_2.firerate);

        EventController.SetPlayerCurrentHealth(health);
        EventController.SetPlayerMaxHealth(maxHealth);
        EventController.SetPlayerSpeed(speed);

        camPos_min = EventController.GetCamPos_TL();
        camPos_max = EventController.GetCamPos_BR();

        playerUpgradeLvl = 0;
        deltaX = 0;
        deltaY = 0;
    }

    void Update()
    {
        if (oldHealth != health)
        {
            EventController.SetPlayerCurrentHealth(health);
            oldHealth = health;
        }

        if (health <= 0)
        {
            companionLevel_1_cached.SetActive(false);
            companionLevel_2_cached.SetActive(false);
            Die();
        }

        FlyInput();
    }

    private void OnDisable()
    {
        EventController.Input_Horizontal -= TakeInput_H;
        EventController.Input_Vertical -= TakeInput_V;
        EventController.PlayerGotHit -= TakeHit;
    }

    private void TakeInput_H(float value)
    {
        inputX = value;
        inputX *= 3;
    }

    private void TakeInput_V(float value)
    {
        inputY = value;
        inputY *= 3;
    }

    private void FlyInput()
    {
        deltaX = 0;
        deltaY = 0;

        if (inputX < 0)
        {
            if (thisTransform.position.x > camPos_min.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        } else
        {
            if (thisTransform.position.x < camPos_max.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        }

        if (inputY < 0)
        {
            if (thisTransform.position.y > camPos_min.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }
        else
        {
            if (thisTransform.position.y < camPos_max.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }

        PlayerAnimation(deltaY);

        thisRigidbody.position += new Vector2(deltaX * speed, deltaY * speed);
    }

    private void PlayerAnimation(float vertical)
    {
        if (frames.Length >= 3)
        {
            if (vertical < -0.01f)
            {
                ChangeFrame(2); //DownSprite
            }
            else
            if (vertical > 0.01f)
            {
                ChangeFrame(0); //UpSprite
            }
            else
            {
                ChangeFrame(1); //NormalSprite
            }
        }
    }

    private void TakeHit(GameObject obj)
    {
        if (obj.tag == "EnemyProjectile")
        {
            Projectile bullet = obj.GetComponent<Projectile>();
            DecreaseHealth(bullet.Damage);
            bullet.CheckDie();
            EventController.AddScore(-bullet.Damage);
        }
        else
        if (obj.tag == "Item")
        {
            PlayerTakeItem(obj.GetComponent<Item>());
            PlaySound(powerUpSound, volumePowUp);
            obj.SetActive(false);
        }
        else
        if (obj.tag == "Enemy")
        {
            DecreaseHealth(35);
            obj.GetComponent<Destroyable>().DecreaseHealth(35);
            PlaySound(enemyImpactSound, volumeEnemyImpact);
        }
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (playerAudio != null)
        {
            if (volume > 0)
            {
                playerAudio.PlayOneShot(clip, volume);
            }
        }
    }

    private void PlayerTakeItem(Item obj)
    {
        switch (obj.item_type)
        {
            case ItemType.Heal:
                Heal((int)obj.value);
                break;

            case ItemType.DamageUP:
                Item_DamageUp(obj);
                break;

            case ItemType.HealthUP:
                Item_MaxHealthUp(obj);
                break;

            case ItemType.SpeedUp:
                Item_SpeedUp(obj);
                break;

            case ItemType.FirerateUp:
                Item_FirerateUp(obj);
                break;

            case ItemType.GunLevelUp:
                Item_GunLvLUp(obj);
                break;

            case ItemType.Upgrade:
                Item_Upgrade(obj);
                break;
        }
        EventController.AddScore(obj.scorePoints);
    }

    private void Item_MaxHealthUp(Item obj)
    {
        maxHealth += (int)obj.value;
        Heal((int)obj.value);
        EventController.SetPlayerMaxHealth(maxHealth);
    }

    private void Item_FirerateUp(Item obj)
    {
        if (playerGun_1.firerate > playerGun_1.firerateCap)
        {
            if (playerGun_1.firerate - obj.value > playerGun_1.firerateCap)
            {
                playerGun_1.firerate -= obj.value;
            }
            else
            {
                playerGun_1.firerate = playerGun_1.firerateCap;
            }
            EventController.SetGun1Firerate(playerGun_1.firerate);
        }
        if (playerGun_2.firerate > playerGun_2.firerateCap)
        {
            if (playerGun_2.firerate - obj.value2 > playerGun_2.firerateCap)
            {
                playerGun_2.firerate -= obj.value2;
            }
            else
            {
                playerGun_2.firerate = playerGun_2.firerateCap;
            }
            EventController.SetGun2Firerate(playerGun_2.firerate);
        }
    }

    private void Item_Upgrade(Item obj)
    {
        switch (playerUpgradeLvl)
        {
            case 0:
                if (companionLevel_1 != null)
                {
                    companionLevel_1_cached.SetActive(true);                    
                }
                break;
            case 1:
                if (companionLevel_2 != null)
                {
                    companionLevel_2_cached.SetActive(true);
                }
                break;
            default:
                maxHealth += (int)obj.value;
                Heal((int)obj.value);
                break;
        }
        playerUpgradeLvl++;
    }

    private void Item_GunLvLUp(Item obj)
    {
        if (playerGun_1.ProjectileLevels.Count - 1 > playerGun_1.currentProjectileLevel)
        {
            playerGun_1.currentProjectileLevel++;
            EventController.PlayerGun1LevelUP();
        }
        if (playerGun_2.ProjectileLevels.Count - 1 > playerGun_2.currentProjectileLevel)
        {
            playerGun_2.currentProjectileLevel++;
            EventController.PlayerGun2LevelUP();
        }
        EventContriller_setGunDamage();
    }    

    private void Item_SpeedUp(Item obj)
    {
        Player player = thisTransform.GetComponent<Player>();
        if (player.speed < player.maxSpeed)
        {
            float speed_diff = player.maxSpeed - player.speed;
            if (speed_diff >= obj.value)
            {
                player.speed += obj.value;                
            }
            else
            {
                player.speed += speed_diff;
            }
            EventController.SetPlayerSpeed(player.speed);
        }
        else
        {
            maxHealth += (int)obj.value2;
            Heal((int)obj.value2);
        }
    }

    private void Item_DamageUp(Item obj)
    {
        playerGun_1.damageUP += (int)obj.value;
        playerGun_2.damageUP += (int)obj.value2;
        EventContriller_setGunDamage();
    }

    private void EventContriller_setGunDamage()
    {
        EventController.SetGun1Damage(playerGun_1.damageUP + playerGun_1.ProjectileLevels[playerGun_1.currentProjectileLevel].Projectile.GetComponent<Projectile>().Damage);
        EventController.SetGun2Damage(playerGun_2.damageUP + playerGun_2.ProjectileLevels[playerGun_2.currentProjectileLevel].Projectile.GetComponent<Projectile>().Damage);
    }
}
