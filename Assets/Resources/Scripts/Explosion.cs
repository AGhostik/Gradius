using UnityEngine;

public class Explosion : AnimatedObject {

    [Header("Explosion")]
    public int Damage = 0;
    public float Radius = 1;

    [Header("Move")]    
    public float axisMultiplier = 0.5f;
    public Vector3 axis = new Vector3(1, 0, 0);
    
    private Transform thisTransform;
    private AudioSource effectAudio;

    protected override void Awake()
    {
        base.Awake();

        thisTransform = transform;
        effectAudio = GetComponent<AudioSource>();

        ChangeFrame();

        if (Damage > 0 && Radius > 0)
            thisTransform.localScale = new Vector2(Radius*2 / render.sprite.bounds.size.x, Radius*2 / render.sprite.bounds.size.y);
    }

    private void OnEnable()
    {
        timer = oneFrameTime;
        axis *= axisMultiplier;        
        currentFrame = 0;
        ChangeFrame();

        if (Damage > 0 && Radius > 0)
        {
            ExplosionDamage();
        }
    }
    
    void Update () {
        Die();
        thisTransform.position += axis * Time.deltaTime;        
    }

    private void OnDisable()
    {
        effectAudio.mute = false;
    }

    private void Die()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (currentFrame < framesLength)
            {
                ChangeFrame(currentFrame);
                currentFrame++;
                timer = oneFrameTime;
            }
            else
            {
                gameObject.SetActive(false);
            }        
        }
    }

    private void ExplosionDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(thisTransform.position, Radius);
        foreach (Collider2D victim in hitColliders)
        {
            if (victim.tag == "Enemy" || victim.tag == "Player")
            {
                victim.GetComponent<Destroyable>().DecreaseHealth(Damage);
            }
        }
    }
}
