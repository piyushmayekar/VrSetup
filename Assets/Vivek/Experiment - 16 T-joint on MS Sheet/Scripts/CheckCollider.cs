using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckCollider : MonoBehaviour
{
    public string Tagname;

    public UnityEvent callAnyOtherMethod;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tagname)
        {
          //  Debug.Log("call get");
            callAnyOtherMethod.Invoke();
        }
    }

}
