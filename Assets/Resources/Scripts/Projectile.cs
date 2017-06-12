using UnityEngine;

public enum Trajectory
{
    Line, Sin
}
public enum EffectType
{
    Die, Pierce
}

public class Projectile : MonoBehaviour {

    [Header("Stats")]
    public int Damage = 5;
    public float Range = 10;
    public float Speed = 1;
    public int PierceCount = 0;
    [Range(0, 100)]
    public float DieEffectDamagePercent = 0;

    [Header("Direction")]
    [Range(0f, 359f)]
    public float Angle = 0;
    public bool RotateAlongTrajectory = true;
    public Trajectory CurrentTrajectory;
    public float Frequency = 1;
    public float Magnitude = 1;
    [Range(0f,5f)]
    public float MagnitudePlusMinus = 0;

    [Header("Animation")]
    public bool ShowRangeDieEffect = false;
    public GameObject PierceEffect;
    public GameObject DieEffect;

    [Header("DEBUG")]
    public float AnglePerpendicular;
    public bool MuteHitEffect = false;
    public GameObject PierceEffect_cached;
    public GameObject DieEffect_cached;
        
    private int pierceCount_new;
    private float TTL;
    private float TTLOld;
    private float sinCosRad;
    private float magnitudeNew;
    private float currentDistance;
    private Vector2 axis;
    private Vector2 axisPerpendicular;
    private Vector2 SinCosTransform;
    private Explosion pierceEffect_parameters;
    private Explosion dieEffect_parameters;
    private Transform thisTransform;
    private AudioSource pierceEffect_audio;
    private AudioSource dieEffect_audio;    
    private Rigidbody2D thisRigidbody;    
    
    void Awake () {
        thisTransform = transform;
        thisRigidbody = GetComponent<Rigidbody2D>();

        TTLOld = Speed != 0 ? Range / Speed : 0;        

        if (PierceEffect != null)
        {
            PierceEffect.SetActive(false);
            PierceEffect_cached = Instantiate(PierceEffect);
            PierceEffect_cached.GetComponent<Explosion>().axis = axis;
            pierceEffect_parameters = PierceEffect_cached.GetComponent<Explosion>();
            pierceEffect_audio = PierceEffect_cached.GetComponent<AudioSource>();            
        }

        if (DieEffect != null)
        {
            DieEffect.SetActive(false);
            DieEffect_cached = Instantiate(DieEffect);
            DieEffect_cached.GetComponent<Explosion>().axis = axis;
            dieEffect_parameters = DieEffect_cached.GetComponent<Explosion>();
            dieEffect_audio = DieEffect_cached.GetComponent<AudioSource>();            
        }

    }

    private void OnEnable()
    {
        thisTransform.localEulerAngles = new Vector3(0, 0, Angle);
        axis = AngleToAxis(Angle);
        axis *= Speed;
        TTL = TTLOld;
        pierceCount_new = PierceCount;
        SinCosTransform = Vector2.zero;        

        if (CurrentTrajectory != Trajectory.Line)
        {
            magnitudeNew = Magnitude;

            if (MagnitudePlusMinus > 0)
            {
                magnitudeNew += Random.Range(-MagnitudePlusMinus, MagnitudePlusMinus);
            }

            AnglePerpendicular = Angle + 90.0f;
            axisPerpendicular = AngleToAxis(AnglePerpendicular) * magnitudeNew;
        }
    }

    private void OnDisable()
    {
        TTL = TTLOld;
        currentDistance = 0;
    }

    // Update is called once per frame
    void Update () {
        TTL -= Time.deltaTime;

        if (TTL <= 0)
        {
            Die(ShowRangeDieEffect);
        }

        MoveDirection();
	}

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    public void CheckDie()
    {
        if (pierceCount_new > 0)
        {
            pierceCount_new--;
            ShowEffect(0);
        }
        else
        {
            Die(true);
        }
    }

    private void MoveDirection()
    {
        currentDistance += Speed * Time.deltaTime;
        switch (CurrentTrajectory)
        {
            case Trajectory.Line:
                thisRigidbody.position += axis * Time.deltaTime;
                break;
            case Trajectory.Sin:
                sinCosRad = (Mathf.Cos(currentDistance * Frequency));
                SinCosTransform = axisPerpendicular * sinCosRad;

                if (RotateAlongTrajectory)
                {
                    thisTransform.localEulerAngles = new Vector3(0, 0, Mathf.Asin(sinCosRad) * Mathf.Rad2Deg);
                }

                thisRigidbody.position += (SinCosTransform + axis) * Time.deltaTime;
                break;
        }
    }

    private Vector2 AngleToAxis(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(betha), Mathf.Sin(betha));
    }

    private void Die(bool showDieEffect)
    {
        if (showDieEffect)
        {
            ShowEffect(EffectType.Die);
        }
        
        gameObject.SetActive(false);
    }

    private void ShowEffect(EffectType effectType)
    {
        // 0 - pierce, 1 - die        
        switch (effectType)
        {
            case EffectType.Pierce:
                if (PierceEffect_cached != null)
                {                    
                    PierceEffect_cached.transform.position = thisTransform.position;
                    pierceEffect_parameters.axis = (SinCosTransform + axis);

                    if (MuteHitEffect)
                    {
                        pierceEffect_audio.mute = true;
                    }

                    PierceEffect_cached.SetActive(true);
                }
                break;
            case EffectType.Die:
                if (DieEffect_cached != null)
                {                    
                    DieEffect_cached.transform.position = thisTransform.position;
                    dieEffect_parameters.axis = (SinCosTransform + axis);
                    dieEffect_parameters.Damage = (int)(Damage / 100.0f * DieEffectDamagePercent);

                    if (MuteHitEffect)
                    {
                        dieEffect_audio.mute = true;
                    }                    

                    DieEffect_cached.SetActive(true);
                }
                break;
        }
    }
}
