using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalJobPlatesManager : MonoBehaviour
{
    [SerializeField] Vector3 initPos;
    [SerializeField] Quaternion initRot;
    void Start()
    {
        initPos = transform.position;
        initRot = transform.rotation;
    }

    public void OnSelectExit()
    {
        transform.SetPositionAndRotation(initPos, initRot);
    }
}
