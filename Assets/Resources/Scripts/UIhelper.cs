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

    protected int screen_width = 1;
    protected int screen_height = 1;
    //

    /// <summary>
    /// Set texture
    /// </summary>
    /// <param name="txt">Texture</param>
    /// <param name="x_fromBottom">Alignment bottom</param>
    /// <param name="y_fromRight">Alignment right</param>
    public Image texture(Texture txt, bool align_Bottom = false, bool align_Right = false)
    {
        return new Image(txt, align_Bottom, align_Right, scale, screen_height, screen_width);
    }

    /// <summary>
    /// Set label text
    /// </summary>
    public Label label(string text)
    {
        return new Label(text, scale);
    }

    /// <summary>
    /// Set textarea
    /// </summary>
    public TextArea textArea()
    {
        return new TextArea(scale);
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
}

public class Image
{
    private Texture texture;
    private float scale;
    private bool align_bottom;
    private bool align_right;
    private int screen_width;
    private int screen_height;

    public Image(Texture txt, bool align_Bottom, bool align_Right, float scale_arg, int scr_height, int scr_width)
    {
        texture = txt;
        scale = scale_arg;
        align_bottom = align_Bottom;
        align_right = align_Right;
        screen_height = scr_height;
        screen_width = scr_width;
    }
    ~Image() { }
    
    /// <summary>
    /// Draw texture at (startX;startY)
    /// </summary>
    public void Draw(int startX, int startY)
    {
        DrawAndResizeBody(texture, startX, startY, 0, 0);
    }

    /// <summary>
    /// Draw texture at (startX;startY) with new width and height
    /// </summary>
    public void DrawAndResize(int startX, int startY, int width = 0, int height = 0)
    {
        DrawAndResizeBody(texture, startX, startY, width, height);
    }

    /// <summary>
    /// Draw texture at (startX;startY) and crop it by percentage
    /// </summary>
    public void DrawPartially(int startX, int startY, float percent)
    {
        DrawAndResizePartiallyBody(texture, startX, startY, 0, 0, percent, true, false);
    }
    public void DrawPartially(int startX, int startY, float percent, bool horizontal, bool vertical)
    {
        DrawAndResizePartiallyBody(texture, startX, startY, 0, 0, percent, horizontal, vertical);
    }

    /// <summary>
    /// Draw texture at (startX;startY) with new width and height and crop it by percentage
    /// </summary>
    public void DrawAndResizePartially(int startX, int startY, int width, int height, float percent)
    {
        DrawAndResizePartiallyBody(texture, startX, startY, width, height, percent, true, false);
    }
    public void DrawAndResizePartially(int startX, int startY, int width, int height, float percent, bool horizontal, bool vertical)
    {
        DrawAndResizePartiallyBody(texture, startX, startY, width, height, percent, horizontal, vertical);
    }

    /// <summary>
    /// Draw 4-side border and progres-line within, cropped by percentage
    /// </summary>
    public void DrawProgresbar(Texture lineTexture, int startX, int startY, float percent)
    {
        DrawProgresbarBody(texture, lineTexture, startX, startY, percent, true, false);
    }
    public void DrawProgresbar(Texture lineTexture, int startX, int startY, float percent, bool horizontal = true, bool vertical = false)
    {
        DrawProgresbarBody(texture, lineTexture, startX, startY, percent, horizontal, vertical);
    }

