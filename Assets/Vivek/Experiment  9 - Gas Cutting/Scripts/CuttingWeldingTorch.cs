using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class CuttingWeldingTorch : MonoBehaviour
{
    public GameObject LeftHand, RightHand, otherLeftHandRotate, otherRightHandRotate;
    List<InputDevice> leftHandDevices;
    List<InputDevice> righthandDevices;
    public GameObject otherTorch;
    public Rigidbody r_Rb, l_Rb;
    // Start is called before the first frame update
    void Start()
    {
        leftHandDevices = new List<InputDevice>();
        righthandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, righthandDevices);
        callEnterGrabObject(true, false);

    }


    public void OnCollisionEnter(Collision other)
    {
        if (GasCuttingManager.instance.isCutting)
        {
            righthandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val1);
            leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val2);

            if ((other.gameObject.tag == "RightHand" || other.gameObject.tag == "LeftHand"))
            {
                if (val2 || val1)
                {
                    callEnterGrabObject(val1, val2);
                }
            }
        }
    }
    public void OnCollisionExit(Collision other)
    {
        if (GasCuttingManager.instance.isCutting)
        {
            righthandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val1);
            leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val2);

            if ((other.gameObject.tag == "RightHand" || other.gameObject.tag == "LeftHand"))
            {
                if (val2 || val1)
                {
                    callEXITGrabObject(val1, val2);
                }
            }
        }
    }
    public void callEnterGrabObject(bool val1, bool val2)
    {
     //   transform.localPosition = otherTorch.transform.position;

        if (val1)
        {
            RightHand.transform.localEulerAngles = otherTorch.transform.localEulerAngles;
            r_Rb.constraints = RigidbodyConstraints.FreezeRotation;
            //RightHand.transform.localPosition = otherRightHandRotate.transform.localPosition;

            // Debug.Log(RightHand.transform.localEulerAngles +"//right hand  " + otherRightHandRotate.transform.localEulerAngles);//right hand
            //r_Rb.constraints = RigidbodyConstraints.FreezeAll;

        }
        if (val2)
        {
            LeftHand.transform.localEulerAngles = otherTorch.transform.localEulerAngles;
            l_Rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    public void callEXITGrabObject(bool val1, bool val2)
    {
        if (val1)
        {
            r_Rb.constraints = RigidbodyConstraints.None;
        }
        if (val2)
        {
            l_Rb.constraints = RigidbodyConstraints.None;
        }
    }
}
