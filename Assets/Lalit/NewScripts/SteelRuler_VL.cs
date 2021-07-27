using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;


public class SteelRuler_VL : MonoBehaviour
{
    private UnityEvent CallMethodOnJobSnap = new UnityEvent();
    public bool readyForOperation;
    public GameObject highlight;
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
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

    //No use 
    public void AssignHighlight(GameObject _Highlight)
    {
        highlight = _Highlight;
        _Highlight.SetActive(true);
    }


}