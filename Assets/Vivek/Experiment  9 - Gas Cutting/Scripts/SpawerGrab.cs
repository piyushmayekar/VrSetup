using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawerGrab : XRGrabInteractable
{
    List<InputDevice> leftHandDevice,rightHandDevice; 
    public Transform grabObject,leftHand,rightHand;
    public string selectObjectName;
    //private Vector3 handPosition;
    private Rigidbody rBody;

    private void Start()
    {
        leftHandDevice = new List<InputDevice>();
        rightHandDevice = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevice);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevice);
    }
    public void SetPositionAsPerHand()
    {
        leftHandDevice[0].TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip);
        rightHandDevice[0].TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip);

        print("Exited..........");
        rBody = grabObject.GetComponent<Rigidbody>();
        rBody.velocity = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;
        GameObject g = grabObject.GetChild(1).gameObject;
        g.transform.parent = leftHand;
        g.transform.position = g.transform.parent.position;
        g.transform.rotation = g.transform.parent.rotation;
        /*if (selectingInteractor.name == "Left Hand")
        {
            g.transform.parent = leftHand;
            g.transform.position = g.transform.parent.position;
            g.transform.rotation = g.transform.parent.rotation;
        }
        else if(selectingInteractor.name == "Right Hand")
        {
            g.transform.parent = rightHand;
            g.transform.position = g.transform.parent.position;
            g.transform.rotation = g.transform.parent.rotation;
        }*/
    }
    public void SetHandOnEnter()
    {
        leftHandDevice[0].TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip);
        rightHandDevice[0].TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip);
        //leftHand.GetChild(0).transform.parent = grabObject.transform;
        print("Enter.............");
        leftHand.GetChild(0).transform.parent = grabObject.transform;
        /*if (selectingInteractor.name == "Left Hand")
        {
            //if(leftGrip)
                
        }
        else if(selectingInteractor.name == "Right Hand")
        {
            if (rightGrip)
                rightHand.GetChild(0).transform.parent = grabObject.transform;
        }*/
        /*if (lHand.selectTarget.name == selectObjectName)
        {
            print("Left Hand Entered...........");
           
        }
        else if(rHand.selectTarget.name == selectObjectName)
        {
            print("Right Hand Entered...........");
        }*/
    }
    
}