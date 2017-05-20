using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : AnimatedObject {
    
    [Header("Following")]
    public GameObject parent;
    public float posAccuracy = 0.1f;
    public int posListLength = 20;

    private float camSpeed;
    private Transform thisTransform;
    private Transform parentTransform;
    private Vector3 distance;
    private Vector3 oldParentPos;

    private List<Vector3> posList = new List<Vector3>();

    // Use this for initialization
    void Start () {
        thisTransform = transform;

        parentTransform = parent.transform;
        oldParentPos = parentTransform.position;
    }

    private void OnEnable()
    {
        EventController.UpdateCameraSpeed += takeCamSpeed;
    }

    private void OnDisable()
    {
        EventController.UpdateCameraSpeed -= takeCamSpeed;
    }

    // Update is called once per frame
    void Update () {
        if (parent != null)
        {
            if (camSpeed > 0)
            {
                updateList();

                oldParentPos += new Vector3(camSpeed * Time.deltaTime, 0, 0);
            }

            followTheParent();
        }
        else
        {
            Destroy(gameObject);
        }
	}

    private void takeCamSpeed(float value)
    {
        camSpeed = value;
    }

    private void updateList()
    {
        for (int i = 0; i < posList.Count; i++)
        {
            posList[i] += new Vector3(camSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void followTheParent()
    {
        distance = parentTransform.position - oldParentPos;

        if (distance.magnitude >= posAccuracy)
        {
            if (posList.Count == posListLength)
            {
                thisTransform.position = posList[0];
                posList.RemoveAt(0);                
            }

            posList.Add(parentTransform.position);
            oldParentPos = parentTransform.position;
        }
    }
}
