using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RotateNozzle : MonoBehaviour
{
    Vector3 initPos;
    Quaternion initRot, newRot;
  //  public this.transform snap1, snap2;
  //  public bool circleValve = false;
 //   bool active, open = true;
    [Header("Freeze Rotation")]
    public bool x, y, z;
    //this.transform nextSnap;
   // OVRGrabbable thisGrabbable;
  //  public OVRGrabber leftGrabber, rightGrabber;
  // float angleSoFar, angleLastFrame;

    public GameObject MeterObject,otherMeterobject;
    public UnityEvent NextStep;
    public float RotateValue;
    public bool ismeter,isRedValve,isGreenValve;
  //  OVRGrabbable EndGrabbable;
    public bool isclockwise;
    // Start is called before the first frame update
    void Start()
    {
      //  EndGrabbable = GetComponent<OVRGrabbable>();
        //    active = true;
        initPos = this.transform.localPosition;
        initRot = this.transform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = initPos;
         newRot = this.transform.localRotation;
       

            if (x)
                newRot.x = initRot.x;
            if (y)
                newRot.y = initRot.y;
            if (z)
                newRot.z = initRot.z;
        
        this.transform.localRotation = newRot;
        if (ismeter) //nozzel meters
        {
            MeterObject.transform.localRotation = newRot;
   //        Debug.Log(MeterObject.transform.rotation.x);
           if (MeterObject.transform.rotation.x >= RotateValue && MeterObject.transform.rotation.x <= (RotateValue+0.1f))
            {
                //MeterObject.transform.localRotation = newRot;
                MeterObject.SetActive(false);
                otherMeterobject.SetActive(true);
                Debug.Log("Is Match  " + MeterObject.transform.localRotation.x);
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                NextStep.Invoke();
                this.gameObject.GetComponent<RotateNozzle>().enabled = false;
                this.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                this.transform.localPosition = initPos;
                this.transform.localRotation =Quaternion.Euler(newRot.x,0,0);
                //EndGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
                // EndGrabbable.enabled = false;
            }
                
        }
        else
        {
            this.transform.localRotation = newRot;
            if (isRedValve)//red bol at tourch
            {
                if (this.transform.localRotation.x >= RotateValue && this.transform.localRotation.x <= (RotateValue + 0.2f))
                {
                    Debug.Log("Is match " + this.transform.localRotation.x);
                    this.gameObject.GetComponent<BoxCollider>().enabled = false;
                    NextStep.Invoke();
                    this.gameObject.GetComponent<RotateNozzle>().enabled = false;
                    // EndGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
                    // EndGrabbable.enabled = false;
                    this.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                    this.transform.localPosition = initPos;
                    this.transform.localRotation = Quaternion.Euler(newRot.x, 0, 0);
                }
            }
            else if (isGreenValve) //green bol at tourch
            { 
                //Debug.Log("Is match " + this.transform.rotation.z);
                if (this.transform.localRotation.z >= RotateValue && this.transform.localRotation.z <= (RotateValue + 0.2f))
                {
                    Debug.Log("Is match " + this.transform.rotation.z);
                    this.gameObject.GetComponent<BoxCollider>().enabled = false;
                    NextStep.Invoke();
                    this.gameObject.GetComponent<RotateNozzle>().enabled = false;
                    this.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                    // EndGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
                    // EndGrabbable.enabled = false;
                    this.transform.localPosition = initPos;
                    this.transform.localRotation = Quaternion.Euler(0, 0,newRot.z);
                }
            }
            else
            {
                if (this.transform.localRotation.z >= RotateValue && this.transform.localRotation.z <= (RotateValue + 0.2f))
                {
                    this.gameObject.GetComponent<BoxCollider>().enabled = false;
                    NextStep.Invoke();
                    this.gameObject.GetComponent<RotateNozzle>().enabled = false;
                    this.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                    this.transform.localPosition = initPos;
                    this.transform.localRotation = Quaternion.Euler(0, 0, newRot.z);
                    //EndGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
                    //EndGrabbable.enabled = false;
                }
            }
        }

        /*        if ((nextSnap == snap1) && ((circleValve && open) || !circleValve))
                {

                    angle = Quaternion.Angle(this.transform.localRotation, snap1.localRotation);

                    if (Mathf.Abs(angle) < 1)
                    {
                        this.transform.localRotation = snap1.localRotation;
                        Snap();
                        thisGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
                        Debug.Log("Call1");
                        nextSnap = snap2;
                        open = false;
                        return;
                    }
                    else if (Mathf.Abs(angle) > 15 && !circleValve)
                    {
                        Debug.Log("Call2");
                        this.transform.localRotation = initRot;
                        Snap();
                        thisGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);

                    }

                }
                if ((nextSnap == snap2) && ((circleValve && !open) || !circleValve))
                {
                    angle = Quaternion.Angle(this.transform.localRotation, snap2.localRotation);
                    if (Mathf.Abs(angle) < 1)
                    {
                        Debug.Log("Call3");
                        this.transform.localRotation = snap2.localRotation;
                        Snap();
                        thisGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
                        nextSnap = snap1;
                        open = true;
                        return;
                    }
                    else if (Mathf.Abs(angle) > 15 && !circleValve)
                    {
                        Debug.Log("Call4");
                        this.transform.localRotation = initRot;
                        Snap();
                        thisGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);

                    }*/


    }
}
