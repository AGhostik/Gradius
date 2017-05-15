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
    public GameObject explosion;

    private int health;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        health = max_health;
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            Die();
        }

        if (objectType == Type.Player)
            Debug.Log("Health: " + health);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitObject = col.gameObject;
        if ((objectType == Type.Enemy && hitObject.tag == "Projectile") ||
            (objectType == Type.Player && hitObject.tag == "EnemyProjectile"))
        {
            Projectile bullet = hitObject.GetComponent<Projectile>();
            health -= bullet.damage;
            bullet.Die();
        }
    }

    private void Die()
    {
        if (explosion != null)
        {
            Instantiate(explosion).transform.position = thisTransform.position;
        }
        Destroy(gameObject);
    }
}
