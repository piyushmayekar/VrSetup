using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ACheckCollisionImpression : MonoBehaviour
{
    public bool isFreeze;
    public string objName;

    public UnityEvent callAnyOtherMethod;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.name.Contains(objName))
        {
            if (isFreeze)
            {
                if (objName.Contains("Steel Rule"))
                {
                    Debug.Log("Obj");
                    other.transform.GetComponent<CustomXRGrabInteractable>().enabled = false;
                    other.transform.GetComponent<Outline>().enabled = false;
                    other.transform.position = transform.position;
                    other.transform.rotation = transform.rotation;

                    ExperimentFlowManager.instance.innerStep++;
                    Debug.Log(ExperimentFlowManager.instance.innerStep);
                    ExperimentFlowManager.instance.Step2_EnableHighlighting(ExperimentFlowManager.instance.innerStep);

                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (objName.Contains("Scriber"))
                {
                    //other.transform.GetComponent<Outline>().enabled = false;
                    ExperimentFlowManager.instance.scriber.GetComponent<Outline>().enabled = false;

                    ExperimentFlowManager.instance.innerStep++;
                    ExperimentFlowManager.instance.Step2_EnableHighlighting(ExperimentFlowManager.instance.innerStep);

                    gameObject.SetActive(false);
                }
                else if (objName.Contains("Punch"))
                {
                    //other.transform.GetComponent<Outline>().enabled = false;
                    ExperimentFlowManager.instance.centerPunch.GetComponent<Outline>().enabled = false;

                    ExperimentFlowManager.instance.innerStep++;

                    if (ExperimentFlowManager.instance.innerStep >= 2)
                    {
                        ExperimentFlowManager.instance.Step4_EnableHighlighting(2);
                    }
                    gameObject.SetActive(false);
                }
                else if (objName.Contains("Hammer"))
                {
                    other.transform.GetComponent<Outline>().enabled = false;

                    ExperimentFlowManager.instance.innerStep++;

                    if (ExperimentFlowManager.instance.innerStep >= 2)
                    {
                        ExperimentFlowManager.instance.Step4_EnableHighlighting(2);
                    }
                    gameObject.SetActive(false);
                }
                else if (objName.Contains("Torch"))
                {
                    if (SceneManager.GetActiveScene().name.Contains("15"))
                    {
                        callAnyOtherMethod.Invoke();
                        Experiment15FlowManager.instance.innerStep++;
                        Experiment15FlowManager.instance.Step13_EnableHighlighting(Experiment15FlowManager.instance.innerStep);
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("14"))
                    {
                        callAnyOtherMethod.Invoke();
                        Experiment14FlowManager.instance.innerStep++;
                        Experiment14FlowManager.instance.Step13_EnableHighlighting(Experiment14FlowManager.instance.innerStep);
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("13"))
                    {
                        ExperimentFlowManager.instance.Step17_EnableHighlighting(2);
                    }

                    gameObject.SetActive(false);
                }
                else if (objName.Contains("Electrode"))
                {
                    if (SceneManager.GetActiveScene().name.Contains("15"))
                    {
                        Experiment15FlowManager.instance.innerStep++;
                        Experiment15FlowManager.instance.Step13_EnableHighlighting(Experiment15FlowManager.instance.innerStep);
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("14"))
                    {
                        Experiment14FlowManager.instance.innerStep++;
                        Experiment14FlowManager.instance.Step13_EnableHighlighting(Experiment14FlowManager.instance.innerStep);
                    }

                    gameObject.SetActive(false);
                }
            }
        }
    }
}
