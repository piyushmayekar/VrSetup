using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaletrig1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject scaletrig;
    public GameObject backtrig;
    public float tim;

    private void Update()
    {
        if (transform.name == "Done")
        {
            if (tim < 10)
            {
                tim++;
            }
            if (tim == 1) 
            {
                scaletrig.SetActive(true);
                backtrig.SetActive(false);
            }
        }
        else
        {
            scaletrig.SetActive(false);
            //backtrig.SetActive(false);
        }
    }
}
