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

    private void Start()
    {
        Audio = GetComponent<AudioSource>();   
    }
    public void SetWireBrushParams(int _brushCount, string _objectTag)
    {
        brushCount = _brushCount;
        objectTag = _objectTag;
        currentCount = 0;
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
                effect.Play();
                Audio.Play();
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
