using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour {

    [Header("Animation")]
    public Sprite[] frames = new Sprite[1];

    [Header("Following")]
    public GameObject parent;
    public float posAccuracy = 0.1f;
    public int posListLength = 20;

    private int current_frame = 0;
    private float timer_start = 0.5f;
    private float timer;
    private SpriteRenderer render;
    private Transform thisTransform;
    private Transform parentTransform;
    private Vector3 distance;
    private Vector3 olpParentPos;

    private List<Vector3> posList = new List<Vector3>();

    // Use this for initialization
    void Start () {
        thisTransform = transform;
        render = gameObject.GetComponent<SpriteRenderer>();

        parentTransform = parent.transform;
        olpParentPos = parentTransform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (parent != null)
        {
            //thisTransform.position += new Vector3(Time.deltaTime, 0, 0);
            olpParentPos += new Vector3(Time.deltaTime, 0, 0);

            updateList();
            Anim();
            followTheParent();
        }
        else
        {
            Destroy(gameObject);
        }
	}

    private void updateList()
    {
        for (int i = 0; i < posList.Count; i++)
        {
            posList[i] += new Vector3(Time.deltaTime, 0, 0);
        }
    }

    private void followTheParent()
    {
        distance = parentTransform.position - olpParentPos;

        if (distance.magnitude >= posAccuracy)
        {
            if (posList.Count == posListLength)
            {
                thisTransform.position = posList[0];
                posList.RemoveAt(0);                
            }

            posList.Add(parentTransform.position);
            olpParentPos = parentTransform.position;
        }
    }

    private void Anim()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (current_frame < frames.Length - 1)
            {
                current_frame++;
            }
            else
            {
                current_frame = 0;
            }
            changeFrame(current_frame);
            timer = timer_start;
        }
    }

    private void changeFrame(int frame_number = 0)
    {
        render.sprite = frames[frame_number];
    }
}
