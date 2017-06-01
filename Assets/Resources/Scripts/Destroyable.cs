using System.Collections.Generic;
using UnityEngine;

public class Destroyable : AnimatedObject {

    [Header("Health")]
    public bool immortal = false;
    public int maxHealth = 100;    
    public GameObject dieEffect;
    
    [Header("Drop")]
    public bool onlyOneDrop = true;
    public List<GameObject> Drop;
    [Range(0f, 100f)]
    public List<float> Chance;

    [Header("Reward")]
    public int score = 20;

    protected int health;
    protected Transform thisTransform;

    protected int old_health;
    protected int old_maxHealth;

    private GameObject dieEffect_cached;
    private List<GameObject> Drop_cached = new List<GameObject>();

    public void Heal(int value)
    {        
        health += value;
        if (health > maxHealth)
            health = maxHealth;
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
        health = maxHealth;
        old_health = health;
        old_maxHealth = maxHealth;

        createDieEffect();
        createDrop();
    }

    protected void Die()
    {        
        if (dieEffect_cached != null)
        {
            dieEffect_cached.transform.position = thisTransform.position;
            dieEffect_cached.SetActive(true);
        }
        activateDrop();
        EventController.addScore(score);
        gameObject.SetActive(false);
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

    private void createDieEffect()
    {
        if (dieEffect != null)
        {
            dieEffect_cached = Instantiate(dieEffect, SceneObjectContainer.DieEffect.transform);
            dieEffect_cached.name = gameObject.name + "_dieEffect";
            dieEffect_cached.SetActive(false);
        }
    }

    private void createDrop()
    {
        for (int i = 0; i < Drop.Count && i < Chance.Count; i++)
        {
            float randomChance = Random.Range(0, 100);
            if (randomChance <= Chance[i])
            {
                GameObject temp = Instantiate(Drop[i], SceneObjectContainer.Items.transform);
                temp.SetActive(false);
                Drop_cached.Add(temp);
                if (onlyOneDrop)
                {
                    break;
                }
            }
        }
    }

    private void activateDrop()
    {
        if (Drop_cached.Count > 0)
        {
            foreach (GameObject drop in Drop_cached)
            {
                drop.transform.position = thisTransform.position;
                drop.SetActive(true);
            }
        }
    }
}
