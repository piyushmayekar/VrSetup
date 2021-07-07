using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour

{
    public static FreezeRotation instance;
    public Transform Freezeangle;
    public bool isFreeze;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void EnableFreeze(bool isfreeze)
    {
        isFreeze = isfreeze;
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
