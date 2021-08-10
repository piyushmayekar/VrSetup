
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Add last left point highlight to compelete the marking
//Add the ruler grab for each marking
public class ScriberMarking_VL : MonoBehaviour
{
    Rigidbody rb;
    public LineSegment_VL lineSegment;
    public LineRenderer MarkingLine;

    public List<GameObject> markingPoints = new List<GameObject>();
    private UnityEvent CallMethodOnMarkingDone = new UnityEvent();
    private UnityEvent CallMethodOnMeasuringDone = new UnityEvent();

    public int countPoint;
    public int currentcount = 0;
    public bool markingDone = false;
    private int index = 1;
    bool readyForOperation;

    private AudioSource audio;


    private GameObject SteelRuler;
    private GameObject ScriberHL;

    public int countMeasurePoint;
    public int currentMeasureCount = 0;
    public bool isMeasuring;
    public Material measurePointMat;
    private void Start()
    {
        audio =  GetComponent<AudioSource>();
    }

    public void SetScriberForMeasuremetn(Transform WorkPiece,int _measureCount,int lineNo)
    {
        string measureObjName = "MeasurePoint" + lineNo.ToString();
        GameObject measureObj = WorkPiece.Find(measureObjName).gameObject;
        measureObj.SetActive(true);
        countMeasurePoint = _measureCount;
        isMeasuring = true;
    }

    public void EmptyParamsForMeaurement()
    {
        isMeasuring = false;
        countMeasurePoint = 0;
        currentMeasureCount = 0;
    }
    public void SetScriberMarkingParams(Transform WorkPiece,Transform steelRuler, int indexOfMarkingLineObject,GameObject _ScriberHL)
    {
        string markingLineObjName = "MarkingLine" + indexOfMarkingLineObject.ToString();
        GameObject markingLine = WorkPiece.Find(markingLineObjName).gameObject;
        markingLine.SetActive(true);
        ScriberHL = _ScriberHL;
        ScriberHL.SetActive(true);
        

        if (markingLine)
        {
            MarkingLine = markingLine.GetComponent<LineRenderer>();
            lineSegment = markingLine.GetComponent<LineSegment_VL>();
        }

        string markingObjName = "MarkingPoints" + indexOfMarkingLineObject.ToString();
        Transform MarkingObject = WorkPiece.Find(markingObjName);
        MarkingObject.gameObject.SetActive(true);

        if (MarkingObject)
        {
            foreach (Transform point in MarkingObject)
            {
                markingPoints.Add(point.gameObject);
                //Debug.Log("marking Points added to list");
            }

            countPoint = markingPoints.Count;
            //Debug.Log("Total marking Point count " + countPoint.ToString());
        }
        SteelRuler = steelRuler.gameObject;
        SteelRuler.GetComponentInChildren<MeshRenderer>().enabled = true;
        SteelRuler.GetComponentInChildren<BoxCollider>().enabled = true;
        SteelRuler.transform.localPosition = lineSegment.steelRulerPositon;
        rb = gameObject.GetComponent<Rigidbody>();
        readyForOperation = true;

        
    }

