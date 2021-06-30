using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class JointMSPlate : MonoBehaviour
{
    public GameObject HighLighttpoint;
    public GameObject tPlat, GrabTpoint, GrabSupportplat, supportplat,highlightsupportplat;

    public SoundPlayer chippingHummer;
    public int countchippinghammer;
    public void Start()
    {
      //  Debug.Log("gas welding");

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "JobFlat")
        {
            Debug.Log("other 1" + other.gameObject.name);
            other.transform.tag = "Untagged";
            this.GetComponent<XRGrabInteractable>().enabled = false;
            GasJointweldingManager.instance.PlaceJobPlate();
            HighLighttpoint.SetActive(true);
            GrabTpoint.GetComponent<BoxCollider>().enabled = true;
            GrabTpoint.GetComponent<Outline>().enabled = true;
        }
     
    /* else  if (other.transform.tag == "ChippingHammer")
        {
            chippingHummer.PlayClip(0);
            countchippinghammer++;
            GasJointweldingManager.instance.hummerhighlight.SetActive(false);

            if (countchippinghammer >= 5)
            {
                GasJointweldingManager.instance.checkChappingHummer();
            }
        }*/
    }
    public void callJob()
    {
        HighLighttpoint.SetActive(false);
        GrabTpoint.SetActive(false);
        tPlat.SetActive(true);
        GrabTpoint.GetComponent<XRGrabInteractable>().enabled = false;
        highlightsupportplat.SetActive(true);
    }
    public void CallSuport()
    {
        highlightsupportplat.SetActive(false);
        GrabSupportplat.SetActive(false);
        supportplat.SetActive(true);
        GasJointweldingManager.instance.CheckJobFlatPlace();

    }
}
