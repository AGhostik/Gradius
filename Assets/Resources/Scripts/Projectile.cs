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
    [Range(0f, 359f)]
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

    protected Vector3 startPos;
    private float currentDistance;

    [HideInInspector()]
    public float angle_perpend;

    [HideInInspector()]
    public bool mute_hitEffect = false;

    protected Vector2 axis;
    protected Vector2 axis_perpendic;
    protected Vector2 sin_transform;

    protected Transform thisTransform;
    protected Rigidbody2D thisRigidbody;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        thisRigidbody = GetComponent<Rigidbody2D>();
        startPos = thisTransform.position;
        thisTransform.localEulerAngles = new Vector3(0, 0, angle);
        axis = axisAngle(angle);
        axis *= ((speed_plus + 1) * speed_mult) * 2;

        if (Sin)
        {
            if (addRandomMagnitude > 0)
            {
                magnitude += Random.Range(0, addRandomMagnitude);
            }

            angle_perpend = angle + 90;
            axis_perpendic = axisAngle(angle_perpend) * magnitude;
        }
    }

    // Update is called once per frame
    void Update () {
        TTL -= Time.deltaTime;
        if (TTL <= 0)
        {
            Die(0);
        }

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

    protected void moveDirection()
    {
        if (Sin)
        {
            sin_transform = axis_perpendic * (Mathf.Sin(currentDistance * frequency));
            //thisTransform.position += (sin_transform + axis) * Time.deltaTime;
            thisRigidbody.position += (sin_transform + axis) * Time.deltaTime;
        }
        else
        {
            //thisTransform.position += axis * Time.deltaTime;
            thisRigidbody.position += axis * Time.deltaTime;
        }
    }

    protected void Die(int effectType = 0)
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

    protected void rangeCheck()
    {
        currentDistance = Vector3.Distance(thisTransform.position, startPos);

        if (currentDistance >= range)
        {
            Die(2);
        }
    }


    protected Vector2 axisAngle(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(betha), Mathf.Sin(betha));
    }

    private void instantianeExplosion(GameObject expl)
    {
        if (expl != null)
        {
            GameObject exp = Instantiate(expl);
            exp.transform.position = thisTransform.position;
            exp.GetComponent<Explosion>().axis = axis;
            if (mute_hitEffect)
            {
                exp.GetComponent<AudioSource>().mute = true;
            }
        }
    }

}
