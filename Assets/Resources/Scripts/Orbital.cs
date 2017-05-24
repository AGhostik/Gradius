using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital : AnimatedObject
{

    public GameObject parent;
    public float rotationSpeed = 90;

    private Transform thisTransform;
    private Transform parentTransform;
    private Vector3 rotationMask;

    // Use this for initialization
    void Start()
    {
        Amination_OnStart();
        thisTransform = transform;
        parentTransform = parent.transform;
        rotationMask = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        thisTransform.RotateAround(parentTransform.position, rotationMask, rotationSpeed * Time.deltaTime);
        timerAnimation();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitObject = col.gameObject;
        if (hitObject.tag == "EnemyProjectile")
        {
            Projectile bullet = hitObject.GetComponent<Projectile>();
            bullet.mute_hitEffect = true;
            bullet.checkDie();
        }
    }
    
}
