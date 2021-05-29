using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour

{

    public Transform Freezeangle;
    public bool isFreeze;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFreeze)
        {
            transform.localEulerAngles = Freezeangle.localEulerAngles;
        }
    }
}
