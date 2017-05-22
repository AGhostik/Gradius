using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Stats")]
    public int damage = 5;
    public float range = 10;
    public float speed_plus = 1;
    public float speed_mult = 1;
    public float TTL = 5;
    public int pierce_count = 0;

    [Header("Direction")]
    public float angle = 0;   
    public bool Sin = false;
    public float frequency = 1;  // Speed of sine movement
    public float magnitude = 1;   // Size of sine movement
    [Range(0f,1f)]
    public float addRandomMagnitude = 0;

    [Header("Animation")]
    public GameObject pierceEffect;
    public GameObject hit_dieEffect;
    public GameObject range_dieEffect;
    
    private Vector3 startPos;
    private float currentDistance;

    [HideInInspector()]
    public float angle_perpend;

    private Vector3 axis;
    private Vector3 sin_transform;

    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        startPos = thisTransform.position;

        magnitude += Random.Range(0, addRandomMagnitude);

        thisTransform.localEulerAngles = new Vector3(0, 0, angle);
        axis = axisAngle(angle);

        angle_perpend = angle + 90;

        axis *= ((speed_plus + 1) * speed_mult) * 2;
    }

    // Update is called once per frame
    void Update () {
        TTL -= Time.deltaTime;
        if (TTL <= 0)
        {
            Die(0);
        }
        rangeRemain();
        rangeCheck();   
        
        moveDirection();
	}

    public void checkDie()
    {
        if (pierce_count > 0)
        {
            pierce_count--;
            instantianeExplosion(pierceEffect);
        }
        else
        {
            Die(1);
        }
    }

    private void moveDirection()
    {
        if (Sin)
        {
            sin_transform = axisAngle(angle_perpend) * (Mathf.Sin(currentDistance * frequency)) * magnitude;
            thisTransform.position += (sin_transform + axis) * Time.deltaTime;
        }
        else
        {
            thisTransform.position += axis * Time.deltaTime;
        }
    }

    private Vector3 axisAngle(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(betha), Mathf.Sin(betha), 0);
    }
    
    private void rangeRemain()
    {
        currentDistance = Vector3.Distance(thisTransform.position, startPos);
    }

    private void Die(int effectType = 0)
    {
        if (effectType == 1)
        {
            instantianeExplosion(hit_dieEffect);
        }
        else
        if (effectType == 2)
        {
            instantianeExplosion(range_dieEffect);
        }

        Destroy(gameObject);
    }

    private void instantianeExplosion(GameObject expl)
    {
        if (expl != null)
        {
            GameObject exp = Instantiate(expl);
            exp.transform.position = thisTransform.position;
            exp.GetComponent<Explosion>().axis = axis;
        }
    }

    private void rangeCheck()
    {
        if (currentDistance >= range)
        {
            Die(2);
        }
    }
}
 