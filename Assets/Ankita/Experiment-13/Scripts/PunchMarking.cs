using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchMarking : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Punch"))
        {
            if (gameObject.name.Contains("Punch"))
            {
                ExperimentFlowManager.instance.centerPunch.GetComponent<Outline>().enabled = false;
                ExperimentFlowManager.instance.innerStep++;
                GetComponent<Collider>().enabled = false;
                GetComponent<PunchMarking>().enabled = false;
            }
        }
        else if (other.name.Contains("Hammer"))
        {
            if (gameObject.name.Contains("Hammer"))
            {
                ExperimentFlowManager.instance.hammer.GetComponent<Outline>().enabled = false;
                ExperimentFlowManager.instance.innerStep++;
                GetComponent<Collider>().enabled = false;
                GetComponent<PunchMarking>().enabled = false;
            }
        }
        if (ExperimentFlowManager.instance.innerStep >= 2)
        {
            ExperimentFlowManager.instance.Step4_EnableHighlighting(2);
        }

    }

   
}
