using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        AndroidJavaObject TM = new AndroidJavaObject("android.telephony.TelephonyManager");

        AndroidJavaObject TM_OBJ1 = TM.CallStatic<AndroidJavaObject>("Instance");

        string imei1 = TM_OBJ1.Call<string>("getDeviceId");



        AndroidJavaObject TM_OBJ2 = TM.GetStatic<AndroidJavaObject>("Instance");

        string imei2 = TM_OBJ2.Call<string>("getDeviceId");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
