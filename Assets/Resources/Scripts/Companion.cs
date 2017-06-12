using System.Collections.Generic;
using UnityEngine;

public class Companion : AnimatedObject {
    
    [Header("Following")]
    public Transform ParentTransform;
    public float posAccuracy = 0.1f;
    public int posListLength = 20;
    
    private Transform thisTransform;    
    private Vector3 distance;
    private Vector3 oldParentPos;

    private List<Vector3> posList = new List<Vector3>();
    
    protected override void Awake()
    {
        base.Awake();
        thisTransform = transform;        
    }

    private void Start()
    {
        oldParentPos = ParentTransform.position;
    }

    void Update ()
    {
        if (ParentTransform != null)
        {
            FollowTheParent();
        }
        else
        {
            Destroy(gameObject);
        }

        TimerAnimation();
	}

    private void FollowTheParent()
    {
        distance = ParentTransform.position - oldParentPos;

        if (distance.magnitude >= posAccuracy)
        {
            if (posList.Count == posListLength)
            {
                thisTransform.position = posList[0];
                posList.RemoveAt(0);
            }

            posList.Add(ParentTransform.position);
            oldParentPos = ParentTransform.position;
        }
    }
}
