using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MarkingBrush_VL : MonoBehaviour
{
    private int brushCount = 0;
    private int currentCount = 0;
    public string objectTag = "";
    private AudioSource Audio;
    Rigidbody rb;

    private UnityEvent CallMethodOnBrushJobDone = new UnityEvent();

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    public void SetBrushParams(int _brushCount, string _objectTag)
    {
        brushCount = _brushCount;
        objectTag = _objectTag;
        currentCount = 0;
    }

    

   

   
    private void EmptyParams()
    {
        brushCount = 0;
        currentCount = 0;
        objectTag = " ";
    }

    public void AssignMethodOnBrushJobDone(UnityAction method)
    {
        if (CallMethodOnBrushJobDone != null)
        {
            CallMethodOnBrushJobDone.RemoveAllListeners();
        }

        CallMethodOnBrushJobDone.AddListener(method);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MarkingPlate")
        {
            rb.freezeRotation = false;
            //Audio.Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MarkingBrushHL")
        {
            HLSound.player.PlayHighlightSnapSound();

            other.gameObject.SetActive(false);
        }

        if (other.tag == "MarkingPlate")
        {
            currentCount++;
            Audio.Play();
            rb.freezeRotation = true;

            if (currentCount == brushCount)
            {

                
                EmptyParams();

                if (CallMethodOnBrushJobDone != null)
                {
                    CallMethodOnBrushJobDone.Invoke();
                }
            }

        }
    }
}
