using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public static int count;
    public bool wait;

    private void Update()
    {
        if (wait)
        {
            //wait
        }
        else
        {
            Destroy(gameObject, 5f);
        }

    }
}
