using System.Collections;
using System.Collections.Generic;
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

    private int playerUpgradeLvl = 0;

    private float inputX;
    private float inputY;
    private float deltaX = 0;
    private float deltaY = 0;

    private Gun playerGun_1;
    private Gun playerGun_2;
    private Vector3 camPos_min;
    private Vector3 camPos_max;    
    private AudioSource playerAudio;
    private GameObject companionLevel_1_cached;
    private GameObject companionLevel_2_cached;    

    void Awake()
    {
        Amination_OnStart();                
        Destroyable_OnStart();

        camPos_min = EventController.getCamPos_TL();
        camPos_max = EventController.getCamPos_BR();

        playerAudio = GetComponent<AudioSource>();
        playerGun_1 = thisTransform.GetChild(0).gameObject.GetComponent<Gun>();
        playerGun_2 = thisTransform.GetChild(1).gameObject.GetComponent<Gun>();
    }

	void Update ()
    {
        if (old_health != health)
        {
            EventController.addPlayerCurrentHealth(health - old_health);            
            old_health = health;            
        }
        if (old_maxHealth != max_health)
        {
            EventController.addPlayerMaxHealth(max_health - old_maxHealth);
            old_maxHealth = max_health;
        }

        if (health <= 0)
        {
            Die();
        }
        flyInput();
    }

    private void OnEnable()
    {
        EventController.Input_Horizontal += takeInput_H;
        EventController.Input_Vertical += takeInput_V;
        EventController.PlayerGotHit += takeHit;
        
        if (companionLevel_1 != null)
        {
            companionLevel_1_cached = Instantiate(companionLevel_1);
            companionLevel_1_cached.GetComponent<Playerghost>().player = thisTransform;            
            companionLevel_1_cached.SetActive(false);
        }
        if (companionLevel_2 != null)
        {
            companionLevel_2_cached = Instantiate(companionLevel_2);
            companionLevel_2_cached.GetComponent<Companion>().parentTransform = thisTransform;
            playerGun_1.Companion = companionLevel_2_cached;
            playerGun_2.Companion = companionLevel_2_cached;
            companionLevel_2_cached.SetActive(false);
        }

        eventContriller_setGunDamage();

        EventController.setGun1Firerate(playerGun_1.firerate);
        EventController.setGun2Firerate(playerGun_2.firerate);

        EventController.addPlayerCurrentHealth(health);
        EventController.addPlayerMaxHealth(max_health);
        EventController.addPlayerSpeed(speed);
    }

    private void OnDisable()
    {
        EventController.Input_Horizontal -= takeInput_H;
        EventController.Input_Vertical -= takeInput_V;
        EventController.PlayerGotHit -= takeHit;
    }

    private void takeInput_H(float value)
    {
        inputX = value;
        inputX *= 3;
    }

    private void takeInput_V(float value)
    {
        inputY = value;
        inputY *= 3;
    }

    private void flyInput()
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

        thisTransform.position += new Vector3(deltaX * speed, deltaY * speed, 0);
    }

    private void PlayerAnimation(float vertical)
    {
        if (frames.Length >= 3)
        {
            if (vertical < -0.01f)
            {
                changeFrame(2); //DownSprite
            }
            else
            if (vertical > 0.01f)
            {
                changeFrame(0); //UpSprite
            }
            else
            {
                changeFrame(1); //NormalSprite
            }
        }
    }

    private void takeHit(GameObject obj)
    {
        if (obj.tag == "EnemyProjectile")
        {
            Projectile bullet = obj.GetComponent<Projectile>();
            decreaseHealth(bullet.damage);
            bullet.checkDie();
        }
        else
        if (obj.tag == "Item")
        {
            playerTakeItem(obj.GetComponent<Item>());
            playSound(powerUpSound, volumePowUp);
            Destroy(obj);
        }
        else
        if (obj.tag == "Enemy")
        {
            decreaseHealth(35);
            obj.GetComponent<Destroyable>().decreaseHealth(35);
            playSound(enemyImpactSound, volumeEnemyImpact);
        }
    }

    private void playSound(AudioClip clip, float volume)
    {
        if (playerAudio != null)
        {
            if (volume > 0)
            {
                playerAudio.PlayOneShot(clip, volume);
            }
        }
    }

    private void playerTakeItem(Item obj)
    {
        switch (obj.item_type)
        {
            case ItemType.Heal:
                Heal((int)obj.value);
                break;

            case ItemType.DamageUP:
                item_DamageUp(obj);
                break;

            case ItemType.HealthUP:
                item_MaxHealthUp(obj);
                break;

            case ItemType.SpeedUp:
                item_SpeedUp(obj);
                break;

            case ItemType.FirerateUp:
                item_FirerateUp(obj);
                break;

            case ItemType.GunLevelUp:
                item_GunLvLUp(obj);
                break;

            case ItemType.Upgrade:
                item_Upgrade(obj);
                break;
        }
        EventController.addScore(obj.scorePoints);
    }

    private void item_MaxHealthUp(Item obj)
    {
        max_health += (int)obj.value;
        Heal((int)obj.value);
    }

    private void item_FirerateUp(Item obj)
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
            EventController.setGun1Firerate(playerGun_1.firerate);
        }
        if (playerGun_2.firerate > playerGun_2.firerateCap)
        {
            if (playerGun_2.firerate - obj.value > playerGun_2.firerateCap)
            {
                playerGun_2.firerate -= obj.value;
            }
            else
            {
                playerGun_2.firerate = playerGun_2.firerateCap;
            }
            EventController.setGun1Firerate(playerGun_2.firerate);
        }
    }

    private void item_Upgrade(Item obj)
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
                max_health += (int)obj.value;
                Heal((int)obj.value);
                break;
        }
        playerUpgradeLvl++;
    }

    private void item_GunLvLUp(Item obj)
    {
        if (playerGun_1.ProjectileLevels.Count - 1 > playerGun_1.currentProjectileLevel)
        {
            playerGun_1.currentProjectileLevel++;
            EventController.playerGun1LevelUP();
        }
        if (playerGun_2.ProjectileLevels.Count - 1 > playerGun_2.currentProjectileLevel)
        {
            playerGun_2.currentProjectileLevel++;
            EventController.playerGun2LevelUP();
        }
        eventContriller_setGunDamage();
    }    

    private void item_SpeedUp(Item obj)
    {
        Player player = thisTransform.GetComponent<Player>();
        if (player.speed < player.maxSpeed)
        {
            float speed_diff = player.maxSpeed - player.speed;
            if (speed_diff >= obj.value)
            {
                player.speed += obj.value;
                EventController.addPlayerSpeed(obj.value);
            }
            else
            {
                player.speed += speed_diff;
                EventController.addPlayerSpeed(speed_diff);
            }
        }
        else
        {
            max_health += (int)obj.value2;
            Heal((int)obj.value2);
        }
    }

    private void item_DamageUp(Item obj)
    {
        playerGun_1.damageUP += (int)obj.value;
        playerGun_2.damageUP += (int)obj.value2;
        eventContriller_setGunDamage();
    }

    private void eventContriller_setGunDamage()
    {
        EventController.setGun1Damage(playerGun_1.damageUP + playerGun_1.ProjectileLevels[playerGun_1.currentProjectileLevel].Projectile.GetComponent<Projectile>().damage);
        EventController.setGun2Damage(playerGun_2.damageUP + playerGun_2.ProjectileLevels[playerGun_2.currentProjectileLevel].Projectile.GetComponent<Projectile>().damage);
    }
}
