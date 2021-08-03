using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;


public class SteelRuler_VL : MonoBehaviour
{
    private UnityEvent CallMethodOnJobSnap = new UnityEvent();
    private UnityEvent CallMethodOnSnapForMeasurement = new UnityEvent();
    public bool readyForOperation;
    public GameObject highlight;
    private AudioSource audio;
    public bool isMeasuring;
    private Rigidbody rb;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (!readyForOperation)
        //{
        //    return;
        //}
        //else
        //{


        //    if (collision.collider.tag == "Job")
        //    {
        //        if (CallMethodOnJobSnap != null)
        //        {
        //            CallMethodOnJobSnap.Invoke();
        //        }
        //        this.transform.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        //        this.transform.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        //        this.transform.GetComponent<XRGrabInteractable>().enabled = true;

        //    }
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMeasuring)
        {
            if (other.tag == "MeasurementHL")
            {
                transform.position = other.transform.position;
                transform.eulerAngles = other.transform.eulerAngles;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                other.gameObject.SetActive(false);
                gameObject.GetComponent<CustomXRGrabInteractable>().enabled = false;
                if (CallMethodOnSnapForMeasurement != null)
                {
                    CallMethodOnSnapForMeasurement.Invoke();
                }
            }
        }
        if (!readyForOperation)
        {
            return;
        }
        else
        {


            if (other.tag == "SteelRuleHighlight")
            {
                if (CallMethodOnJobSnap != null)
                {
                    CallMethodOnJobSnap.Invoke();
                }
                audio.Play();
                highlight.SetActive(false);
                this.transform.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
             //   this.transform.gameObject.GetComponentInChildren<Rigidbody>().isKinematic = true;
                this.transform.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                this.transform.GetComponent<XRGrabInteractable>().enabled = false;

            }
        }
    }

    public void AssignMethodOnSnapToJob(UnityAction method)
    {
        if (CallMethodOnJobSnap != null)
        {
            CallMethodOnJobSnap.RemoveAllListeners();
        }

        CallMethodOnJobSnap.AddListener(method);
    }

    public void AssignMethodOnSnapForMeasurement(UnityAction method)
    {
        if (CallMethodOnSnapForMeasurement != null)
        {
            CallMethodOnSnapForMeasurement.RemoveAllListeners();
        }
        CallMethodOnSnapForMeasurement.AddListener(method);
    }

    //No use 
    public void AssignHighlight(GameObject _Highlight)
    {
        highlight = _Highlight;
        _Highlight.SetActive(true);
    }


}