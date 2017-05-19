using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    
    [Header("Ammunition")]
    public GameObject Bullet;    

    [Header("Start position")]
    public GameObject empty_startPos;

    [Header("Stats")]
    public int damageUP = 0;
    public float firerate = 1;
    public int shotsBetweenPause = 3;
    public float pause_length = 0;
    public bool rotate = false;
    public float rotate_speed = 30;

    [Header("Control")]    
    public bool autoShot = true;
    public string inputAxisName = "Fire";

    public float angle = 0;
    private float firerate1_timer = 0;
    private float pause_timer = 0;
    private int shot_count = 0;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        updateAngle();

        if (autoShot)
        {
            Shot();
        }
        else
        if (Input.GetAxisRaw(inputAxisName) != 0)
        {
            Shot();
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

    private void Shot()
    {
        if (Bullet != null)
        {
            if (shot_count < shotsBetweenPause)
            {
                if (firerate1_timer >= firerate)
                {
                    GameObject bullet = Instantiate(Bullet);
                    Projectile proj = bullet.GetComponent<Projectile>();
                    proj.damage += damageUP;

                    if (rotate)
                    {
                        proj.angle = angle;
                    }

                    if (empty_startPos != null)
                    {
                        bullet.transform.position = empty_startPos.transform.position;
                    }
                    else
                    {
                        bullet.transform.position = gameObject.transform.position;
                    }
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
