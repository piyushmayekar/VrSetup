using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class measure : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] GreenScale;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GreenScale[0].activeSelf == false && GreenScale[1].activeSelf == false && GreenScale[2].activeSelf == false)
        {
            transform.name = "Done";
        }
    }
}
