using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS {

    public float fpsUpdateInterval;
    public bool useInvoke;
    public bool printAvgFps;
    public Vector2 pos;

    public int fps;

    private int fpsAvg;
    private float fpsLastUpdate;
    private float fpsLastFrame;

    private GUIStyle style;

    void Start()
    {
        fpsUpdateInterval = 1.0f;
        useInvoke = true;
        printAvgFps = false;
        fps = 60;
        fpsAvg = 60;
        fpsLastUpdate = 0.0f;
        fpsLastFrame = 0.0f;

    }

    void UpdateFps()
    {
        fps = Mathf.RoundToInt((Time.frameCount - fpsLastFrame) / (Time.time - fpsLastUpdate));
        fpsAvg = Mathf.RoundToInt(Time.frameCount / Time.time);
        fpsLastUpdate = Time.time;
        fpsLastFrame = Time.frameCount;
    }

    void FixedUpdate()
    {
        if (useInvoke)
            return;

        if (fpsLastUpdate + fpsUpdateInterval < Time.time)
            UpdateFps();
    }
}
