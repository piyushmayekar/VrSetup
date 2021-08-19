using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckCollision : MonoBehaviour
{
    public string tagName;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.tag == tagName)
        {
            if (tagName == "Job")
            {
                other.GetComponent<Outline>().enabled = false;
                other.GetComponent<XRGrabInteractable>().enabled = false;
                other.GetComponent<BoxCollider>().enabled = false;
                other.transform.localPosition = transform.localPosition;
                other.transform.localRotation = transform.localRotation;
                ExperimentFlowManager.instance.innerStep++;
                ExperimentFlowManager.instance.Step7_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
            }
            gameObject.SetActive(false);
            if (ExperimentFlowManager.instance.cntSteps == 25)
            {
                ExperimentFlowManager.instance.Step17_EnableHighlighting(2);
            }


        }
    }
}
