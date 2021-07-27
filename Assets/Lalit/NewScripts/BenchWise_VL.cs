using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;


public class BenchWise_VL : MonoBehaviour
{
    public Transform movingPart;
    public Handle_VL handle;
    public BenchWiseJaw jaw;
    public float speed;
    public void MoveForward()
    {
        Vector3 pos = movingPart.localPosition;
        pos.z -= Time.deltaTime * speed;
        movingPart.localPosition = pos;
        handle.transform.Rotate(Vector3.forward, Space.Self);
    }

    public void MoveBackWard()
    {
        Vector3 pos = movingPart.localPosition;
        pos.z += Time.deltaTime * speed;
        movingPart.localPosition = pos;
        handle.transform.Rotate(-Vector3.forward, Space.Self);

    }

    public void SetRotationDirection(bool isClockWise)
    {
        handle.isClockwise = isClockWise;
        handle.CanMove = true;
        handle.SetHighlightMat();
        jaw.jobfitted = false;

        
    }

    public void AssignMethodOnJobFit(UnityAction method)
    {
        //Debug.Log("Assign");
        jaw.AssignMethod(method);
    }
}
