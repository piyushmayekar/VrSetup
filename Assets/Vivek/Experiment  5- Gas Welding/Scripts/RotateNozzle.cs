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
    [Header("Freeze Rotation")]
    public bool x, y, z, ishideMesh;
    public GameObject MeterObject, otherMeterobject;
    public Transform leftHand, grabObject;
    public GameObject OtherRotate;
    public MeshRenderer HideMesh;
    public UnityEvent NextStep;
    public int RotateValue;
    public bool ismeter, isRegulator;
    public bool isclockwise;
    Quaternion t_rotate;
    public float speed = 20f;
    List<InputDevice> leftHandDevices;
    List<InputDevice> righthandDevices;

    void Start()
    {
        leftHandDevices = new List<InputDevice>();
        righthandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, righthandDevices);

        initPos = this.transform.localPosition;
        initRot = this.transform.localRotation;
        t_rotate = transform.localRotation;
    }
    public void EnableMeterObject()
    {
        MeterObject.SetActive(false);
        otherMeterobject.SetActive(true);
    }


    public void callEnterGrabObject()
    {
        //  Debug.Log(OtherRotate.name + "  OtherRotate call grab enter **** " + this.gameObject.name);

        this.transform.localPosition = initPos;
        if (isclockwise)
        {
            if (x)
                t_rotate.x += Time.deltaTime * speed;
            if (y)
                t_rotate.y += Time.deltaTime * speed;
            if (z)
                t_rotate.z += Time.deltaTime * speed;
        }
        else
        {
            if (x)
                t_rotate.x -= Time.deltaTime * speed;
            if (y)
                t_rotate.y -= Time.deltaTime * speed;
            if (z)
                t_rotate.z -= Time.deltaTime * speed;
        }
        if (ishideMesh)
        {
            HideMesh.enabled = false;
        }
        OtherRotate.SetActive(true);
        OtherRotate.transform.localRotation = Quaternion.Euler(t_rotate.x, t_rotate.y, t_rotate.z);
        if (ismeter)
        {
            MeterObject.transform.localRotation = Quaternion.Inverse(Quaternion.Euler(t_rotate.x, t_rotate.y, t_rotate.z));
            // MeterObject.transform.localRotation = OtherRotate.transform.localRotation;
        }
        if (isclockwise)
        {
            if (z)
            {
                //Debug.Log("callzzzzzzzzzzzz grab enter");
                if (OtherRotate.transform.eulerAngles.z > RotateValue && OtherRotate.transform.eulerAngles.z < (RotateValue + 20))//20 hata 
                {
                    CallEndMethod();
                }
            }
            else if (y)
            {
                if (OtherRotate.transform.eulerAngles.y > RotateValue && OtherRotate.transform.eulerAngles.y < (RotateValue + 20))
                {
                    CallEndMethod();
                }
            }
            else if (x)
            {
                if (OtherRotate.transform.localEulerAngles.x > RotateValue && OtherRotate.transform.localEulerAngles.x < (RotateValue + 20))
                {
                    CallEndMethod();
                }

            }
        }
        else
        {
            if (z)
            {
                transform.localRotation = Quaternion.Euler(t_rotate.x, t_rotate.y, -t_rotate.z);
                if (transform.localEulerAngles.z > RotateValue && transform.localEulerAngles.z < (RotateValue + 20))
                {
                    CallEndMethod();
                }
            }
            else if (y)
            {
                transform.localRotation = Quaternion.Euler(t_rotate.x, -t_rotate.y, t_rotate.z);
                if (transform.eulerAngles.y > RotateValue && transform.eulerAngles.y < (RotateValue + 20))
                {
                    CallEndMethod();
                }
            }
            else if (x)
            {
                transform.localRotation = Quaternion.Euler(-t_rotate.x, t_rotate.y, t_rotate.z);
                //   transform.localRotation = Quaternion.Euler(-t_rotate.x, t_rotate.y, t_rotate.z);

                if (transform.localEulerAngles.x > RotateValue && transform.localEulerAngles.x < (RotateValue + 20))
                {
                    CallEndMethod();
                }

            }
        }
    }



    void CallEndMethod()
    {
        //  Debug.Log("Call next step   " + OtherRotate.name);
        this.gameObject.GetComponent<RotateNozzle>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        if (isRegulator)
        {
            Destroy(this.gameObject.GetComponent<RotateNozzle>());
        }
        if (this.gameObject.GetComponent<XRGrabInteractable>())
        {
            this.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        }
        NextStep.Invoke();
    }
    public void callExitObjectGrab()
    {
        if (ishideMesh)
        {
            HideMesh.enabled = true;
        }
        if (!isRegulator)
            OtherRotate.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val);
        righthandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val1);

        if ((other.gameObject.tag == "RightHand" || other.gameObject.tag == "LeftHand"))
        {
            if (val || val1)
            {
                callEnterGrabObject();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag == "RightHand" || other.gameObject.tag == "LeftHand"))
        {
            callExitObjectGrab();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val);
        righthandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool val1);
        // print(val1);
        if ((other.gameObject.tag == "RightHand" || other.gameObject.tag == "LeftHand"))
        {
            if (val || val1)
            {
                callEnterGrabObject();
            }
        }
    }
}
