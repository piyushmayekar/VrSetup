using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlatFile_VL : MonoBehaviour
{
    public int timeCount;
    public int currentCount = 0;
    public bool completed;
    private AudioSource Audio;

    private UnityEvent CallMethodOnFillingDone = new UnityEvent();

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }
    public void SetFlatFileParams(int _timeCount)
    {
        timeCount = _timeCount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Job")
        {
            currentCount++;
            if (currentCount > timeCount)
            {
                EmptyParams();
                
                    if (CallMethodOnFillingDone != null)
                    {
                        CallMethodOnFillingDone.Invoke();                      
                    }
               
            }
            else
            {
                //ReadStepsFromJson.instance.tablet.SetActive(true);
                //ReadStepsFromJson.instance.stepText.text = "\nDo filling on Job Edges.\n" + currentCount.ToString() + "/10";
                Audio.Play();
                Debug.Log(currentCount);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Job")
        {
            Audio.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FlatFileHighlight")
        {
            HLSound.player.PlayHighlightSnapSound();
            other.gameObject.SetActive(false);
        }
    }
    public void EmptyParams()
    {
        timeCount = 0;
        currentCount = 0;
    }

    public void AssignMethodOnFillingDone(UnityAction method)
    {
        if (CallMethodOnFillingDone != null)
        {
            CallMethodOnFillingDone.RemoveAllListeners();
        }
        CallMethodOnFillingDone.AddListener(method);
    }
}
