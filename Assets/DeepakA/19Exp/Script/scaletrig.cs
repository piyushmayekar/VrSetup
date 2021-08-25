using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaletrig : MonoBehaviour
{
    // Start is called before the first frame update
    public string trigName;

    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == trigName)
        {
            transform.position = other.transform.position;
            transform.eulerAngles = other.transform.eulerAngles;
            transform.GetChild(0).name = "trigon";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == trigName)
        {
            transform.GetChild(0).name = "trigoff";
        }
    }
}
