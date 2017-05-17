using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIhelper
{

    public UIhelper()
    {
    }
    public UIhelper(float ui_scale)
    {
        if (ui_scale > 0)
        {
            scale = ui_scale;
        }
    }
    public UIhelper(int width, int height)
    {
        if (height > 0)
        {
            screen_height = height;
        }
        if (width > 0)
        {
            screen_width = width;
        }

        if (screen_height < screen_width)
        {
            scale = (Screen.height / (float)screen_height);
        }
        else
        {
            scale = (Screen.width / (float)screen_width);
        }
    }
    ~UIhelper()
    {
    }

    //var
    protected float scale = 1f;

    private int screen_width = 1;
    private int screen_height = 1;
    //
    
    public Image texture(Texture txt)
    {
        return new Image(txt, scale);
    }
    public Label label(string text)
    {
        return new Label(text, scale);
    }

    //get-set
    public float get_ScaleMultiplier()
    {
        return scale;
    }
    public void set_ScaleMultiplier(float new_scale)
    {
        if (new_scale > 0)
        {
            scale = new_scale;
        }
    }

    public int get_ScreenWidth()
    {
        return screen_width;
    }
    public int get_ScreenHeight()
    {
        return screen_height;
    }

    /// <summary>
    /// Draw 4-side border and progres-line within, cropped by percentage
    /// </summary>
    public void ProgresbarDraw(Texture border, Texture lineWithinBorder, int startX, int startY, float percent)
    {
        int differenceX = (border.width > lineWithinBorder.width ? ((border.width - lineWithinBorder.width) / 2) : 0);
        int differenceY = (border.height > lineWithinBorder.height ? ((border.height - lineWithinBorder.height) / 2) : 0);

        Image bord = new Image(border, scale);
        Image lin = new Image(lineWithinBorder, scale);

        bord.Draw(startX, startY);

        lin.DrawPartially(startX + differenceX, startY + differenceY, percent);
    }

    public void ProgresbarDraw(Texture border, Texture lineWithinBorder, int startX, int startY, float percent, bool horizontal = true, bool vertical = false)
    {
        int differenceX = (border.width > lineWithinBorder.width ? ((border.width - lineWithinBorder.width) / 2) : 0);
        int differenceY = (border.height > lineWithinBorder.height ? ((border.height - lineWithinBorder.height) / 2) : 0);

        Image bord = new Image(border, scale);
        Image lin = new Image(border, scale);

        bord.Draw(startX, startY);

        lin.DrawPartially(startX + differenceX, startY + differenceY, percent, horizontal, vertical);        
    }
}

public class Image
{
    private Texture texture;
    private float scale;

    public Image(Texture txt, float scale_arg)
    {
        texture = txt;
        scale = scale_arg;
    }
    ~Image() { }

    /// <summary>
    /// Draw texture at (startX;startY)
    /// </summary>
    public void Draw(int startX, int startY)
    {
        GUI.DrawTexture(new Rect(
            startX * scale,
            startY * scale,
            texture.width * scale,
            texture.height * scale),
            texture, ScaleMode.StretchToFill);
    }

    /// <summary>
    /// Draw texture at (startX;startY) with new width and height
    /// </summary>
    public void DrawAndResize(int startX, int startY, int width = 0, int height = 0)
    {
        int new_width = texture.width;
        int new_height = texture.height;

        if (width > 0)
        {
            new_width = width;
        }
        if (height > 0)
        {
            new_height = height;
        }

        GUI.DrawTexture(new Rect(
            startX * scale,
            startY * scale,
            new_width * scale,
            new_height * scale),
            texture, ScaleMode.StretchToFill);
    }

    /// <summary>
    /// Draw texture at (startX;startY) and crop it by percentage
    /// </summary>
    public void DrawPartially(int startX, int startY, float percent)
    {
        Rect pos;
        Rect texC;

        pos = new Rect(
            startX * scale,
            startY * scale,
            (texture.width / 100.0f * percent) * scale,
            texture.height * scale);

        texC = new Rect(0f, 0f, percent / 100, 1f);

        GUI.DrawTextureWithTexCoords(pos, texture, texC);
    }
    public void DrawPartially(int startX, int startY, float percent, bool horizontal, bool vertical)
    {
        Rect pos;
        Rect texC;
        int pos_w = texture.width;
        int pos_h = texture.height;
        float texC_w = 1f;
        float texC_h = 1f;

        if (horizontal)
        {
            pos_w = (int)(texture.width / 100 * percent);
            texC_w = percent / 100;
        }
        if (vertical)
        {
            pos_h = (int)(texture.height / 100.0f * percent);
            texC_h = percent / 100;
        }

        pos = new Rect(
            startX * scale,
            startY * scale,
            pos_w * scale,
            pos_h * scale);

        texC = new Rect(0f, 0f, texC_w, texC_h);

        GUI.DrawTextureWithTexCoords(pos, texture, texC);
    }

    /// <summary>
    /// Draw texture at (startX;startY) with new width and height and crop it by percentage
    /// </summary>
    public void DrawAndResizePartially(int startX, int startY, int width, int height, float percent)
    {
        Rect pos;
        Rect texC;

        pos = new Rect(
            startX * scale,
            startY * scale,
            (width / 100.0f * percent) * scale,
            height * scale);

        texC = new Rect(0f, 0f, percent / 100, 1f);

        GUI.DrawTextureWithTexCoords(pos, texture, texC);
    }
    public void DrawAndResizePartially(int startX, int startY, int width, int height, float percent, bool horizontal, bool vertical)
    {
        Rect pos;
        Rect texC;
        int pos_w = width;
        int pos_h = height;
        float texC_w = 1f;
        float texC_h = 1f;

        if (horizontal)
        {
            pos_w = (int)(width / 100.0f * percent);
            texC_w = percent / 100;
        }
        if (vertical)
        {
            pos_h = (int)(height / 100.0f * percent);
            texC_h = percent / 100;
        }

        pos = new Rect(
            startX * scale,
            startY * scale,
            pos_w * scale,
            pos_h * scale);

        texC = new Rect(0f, 0f, texC_w, texC_h);

        GUI.DrawTextureWithTexCoords(pos, texture, texC);
    }
}

public class Label
{
    private float scale;
    private string text;
    private GUIStyle style;

    public Label(string label_arg, float scale_arg)
    {
        text = label_arg;
        scale = scale_arg;

        style = new GUIStyle();
        style.fontSize = (int)(5 * scale);
        style.normal.textColor = Color.white;
        //style.font = Resources.Load("FontS") as Font;
    }
    ~Label() { }

    /// <summary>
    /// Draw text label
    /// </summary>
    public void Draw(int startX, int startY, int width = 1, int height = 1)
    {
        GUI.Label(new Rect(startX * scale, startY * scale, width * scale, height * scale), text, style);
    }

    public void setStyle()
    {
        
    }
}
