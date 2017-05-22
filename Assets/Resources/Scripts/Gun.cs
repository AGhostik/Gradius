using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAxis
{
    A, B, X, Y
}

public class Gun : MonoBehaviour {

    [Header("Ammunition")]
    public List<GameObject> Projectiles;

    [Header("Sound")]
    public List<AudioClip> shotSound;
    [Range(0f, 5f)]
    public float volume = 1;

    [Header("Start position")]
    public GameObject muzzle;
    public List<GameObject> Companions;

    [Header("Stats")]
    public int damageUP = 0;
    public float firerate = 1;
    public float firerateCap = 0.1f;
    public int shotsBetweenPause = 3;
    public float pause_length = 0;

    [Header("Rotate projectile")]
    public bool rotate = false;
    public bool clockwise = false;
    public float minAngle = 0;
    public float maxAngle = 360;
    public float rotate_speed = 30;

    [Header("Control")]
    public bool autoShot = true;
    public InputAxis input;

    [HideInInspector()]
    public int projectile_level = 0;
    [HideInInspector()]
    public bool canUpgrade = true;
    [HideInInspector()]
    public bool canFirerateUp = true;

    private float angle = 0;
    private bool old_autoShot;
    private int shot_count = 0;    
    private float firerate1_timer = 0;
    private float pause_timer = 0;

    private AudioSource audi;
    private Transform gun_transform;
    private Transform muzzle_transform;

    // Use this for initialization
    void Start () {
        gun_transform = transform;
        audi = GetComponent<AudioSource>();
        muzzle_transform = muzzle == null ? null : muzzle.transform;
        old_autoShot = autoShot;
        if (transform.parent != null && transform.parent.tag == "Player")
        {
            if (gun_transform.name == "Gun1")
            {
                EventController.setGun1Damage(damageUP + Projectiles[projectile_level].GetComponent<Projectile>().damage);
                EventController.setGun1Firerate(firerate);
            }
            if (gun_transform.name == "Gun2")
            {
                EventController.setGun2Damage(damageUP + Projectiles[projectile_level].GetComponent<Projectile>().damage);
                EventController.setGun2Firerate(firerate);
            }
        }
        angle = minAngle;
	}
    
    // Update is called once per frame
    void Update () {
        updateAngle();

        if (old_autoShot != autoShot)
        {
            if (autoShot)
            {
                unsubscribeInput();
            }
            else
            {
                subscribeInput();
            }
            old_autoShot = autoShot;
        }

        if (autoShot)
        {
            Shot();
        }
    }

    private void OnEnable()
    {
        if (!autoShot)
        {
            subscribeInput();
        }
    }

    private void OnDisable()
    {
        unsubscribeInput();
    }

    public void firerateUp(float value)
    {
        if (firerate > firerateCap)
        {
            if (firerate - value > firerateCap)
            {
                firerate -= value;
            }
            else
            {
                firerate = firerateCap;
                canFirerateUp = false;
            }
            if (gun_transform.name == "Gun1")
            {
                EventController.setGun1Firerate(firerate);
            }
            if (gun_transform.name == "Gun2")
            {
                EventController.setGun2Firerate(firerate);
            }
        }
    }

    public void upgradeGun()
    {
        if (projectile_level < Projectiles.Count - 1)
        {
            projectile_level++;

            //как-то стыдно за такие сточки, нудаладна, и так сойдет
            if (gun_transform.name == "Gun1")
            {
                EventController.playerGun1LevelUP();
            }
            if (gun_transform.name == "Gun2")
            {
                EventController.playerGun2LevelUP();
            }

            if (projectile_level == Projectiles.Count - 1)
            {
                canUpgrade = false;
            }
        }
    }

    private void subscribeInput()
    {
        switch (input)
        {
            case InputAxis.A:
                EventController.InputA += Shot;
                break;
            case InputAxis.B:
                EventController.InputB += Shot;
                break;
            case InputAxis.X:
                EventController.InputX += Shot;
                break;
            case InputAxis.Y:
                EventController.InputY += Shot;
                break;
        }
    }

    private void unsubscribeInput()
    {
        switch (input)
        {
            case InputAxis.A:
                EventController.InputA -= Shot;
                break;
            case InputAxis.B:
                EventController.InputB -= Shot;
                break;
            case InputAxis.X:
                EventController.InputX -= Shot;
                break;
            case InputAxis.Y:
                EventController.InputY -= Shot;
                break;
        }
    }
    
    private void updateAngle()
    {
        if (rotate)
        {     
            float delta = rotate_speed * Time.deltaTime;
            if (clockwise)
            {
                angle += delta;
                if (angle < minAngle || angle > maxAngle)
                {
                    angle = minAngle;
                }
            }
            else
            {
                angle -= delta;
                if (angle < minAngle || angle > maxAngle)
                {
                    angle = maxAngle;
                }
            }
        }

        if (angle > maxAngle)
        {
            angle -= maxAngle;
        }
    }

    private void createProjectile()
    {
        GameObject bullet = Instantiate(Projectiles[projectile_level]);
        Projectile proj = bullet.GetComponent<Projectile>();
        proj.damage += damageUP;

        if (rotate)
        {
            proj.angle = angle;
        }

        if (muzzle != null)
        {
            bullet.transform.position = muzzle_transform.position;
        }
        else
        {
            bullet.transform.position = gun_transform.position;
        }

        foreach (GameObject comp in Companions)
        {
            Instantiate(bullet).transform.position = comp.transform.position;
        }

        playSound();
    }

    private void Shot()
    {
        if (Projectiles.Count > 0 && Projectiles[projectile_level] != null)
        {
            if (shot_count < shotsBetweenPause)
            {
                if (firerate1_timer >= firerate)
                {
                    createProjectile();

                    shot_count++;
                    firerate1_timer = 0;
                    pause_timer = 0;
                }
                else
                {
                    firerate1_timer += Time.deltaTime;
                }
            }
            else
            {
                if (pause_timer >= pause_length)
                {
                    shot_count = 0;
                }
                else
                {
                    pause_timer += Time.deltaTime;
                }
            }
        }
    }

    private void playSound()
    {
        if (shotSound.Count > 0)
        {
            if (shotSound[projectile_level] != null && volume > 0)
            {
                audi.PlayOneShot(shotSound[projectile_level], volume);
            }
        }
    }
}
