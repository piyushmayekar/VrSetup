using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placingplate : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Welding_Base")
        {
            transform.name = "Done";
        }
    }
}
