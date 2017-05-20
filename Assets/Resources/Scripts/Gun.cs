using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    
    [Header("Ammunition")]
    public List<GameObject> Projectiles;    

    [Header("Start position")]
    public GameObject muzzle;
    public List<GameObject> Companions;

    [Header("Stats")]
    public int damageUP = 0;
    public float firerate = 1;
    public int shotsBetweenPause = 3;
    public float pause_length = 0;

    [Header("Rotate projectile")]
    public bool rotate = false;
    public float angle = 0;
    public float rotate_speed = 30;

    [Header("Control")]    
    public bool autoShot = true;
    public string inputAxisName = "X";

    private int shot_count = 0;
    private int projectile_level = 0;
    private float firerate1_timer = 0;
    private float pause_timer = 0;    

    private Transform gun_transform;
    private Transform muzzle_transform;

    // Use this for initialization
    void Start () {
        gun_transform = transform;
        muzzle_transform = muzzle == null ? null : muzzle.transform;
	}
    
    // Update is called once per frame
    void Update () {
        updateAngle();

        if (autoShot)
        {
            Shot();
        }
    }

    private void OnEnable()
    {
        if (!autoShot)
        {
            switch (inputAxisName)
            {
                case "A":
                    EventController.InputA += Shot;
                    break;
                case "B":
                    EventController.InputB += Shot;
                    break;
                case "X":
                    EventController.InputX += Shot;
                    break;
                case "Y":
                    EventController.InputY += Shot;
                    break;
            }
        }
    }

    private void OnDisable()
    {
        switch (inputAxisName)
        {
            case "A":
                EventController.InputA -= Shot;
                break;
            case "B":
                EventController.InputB -= Shot;
                break;
            case "X":
                EventController.InputX -= Shot;
                break;
            case "Y":
                EventController.InputY -= Shot;
                break;
        }
    }

    private void upgradeGun()
    {
        if (projectile_level < Projectiles.Count - 1)
        {
            projectile_level++;
        }
    }

    private void updateAngle()
    {
        if (rotate)
        {
            angle += rotate_speed * Time.deltaTime;
        }

        if (angle >= 360)
        {
            angle -= 360;
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
}
