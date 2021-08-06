using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PiyushUtils;


public class TrySquare_VL : MonoBehaviour
{
    Rigidbody rb;
    public UnityEvent CallMethodOnCheckRightAngle = new UnityEvent(); 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TrySquareHL")
        {
            other.gameObject.SetActive(false);
            HLSound.player.PlayHighlightSnapSound();
            CheckRightAngle(other.transform);
        }
    }

    public void AssignMethodOnCheckRightAngle(UnityAction method)
    {
        if (CallMethodOnCheckRightAngle != null)
        {
            CallMethodOnCheckRightAngle.RemoveAllListeners();
        }

        CallMethodOnCheckRightAngle.AddListener(method);
    }

    IEnumerator CheckAngle()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Debug.Log("Checking Angle");
        yield return new WaitForSeconds(1);
        Debug.Log("Checked Angle");
        if (CallMethodOnCheckRightAngle != null)
        {
            gameObject.GetComponent<CustomXRGrabInteractable>().enabled = true;
            rb.constraints = RigidbodyConstraints.None;
            CallMethodOnCheckRightAngle.Invoke();
        }
    }

    public void CheckRightAngle(Transform Hl)
    {
        transform.position = Hl.position;
        transform.eulerAngles = Hl.eulerAngles;
        StartCoroutine(CheckAngle());
    }


}
