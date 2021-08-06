using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HackSaw_VL : MonoBehaviour
{
    Rigidbody rb;
    LineRenderer cuttingLine;
    LineSegment_VL lineSegment;

    public List<GameObject> cuttingPoints = new List<GameObject>();
    private UnityEvent CallMethodOnCuttingDone = new UnityEvent();
    public List<GameObject> punchPoints = new List<GameObject>();

    public int cutPointCount;
    public int currentCount = 0;
    public bool cuttingDone;
    private int index = 0;
    public bool readyForOperation;

    private GameObject HackSawHL;

    private float cutTime = 10f;
    public float speed = 2f;

    private AudioSource audio;

    private void Start()
    {
       rb = gameObject.GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HackSawHighlight")
        {
            HLSound.player.PlayHighlightSnapSound();

            other.gameObject.SetActive(false);
        }
    }

    public void SetHackSawCuttingParams(Transform workPiece,int indexOfCuttingLineObject)
    {
        string cuttingPointObjName = "HacksawCutPoints" + indexOfCuttingLineObject.ToString();
        GameObject cuttingPointObj = workPiece.Find(cuttingPointObjName).gameObject;
        cuttingPointObj.SetActive(true);

        string cuttingLineObjName = "CuttingLine" + indexOfCuttingLineObject.ToString();
        GameObject cuttingLineObj = workPiece.Find(cuttingLineObjName).gameObject;
        cuttingLineObj.SetActive(true);

        string punchPointObjName = "PunchPoints" + indexOfCuttingLineObject.ToString();
        GameObject punchPointObject = workPiece.Find(punchPointObjName).gameObject;
        punchPointObject.SetActive(false);

        if (punchPointObject)
        {
            foreach (Transform punchPoint in punchPointObject.transform)
            {
                punchPoints.Add(punchPoint.gameObject);
            }
        }

        if (cuttingLineObj)
        {
            cuttingLine = cuttingLineObj.GetComponent<LineRenderer>();
            lineSegment = cuttingLineObj.GetComponent<LineSegment_VL>();
        }

        if (cuttingPointObj)
        {
            foreach (Transform cutPoint in cuttingPointObj.transform)
            {
                cuttingPoints.Add(cutPoint.gameObject);
            }

            cutPointCount = cuttingPoints.Count;
        }

        readyForOperation = true;
       // HackSawHL.SetActive(true);

        


    }

    private void OnCollisionStay(Collision collision)
    {
        if (!readyForOperation)
        {
            return;
        }
        else
        {
            if (collision.collider.tag == "Job")
            {
                rb.freezeRotation = true;
                
                
            }

            if (collision.collider.tag == null)
            {
                return;
            }
            else
            {
               

                if (collision.collider.tag == "HacksawCutPoint")
                {
                    cutTime -= Time.deltaTime * speed;
                    if (!audio.isPlaying)
                    {
                        audio.Play();
                    }
                    //rb.freezeRotation = true;
                    if (cutTime <= 0)
                    {
                        

                        int index = currentCount;

                        if (currentCount == cutPointCount - 1 )
                        {

                           
                            if (CallMethodOnCuttingDone != null)
                            {
                                ReduceLineSize(index);
                                //if (punchPoints[currentCount] != null)
                                //{
                                //    punchPoints[currentCount].SetActive(false);
                                //}
                                
                                cuttingPoints[currentCount].gameObject.GetComponent<MeshRenderer>().enabled = false;
                                cuttingPoints[currentCount].gameObject.GetComponent<BoxCollider>().enabled = false;
                                CallMethodOnCuttingDone.Invoke();
                                
                            }
                        }
                        else
                        {
                            ReduceLineSize(index);
                            //if (punchPoints[currentCount] != null)
                            //{
                            //    punchPoints[currentCount].SetActive(false);
                            //}
                            
                            cuttingPoints[currentCount].gameObject.GetComponent<MeshRenderer>().enabled = false;
                            cuttingPoints[currentCount].gameObject.GetComponent<BoxCollider>().enabled = false;
                            currentCount++;
                            cuttingPoints[currentCount].SetActive(true);
                            cutTime = 7f;
                        }
                    }

                    

                }
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
            if (collision.collider.tag == null)
            {
                return;
            }
            else
            {
                if (collision.collider.tag == "Job")
                {
                    rb.freezeRotation = false;
                    audio.Stop();
                }

                //if (collision.collider.tag == "HacksawCutPoint")
                //{
                //    rb.freezeRotation = false;
                //}
            }
        }
    }

    public void AssignMethodOnCuttingDone(UnityAction method)
    {
        if (CallMethodOnCuttingDone != null)
        {
            CallMethodOnCuttingDone.RemoveAllListeners();
        }

        CallMethodOnCuttingDone.AddListener(method);
    }

    public void EmptyParams()
    {
        cutPointCount = 0;
        currentCount = 0;
        cuttingDone = false;
        cuttingPoints.Clear();
        cuttingLine = null;
        punchPoints.Clear();
        lineSegment = null;


    }

    void ReduceLineSize(int _index)
    {
        if (lineSegment)
        {
            Vector3 pos = lineSegment.GetPositionAtIndex(_index);

            if (cuttingLine)
            {
                cuttingLine.SetPosition(index, pos);
                
            }
        }
        else
        {
            Debug.Log("No line Segment");
        }
    }
    
}
