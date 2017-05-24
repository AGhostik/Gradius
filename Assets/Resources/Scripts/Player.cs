using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destroyable {

    [Header("Stats")]
    [Range(0f,5f)]
    public float speed = 1.2f;
    public float maxSpeed = 3;

    [Header("Sound")]
    public AudioClip powerUpSound;
    [Range(0f, 5f)]
    public float volumePowUp = 1;

    public AudioClip enemyImpactSound;
    [Range(0f, 5f)]
    public float volumeEnemyImpact = 1;

    private float inputX;
    private float inputY;
    private float deltaX = 0;
    private float deltaY = 0;
    private Vector3 pos_min;
    private Vector3 pos_max;
    private int player_upgrade_Lvl = 0;
    private AudioSource player_audio;
    private Gun player_gun1;
    private Gun player_gun2;

    void Start()
    {
        Amination_OnStart();                
        Destroyable_OnStart();

        player_audio = GetComponent<AudioSource>();
        player_gun1 = thisTransform.GetChild(0).gameObject.GetComponent<Gun>();
        player_gun2 = thisTransform.GetChild(1).gameObject.GetComponent<Gun>();

        EventController.setGun1Damage(player_gun1.damageUP + player_gun1.Projectiles[player_gun1.projectile_level].GetComponent<Projectile>().damage);
        EventController.setGun1Firerate(player_gun1.firerate);

        EventController.setGun2Damage(player_gun2.damageUP + player_gun2.Projectiles[player_gun2.projectile_level].GetComponent<Projectile>().damage);
        EventController.setGun2Firerate(player_gun2.firerate);

        EventController.addPlayerCurrentHealth(health);
        EventController.addPlayerMaxHealth(max_health);
        EventController.addPlayerSpeed(speed);
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
        EventController.UpdateCameraWorldPoint_TL += takeCamWP_TL;
        EventController.UpdateCameraWorldPoint_BR += takeCamWP_BR;
        EventController.PlayerGotHit += takeHit;
    }

    private void OnDisable()
    {
        EventController.Input_Horizontal -= takeInput_H;
        EventController.Input_Vertical -= takeInput_V;
        EventController.UpdateCameraWorldPoint_TL -= takeCamWP_TL;
        EventController.UpdateCameraWorldPoint_BR -= takeCamWP_BR;
        EventController.PlayerGotHit -= takeHit;
    }

    private void takeCamWP_TL(Vector3 vector)
    {
        pos_min = vector;
    }

    private void takeCamWP_BR(Vector3 vector)
    {
        pos_max = vector;
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
            if (thisTransform.position.x > pos_min.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        } else
        {
            if (thisTransform.position.x < pos_max.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        }

        if (inputY < 0)
        {
            if (thisTransform.position.y > pos_min.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }
        else
        {
            if (thisTransform.position.y < pos_max.y)
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
        if (player_audio != null)
        {
            if (volume > 0)
            {
                player_audio.PlayOneShot(clip, volume);
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

    private void instantiateOrbital(GameObject obj, GameObject parent, Vector3 difference)
    {
        GameObject orbit = Instantiate(obj);
        orbit.GetComponent<Orbital>().parent = parent;
        orbit.transform.position = transform.position - difference;
        orbit.transform.SetParent(parent.transform);
    }

    private void item_MaxHealthUp(Item obj)
    {
        max_health += (int)obj.value;
        Heal((int)obj.value);
    }

    private void item_FirerateUp(Item obj)
    {
        if (player_gun1.canFirerateUp)
        {
            player_gun1.firerateUp(obj.value); //EventController firerate внутри функции
        }

        if (player_gun2.canFirerateUp)
        {
            player_gun2.firerateUp(obj.value2);//EventController firerate внутри функции
        }
    }

    private void item_Upgrade(Item obj)
    {
        if (player_upgrade_Lvl == 0)
        {
            GameObject ghost_parent = new GameObject();
            ghost_parent.name = "PlayerGhost";
            ghost_parent.transform.position = transform.position;
            ghost_parent.AddComponent<Playerghost>().player = transform;

            instantiateOrbital(obj.object2, ghost_parent, new Vector3(0, 0.5f, 0));
            instantiateOrbital(obj.object2, ghost_parent, new Vector3(0, -0.5f, 0));
            instantiateOrbital(obj.object2, ghost_parent, new Vector3(0.5f, 0, 0));
            instantiateOrbital(obj.object2, ghost_parent, new Vector3(-0.5f, 0, 0));
        }
        else
        if (player_upgrade_Lvl == 1)
        {
            GameObject option = Instantiate(obj.object1);
            option.GetComponent<Companion>().parent = gameObject;
            player_gun1.Companions.Add(option);
            player_gun2.Companions.Add(option);
        }
        else
        {
            max_health += (int)obj.value;
            Heal((int)obj.value);
        }
        player_upgrade_Lvl++;
    }

    private void item_GunLvLUp(Item obj)
    {
        if (player_gun1.canUpgrade || player_gun2.canUpgrade)
        {
            if (player_gun1.canUpgrade && player_gun1.Projectiles[player_gun1.projectile_level] != null)
            {
                if (player_gun1.projectile_level < player_gun1.Projectiles.Count - 1)
                {
                    player_gun1.projectile_level++;
                    EventController.playerGun1LevelUP();

                    if (player_gun1.projectile_level == player_gun1.Projectiles.Count - 1)
                    {
                        player_gun1.canUpgrade = false;
                    }
                }
                EventController.setGun1Damage(player_gun1.damageUP + player_gun1.Projectiles[player_gun1.projectile_level].GetComponent<Projectile>().damage);
            }
            if (player_gun2.canUpgrade && player_gun2.Projectiles[player_gun2.projectile_level] != null)
            {
                if (player_gun2.projectile_level < player_gun2.Projectiles.Count - 1)
                {
                    player_gun2.projectile_level++;
                    EventController.playerGun2LevelUP();

                    if (player_gun2.projectile_level == player_gun2.Projectiles.Count - 1)
                    {
                        player_gun2.canUpgrade = false;
                    }
                }
                EventController.setGun2Damage(player_gun2.damageUP + player_gun2.Projectiles[player_gun2.projectile_level].GetComponent<Projectile>().damage);
            }
        }        
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
        player_gun1.damageUP += (int)obj.value;
        player_gun2.damageUP += (int)obj.value2;
        EventController.setGun1Damage(player_gun1.damageUP + player_gun1.Projectiles[player_gun1.projectile_level].GetComponent<Projectile>().damage);
        EventController.setGun2Damage(player_gun2.damageUP + player_gun2.Projectiles[player_gun2.projectile_level].GetComponent<Projectile>().damage);
    }
}
