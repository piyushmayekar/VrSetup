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
            transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
            transform.eulerAngles = new Vector3(other.transform.eulerAngles.x, other.transform.eulerAngles.y, transform.eulerAngles.z);
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
