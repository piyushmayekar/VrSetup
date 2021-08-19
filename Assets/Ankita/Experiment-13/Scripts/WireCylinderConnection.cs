using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCylinderConnection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("pipe"))
        {
            other.gameObject.SetActive(false);
            gameObject.GetComponent<Outline>().enabled = false;
            ExperimentFlowManager.instance.innerStep++;
            ExperimentFlowManager.instance.Step8_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
            Destroy(gameObject.GetComponent<WireCylinderConnection>());
        }
        else if(other.name.Contains("SphereEnd"))
        {
            other.name = "DoneWire";
            other.GetComponent<Outline>().enabled = false;
            gameObject.GetComponent<Outline>().enabled = false;
            ExperimentFlowManager.instance.innerStep++;
            ExperimentFlowManager.instance.Step8_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
            Destroy(gameObject.GetComponent<WireCylinderConnection>());
        }
        else if (other.name.Contains("Nozzle"))
        {
            Destroy(other.gameObject);
            GetComponent<MeshRenderer>().enabled = false;
            ExperimentFlowManager.instance.innerStep++;
            //ExperimentFlowManager.instance.Step9_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
            Destroy(gameObject.GetComponent<WireCylinderConnection>());
        }
    }

}
