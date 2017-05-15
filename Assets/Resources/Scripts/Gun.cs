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

    [Header("Control")]
    public bool autoShot = true;
    public string inputAxisName = "Fire";

    private float firerate1_timer = 1;

    // Use this for initialization
    void Start () {        
	}
	
	// Update is called once per frame
	void Update () {
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

    private void Shot()
    {
        if (Bullet != null)
        {
            if (firerate1_timer >= firerate)
            {
                GameObject bullet = Instantiate(Bullet);
                bullet.GetComponent<Projectile>().damage += damageUP;

                if (empty_startPos != null)
                {
                    bullet.transform.position = empty_startPos.transform.position;
                }
                else
                {
                    bullet.transform.position = gameObject.transform.position;
                }

                firerate1_timer = 0;
            }
            if (firerate1_timer < firerate) firerate1_timer += Time.deltaTime;
        }
    }
}
