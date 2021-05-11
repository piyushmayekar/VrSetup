using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiseHandle : MonoBehaviour
{
    private Material defaultMat;
    public Material HighLightMat;

    MeshRenderer meshRenderer;

    BenchWise wise;

    public LocomotionController loco;
    private void Start()
    {
        wise = GetComponentInParent<BenchWise>();
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            defaultMat = meshRenderer.sharedMaterial;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "LeftHand")
        {
            meshRenderer.sharedMaterial = HighLightMat;
            wise.HasControl = true;
            loco.enableLeftTeleport = false;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LeftHand")
        {
            meshRenderer.sharedMaterial = defaultMat;
            wise.HasControl = false;
            loco.enableLeftTeleport = true;
        }
    }
}
