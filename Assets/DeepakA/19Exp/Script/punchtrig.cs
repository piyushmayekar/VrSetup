using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchtrig : MonoBehaviour
{
    public GameObject punchOn;
    public GameObject pos;
    public bool trig = false;

    private void Start()
    {
        punchOn.GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (trig == true)
        {
            punchOn.GetComponent<Collider>().enabled = true;
            Instantiate(Resources.Load("Hammer"), pos.transform.position, pos.transform.rotation);
            trig = false;
        }
        else
        {
            punchOn.GetComponent<Collider>().enabled = false;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HammerHit")
        {
            trig = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HammerHit")
        {
            trig = false;
        }
    }
}