    public void SetScriberMarkingParams(Transform WorkPiece, Transform steelRuler, int indexOfMarkingLineObject)
    {
        string markingLineObjName = "MarkingLine" + indexOfMarkingLineObject.ToString();
        GameObject markingLine = WorkPiece.Find(markingLineObjName).gameObject;
        markingLine.SetActive(true);
        


        if (markingLine)
        {
            MarkingLine = markingLine.GetComponent<LineRenderer>();
            lineSegment = markingLine.GetComponent<LineSegment_VL>();
        }

        string markingObjName = "MarkingPoints" + indexOfMarkingLineObject.ToString();
        Transform MarkingObject = WorkPiece.Find(markingObjName);
        MarkingObject.gameObject.SetActive(true);

        if (MarkingObject)
        {
            foreach (Transform point in MarkingObject)
            {
                markingPoints.Add(point.gameObject);
               // Debug.Log("marking Points added to list");
            }

            countPoint = markingPoints.Count;
            //Debug.Log("Total marking Point count " + countPoint.ToString());
        }
        SteelRuler = steelRuler.gameObject;
        SteelRuler.GetComponentInChildren<MeshRenderer>().enabled = true;
        SteelRuler.GetComponentInChildren<BoxCollider>().enabled = true;
        SteelRuler.transform.localPosition = lineSegment.steelRulerPositon;
        rb = gameObject.GetComponent<Rigidbody>();
        readyForOperation = true;


    }
    private void DrawLine(int _index)
    {
        if (lineSegment)
        {
            Vector3 pos = lineSegment.GetPositionAtIndex(_index);
            if (MarkingLine)
            {
                MarkingLine.SetPosition(index, pos);
            }
            else
            {
                Debug.LogError("No Markingline Found");
            }
        }
        else
        {
            Debug.LogError("No Line Segmenmt Found");
        }



    }

    private void EmptyParams()
    {
        // yield return new WaitForSeconds(0.1f);
        MarkingLine = null;
        markingPoints.Clear();
        lineSegment = null;
        markingDone = false;
        currentcount = 0;
        countPoint = 0;
        SteelRuler = null;
       // Debug.Log("Reseting the params of scriber");

    }

    public void EmptyParamsForMeasuring()
    {
        currentMeasureCount = 0;
        countMeasurePoint = 0;
    }

    public void AssignMethodOnMarkingDone(UnityAction method)
    {
        if (CallMethodOnMarkingDone != null)
        {
            CallMethodOnMarkingDone.RemoveAllListeners();
        }

        CallMethodOnMarkingDone.AddListener(method);
    }

    public void AssignMethodOnMeasuringDone(UnityAction method)
    {
        if (CallMethodOnMeasuringDone != null)
        {
            CallMethodOnMeasuringDone.RemoveAllListeners();
        }
        CallMethodOnMeasuringDone.AddListener(method);
    }
    #region InBuilt Methods
    private void OnTriggerExit(Collider other)
    {
        if (!readyForOperation)
        {
            return;
        }
        else
        {
            //audio.Stop();
            if (other.tag == "MarkPoint")
            {
                rb.freezeRotation = false;

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (isMeasuring)
        {
            if (other.tag == "MarkPoint")
            {
                other.gameObject.GetComponent<MeshRenderer>().sharedMaterial = measurePointMat;
                other.tag = "Disable";
                HLSound.player.PlayHighlightSnapSound();
                currentMeasureCount++;
                if (currentMeasureCount == countMeasurePoint)
                {
                    if (CallMethodOnMeasuringDone != null)
                    {
                        CallMethodOnMeasuringDone.Invoke();
                    }
                    //Invoke method
                }
            }
        }
        if (!readyForOperation)
        {
            return;
        }
        else
        {
            if (other.tag == "ScriberHighlight")
            {
                HLSound.player.PlayHighlightSnapSound();

                other.gameObject.SetActive(false);
                //ScriberHL.SetActive(false);

            }

            if (other.tag == "MarkPoint")
            {
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
                
                other.transform.GetComponent<MeshRenderer>().enabled = false;
                other.transform.GetComponent<SphereCollider>().enabled = false;
                int index = currentcount;
                DrawLine(index);
                currentcount++;
                rb.freezeRotation = true;

                if (currentcount >= countPoint)
                {
                    markingDone = true;
                    audio.Stop();
                    SteelRuler.GetComponentInChildren<MeshRenderer>().enabled = false;
                    SteelRuler.GetComponentInChildren<BoxCollider>().enabled = false;
                    EmptyParams();
                    if (CallMethodOnMarkingDone != null)
                    {
                        CallMethodOnMarkingDone.Invoke();

                    }
                    rb.freezeRotation = false;

                }
                else
                {
                    markingPoints[currentcount].SetActive(true);
                }

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
    #endregion

   
    


}
