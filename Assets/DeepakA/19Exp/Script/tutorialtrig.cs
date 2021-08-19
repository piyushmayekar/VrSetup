using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialtrig : MonoBehaviour
{
    // Start is called before the first frame update
    public string trigObj = "Handtrig";

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == trigObj)
        {
            gameObject.SetActive(false);
        }
    }
}
