using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckCollisionExp14 : MonoBehaviour
{
    public string tagName;
    public GameObject nextObj;
    public int cnt = 0;
    public SoundPlayer soundPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == tagName)
        {
            if (tagName == "Job")
            {
                other.GetComponent<Outline>().enabled = false;
                other.GetComponent<XRGrabInteractable>().enabled = false;
                other.GetComponent<BoxCollider>().enabled = false;
                other.transform.localPosition = transform.localPosition;
                other.transform.localRotation = transform.localRotation;

                if (SceneManager.GetActiveScene().name.Contains("15"))
                {
                    Experiment15FlowManager.instance.innerStep++;
                    Experiment15FlowManager.instance.Step4_EnableHighlighting(Experiment15FlowManager.instance.innerStep);
                }
                else if (SceneManager.GetActiveScene().name.Contains("14"))
                {
                    Experiment14FlowManager.instance.innerStep++;
                    Experiment14FlowManager.instance.Step4_EnableHighlighting(Experiment14FlowManager.instance.innerStep);
                }

                gameObject.SetActive(false);
            }
            else if (tagName == "FlatFile")
            {
                if (transform.name.Contains("Sheet"))
                {
                    PlaySound();
                    cnt++;
                    Debug.Log(transform.name);
                    if (cnt >= 5)
                    {
                        nextObj.SetActive(true);
                        transform.parent.gameObject.SetActive(false);
                        Experiment14FlowManager.instance.innerStep++;
                        Experiment14FlowManager.instance.Step2_EnableHighlighting(Experiment14FlowManager.instance.innerStep);
                    }
                }
                else
                {
                    //other.transform.parent.GetComponent<Outline>().enabled = false;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
                if (SceneManager.GetActiveScene().name.Contains("15"))
                {
                    if (Experiment15FlowManager.instance.cntSteps == 24)
                    {
                        Experiment15FlowManager.instance.Step16_EnableHighlighting(2);
                    }
                }
                else if (SceneManager.GetActiveScene().name.Contains("14"))
                {
                    if (Experiment14FlowManager.instance.cntSteps == 25)
                    {
                        Experiment14FlowManager.instance.Step16_EnableHighlighting(2);
                    }
                }
            }
        }
    }

    private void PlaySound()
    {
        soundPlayer.PlayClip(soundPlayer.Clips[0], true);
    }
}
