using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckJob1 : MonoBehaviour
{
    public GameObject[] obj;
    private int i;
    private int ia;
    private int ib;
    private int ic;

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (obj[0].transform.name == "Done" && obj[1].transform.name == "Done")
        {
            if (i < 5)
            {
                i++;
                transform.name = "Done";

            }
        }

        if (obj[0].transform.name == "Done" && obj[1].transform.name == "Linedown")
        {
            if (ia < 5)
            {
                ia++;
                transform.name = "halfDone";
                //obj[1].SetActive(true);
                
            }
        }
       
        if (obj[0].transform.name == "Lineup" && obj[1].transform.name == "Linedown")
        {
            if (ic < 5)
            {
                ic++;
                obj[1].SetActive(false);
            }
        }
    }
}
