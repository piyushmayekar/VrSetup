using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;


public class CustomSocket : MonoBehaviour
{
    private UnityEvent CallMethodOnJobSnap = new UnityEvent();
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Job")
        {
            Debug.Log("called job");
            HLSound.player.PlayHighlightSnapSound();
            if (other.gameObject.GetComponent<XRGrabInteractable>() != null)
            {
                other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
            }
            other.transform.position = transform.position;
            other.transform.eulerAngles = transform.eulerAngles;
            if (CallMethodOnJobSnap != null)
            {
                CallMethodOnJobSnap.Invoke();
            }
        }
    }

    public void AssignMethodOnCleaningJobDone(UnityAction method)
    {
        if (CallMethodOnJobSnap != null)
        {
            CallMethodOnJobSnap.RemoveAllListeners();
        }

        CallMethodOnJobSnap.AddListener(method);
    }
}
