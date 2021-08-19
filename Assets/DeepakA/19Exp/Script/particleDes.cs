using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDes : MonoBehaviour
{
    // Start is called before the first frame update
    public string trigName;

    private void Start()
    {
       // Destroy(gameObject, 5);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == trigName)
        {
            Destroy(gameObject,0.1f);
            other.gameObject.transform.name = "Done";
        }
        if (other.gameObject.name == "Done" || other.gameObject.name == "polySurface121")
        {
            Destroy(gameObject);
        }
    }
}
