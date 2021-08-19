using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldingJoint : MonoBehaviour
{
    public Material blackMaterial;
    public int countdotpoint, countlinePoint;
    public bool isWelding, isChippingHammer; 
    public SoundPlayer soundPlayer;

    private void Start()
    {
        countdotpoint = countlinePoint = 0;
        Debug.Log("Started");
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "Light")
        {
            transform.Find("Point Light").GetComponent<Light>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Light")
        {
            transform.Find("Point Light").GetComponent<Light>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "DotPoint")
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).GetComponent<AudioSource>().Play();
            countdotpoint++;
            //other.transform.GetComponent<MeshRenderer>().material = blackMaterial;
            other.transform.GetComponent<AfterWeldingEffect>().enabled = true;
            other.transform.GetComponent<SphereCollider>().enabled = false;

            if (countdotpoint == 4)
            {
                ExperimentFlowManager.instance.Step17_EnableHighlighting(3);
            }
        }
        if (isWelding && other.transform.tag == "Slag")
        {
            if (ExperimentFlowManager.instance.weldLine.transform.GetChild(countlinePoint).name == other.transform.name)
            {
                transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                transform.GetChild(0).GetComponent<AudioSource>().Play();
                countlinePoint++;
                if (!isChippingHammer)
                {
                    other.transform.GetComponent<MeshRenderer>().enabled = false;
                    other.transform.GetComponent<BoxCollider>().enabled = false;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    other.transform.GetChild(2).gameObject.SetActive(true);

                    if (countlinePoint + 1 <= ExperimentFlowManager.instance.weldLine.transform.childCount)
                    {
                        ExperimentFlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                        ExperimentFlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                    }
                    else
                    {
                        isWelding = false;
                        ExperimentFlowManager.instance.Step18_EnableHighlighting(2);
                    }
                }
                else
                {
                    Debug.Log("In chipping");
                    other.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    gameObject.GetComponent<Outline>().enabled = false;
                    StartCoroutine(HammerBreakpoint(other.gameObject));
                    if (countlinePoint + 1 <= ExperimentFlowManager.instance.weldLine.transform.childCount)
                    {
                        ExperimentFlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                        ExperimentFlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                    }
                    else
                    {
                        //ExperimentFlowManager.instance.Step20_EnableHighlighting(3);
                    }
                }
            }
        }
    }
    public void PlayHitSound()
    {
        soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0], false);
    }

    public IEnumerator HammerBreakpoint(GameObject other)
    {
        PlayHitSound();
        other.transform.GetComponent<MeshRenderer>().enabled = false;
        other.transform.GetComponent<BoxCollider>().enabled = false;
        yield return null;
        //yield return new WaitForSeconds(0.1f);
        //other.transform.GetChild(1).gameObject.SetActive(false);
        //other.transform.GetChild(2).gameObject.SetActive(false);
        //yield return new WaitForSeconds(0.2f);
        //other.gameObject.SetActive(false);
    }
}
