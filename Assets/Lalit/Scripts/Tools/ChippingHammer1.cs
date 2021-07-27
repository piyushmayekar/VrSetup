using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChippingHammer1 : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Slag" )
        {
            GameObject slag = collision.gameObject;
            slag.transform.parent = null;
            Rigidbody rb = slag.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;

        }
    }
}
