using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjManager : MonoBehaviour
{
    public string objName;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(objName))
        {
            if (other.name.Contains("Scriber"))
            {
                transform.GetComponent<Outline>().enabled = false;
                transform.GetComponent<BoxCollider>().isTrigger = false;
                other.gameObject.SetActive(false);
            }
            else
            {
                transform.GetComponent<XRGrabInteractable>().enabled = false; 
                transform.GetComponent<Outline>().enabled = false;
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;
                other.gameObject.SetActive(false);
            }
            ExperimentFlowManager.instance.innerStep++;
            Debug.Log(ExperimentFlowManager.instance.innerStep);
            ExperimentFlowManager.instance.Step2_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
        }
    }
}
