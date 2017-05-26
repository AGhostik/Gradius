using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : AnimatedObject {
    
    [Header("Following")]
    public Transform parentTransform;
    public float posAccuracy = 0.1f;
    public int posListLength = 20;
    
    private Transform thisTransform;    
    private Vector3 distance;
    private Vector3 oldParentPos;

    private List<Vector3> posList = new List<Vector3>();

    // Use this for initialization
    void Start () {
        Amination_OnStart();
        thisTransform = transform;
        oldParentPos = parentTransform.position;
    }

    // Update is called once per frame
    void Update () {
        if (parentTransform != null)
        {
            followTheParent();
        }
        else
        {
            Destroy(gameObject);
        }

        timerAnimation();
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
