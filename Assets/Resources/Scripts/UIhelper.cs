using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIhelper {

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

    //
    private float scale = 1f;


    public int screen_width = 1;
    public int screen_height = 1;
    //

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
    
    public void TextureDraw(Texture texture, float startX, float startY, float width = 0, float height = 0, float sizeMultiplier = 1f)
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
            new_width * scale * sizeMultiplier,
            new_height * scale * sizeMultiplier),
            texture, ScaleMode.StretchToFill);
    }

    public void TextureDrawPartially(Texture texture, float percent, float startX, float startY, float width, float height, float sizeMultiplier = 1f)
    {
        Rect pos;
        Rect texC;

        pos = new Rect(
            startX * scale,
            startY * scale,
            (width / 100.0f * percent) * scale * sizeMultiplier,
            height * scale * sizeMultiplier);

        texC = new Rect(0f, 0f, percent / 100, 1f);

        GUI.DrawTextureWithTexCoords(pos, texture, texC);
    }
    public void TextureDrawPartially(Texture texture, float percent ,float startX, float startY, float width, float height, bool horizontal, bool vertical, float sizeMultiplier = 1f)
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
            pos_w * scale * sizeMultiplier,
            pos_h * scale * sizeMultiplier);

        texC = new Rect(0f, 0f, texC_w, texC_h);
        //
        //horizontal sample:
        //pos = new Rect(startX, startY, (width / 100.0f * percent), height);
        //texC = new Rect(0f, 0f, percent / 100, 1f);
        //
        GUI.DrawTextureWithTexCoords(pos, texture, texC);
    }


    /// <summary>
    /// Draw 4-side border and progres-line within, cropped by percentage
    /// </summary>
    public void ProgresbarDraw(Texture border, Texture lineWithinBorder, float percent, float startX, float startY, float sizeMultiplier = 1)
    {
        float differenceX = (border.width > lineWithinBorder.width ? ((border.width - lineWithinBorder.width) / 2) * sizeMultiplier : 0);
        float differenceY = (border.height > lineWithinBorder.height ? ((border.height - lineWithinBorder.height) / 2) * sizeMultiplier : 0);
        
        float border_x = border.width * sizeMultiplier;
        float border_y = border.height * sizeMultiplier;
        float line_x = lineWithinBorder.width * sizeMultiplier;
        float line_y = lineWithinBorder.height * sizeMultiplier;
        
        TextureDraw(border,
           startX,
           startY,
           border_x,
           border_y
           );

        TextureDrawPartially(lineWithinBorder,
            percent, 
            startX + differenceX, 
            startY + differenceY,
            line_x,
            line_y
            );
    }

    public void ProgresbarDraw(Texture border, Texture lineWithinBorder, float percent, float startX, float startY, bool horizontal = true, bool vertical = false, float sizeMultiplier = 1)
    {
        float differenceX = (border.width > lineWithinBorder.width ? ((border.width - lineWithinBorder.width) / 2) * sizeMultiplier : 0);
        float differenceY = (border.height > lineWithinBorder.height ? ((border.height - lineWithinBorder.height) / 2) * sizeMultiplier : 0);

        float border_x = border.width * sizeMultiplier;
        float border_y = border.height * sizeMultiplier;
        float line_x = lineWithinBorder.width * sizeMultiplier;
        float line_y = lineWithinBorder.height * sizeMultiplier;

        TextureDrawPartially(lineWithinBorder,
            percent,
            startX + differenceX, 
            startY + differenceY,
            line_x,
            line_y, 
            horizontal,
            vertical
            );

        TextureDraw(border, 
            startX,
            startY,
            border_x,
            border_y
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
