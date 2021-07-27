using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;



public class WeldingFlame_VL : MonoBehaviour
{
    public List<GameObject> weldingPoints = new List<GameObject>();
    public Material slagMat;
    public Material weldingFinishMat;
    public Material headMat;

    public int countPoint;
    public int currentCount = 0;
    public bool fusionRunDone = false;

    public bool isPointedOnWeldingPoint;
    public bool isContactWithFillerRod;
    private UnityEvent CallMethodOnFusionRunDone = new UnityEvent();

    [SerializeField]
    private FillerRod_VL fillerRod;
    private GameObject currentWeldPoint;
    private Material currentWeldPointMat;
    public float speed = 50f;
    public bool weldPointHeated = false;

    public Rigidbody WeldingTorchRb;

    public bool freezXPos;
    public bool freezZPos;
    public void SetFillerRod(GameObject _fillerRod)
    {

        fillerRod = _fillerRod.GetComponent<FillerRod_VL>();
    }
    public void SetWeldingFlameParams(Transform WorkPiece, int indexOfWeldingLineObject)
    {
        string weldingPointObjName = "WeldingPoints" + indexOfWeldingLineObject.ToString();
        GameObject weldingPointObject = WorkPiece.Find(weldingPointObjName).gameObject;
        weldingPointObject.SetActive(true);

        if (weldingPointObject)
        {
            foreach (Transform weldingPoint in weldingPointObject.transform)
            {
                weldingPoints.Add(weldingPoint.gameObject);
            }

            countPoint = weldingPoints.Count;
        }
    }

   

    private void EmptyParams()
    {
        weldingPoints.Clear();
        countPoint = 0;
        currentCount = 0;
        fusionRunDone = false;
    }

    private void Update()
    {
        if (weldPointHeated)
        {
            fillerRod.GetComponent<Outline>().enabled = true;
        }
               
    }

    private void OnTriggerEnter(Collider other)
    {

       

        if (fillerRod != null)
        {
            if (other.tag == "WeldingTorchHighlight")
            {
                HLSound.player.PlayHighlightSnapSound();
                other.gameObject.SetActive(false);

                Vector3 pos = WeldingTorchRb.transform.localPosition;
                pos = other.transform.position;
                WeldingTorchRb.transform.localPosition = pos;

                if (freezXPos)
                {
                    WeldingTorchRb.constraints = RigidbodyConstraints.FreezePositionX;
                }
                else if (freezZPos)
                {
                    WeldingTorchRb.constraints = RigidbodyConstraints.FreezePositionZ;
                }
                WeldingTorchRb.transform.localEulerAngles = other.transform.localEulerAngles;
                WeldingTorchRb.GetComponent<CustomXRGrabInteractable>().trackRotation = false;
            }

            if (other.tag == "FillerRod" && weldPointHeated)
            {
                Debug.Log("Filler Rod Entered When headed");
                if (weldingPoints[currentCount] != null)
                {
                    weldingPoints[currentCount].GetComponent<WeldHeadPonit>().enabled = true;
                    weldingPoints[currentCount].GetComponent<CapsuleCollider>().enabled = false;
                    fillerRod.ReduceScale();
                   
                    currentCount++;

                    if (currentCount >= countPoint)
                    {
                        fusionRunDone = true;
                        WeldingTorchRb.constraints = RigidbodyConstraints.None;
                        WeldingTorchRb.GetComponent<CustomXRGrabInteractable>().trackRotation = true;

                        EmptyParams();
                        if (CallMethodOnFusionRunDone != null)
                        {
                            CallMethodOnFusionRunDone.Invoke();
                        }
                    }
                    else
                    {
                        if (weldingPoints[currentCount] != null)
                        {
                            weldingPoints[currentCount].SetActive(true);
                        }
                    }

                    weldPointHeated = false;
                    currentWeldPoint = null;
                }


            }else if (other.tag == "GasWeldingPoint" && !fusionRunDone)
            {
                if (weldingPoints[currentCount] != null)
                {
                    currentWeldPoint = weldingPoints[currentCount];
                    currentWeldPoint.GetComponent<MeshRenderer>().sharedMaterial = headMat;


                }
            }

        }

           

        if (other.tag == "FillerRod" && !weldPointHeated)
        {
            Debug.Log("Filler Rod Entered When Not headed");
        }
       

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "GasWeldingPoint" && !weldPointHeated)
        {
            
            if (currentWeldPoint != null)
            {
                if (other.gameObject == currentWeldPoint)
                {
                   weldPointHeated = MeltWeldPoint(other.gameObject);
                }
            }
            
        }

       
    }

    private bool MeltWeldPoint(GameObject gameObject)
    {

        Color color = gameObject.GetComponent<MeshRenderer>().sharedMaterial.color;
        color.r += Time.deltaTime * speed;
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = color;
        //Debug.Log(color.r.ToString());
        if (color.r >= 0.99f)
        {
            return true;
        }
        else
        {
           return false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //if (collision.collider.tag == "FillerRod")
        //{
        //    isContactWithFillerRod = false;
        //}

        if (fillerRod != null)
        {
            if (other.tag == "GasWeldingPoint")
            {
                Debug.Log("Flame is NO Contact with fillerRod");
            }
        }
        
    }

    public void AssignMethodOnFusionRunDone(UnityAction method)
    {
        if (CallMethodOnFusionRunDone != null)
        {
            CallMethodOnFusionRunDone.RemoveAllListeners();
        }

        CallMethodOnFusionRunDone.AddListener(method);
    }
}
