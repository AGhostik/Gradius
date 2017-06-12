using UnityEngine;

public class Fan : Destroyable
{

    [Header("Move")]
    [Range(0f, 5f)]
    public float moveSpeed = 0;
    [Range(0f, 359f)]
    public float angle = 180;

    private int oldFrame;
    private Vector3 camPos_min;
    private Vector3 camPos_max;
    private Vector2 move;
    private Vector2 axis;
    private Rigidbody2D thisRigidbody;

    protected override void Awake()
    {
        base.Awake();
        thisRigidbody = GetComponent<Rigidbody2D>();
        SceneObjectContainer.AddProjectileContainer(gameObject.tag);

        axis = axisAngle(angle);
        move = axis * moveSpeed;
    }
    
    protected void Start()
    {
        camPos_min = EventController.GetCamPos_TL();
        camPos_max = EventController.GetCamPos_BR();
    }
    
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        thisRigidbody.position += move * Time.deltaTime;
        TimerAnimation();

        if (thisTransform.position.x < camPos_min.x)
        {
            thisRigidbody.position = new Vector2(camPos_max.x, thisTransform.position.y);
        }
    }    

    private Vector2 axisAngle(float alpha)
    {
        float betha = alpha * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(betha), Mathf.Sin(betha));
    }
}