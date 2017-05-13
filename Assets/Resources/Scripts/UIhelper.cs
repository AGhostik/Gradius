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
    private float scale = 1f;

    private int screen_width = 1;
    private int screen_height = 1;
    //
    


    //get-set
    public float getScaleMultiplier()
    {
        return scale;
    }
    public void setScaleMultiplier(float new_scale)
    {
        if (new_scale > 0)
        {
            scale = new_scale;
        }
    }

    public int getScreenWidth_int()
    {
        return screen_width;
    }
    public int getScreenHeight_int()
    {
        return screen_height;
    }

    public float getScreenWidth()
    {
        return screen_width;
    }
    public float getScreenHeight()
    {
        return screen_height;
    }    
  
    public void TextureDraw(Texture texture, float startX, float startY)
    {
        GUI.DrawTexture(new Rect(
            startX * scale,
            startY * scale,
            texture.width * scale,
            texture.height * scale),
            texture, ScaleMode.StretchToFill);
    }

    public void TextureDrawAndResize(Texture texture, float startX, float startY, float width = 0, float height = 0)
    {
        float new_width = texture.width;
        float new_height = texture.height;

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

    public void TextureDrawPartially(Texture texture, float percent, float startX, float startY)
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
    public void TextureDrawPartially(Texture texture, float percent, float startX, float startY, bool horizontal, bool vertical)
    {
        Rect pos;
        Rect texC;
        float pos_w = texture.width;
        float pos_h = texture.height;
        float texC_w = 1f;
        float texC_h = 1f;

        if (horizontal)
        {
            pos_w = (texture.width / 100.0f * percent);
            texC_w = percent / 100;
        }
        if (vertical)
        {
            pos_h = (texture.height / 100.0f * percent);
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

    public void TextureDrawAndResizePartially(Texture texture, float percent, float startX, float startY, float width, float height)
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
    public void TextureDrawAndResizePartially(Texture texture, float percent, float startX, float startY, float width, float height, bool horizontal, bool vertical)
    {
        Rect pos;
        Rect texC;
        float pos_w = width;
        float pos_h = height;
        float texC_w = 1f;
        float texC_h = 1f;

        if (horizontal)
        {
            pos_w = (width / 100.0f * percent);
            texC_w = percent / 100;
        }
        if (vertical)
        {
            pos_h = (height / 100.0f * percent);
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
    /// Draw 4-side border and progres-line within, cropped by percentage
    /// </summary>
    public void ProgresbarDraw(Texture border, Texture lineWithinBorder, float percent, float startX, float startY)
    {
        float differenceX = (border.width > lineWithinBorder.width ? ((border.width - lineWithinBorder.width) / 2) : 0);
        float differenceY = (border.height > lineWithinBorder.height ? ((border.height - lineWithinBorder.height) / 2) : 0);
        
        TextureDraw(border,
           startX,
           startY
           );

        TextureDrawPartially(lineWithinBorder,
            percent,
            startX + differenceX,
            startY + differenceY
            );
    }

    public void ProgresbarDraw(Texture border, Texture lineWithinBorder, float percent, float startX, float startY, bool horizontal = true, bool vertical = false)
    {
        float differenceX = (border.width > lineWithinBorder.width ? ((border.width - lineWithinBorder.width) / 2) : 0);
        float differenceY = (border.height > lineWithinBorder.height ? ((border.height - lineWithinBorder.height) / 2) : 0);
        
        TextureDrawPartially(lineWithinBorder,
            percent,
            startX + differenceX,
            startY + differenceY,
            horizontal,
            vertical
            );

        TextureDraw(border,
            startX,
            startY
            );
    }

    /// <summary>
    /// Draw text label
    /// </summary>
    public void DrawLabel()
    {
        /*
         * style = new GUIStyle();
        style.fontSize =(int)( 16 * Global.gui_scale);
        style.normal.textColor = Color.white;
        style.font = Resources.Load("FontS") as Font;
         * 
         * GUI.Label(new Rect((Screen.width - 592 * Global.gui_scale)/2 + 308 * Global.gui_scale,(Screen.height - 512 * Global.gui_scale)/2 + 42 * Global.gui_scale,200 * Global.gui_scale,80 * Global.gui_scale),"Защита: " + mi.all_items[cursoritem].GetComponent<Item>().Protection, style);
         */
    }
}
