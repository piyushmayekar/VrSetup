using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingJaw : MonoBehaviour
{
    BenchWise bencwise;

    private void Start()
    {
        bencwise = GetComponentInParent<BenchWise>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Job")
        {
            bencwise.jobFitted = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (bencwise.jobFitted && other.tag == "Job")
        {
            bencwise.jobFitted = false;
        }


    }
}
