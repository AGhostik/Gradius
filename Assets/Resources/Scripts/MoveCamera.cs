using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Texture progres_border;
    public Texture progres_line;

    private float percent;

    // Use this for initialization
    void Start () {
        if (Screen.fullScreen)
        {
            Cursor.visible = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        moveTo(100);
        percentageCalculate(100);

        if (percent >= 100)
        {
            Time.timeScale = 0;
        }

        if (Input.GetAxisRaw("Menu") != 0)
        {
            Application.Quit();
        }
	}   

    void OnGUI() {
        UIhelper drawUI = new UIhelper(256, 144);

        drawUI.ProgresbarDraw(progres_border, progres_line, percent, 2, 2);        
    }    

    void moveTo(int finish_Point_X = 10)
    {
        if (transform.position.x < finish_Point_X)
        {
            transform.position += new Vector3(Time.deltaTime, 0, 0);
        }
    }

    void percentageCalculate(int finish_Point_X = 10)
    {
        percent = (transform.position.x / finish_Point_X) * 100;
    }
}
