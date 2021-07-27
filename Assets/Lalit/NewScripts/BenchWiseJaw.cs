using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BenchWiseJaw : MonoBehaviour
{
    public Handle_VL Handle;
    private UnityEvent CallMethodOnJobFit = new UnityEvent();
    public bool jobfitted = false;
    public bool disable = true;
    private AudioSource hitSound;

    private void Start()
    {
        hitSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Job")
        {

            if (!disable)
            {
                if (jobfitted)
                {
                    Debug.Log("Shit was here");
                    return;
                }
                else
                {
                    Handle.StopSound();
                   
                    jobfitted = true;
                    Handle.CanMove = false;
                    Handle.SetHighDefaultMat();
                    hitSound.Play();
                    if (CallMethodOnJobFit != null)
                    {
                        CallMethodOnJobFit.Invoke();
                    }
                }
            }
        }
    }

    


    public void AssignMethod(UnityAction method)
    {
        if (CallMethodOnJobFit != null)
        {
            CallMethodOnJobFit.RemoveAllListeners();
        }

        CallMethodOnJobFit.AddListener(method);
    }
}
