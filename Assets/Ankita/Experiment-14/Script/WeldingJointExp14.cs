using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeldingJointExp14 : MonoBehaviour
{
    public Material blackMaterial;
    public int countdotpoint, countlinePoint;
    public bool isWelding, isChippingHammer;
    public SoundPlayer soundPlayer;

    public bool isElectrode = false, isPoint = false;

    private void Start()
    {
        countdotpoint = countlinePoint = 0;
    }
    private void OnTriggerStay(Collider other)
    {
        //if (other.transform.name == "Light")
        //{
        //    transform.Find("Point Light").GetComponent<Light>().enabled = true;
        //}
        if (other.transform.tag == "DotPoint")
        {
            if (isElectrode)
            {
                transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                transform.GetChild(0).GetComponent<AudioSource>().Play();
                countdotpoint++;
                //other.transform.GetComponent<MeshRenderer>().material = blackMaterial;
                other.transform.GetComponent<AfterWeldingEffect>().enabled = true;
                other.transform.GetComponent<SphereCollider>().enabled = false;

                if (countdotpoint == 4)
                {
                    if (SceneManager.GetActiveScene().name.Contains("15"))
                    {
                        Experiment15FlowManager.instance.Step13_EnableHighlighting(4);
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("14"))
                    {
                        Experiment14FlowManager.instance.Step13_EnableHighlighting(4);
                    }
                }
            }
        }
        else if (isWelding && other.transform.tag == "Slag")
        {
            if (!isChippingHammer)
            {
                if (isElectrode)
                {
                    transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    transform.GetChild(0).GetComponent<AudioSource>().Play();
                    other.transform.GetComponent<MeshRenderer>().enabled = false;
                    other.transform.GetComponent<BoxCollider>().enabled = false;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    other.transform.GetChild(2).gameObject.SetActive(true);

                    if (SceneManager.GetActiveScene().name.Contains("15"))
                    {
                        if (countlinePoint + 2 <= Experiment15FlowManager.instance.weldLine.transform.childCount)
                        {
                            countlinePoint++;
                            Experiment15FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                            Experiment15FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                        }
                        else
                        {
                            isWelding = false;
                            transform.Find("Point Light").GetComponent<Light>().enabled = false;
                            Experiment15FlowManager.instance.Step14_EnableHighlighting(2);
                        }
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("14"))
                    {
                        if (countlinePoint + 2 <= Experiment14FlowManager.instance.weldLine.transform.childCount)
                        {
                            countlinePoint++;
                            Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                            Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                        }
                        else
                        {
                            isWelding = false;
                            transform.Find("Point Light").GetComponent<Light>().enabled = false;
                            Experiment14FlowManager.instance.Step14_EnableHighlighting(2);
                        }
                    }

                }
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Electrode")
        {
            isElectrode = false;
        }
        else if (other.transform.name == "Light")
        {
            transform.Find("Point Light").GetComponent<Light>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Light")
        {
            transform.Find("Point Light").GetComponent<Light>().enabled = true;
        }
        else if (other.transform.tag == "Electrode")
        {
            isElectrode = true;
        }
        else if (other.transform.tag == "DotPoint")
        {
            if (isElectrode)
            {
                transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                transform.GetChild(0).GetComponent<AudioSource>().Play();
                countdotpoint++;
                //other.transform.GetComponent<MeshRenderer>().material = blackMaterial;
                other.transform.GetComponent<AfterWeldingEffect>().enabled = true;
                other.transform.GetComponent<SphereCollider>().enabled = false;

                if (countdotpoint == 4)
                {
                    if (SceneManager.GetActiveScene().name.Contains("15"))
                    {
                        Experiment15FlowManager.instance.Step13_EnableHighlighting(4);
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("14"))
                    {
                        Experiment14FlowManager.instance.Step13_EnableHighlighting(4);
                    }
                }
            }
        }
        else if (isWelding && other.transform.tag == "Slag")
        {
            if (!isChippingHammer)
            {
                if (isElectrode)
                {
                    transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    transform.GetChild(0).GetComponent<AudioSource>().Play();
                    other.transform.GetComponent<MeshRenderer>().enabled = false;
                    other.transform.GetComponent<BoxCollider>().enabled = false;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    other.transform.GetChild(2).gameObject.SetActive(true);

                    if (SceneManager.GetActiveScene().name.Contains("15"))
                    {
                        if (countlinePoint + 2 <= Experiment15FlowManager.instance.weldLine.transform.childCount)
                        {
                            countlinePoint++;
                            Experiment15FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                            Experiment15FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                        }
                        else
                        {
                            isWelding = false;
                            Experiment15FlowManager.instance.Step14_EnableHighlighting(2);
                        }
                    }
                    else if (SceneManager.GetActiveScene().name.Contains("14"))
                    {
                        if (countlinePoint + 2 <= Experiment14FlowManager.instance.weldLine.transform.childCount)
                        {
                            countlinePoint++;
                            Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                            Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                        }
                        else
                        {
                            isWelding = false;
                            Experiment14FlowManager.instance.Step14_EnableHighlighting(2);
                        }
                    }

                }
            }
            if (isChippingHammer)
            {
                other.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                gameObject.GetComponent<Outline>().enabled = false;
                StartCoroutine(HammerBreakpoint(other.gameObject));

                if (SceneManager.GetActiveScene().name.Contains("15"))
                {
                    if (countlinePoint + 2 <= Experiment15FlowManager.instance.weldLine.transform.childCount)
                    {
                        countlinePoint++;

                        Experiment15FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                        Experiment15FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                    }
                    else
                    {
                        Experiment15FlowManager.instance.Step16_EnableHighlighting(3);
                    }
                }
                else if (SceneManager.GetActiveScene().name.Contains("14"))
                {
                    if (countlinePoint + 2 <= Experiment14FlowManager.instance.weldLine.transform.childCount)
                    {
                        countlinePoint++;

                        Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                        Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                    }
                    else
                    {
                        Experiment14FlowManager.instance.Step16_EnableHighlighting(3);
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

    /*private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Electrode")
        {
            isElecctrode = true;
        }
        if (other.transform.tag == "DotPoint" && isElecctrode)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).GetComponent<AudioSource>().Play();
            countdotpoint++;
            other.transform.GetComponent<MeshRenderer>().material = blackMaterial;
            other.transform.GetComponent<SphereCollider>().enabled = false;

            if (countdotpoint == 4)
            {
                Experiment14FlowManager.instance.Step13_EnableHighlighting(4);
            }
        }
        if (isWelding && other.transform.tag == "Slag")
        {
            Debug.Log(Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).name + " --> " + other.transform.name);
            if (Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).name == other.transform.name)
            {
                countlinePoint++;
                if (!isChippingHammer)
                {
                    if (isElecctrode)
                    {
                        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                        transform.GetChild(0).GetComponent<AudioSource>().Play();
                        other.transform.GetComponent<MeshRenderer>().enabled = false;
                        other.transform.GetComponent<BoxCollider>().enabled = false;
                        other.transform.GetChild(1).gameObject.SetActive(true);
                        other.transform.GetChild(2).gameObject.SetActive(true);

                        if (countlinePoint + 1 <= Experiment14FlowManager.instance.weldLine.transform.childCount)
                        {
                            Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<MeshRenderer>().enabled = true;
                            Experiment14FlowManager.instance.weldLine.transform.GetChild(countlinePoint).GetComponent<BoxCollider>().enabled = true;
                        }
                        else
                        {
                            isWelding = false;
                            Experiment14FlowManager.instance.Step14_EnableHighlighting(2);
                        }
                    }
                }
            }
        }
    }
    */
}
