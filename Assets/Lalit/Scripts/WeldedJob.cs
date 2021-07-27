using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldedJob : MonoBehaviour
{
    public bool tagWeld1, tagWeld2;
    public bool measuredAngle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "TrySquare")
        {
            measuredAngle = true; 
        }
    }
}
