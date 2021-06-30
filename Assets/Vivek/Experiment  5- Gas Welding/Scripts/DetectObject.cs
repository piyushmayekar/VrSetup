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
        }else if(other.gameObject. name == "Cutting welding tourch")
        {
            GasCuttingManager.instance.Checktourch90degree();
            this.gameObject.SetActive(false);
        }
    }
   
}
