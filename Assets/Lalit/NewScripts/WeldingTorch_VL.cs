using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeldingTorch_VL : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WeldingTorchHighlight")
        {
            other.gameObject.SetActive(false);
        }
    }
}
