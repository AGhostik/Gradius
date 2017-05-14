using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour {

    [Header("Stats")]
    [Range(0f,5f)]
    public float speed = 1.2f;
    public int max_health = 100;

    [Header("Weapons")]
    public GameObject GunA;
    public GameObject GunB;
    public GameObject GunX;
    public GameObject GunY;

    [Header("Ammunition")]
    public GameObject Bullet;
    public GameObject Bullet2;
    public GameObject Laser;
    public GameObject Laser2;
    public GameObject Rocket;

    [Header("Rate of fire")]
    public float firerateA = 1;
    public float firerateB = 1;
    public float firerateX = 1;
    public float firerateY = 1;

    [Header("Damape UP's")]
    public int damageA = 0;
    public int damageB = 0;
    public int damageX = 0;
    public int damageY = 0;

    [Header("Animation")]
    public Sprite NormalSprite;
    public Sprite UpSprite;
    public Sprite DownSprite;
    public GameObject explosion;

    //private
    private GameObject ProjectileA;
    private GameObject ProjectileB;
    private GameObject ProjectileX;
    private GameObject ProjectileY;

    private float firerateA_timer = 1;
    private float firerateB_timer = 1;
    private float firerateX_timer = 1;
    private float firerateY_timer = 1;

    private int current_health;

    private Vector3 pos_min;
    private Vector3 pos_max;
    private Camera main_cam;
    private SpriteRenderer render;

    // Use this for initialization
    void Start()
    {
        main_cam = Camera.main;
        render = gameObject.GetComponent<SpriteRenderer>();

        ProjectileA = Laser2;
        ProjectileB = Laser;
        ProjectileX = Bullet;
        ProjectileY = Rocket;

        current_health = max_health;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(Time.deltaTime,0,0);

        pos_min = main_cam.ScreenToWorldPoint(new Vector3 (0,0,0));
        pos_max = main_cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        flyInput();

        shotInput();

        if (current_health <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            damageUp(ref damageX, 100);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyProjectile")
        {
            current_health -= col.gameObject.GetComponent<Projectile>().damage;
        }
    }

    private void OnGUI()
    {
        UIhelper draw = new UIhelper(256, 144);
        draw.label("Health: " + current_health).Draw(35,2);
    }

    private void Die()
    {
        Time.timeScale = 0.5f;
        if (explosion != null)
        {
            Instantiate(explosion).transform.position = gameObject.transform.position;
        }
        Destroy(gameObject);
    }

    private void damageUp(ref int damage, int value)
    {
        damage += value;
    }

    private void shotInput()
    {
        if (firerateA_timer >= firerateA)
        {
            if (Input.GetAxisRaw("FireA") != 0)
            {
                GameObject bullet = Instantiate(ProjectileA);
                bullet.transform.position = GunA.transform.position;
                bullet.GetComponent<Projectile>().damage += damageA;
                firerateA_timer = 0;
            }
        }
        if (firerateB_timer >= firerateB)
        {
            if (Input.GetAxisRaw("FireB") != 0)
            {
                GameObject bullet = Instantiate(ProjectileB);
                bullet.transform.position = GunB.transform.position;
                bullet.GetComponent<Projectile>().damage += damageB;
                firerateB_timer = 0;
            }
        }
        if (firerateX_timer >= firerateX)
        {
            if (Input.GetAxisRaw("FireX") != 0)
            {
                GameObject bullet = Instantiate(ProjectileX);
                bullet.transform.position = GunX.transform.position;
                bullet.GetComponent<Projectile>().damage += damageX;
                firerateX_timer = 0;
            }
        }
        if (firerateY_timer >= firerateY)
        {
            if (Input.GetAxisRaw("FireY") != 0)
            {
                GameObject bullet = Instantiate(ProjectileY);
                bullet.transform.position = GunY.transform.position;
                bullet.GetComponent<Projectile>().damage += damageY;
                firerateY_timer = 0;
            }
        }

        if (firerateA_timer < firerateA) firerateA_timer += Time.deltaTime;
        if (firerateB_timer < firerateB) firerateB_timer += Time.deltaTime;
        if (firerateX_timer < firerateX) firerateX_timer += Time.deltaTime;
        if (firerateY_timer < firerateY) firerateY_timer += Time.deltaTime;
    }

    private void flyInput()
    {
        float inputX = Input.GetAxis("Horizontal") * 3;
        float inputY = Input.GetAxis("Vertical") * 3;

        float deltaX = 0, deltaY = 0;

        if (inputX < 0)
        {
            if (transform.position.x > pos_min.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        } else
        {
            if (transform.position.x < pos_max.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        }

        if (inputY < 0)
        {
            if (transform.position.y > pos_min.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }
        else
        {
            if (transform.position.y < pos_max.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }

        changeSprite(deltaY);

        transform.position += new Vector3(deltaX * speed, deltaY * speed, 0);
    }

    private void changeSprite(float vertical)
    {
        if (UpSprite != null && DownSprite != null)
        {
            if (vertical < -0.01f)
            {
                render.sprite = DownSprite;
            }
            else
            if (vertical > 0.01f)
            {
                render.sprite = UpSprite;
            }
            else
            {
                render.sprite = NormalSprite;
            }
        }
    }
}
