using UnityEngine;

public class Garun : Destroyable
{

    [Header("Move")]
    [Range(0f, 5f)]
    public float moveSpeed = 0;
    [Range(0f, 359f)]
    public float angle = 180;
    public float frequency = 3;
    public float magnitude = 1; 
    
    private float anglePerpendicular;
    private Rigidbody2D thisRigidbody;
    private Vector2 colliderSize;
    private Vector3 camPos_min;
    private Vector2 sinTransform;
    private Vector2 startPos;
    private Vector2 move;
    private Vector2 axis;

    protected override void Awake()
    {
        base.Awake();

        thisRigidbody = GetComponent<Rigidbody2D>();
        SceneObjectContainer.AddProjectileContainer(gameObject.tag);

        anglePerpendicular = angle + 90.0f;
        axis = axisAngle(angle);
        move = axis * moveSpeed;
    }
    
    void Start()
    {        
        startPos = thisTransform.position;
        camPos_min = EventController.GetCamPos_TL();        
    }
    
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        sinTransform = axisAngle(anglePerpendicular) * (Mathf.Sin(frequency * Vector3.Distance(startPos, thisTransform.position))) * magnitude;
        thisRigidbody.position += (sinTransform + move) * Time.deltaTime;
        TimerAnimation();

        if (thisTransform.position.x < camPos_min.x)
        {
            thisRigidbody.position = startPos;
        }
    }    

    private Vector2 axisAngle(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(betha), Mathf.Sin(betha));
    }
}