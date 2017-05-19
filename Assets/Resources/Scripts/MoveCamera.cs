using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public bool stop = false;
    public float finish_posX = 100;

    private float percent;
    private Transform thisTransform;
    private EventController.setlevelProgress sendPercent = UIDraw.SetLevelProgress;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        if (Screen.fullScreen)
        {
            Cursor.visible = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!stop)
        {
            moveTo();
        }
        percentageCalculate();
        sendPercent(percent);

        if (percent >= 100)
        {
            Time.timeScale = 0;
        }

        if (Input.GetAxisRaw("Menu") != 0)
        {
            Application.Quit();
        }
	}       

    private void moveTo()
    {
        if (thisTransform.position.x < finish_posX)
        {
            thisTransform.position += new Vector3(Time.deltaTime, 0, 0);
        }
    }

    private void percentageCalculate()
    {
        percent = (thisTransform.position.x / finish_posX) * 100;
    }
}
