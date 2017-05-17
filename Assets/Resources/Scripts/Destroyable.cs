using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Player, Enemy
}

public class Destroyable : MonoBehaviour {

    [Header("Health")]
    public int max_health = 100;

    public Type objectType = Type.Enemy;
    public GameObject die_Effect;

    protected int health;
    protected Transform thisTransform;

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
        Destroy(gameObject);
    }
}