    //Private:
    private void DrawAndResizeBody(Texture txt, int startX, int startY, int width, int height)
    {
        int new_width = txt.width;
        int new_height = txt.height;

        if (width > 0)
        {
            new_width = width;
        }
        if (height > 0)
        {
            new_height = height;
        }

        int new_startX = startX;
        int new_startY = startY;

        if (align_bottom)
        {
            new_startY = screen_height - startY - new_height;            
        }
        if (align_right)
        {
            new_startX = screen_width - startX - new_width;
        }

        GUI.DrawTexture(new Rect(
            new_startX * scale,
            new_startY * scale,
            new_width * scale,
            new_height * scale),
            txt, ScaleMode.StretchToFill);
    }
    private void DrawAndResizePartiallyBody(Texture txt, int startX, int startY, int width, int height, float percent, bool horizontal, bool vertical)
    {
        int new_startX = startX;
        int new_startY = startY;

        int new_width = txt.width;
        int new_height = txt.height;

        if (width > 0)
        {
            new_width = width;
        }
        if (height > 0)
        {
            new_height = height;
        }

        float pos_w = new_width;
        float pos_h = new_height;

        float texC_w = 1f;
        float texC_h = 1f;

        Rect pos;
        Rect texC;

        if (align_bottom)
        {
            new_startY = screen_height - startY - new_height;            
        }
        if (align_right)
        {
            new_startX = screen_width - startX - new_width;
        }

        if (horizontal)
        {
            pos_w = (new_width * percent / 100.0f);
            texC_w = percent / 100;
        }
        if (vertical)
        {
            pos_h = (new_height * percent / 100.0f);
            texC_h = percent / 100;
        }

        pos = new Rect(
            new_startX * scale,
            new_startY * scale,
            pos_w * scale,
            pos_h * scale);

        texC = new Rect(0f, 0f, texC_w, texC_h);

        GUI.DrawTextureWithTexCoords(pos, txt, texC);
    }
    private void DrawProgresbarBody(Texture txt, Texture lineTexture, int startX, int startY, float percent, bool horizontal, bool vertical)
    {
        int differenceX = (txt.width > lineTexture.width ? ((txt.width - lineTexture.width) / 2) : 0);
        int differenceY = (txt.height > lineTexture.height ? ((txt.height - lineTexture.height) / 2) : 0);

        DrawAndResizeBody(txt, startX, startY, 0, 0);
        DrawAndResizePartiallyBody(lineTexture, startX + differenceX, startY + differenceY, 0, 0, percent, horizontal, vertical);
    }
}

public class Label
{
    private float scale;
    private string text;
    private GUIStyle style = new GUIStyle();

    public Label(string label_arg, float scale_arg)
    {
        text = label_arg;
        scale = scale_arg;

        style.fontSize = (int)(2 * scale);
        style.normal.textColor = Color.white;
    }
    ~Label() { }

    /// <summary>
    /// Draw text label
    /// </summary>
    public void Draw(int startX, int startY, int font_size)
    {
        style.fontSize = (int)(font_size * scale);
        GUI.Label(new Rect(startX * scale, startY * scale, 1 * scale, 1 * scale), text, style);

        //Vector2 size = style.CalcSize(new GUIContent(text));
        //Debug.Log(size/scale + " " + text);
    }
    public void Draw(int startX, int startY, int width, int height)
    {
        GUI.Label(new Rect(startX * scale, startY * scale, width * scale, height * scale), text, style);
    }

    /// <summary>
    /// To load custom font use: Font font = Resources.Load("myFontName") as Font;
    /// </summary>
    public void setStyle(GUIStyle new_style)
    {
        style = new_style;
        style.fontSize = (int)(style.fontSize * scale);
    }
}

public class TextArea
{
    private float scale;

    private GUIStyle style = new GUIStyle(GUI.skin.textArea);

    public TextArea(float scale_arg)
    {
        scale = scale_arg;
        style.fontSize = (int)(4 * scale);
    }
    ~TextArea() { }

    public void Draw(ref string text, int startX, int startY, int width, int height)
    {
        text = GUI.TextArea(new Rect(startX * scale, startY * scale, width * scale, height * scale), text, style);
    }
    public void Draw(ref string text, int startX, int startY, int width, int height, int font_scale)
    {
        style.fontSize = (int)(font_scale * scale);
        text = GUI.TextArea(new Rect(startX * scale, startY * scale, width * scale, height * scale), text, style);
    }

    /// <summary>
    /// To load custom font use: Font font = Resources.Load("myFontName") as Font;
    /// </summary>
    public void setStyle(GUIStyle new_style)
    {
        style = new_style;
        style.fontSize = (int)(style.fontSize * scale);
    }    
}