using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Texture progres_border;
    public Texture progres_line;

    private float percent;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        moveTo(100);
        percentageShow(100);

        if (percent >= 100)
        {
            Time.timeScale = 0;
        }

        if (Input.GetAxisRaw("Menu") != 0)
        {
            Application.Quit();
        }
	}   

    void OnGUI()
    {
        //31 * 7 --x10-- 310*70
        //29 * 5 --x10-- 290*50
        float size_mult = (Screen.height / 720f);

        float scale = 5f;
        float difference = 1f * scale;
        float bottom_space = 8f;

        float border_x = progres_border.width * scale;
        float border_y = progres_border.height * scale;
        float line_x = progres_line.width * scale;
        float line_y = progres_line.height * scale;
        
        guiDrawProgresLine(
            (Screen.width - (size_mult * line_x)) / 2f,
            size_mult * (720f - (line_y + difference)  - bottom_space),
            size_mult * line_x,
            size_mult * line_y
            );

        guiDrawProgresBorder(
            (Screen.width - (size_mult * border_x))/2f,
            size_mult * (720f - border_y - bottom_space),
            size_mult * border_x,
            size_mult * border_y
            );
    }

    void guiDrawProgresLine(float startX, float startY, float width, float height)
    {
        GUI.DrawTextureWithTexCoords(new Rect(startX, startY, (width / 100.0f * percent), height),
            progres_line,
            new Rect(0f, 0f, percent / 100, 1f));
    }

    void guiDrawProgresBorder(float startX, float startY, float width, float height)
    {
        GUI.DrawTexture(new Rect(startX, startY, width, height),
            progres_border);
    }

    void moveTo(int finish_Point_X = 10)
    {
        if (this.transform.position.x < finish_Point_X)
        {
            this.transform.position += new Vector3(Time.deltaTime, 0, 0);
        }
    }

    void percentageShow(int finish_Point_X = 10)
    {
        percent = (this.transform.position.x / finish_Point_X) * 100;
    }
}
