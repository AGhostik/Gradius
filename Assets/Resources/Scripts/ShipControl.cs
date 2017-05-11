using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour {

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

        Debug.Log("Horizontal: " + Input.GetAxis("Horizontal") + " Vertical: " + Input.GetAxis("Vertical"));

        /*
        points_wtf[0].transform.position = pos_min;
        points_wtf[1].transform.position = new Vector3(pos_min.x,pos_max.y,0);
        points_wtf[2].transform.position = new Vector3(pos_max.x, pos_min.y, 0);
        points_wtf[3].transform.position = pos_max;
        */
    }

    private void flyInput()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float deltaX = 0, deltaY = 0;

        if (inputX < 0)
        {
            if (this.transform.position.x > pos_min.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        } else
        {
            if (this.transform.position.x < pos_max.x)
            {
                deltaX = inputX * Time.deltaTime;
            }
        }

        if (inputY < 0)
        {
            if (this.transform.position.y > pos_min.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }
        else
        {
            if (this.transform.position.y < pos_max.y)
            {
                deltaY = inputY * Time.deltaTime;
            }
        }

        this.transform.position += new Vector3(deltaX, deltaY, 0);
    }
}
