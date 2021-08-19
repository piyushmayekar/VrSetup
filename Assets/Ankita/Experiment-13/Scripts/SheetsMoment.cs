using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SheetsMoment : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Sheet"))
        {
            if (gameObject.name.Contains("Bench") || gameObject.name.Contains("Last"))
            {
                other.GetComponent<XRGrabInteractable>().enabled = false;
            }
            else if (gameObject.name.Contains("Table"))
            {
                other.GetComponent<XRGrabInteractable>().enabled = false;
                other.GetComponent<Collider>().enabled = false;
            }

            other.transform.position = transform.localPosition;
            other.transform.rotation = transform.localRotation;
            other.GetComponent<Outline>().enabled = false;
            ExperimentFlowManager.instance.innerStep++;
            if (ExperimentFlowManager.instance.cntSteps == 4 && ExperimentFlowManager.instance.innerStep == 2)
            {
                ExperimentFlowManager.instance.Step5_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
            }
            else if(ExperimentFlowManager.instance.cntSteps >= 5)
            {
                ExperimentFlowManager.instance.Step6_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
            }
            gameObject.SetActive(false);
        }
        if (gameObject.name.Contains("HammerImpression") && other.transform.tag.Contains("Hammer"))
        {
            ExperimentFlowManager.instance.hammer.GetComponent<Outline>().enabled = false;
            gameObject.SetActive(false);
        }
    }
}
