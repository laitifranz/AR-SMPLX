using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using System;
using UnityEngine.UI;
using TMPro;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    public GameObject textToHide;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool firstTimeTouch = true;
    public float plane_y = 0f;

    private int value_received = -1;
    private float distance_value = 0f;
    private float DIFF_MAX_DISTANCE = 0f;

    public Button warmUpButton;
    public Button ex1Button;
    public Button ex2Button;
    public Button stretchingButton;


    public TMP_Text distance_text;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        raycastManager = FindObjectOfType<ARRaycastManager>();

        warmUpButton.GetComponent<Button>().onClick.AddListener(() => { SetValue(0); });
        ex1Button.GetComponent<Button>().onClick.AddListener(() => { SetValue(1); });
        ex2Button.GetComponent<Button>().onClick.AddListener(() => { SetValue(2); });
        stretchingButton.GetComponent<Button>().onClick.AddListener(() => { SetValue(3); });

        distance_text.text = "Distance";
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (firstTimeTouch) { 
            if(placementPoseIsValid && Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began && value_received != -1)
            {
                firstTimeTouch = false;
                PlaceObject();
            }
        }
    }

    private void SetValue(int value)
    {
        value_received = value;

        switch (value)
        {
            case 0:
                warmUpButton.interactable = false;
                ex1Button.gameObject.SetActive(false);
                ex2Button.gameObject.SetActive(false);
                stretchingButton.gameObject.SetActive(false);
                break;
            case 1:
                ex1Button.interactable = false;
                warmUpButton.gameObject.SetActive(false);
                ex2Button.gameObject.SetActive(false);
                stretchingButton.gameObject.SetActive(false);
                break;
            case 2:
                ex2Button.interactable = false;
                warmUpButton.gameObject.SetActive(false);
                ex1Button.gameObject.SetActive(false);
                stretchingButton.gameObject.SetActive(false);
                break;
            case 3:
                stretchingButton.interactable = false;
                warmUpButton.gameObject.SetActive(false);
                ex1Button.gameObject.SetActive(false);
                ex2Button.gameObject.SetActive(false);
                break;
        }
    }

    private void PlaceObject()
    {
        plane_y = placementPose.position[1];
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);

        DIFF_MAX_DISTANCE = GameObject.FindWithTag("smplx").transform.GetComponent<SMPLX>().DIFF_MAX_DISTANCE;

        Debug.Log("value button: " + value_received);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            if (firstTimeTouch) {
                textToHide.SetActive(true);
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            else {
                textToHide.SetActive(false);
                placementIndicator.SetActive(false);
                ReceiveValueDistance();
            }
        }
        else
        {
            textToHide.SetActive(false);
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            //just for the indicator to move it around
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void ReceiveValueDistance()
    {
        distance_value = GameObject.FindWithTag("smplx").transform.GetComponent<SMPLX>().diff;
        if (distance_value != 0f)
            if (distance_value >= DIFF_MAX_DISTANCE)
                distance_text.color = Color.red;
            else
                distance_text.color = Color.green;
            distance_text.text = Math.Round(distance_value, 2).ToString() + " m";
    }
}
