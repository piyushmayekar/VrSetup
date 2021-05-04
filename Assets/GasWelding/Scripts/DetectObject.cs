using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObject : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Kit")
        {
            GasWeldingStep.instance.countppekit++;
            // Debug.LogError(GasWeldingStep.instance.countppekit);
            if(GasWeldingStep.instance.countppekit>=6)
            {
                GasWeldingStep.instance.OnEnableStep2object();
            }
            other.gameObject.SetActive(false); 
        }/*else if(other.gameObject.tag =="CrackTab")
        {
            GasWeldingStep.instance.countCrackTab++;
            other.gameObject.tag = "Untagged";
            other.gameObject.GetComponent<BoxCollider>().enabled=false;
            other.gameObject.GetComponent<Outline>().enabled = false;
            if (GasWeldingStep.instance.countCrackTab==2)
            {
                GasWeldingStep.instance.OnEnableStep4object();
            }
            other.gameObject.SetActive(false);
        }*/
    }
   
}
