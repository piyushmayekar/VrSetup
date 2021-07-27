using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle_VL : MonoBehaviour
{
    public BenchWise_VL BenchWise;
    public bool CanMove;
    public bool isClockwise;
    private Material defaultMat;
    public Material HighlightMat;
    private AudioSource Audio;

    private void Start()
    {
        defaultMat = gameObject.GetComponent<MeshRenderer>().material;
        Audio = GetComponent<AudioSource>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "LeftHand" || other.tag == "RightHand")
        {
            if (CanMove)
            {
                if (!Audio.isPlaying)
                {
                    Audio.Play();
                }
                

                if (isClockwise)
                {
                    BenchWise.MoveForward();
                }
                else
                {
                    BenchWise.MoveBackWard();
                }
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LeftHand" || other.tag == "RightHand")
        {
           // Audio.Stop();
        }
    }

    public void SetHighlightMat()
    {
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = HighlightMat;
    }

    public void SetHighDefaultMat()
    {
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = defaultMat;
    }

    public void StopSound()
    {
        Audio.Stop();
    }
}
