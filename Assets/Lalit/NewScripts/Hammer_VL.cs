using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer_VL : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.freezeRotation = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.freezeRotation = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HammerHighlight")
        {
            HLSound.player.PlayHighlightSnapSound();
            other.gameObject.SetActive(false);
        }
    }
}
