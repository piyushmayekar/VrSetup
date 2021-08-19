using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFinalTask1 : MonoBehaviour
{
    public GameObject[] obj;

    
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (obj[0].transform.name == "halfDone" && obj[1].transform.name == "halfDone")
        {
            if (i < 2)
            {
                i++;
                transform.name = "Done";

            }
        }
    }
}
