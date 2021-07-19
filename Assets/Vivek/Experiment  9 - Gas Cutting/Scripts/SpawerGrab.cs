using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static PiyushUtils.CustomXRGrabInteractable;

public class SpawerGrab : XRGrabInteractable
{
    List<InputDevice> leftHandDevice,rightHandDevice; 
    public Transform grabObject,leftHand,rightHand,leftOriginalParent,rightOriginalParent;
    public string selectObjectName;
    //private Vector3 handPosition;
    private Rigidbody rBody;

    public Collider sphereofSpanner;
    XRBaseInteractor Maininteractor;

    [SerializeField] internal InteractorType grabbedInteractorType = InteractorType.None;

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
        if(g.name == "HandPresenceLeft")
        {
            g.transform.parent = leftOriginalParent;
            g.transform.position = g.transform.parent.position;
            g.transform.rotation = g.transform.parent.rotation;

        }
        else if(g.name == "HandPresenceRight")
        {
            g.transform.parent = rightOriginalParent;
            g.transform.position = g.transform.parent.position;
            g.transform.rotation = g.transform.parent.rotation;
        }
        Maininteractor = null;
    }
    public void SetHandOnEnter()
{
    leftHandDevice[0].TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip);
    rightHandDevice[0].TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip);
    //leftHand.GetChild(0).transform.parent = grabObject.transform;
    //print("Enter....1........." + hand.transform.name);
    //print("Enter......2......."+ hand.GetChild(0).transform.name);
    // print("Enter.......3......" + hand.GetChild(1).transform.name);
    //print("Enter.......4......" + grabObject.transform.name);
    //hand.GetChild(0).transform.parent = grabObject.transform;
    if (Maininteractor != null)
    {
        print("Entered.............");
        if (Maininteractor.CompareTag(_Constants.LEFT_HAND_TAG))
        {
            leftHand = grabObject.transform;
        }
        else if (Maininteractor.CompareTag(_Constants.RIGHT_HAND_TAG))
        {
            rightHand = grabObject.transform;
        }
    }
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
    /*private void OnTriggerEnter(Collider other)
    {
       
        sphereofSpanner = other.GetComponent<Collider>();
        Debug.Log("niche  " + other.gameObject.name);
        if (other.gameObject.tag == "LeftHand" )
        {
            SetHandOnEnter(other.gameObject.transform);
        }else if( other.gameObject.tag == "RightHand")
        {
            SetHandOnEnter(other.gameObject.transform);
        }

    }*/
   /*private void OnTriggerExit(Collider other)
    {
       
        sphereofSpanner = other.GetComponent<Collider>();
        Debug.Log("niche  " + other.gameObject.name);
        if (other.gameObject.tag == "LeftHand" )
        {
            SetPositionAsPerHand(other.gameObject.transform);
        }
        else if( other.gameObject.tag == "RightHand")
        {
            SetPositionAsPerHand(other.gameObject.transform);
        }
    }*/

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        DetermineInteractorType(args.interactor);
        base.OnHoverEntered(args);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        grabbedInteractorType = InteractorType.None;
      
    }
    public void DetermineInteractorType(XRBaseInteractor interactor)
    {
        Maininteractor = interactor;
        if (interactor.CompareTag(_Constants.LEFT_HAND_TAG))
        {
            grabbedInteractorType = InteractorType.LeftHand;
        }
        else if (interactor.CompareTag(_Constants.RIGHT_HAND_TAG))
        {
            grabbedInteractorType = InteractorType.RightHand;
        }
        else if (interactor is XRSocketInteractor)
        {
            grabbedInteractorType = InteractorType.Socket;
        }
    }
}
