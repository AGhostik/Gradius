using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        moveTo(100);
        percentageShow(100);
	}

    void moveTo (int finish_Point_X = 10) {
        if (this.transform.position.x < finish_Point_X)
        {
            this.transform.position += new Vector3(Time.deltaTime,0,0);
        }
    }

    void percentageShow(int finish_Point_X = 10)
    {
        float percent = (this.transform.position.x / finish_Point_X) * 100;
       // Debug.Log(percent + " %");
    }
}
