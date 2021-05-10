using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public bool enableWorking;
    public static int hitcount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HammerHighlight")
        {
            other.gameObject.SetActive(false);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableWorking)
        {
            if (collision.collider.tag == "PunchHead")
            {
                //PlayAudio.ins.PlayHammerSound();
                hitcount++;
                if (hitcount > 8 && Job.UseScriber)
                {
                    Debug.Log("Done Hammering");
                    Job.UseDotPunch = true;
                    TaskBoard.ins.ProceedButton.enabled = true;
                    TaskBoard.ins.ToggleButtonColor(true);
                    hitcount = 0;
                }
            }
        }
    }
    public void onGrab()
    {
        enableWorking = true;
    }

    public void OnUnGrab()
    {
        enableWorking = false;
    }
}
