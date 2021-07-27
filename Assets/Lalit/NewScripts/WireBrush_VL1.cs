using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireBrush_VL1 : MonoBehaviour
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
}
