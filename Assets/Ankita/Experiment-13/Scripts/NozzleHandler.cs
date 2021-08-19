using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class NozzleHandler : MonoBehaviour
{
    Vector3 initPos;
    Quaternion initRot, newRot;
    [Header("Freeze Rotation")]
    public bool x, y, z;

    public GameObject MeterObject, otherMeterobject;
    public float RotateValue;
    public bool ismeter, isRedValve, isGreenValve;
    //  OVRGrabbable EndGrabbable;
    public bool isclockwise;
    // Start is called before the first frame update
    void Start()
    {
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
        {
            MeterObject.transform.localRotation = newRot;
            if (MeterObject.transform.rotation.x >= RotateValue && MeterObject.transform.rotation.x <= (RotateValue + 0.1f))
            {
                MeterObject.SetActive(false);
                otherMeterobject.SetActive(true);
                Debug.Log("Is Match  " + MeterObject.transform.localRotation.x);
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                this.gameObject.GetComponent<NozzleHandler>().enabled = false;
                this.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                this.transform.localPosition = initPos;
                this.transform.localRotation = Quaternion.Euler(newRot.x, 0, 0);
                ExperimentFlowManager.instance.innerStep++;
                //ExperimentFlowManager.instance.Step10_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
            }

        }
    }
}
