using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillerRodHeatPart : MonoBehaviour
{
    FillerRod_VL fillerRod;

    private void Start()
    {
        fillerRod = transform.GetComponentInParent<FillerRod_VL>();
    }

   
}
