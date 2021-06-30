using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scriber : MonoBehaviour
{
    public bool enableWorking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ScriberHighlight")
        {
            Debug.Log("Callinf scriber");
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
