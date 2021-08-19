using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFinalTask : MonoBehaviour
{
    public GameObject[] obj;
    public static int Tasknum;
    
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (obj[0].transform.name == "Done" && obj[1].transform.name == "Done" && obj[2].transform.name == "Done" && obj[3].transform.name == "Done" && obj[4].transform.name == "Done" && obj[5].transform.name == "Done")
        {
            if (i < 2)
            {
                i++;
                transform.name = "Done";
                if (Tasknum < 4 && i == 1)
                {
                    Tasknum++;
                    print("Task: " +Tasknum); ;
                }
            }
        }
    }
}
