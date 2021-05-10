using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackSaw : MonoBehaviour
{
    public bool enableWorking;



    private void Update()
    {


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HackSawHighlight")
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
