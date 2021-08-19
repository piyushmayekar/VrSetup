using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialtrig1 : MonoBehaviour
{
    // Start is called before the first frame update
    public string trigObj = "Handtrig";
    public float tim;
    public GameObject otherscale;
    public GameObject pos;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == trigObj)
        {
            tim += 1 * Time.deltaTime;
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            other.transform.position = pos.transform.position;
            other.transform.eulerAngles = pos.transform.eulerAngles;
            if (tim > 4)
            {
                tim = 0;
                gameObject.SetActive(false);
                otherscale.SetActive(true);
            }
            
        }
    }
}
