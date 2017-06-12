using UnityEngine;

public class Orbital : AnimatedObject
{
    public float rotationSpeed = 90;

    private Transform thisTransform;
    private Transform parentTransform;
    private Vector3 rotationMask;
    
    protected override void Awake()
    {
        base.Awake();
        thisTransform = transform;
        parentTransform = transform.parent.transform;
        rotationMask = new Vector3(0, 0, 1);
    }
    
    void Update()
    {
        thisTransform.RotateAround(parentTransform.position, rotationMask, rotationSpeed * Time.deltaTime);
        TimerAnimation();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitObject = col.gameObject;
        if (hitObject.tag == "EnemyProjectile")
        {
            Projectile bullet = hitObject.GetComponent<Projectile>();
            bullet.MuteHitEffect = true;
            bullet.CheckDie();
        }
    }
    
}
