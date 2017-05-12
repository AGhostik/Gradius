using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour {

    [Header("Ship stats")]
    [Range(0f,8f)]

    public float speed = 1.2f;

    [Header("Ship weapons")]

    public GameObject GunA;
    public GameObject GunB;
    public GameObject GunX;
    public GameObject GunY;

    [Header("Weapons ammunition")]

    public GameObject BulletA;
    public GameObject BulletB;
    public GameObject BulletX;
    public GameObject BulletY;

    [Header("Weapons rate of fire")]

    public float firerateA = 1;
    public float firerateB = 1;
    public float firerateX = 1;
    public float firerateY = 1;
    
    private float firerateA_timer = 1;
    private float firerateB_timer = 1;
    private float firerateX_timer = 1;
    private float firerateY_timer = 1;

    private Vector3 pos_min;
    private Vector3 pos_max;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += new Vector3(Time.deltaTime,0,0);

        pos_min = Camera.main.ScreenToWorldPoint(new Vector3 (0,0,0));
        pos_max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        flyInput();

        shotInput();
    }

    private void shotInput()
    {
        if (firerateA_timer >= firerateA)
        {
            if (Input.GetAxisRaw("FireA") != 0)
            {
                Instantiate(BulletA).transform.position = GunA.transform.position;
                firerateA_timer = 0;
            }
        }
        if (firerateB_timer >= firerateB)
        {
            if (Input.GetAxisRaw("FireB") != 0)
            {
                Instantiate(BulletB).transform.position = GunB.transform.position;
                firerateB_timer = 0;
            }
        }
        if (firerateX_timer >= firerateX)
        {
            if (Input.GetAxisRaw("FireX") != 0)
            {
                Instantiate(BulletX).transform.position = GunX.transform.position;
                firerateX_timer = 0;
            }
        }
        if (firerateY_timer >= firerateY)
        {
            if (Input.GetAxisRaw("FireY") != 0)
            {
                Instantiate(BulletY).transform.position = GunY.transform.position;
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
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

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

        transform.position += new Vector3(deltaX * speed, deltaY * speed, 0);
    }
}
