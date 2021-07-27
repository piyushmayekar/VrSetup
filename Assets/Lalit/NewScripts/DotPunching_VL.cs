using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DotPunching_VL : MonoBehaviour
{
    public List<GameObject> punchHitPoints = new List<GameObject>();
    public List<GameObject> punchPoints = new List<GameObject>();
    public int countPoint;
    public int currentCount = 0;
    public bool punchingDone = false;
    public bool isPointedOnHitPoint = false;
    private UnityEvent CallMethodOnPunchingDone = new UnityEvent();
    bool readyForOperation;
    Rigidbody rb;

    private GameObject CenterPunchHL;

    private AudioSource audio;
   // public AudioClip[] clips;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void SetCenterPunchParams(Transform WorkPiece, int indexOfPunchingLineObject, GameObject centerPunchHL, GameObject HammerHL)
    {
        string punchHitPointObjName = "PunchHitPoints" + indexOfPunchingLineObject.ToString();
        GameObject punchHitPointObject = WorkPiece.Find(punchHitPointObjName).gameObject;
        punchHitPointObject.SetActive(true);
        rb = gameObject.GetComponent<Rigidbody>();
        CenterPunchHL = centerPunchHL;
        CenterPunchHL.SetActive(true);
        HammerHL.SetActive(true);

        if (punchHitPointObject)
        {
            foreach (Transform punchHitPoint in punchHitPointObject.transform)
            {
                punchHitPoints.Add(punchHitPoint.gameObject);
            }
            countPoint = punchHitPoints.Count;
        }

        string punchPointObjName = "PunchPoints" + indexOfPunchingLineObject.ToString();
        GameObject punchPointObject = WorkPiece.Find(punchPointObjName).gameObject;
        punchPointObject.SetActive(true);

        if (punchPointObject)
        {
            foreach (Transform punchPoint in punchPointObject.transform)
            {
                punchPoints.Add(punchPoint.gameObject);
            }
        }
        readyForOperation = true;
    }
    public void SetCenterPunchParams(Transform WorkPiece, int indexOfPunchingLineObject)
    {
        string punchHitPointObjName = "PunchHitPoints" + indexOfPunchingLineObject.ToString();
        GameObject punchHitPointObject = WorkPiece.Find(punchHitPointObjName).gameObject;
        punchHitPointObject.SetActive(true);
        rb = gameObject.GetComponent<Rigidbody>();
       

        if (punchHitPointObject)
        {
            foreach (Transform punchHitPoint in punchHitPointObject.transform)
            {
                punchHitPoints.Add(punchHitPoint.gameObject);
            }
            countPoint = punchHitPoints.Count;
        }

        string punchPointObjName = "PunchPoints" + indexOfPunchingLineObject.ToString();
        GameObject punchPointObject = WorkPiece.Find(punchPointObjName).gameObject;
        punchPointObject.SetActive(true);

        if (punchPointObject)
        {
            foreach (Transform punchPoint in punchPointObject.transform)
            {
                punchPoints.Add(punchPoint.gameObject);
            }
        }
        readyForOperation = true;
    }
    private void EmptyParams()
    {
        punchHitPoints.Clear();
        punchPoints.Clear();
        countPoint = 0;
        currentCount = 0;
        punchingDone = false;
    }
    public void AssignMethodOnPunchingDone(UnityAction method)
    {
        if (CallMethodOnPunchingDone != null)
        {
            CallMethodOnPunchingDone.RemoveAllListeners();
        }

        CallMethodOnPunchingDone.AddListener(method);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!readyForOperation)
        {
            return;
        }
        else
        {
            if (other.tag == "CenterPunchHighlight")
            {
                HLSound.player.PlayHighlightSnapSound();

                other.gameObject.SetActive(false);
            }

            if (other.tag == "PunchPoint")
            {
                isPointedOnHitPoint = true;
                rb.freezeRotation = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!readyForOperation)
        {
            return;
        }
        else
        {
            if (other.tag == "PunchPoint")
            {
                isPointedOnHitPoint = false;
                rb.freezeRotation = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!readyForOperation)
        {
            return;
        }
        else
        {
            if (collision.collider.tag == "Hammer" && isPointedOnHitPoint)
            {
                audio.Play();
                punchHitPoints[currentCount].GetComponent<MeshRenderer>().enabled = false;
                punchHitPoints[currentCount].GetComponent<SphereCollider>().enabled = false;

                punchPoints[currentCount].SetActive(true);

                currentCount++;

                if (currentCount == countPoint)
                {
                    punchingDone = true;
                    EmptyParams();
                    if (CallMethodOnPunchingDone != null)
                    {
                        CallMethodOnPunchingDone.Invoke();
                    }
                    rb.freezeRotation = false;
                }
                else
                {
                    punchHitPoints[currentCount].SetActive(true);
                }

                isPointedOnHitPoint = false;
            }

            if (collision.collider.tag == "Job")
            {
                rb.freezeRotation = true;
            }
        }

        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!readyForOperation)
        {
            return;
        }
        else
        {
            if (collision.collider.tag == "Job")
            {
                rb.freezeRotation = false;
            }
        }
    }


}
