using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public int i_scale;
    public GameObject[] scale;

    void Start()
    {
        //i_scale = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;i < scale.Length;i++)
        {
            if (i == i_scale)
            {
                scale[i_scale].SetActive(true);
            }
            else
            {
                scale[i].SetActive(false);
            }
        }
    }
}
