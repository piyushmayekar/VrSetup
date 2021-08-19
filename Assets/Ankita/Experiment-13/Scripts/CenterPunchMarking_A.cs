using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPunchMarking_A : MonoBehaviour
{
    public bool isCubeEntered = false;
    public Transform markingPoint = null;
    //Rigidbody rb;
    public SoundPlayer soundPlayer;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "_MarkingLinePoint")
        {
            isCubeEntered = true;
            markingPoint = other.transform;
            //rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "_MarkingLinePoint")
        {
            isCubeEntered = false;
            markingPoint = null;
            //rb.constraints = RigidbodyConstraints.None;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Contains("Hammer"))
        {
            if (isCubeEntered)
            {
                isCubeEntered = false;

                PlayHitSound();
                Debug.Log("Do punch");
                Destroy(markingPoint.GetComponent<BoxCollider>());
                Destroy(markingPoint.GetComponent<MeshRenderer>());
                markingPoint.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

                markingPoint = null;

                ExperimentFlowManager.instance.PunchPoints();
            }
        }
    }

    public void PlayHitSound()
    {
        soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0], false);
    }
}
