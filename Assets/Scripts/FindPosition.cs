using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPosition : MonoBehaviour
{
    float footPosition;
    float hitPose_y;

    public GameObject other;

    void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == "left_foot")
            {
                footPosition = child.position[1];
                //Debug.Log("Child found. Name: " + child.name + " > global pos: " + child.position);
            }
        }

        hitPose_y = GameObject.Find("Interaction").transform.GetComponent<ARTapToPlaceObject>().plane_y;

        if (hitPose_y != 0f)
        {
            transform.localPosition = new Vector3(0, hitPose_y - footPosition, 0);

        }
    }
}
