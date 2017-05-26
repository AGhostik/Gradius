using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Stats")]
    public int damage = 5;
    public float range = 10;
    public float speed = 1;
    public int pierceCount = 0;

    [Header("Direction")]
    [Range(0f, 359f)]
    public float angle = 0;   

    public bool Sin = false;
    public float frequency = 1;
    public float magnitude = 1;
    [Range(0f,1f)]
    public float addRandomMagnitude = 0;

    [Header("Animation")]
    public GameObject pierceEffect;
    public GameObject hitDieEffect;
    public GameObject rangeDieEffect;
    
    [HideInInspector()]
    public float anglePerpendicular;
    [HideInInspector()]
    public bool muteHitEffect = false;

    public GameObject pierceEffect_cached;
    public GameObject hitDieEffect_cached;
    public GameObject rangeDieEffect_cached;

    private float ttl;
    private float ttlOld;
    private float currentDistance;
    private Vector3 startPos;
    private Vector2 axis;
    private Vector2 axisPerpendicular;
    private Vector2 sinTransform;
    private Transform thisTransform;    
    private AudioSource pierceEffect_audio;
    private AudioSource hitDieEffect_audio;
    private AudioSource rangeDieEffect_audio;
    private Rigidbody2D thisRigidbody;    

    // Use this for initialization
    void Awake () {
        thisTransform = transform;
        thisRigidbody = GetComponent<Rigidbody2D>();

        ttlOld = speed != 0 ? range / speed : 0;        

        if (pierceEffect != null)
        {
            pierceEffect_cached = Instantiate(pierceEffect);
            pierceEffect_cached.GetComponent<Explosion>().axis = axis;
            pierceEffect_audio = pierceEffect_cached.GetComponent<AudioSource>();
            pierceEffect_cached.SetActive(false);
        }

        if (hitDieEffect != null)
        {
            hitDieEffect_cached = hitDieEffect != null ? Instantiate(hitDieEffect) : null;
            hitDieEffect_cached.GetComponent<Explosion>().axis = axis;
            hitDieEffect_audio = hitDieEffect_cached.GetComponent<AudioSource>();
            hitDieEffect_cached.SetActive(false);
        }

        if (rangeDieEffect != null)
        {
            rangeDieEffect_cached = rangeDieEffect != null ? Instantiate(rangeDieEffect) : null;
            rangeDieEffect_cached.GetComponent<Explosion>().axis = axis;
            rangeDieEffect_audio = rangeDieEffect_cached.GetComponent<AudioSource>();
            rangeDieEffect_cached.SetActive(false);
        }

    }

    private void OnEnable()
    {
        startPos = thisTransform.position;
        thisTransform.localEulerAngles = new Vector3(0, 0, angle);
        axis = axisAngle(angle);
        axis *= speed;
        ttl = ttlOld;

        if (Sin)
        {
            if (addRandomMagnitude > 0)
            {
                magnitude += Random.Range(0, addRandomMagnitude);
            }

            anglePerpendicular = angle + 90;
            axisPerpendicular = axisAngle(anglePerpendicular) * magnitude;
        }
    }

    private void OnDisable()
    {
        ttl = ttlOld;
        currentDistance = 0;
    }

    // Update is called once per frame
    void Update () {
        ttl -= Time.deltaTime;

        rangeCheck();

        if (ttl <= 0)
        {
            Die(0);
        }

        moveDirection();
	}

    public void checkDie()
    {
        if (pierceCount > 0)
        {
            pierceCount--;
            showEffect(0);
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
            sinTransform = axisPerpendicular * (Mathf.Sin(currentDistance * frequency));
            //thisTransform.position += (sin_transform + axis) * Time.deltaTime;
            thisRigidbody.position += (sinTransform + axis) * Time.deltaTime;
        }
        else
        {
            //thisTransform.position += axis * Time.deltaTime;
            thisRigidbody.position += axis * Time.deltaTime;
        }
    }

    private void rangeCheck()
    {
        currentDistance = Vector3.Distance(thisTransform.position, startPos);

        if (currentDistance >= range)
        {
            Die(2);
        }
    }

    private Vector2 axisAngle(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(betha), Mathf.Sin(betha));
    }

    private void Die(int effectType)
    {
        // 0 - pierce, 1 - hit, 2 - range
        showEffect(effectType);

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void showEffect(int effecttype)
    {
        // 0 - pierce, 1 - hit, 2 - range        
        switch (effecttype)
        {
            case 0:
                if (pierceEffect_cached != null)
                {
                    pierceEffect_cached.SetActive(true);
                    pierceEffect_cached.transform.position = thisTransform.position;
                    if (muteHitEffect)
                    {
                        pierceEffect_audio.mute = true;
                    }
                }
                break;
            case 1:
                if (hitDieEffect_cached != null)
                {
                    hitDieEffect_cached.SetActive(true);
                    hitDieEffect_cached.transform.position = thisTransform.position;
                    if (muteHitEffect)
                    {
                        hitDieEffect_audio.mute = true;
                    }
                }
                break;
            case 2:
                if (rangeDieEffect_cached != null)
                {
                    rangeDieEffect_cached.SetActive(true);
                    rangeDieEffect_cached.transform.position = thisTransform.position;
                    if (muteHitEffect)
                    {
                        rangeDieEffect_audio.mute = true;
                    }
                }
                break;
        }
    }
}
