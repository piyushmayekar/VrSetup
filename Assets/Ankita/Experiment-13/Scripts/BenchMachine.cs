using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BenchMachine : MonoBehaviour
{
    private void Start()
    {
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if(collision.collider.name.Contains("Hand"))
    //    //{
    //    GetComponent<BenchMachine>().enabled = false;
    //    ExperimentFlowManager.instance.innerStep++;
    //        ExperimentFlowManager.instance.CallRotateReposition();
    //        transform.Find("Handle Highlight").GetComponent<MeshRenderer>().enabled = false;
    //        GetComponent<XRGrabInteractable>().enabled = false;
    //        GetComponent<Collider>().enabled = false;
    //    //}
    //}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Called");
        Destroy(GetComponent<BenchMachine>());
        ExperimentFlowManager.instance.CallRotateReposition();
        transform.Find("Handle Highlight").GetComponent<MeshRenderer>().enabled = false;
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
