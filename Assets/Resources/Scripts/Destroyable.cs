using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Player, Enemy
}

public class Destroyable : MonoBehaviour {

    [Header("Type")]
    public Type objectType = Type.Enemy;

    [Header("Health")]
    public int max_health = 100;    
    public GameObject die_Effect;

    [Header("Drop")]
    public bool onlyOneDrop = true;
    public List<GameObject> Drop;
    [Range(0f, 100f)]
    public List<float> Chance;

    protected int health;
    protected Transform thisTransform;

    public void Heal(int value)
    {        
        health += value;
        if (health > max_health)
            health = max_health;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitObject = col.gameObject;
        if ((objectType == Type.Enemy && hitObject.tag == "Projectile") ||
            (objectType == Type.Player && hitObject.tag == "EnemyProjectile"))
        {
            Projectile bullet = hitObject.GetComponent<Projectile>();
            health -= bullet.damage;
            bullet.checkDie();
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
}
