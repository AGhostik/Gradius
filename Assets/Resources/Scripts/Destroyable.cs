using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : AnimatedObject {

    [Header("Health")]
    public bool immortal = false;
    public int max_health = 100;    
    public GameObject die_Effect;

    [Header("Sound")]
    public AudioClip someSound;
    [Range(0f, 5f)]
    public float volume = 1;

    [Header("Drop")]
    public bool onlyOneDrop = true;
    public List<GameObject> Drop;
    [Range(0f, 100f)]
    public List<float> Chance;
    
    protected int health;
    protected Transform thisTransform;

    protected int old_health;
    protected int old_maxHealth;

    private int player_upgrade_Lvl;
    private Gun player_gun1;
    private Gun player_gun2;
    private AudioSource player_audio;

    public void Heal(int value)
    {        
        health += value;
        if (health > max_health)
            health = max_health;
    }

    /// <summary>
    /// Put in Player.OnStart
    /// </summary>
    protected void Destroyable_OnStart()
    {
        thisTransform = transform;
        health = max_health;
        old_health = health;
        old_maxHealth = max_health;        
        if (thisTransform.tag == "Player")
        {
            player_upgrade_Lvl = 0;
            player_gun1 = thisTransform.GetChild(0).GetComponent<Gun>();
            player_gun2 = thisTransform.GetChild(1).GetComponent<Gun>();
            player_audio = GetComponent<AudioSource>();
        }
    }

    protected void Die()
    {        
        if (die_Effect != null)
        {
            Instantiate(die_Effect).transform.position = thisTransform.position;
        }
        for (int i = 0; i < Drop.Count; i++)
        {
            if (i + 1 > Chance.Count)
            {
                break;
            }

            float randomChance = Random.Range(0, 100);
            if (randomChance <= Chance[i])
            {
                Instantiate(Drop[i]).transform.position = thisTransform.position;
                if (onlyOneDrop)
                {
                    break;
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitObject = col.gameObject;
        if (thisTransform.tag == "Player")
        {
            if (hitObject.tag == "EnemyProjectile")
            {
                Projectile bullet = hitObject.GetComponent<Projectile>();
                decreaseHealth(bullet.damage);
                bullet.checkDie();                
            }
            else
            if (hitObject.tag == "Item")
            {
                playerTakeItem(hitObject.GetComponent<Item>());
                playSound();
                Destroy(hitObject);
            }
        } else
        if (hitObject.tag == "Projectile")
        {
            Projectile bullet = hitObject.GetComponent<Projectile>();
            decreaseHealth(bullet.damage);
            bullet.checkDie();
        }
    }

    private void playSound()
    {
        if (player_audio != null)
        {
            if (someSound != null && volume > 0)
            {
                player_audio.PlayOneShot(someSound, volume);
            }
        }
    }

    private void playerTakeItem(Item obj)
    {
        //здесь код становится похож на дерьмо
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
            player_gun1.upgradeGun(); //EventController upgradeGun внутри функции
            player_gun2.upgradeGun();
            EventController.setGun1Damage(player_gun1.damageUP + player_gun1.Projectiles[player_gun1.projectile_level].GetComponent<Projectile>().damage);
            EventController.setGun2Damage(player_gun2.damageUP + player_gun2.Projectiles[player_gun2.projectile_level].GetComponent<Projectile>().damage);
        }
        else
        {
            item_DamageUp(obj);
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

    private void decreaseHealth(int damage)
    {
        if (!immortal)
        {
            if (damage > health)
            {
                health = 0;
            }
            else
            {
                health -= damage;
            }
        }
    }
}
