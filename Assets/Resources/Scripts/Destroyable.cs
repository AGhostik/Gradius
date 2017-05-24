using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : AnimatedObject {

    [Header("Health")]
    public bool immortal = false;
    public int max_health = 100;    
    public GameObject die_Effect;
    
    [Header("Drop")]
    public bool onlyOneDrop = true;
    public List<GameObject> Drop;
    [Range(0f, 100f)]
    public List<float> Chance;
    
    protected int health;
    protected Transform thisTransform;

    protected int old_health;
    protected int old_maxHealth;    

    public void Heal(int value)
    {        
        health += value;
        if (health > max_health)
            health = max_health;
    }

    public void decreaseHealth(int damage)
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

    /// <summary>
    /// Put in Player.OnStart
    /// </summary>
    protected void Destroyable_OnStart()
    {
        thisTransform = transform;
        health = max_health;
        old_health = health;
        old_maxHealth = max_health;        
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
            switch (hitObject.tag)
            {
                case "EnemyProjectile":
                case "Item":
                case "Enemy":                    
                    EventController.playerHitAdd(hitObject);
                    break;
            }
        }
        else
        {
            if (hitObject.tag == "Projectile")
            {
                Projectile bullet = hitObject.GetComponent<Projectile>();
                decreaseHealth(bullet.damage);
                bullet.checkDie();
            }
        }
    }  
}
