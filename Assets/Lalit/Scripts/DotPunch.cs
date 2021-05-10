using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotPunch : MonoBehaviour
{
    public bool enableWorking;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CenterPunchHighlight")
        {
            other.gameObject.SetActive(false);

        }
    }

    public void onGrab()
    {
        enableWorking = true;
    }

    public void OnUnGrab()
    {
        enableWorking = false;
    }
}
