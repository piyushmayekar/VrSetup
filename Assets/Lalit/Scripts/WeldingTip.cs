using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldingTip : MonoBehaviour
{
    public GameObject effect;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Job")
        {
            effect.SetActive(true);
        }

        if (other.tag == "WeldPoint")
        {
            WeldLinePoint p = other.gameObject.GetComponent<WeldLinePoint>();
            p.ShowPoint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Job")
        {
            effect.SetActive(false);
        }
    }
}
