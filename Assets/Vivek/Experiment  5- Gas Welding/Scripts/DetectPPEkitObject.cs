using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPPEkitObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.tag == "Kit")
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.SetActive(false);
        }*/

        if (other.gameObject.name == "RightHandAnchor")
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.SetActive(false);
        }
    }
}
