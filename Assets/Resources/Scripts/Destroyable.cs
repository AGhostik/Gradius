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
    public int Score = 20;

    protected int health;
    protected Transform thisTransform;

    protected int oldHealth;
    protected int oldMaxHealth;
    
    private GameObject dieEffect_cached;
    private List<GameObject> drop_cached = new List<GameObject>();
    
    protected override void Awake()
    {
        base.Awake();

        thisTransform = transform;
        health = maxHealth;
        oldHealth = health;
        oldMaxHealth = maxHealth;

        CreateDieEffect();
        CreateDrop();
    }

    public void Heal(int value)
    {
        health += value;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void DecreaseHealth(int damage)
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

    protected void Die()
    {        
        if (dieEffect_cached != null)
        {
            dieEffect_cached.transform.position = thisTransform.position;
            dieEffect_cached.SetActive(true);
        }
        ActivateDrop();
        EventController.AddScore(Score);
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
                    EventController.PlayerHitAdd(hitObject);
                    break;
            }
        }
        else
        {
            if (hitObject.tag == "Projectile")
            {
                Projectile bullet = hitObject.GetComponent<Projectile>();
                DecreaseHealth(bullet.Damage);
                bullet.CheckDie();
            }
        }
    }

    private void CreateDieEffect()
    {
        if (dieEffect != null)
        {
            dieEffect_cached = Instantiate(dieEffect, SceneObjectContainer.DieEffect.transform);
            dieEffect_cached.name = gameObject.name + "_dieEffect";
            dieEffect_cached.SetActive(false);
        }
    }

    private void CreateDrop()
    {
        for (int i = 0; i < Drop.Count && i < Chance.Count; i++)
        {
            float randomChance = Random.Range(0, 100);
            if (randomChance <= Chance[i])
            {
                GameObject temp = Instantiate(Drop[i], SceneObjectContainer.Items.transform);
                temp.SetActive(false);
                drop_cached.Add(temp);
                if (onlyOneDrop)
                {
                    break;
                }
            }
        }
    }

    private void ActivateDrop()
    {
        if (drop_cached.Count > 0)
        {
            foreach (GameObject drop in drop_cached)
            {
                drop.transform.position = thisTransform.position;
                drop.SetActive(true);
            }
        }
    }
}
