using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public bool stop = false;
    public float speed = 1;
    public float finish_posX = 100;

    private bool old_stopState;
    private float percent;
    private float _speed;
    private Transform thisTransform;

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        _speed = speed;
        EventController.setCamSpeed(speed);
        old_stopState = !stop;        
    }
	
	// Update is called once per frame
	void Update () {        
        percentageCalculate();
        sendPercent();

        if (percent >= 100)
        {
            stop = true;
        }

        if (stop != old_stopState)
        {
            changeSpeed(stop);
            old_stopState = stop;
        }

        moveTo();
	}

    private void changeSpeed(bool isStop)
    {
        if (isStop)
        {
            speed = 0;
            EventController.setCamSpeed(0);
        }
        else
        {
            speed = _speed;
            EventController.setCamSpeed(speed);
        }
    }

    private void moveTo()
    {
        if (thisTransform.position.x < finish_posX)
        {
            thisTransform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    private void percentageCalculate()
    {
        percent = (thisTransform.position.x / finish_posX) * 100;
    }

    private void sendPercent()
    {
        EventController.setLevelProgress(percent);
    }
}
