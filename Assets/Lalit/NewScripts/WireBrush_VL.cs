using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WireBrush_VL : MonoBehaviour
{
    private int brushCount = 0;
    private int currentCount = 0;
    public string objectTag = "";
    public ParticleSystem effect;
    private AudioSource Audio;

    private UnityEvent CallMethodOnCleaningJobDone = new UnityEvent();
    private Rigidbody rb;
    private void Start()
    {
        Audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    public void SetWireBrushParams(int _brushCount, string _objectTag)
    {
        brushCount = _brushCount;
        objectTag = _objectTag;
        currentCount = 0;
    }


    private void OnCollisionExit(Collision collision)
    {
        rb.freezeRotation = false;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == objectTag)
        {
            currentCount++;
            //Effect
            

            if (currentCount == brushCount)
            {
                
                
                EmptyParams();
                
                if (CallMethodOnCleaningJobDone != null)
                {
                    CallMethodOnCleaningJobDone.Invoke();
                }
            }
            else
            {
                ReadStepsFromJson.instance.tablet.SetActive(true);
                ReadStepsFromJson.instance.stepText.text = "\nPick up C.S. brush and clean the surface.\n" + currentCount.ToString() + "/10";
                effect.Play();
                Audio.Play();
                rb.freezeRotation = true;
                //Debug.Log(currentCount);
            }
        }
    }

    private void EmptyParams()
    {
        brushCount = 0;
        currentCount = 0;
        objectTag = " ";
    }

    public void AssignMethodOnCleaningJobDone(UnityAction method)
    {
        if (CallMethodOnCleaningJobDone != null)
        {
            CallMethodOnCleaningJobDone.RemoveAllListeners();
        }

        CallMethodOnCleaningJobDone.AddListener(method);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WireBrushHighlight")
        {
            HLSound.player.PlayHighlightSnapSound();
            other.gameObject.SetActive(false);
        }
    }
}
