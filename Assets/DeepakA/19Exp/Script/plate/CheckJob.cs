using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckJob : MonoBehaviour
{
    public GameObject obj;
    private int i;
    public int ia = 0;
    private int iaa = 0;
    private int childNames;
    private bool countOff = true;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        iaa = ia;
    }

    // Update is called once per frame
    void Update()
    {
        if (countOff == true) {
            counter();
            countOff = false;
        }
        if (obj.transform.name == "Done")
        {
            if (i < 5)
            {
                i++;
                transform.name = "Done";

                GameObject obj1 = GameObject.FindWithTag("MarkPoint");
                if (obj1 != null)
                {
                   // obj1.transform.name = "0";
                }
                GameObject obj2 = GameObject.FindWithTag("DotPoint");
                if (obj2 != null)
                {
                   // obj2.transform.name = "0";
                }
                GameObject obj3 = GameObject.FindWithTag("Spark");
                if (obj3 != null)
                {
                   // obj3.transform.name = "0";
                }
                GameObject obj4 = GameObject.FindWithTag("Hammer");
                if (obj4 != null)
                {
                   // obj4.transform.name = "0";
                }
            }
        }
    }
    public void counter()
    {
        for (int i = 0;i< transform.childCount;i++)
        {
           // childNames = childNames + i;
            //print("ChildName: "+ ia);
            transform.GetChild(i).transform.name = "" + iaa;
            iaa++;

        }
    }
}
