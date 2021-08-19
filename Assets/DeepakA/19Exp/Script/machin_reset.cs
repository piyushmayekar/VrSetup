using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machin_reset : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject plugpos;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,plugpos.transform.position,1);
        transform.rotation =Quaternion.Lerp(transform.rotation, plugpos.transform.rotation,1);
    }
}
