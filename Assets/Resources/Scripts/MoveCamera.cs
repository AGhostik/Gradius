using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public float finish_posX = 100;
    public Texture progres_border;
    public Texture progres_line;

    private float percent;
    private Transform thisTransform;

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
        moveTo();
        percentageCalculate();

        if (percent >= 100)
        {
            Time.timeScale = 0;
        }

        if (Input.GetAxisRaw("Menu") != 0)
        {
            Application.Quit();
        }
	}   

    //удалить
    void OnGUI() {
        UIhelper drawUI = new UIhelper(256, 144);

        drawUI.ProgresbarDraw(progres_border, progres_line, 2, 2, percent);
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
