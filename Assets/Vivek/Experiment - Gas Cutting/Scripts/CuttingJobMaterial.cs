using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class CuttingJobMaterial : MonoBehaviour
{
    public static CuttingJobMaterial instance;
    [SerializeField] List<GameObject> markingPoints;
    [SerializeField] List<MarkingLinePoint> markingLinePoints;
    [SerializeField] LineRenderer markingLine, markingLine2, markingLine3;
    [SerializeField] int currentMarking = 0;
    [SerializeField] List<GameObject> scriberHighlights;
 public   int totalLineMarkingPoints = 0, totalLineMarkingPoints2,totalLineMarkingPoints3;
   public int lineMarkingPoint = 0;
    public int LineMarkingPoint { get => lineMarkingPoint; set => lineMarkingPoint = value; }

    [Header("-------Hammer punch-------")]
    //public GameObject SCriberHitpoint;
    [SerializeField] List<PunchMarkingPoint> centerPunchMarkingPoints;
    [SerializeField] int currentCPMarkingPointIndex = 0;
    PunchMarkingPoint CurrentMarkingPoint => centerPunchMarkingPoints[currentCPMarkingPointIndex];
    public GameObject SteelScale;
    public int CountLine;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
  //      StartScriberMarking();
    }

    //SCRIBER MARKING
    public void StartScriberMarking()
    {
        Debug.Log("StartScriberMarking");
        TurnOnMarkingPoint();
        totalLineMarkingPoints = markingLine.transform.childCount;
        /*totalLineMarkingPoints2 = markingLine2.transform.childCount;
        totalLineMarkingPoints3 = markingLine3.transform.childCount;*/
    }

    void TurnOnMarkingPoint()
    {
        markingPoints[currentMarking].SetActive(true);
        Debug.Log("TurnOnMarkingPoint   " + markingPoints[currentMarking].name);
        if (markingPoints[currentMarking].GetComponentInChildren<MarkingPointGet>())
            markingPoints[currentMarking].GetComponentInChildren<MarkingPointGet>().OnMarkingDone += OnMarkingDone;
    }
    public int countpoint;

    void OnMarkingDone()
    {
        Debug.Log("Call OnMarkingDone");
        countpoint++;
        if (countpoint==2|| countpoint ==4 || countpoint==6)
        {
            return;
        }
        else
        {
        currentMarking++;
        TurnOnMarkingPoint();

        }
      /*   if (currentMarking < markingPoints.Count)
        {
            TurnOnMarkingPoint();
        }*/
      if (currentMarking ==  1)
        {
            markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
            markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);
        }
     /* else if( currentMarking == 2)
        {
            markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
            markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);
        }*/
    }
    void OnMarkingPointScriberEnter(int index, Vector3 position)
    {
        Debug.Log("OnMarkingPointScriberEnter");
        if (index == LineMarkingPoint)
        {
            markingLinePoints[index].gameObject.SetActive(false);
            OnMarkingDone(position);
        }
    }
    void OnMarkingDone(Vector3 position)
    {
     //   Debug.Log("OnMarkingDone");
        lineMarkingPoint++;
        markingLine.positionCount++;
        markingLine.SetPosition(markingLine.positionCount - 1, position);
        if (lineMarkingPoint >= totalLineMarkingPoints)
        {
            Debug.Log("DoneMarking");
            //OnScriberMarkingDone?.Invoke();
            CountLine += 1;

            Enable_2_LinePoint();

        }
        scriberHighlights.ForEach(highlight => highlight.SetActive(false));
    }
    public void Enable_2_LinePoint()
    {
        if (CountLine==3)
        {
            StartCenterPunchMarking();

        }
        else
        {
            if (CountLine == 1)
            {
               /* currentMarking++;
                if (currentMarking < markingPoints.Count)
                {
                    TurnOnMarkingPoint();
                }*//* currentMarking++;
                if (currentMarking < markingPoints.Count)
                {
                    TurnOnMarkingPoint();
                }*/
                totalLineMarkingPoints = markingLine2.transform.childCount;
                markingLine = markingLine2;
                currentMarking++;
                SteelScale.GetComponent<BoxCollider>().enabled = true;
                Debug.Log("DoneMarking ***");
                TurnOnMarkingPoint();
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);
                lineMarkingPoint = 0;
                //  currentMarking = 0;
            }
            else
            {
                currentMarking++;
                if (currentMarking < markingPoints.Count)
                {
                    TurnOnMarkingPoint();
                }
                totalLineMarkingPoints = markingLine3.transform.childCount;
                markingLine = markingLine3;

                SteelScale.GetComponent<BoxCollider>().enabled = true;
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);
                lineMarkingPoint = 0;
                currentMarking++;
                TurnOnMarkingPoint();
                //  currentMarking = 0;

            }
        }
    }
    //CENTER PUNCH
    public void StartCenterPunchMarking()
    {
        Debug.Log("StartCenterPunchMarking hammmer inside ");
        // SCriberHitpoint.transform.tag = "DotPoint";
        CurrentMarkingPoint.gameObject.SetActive(true);
        CenterPunch.OnHammerHit += OnHammerHit;
    }
    private void OnHammerHit()
    {
        if (CurrentMarkingPoint.IsCenterPunchInside)
        {
        Debug.Log("Call hammmer inside ");
            CurrentMarkingPoint.MarkingDone();
            currentCPMarkingPointIndex++;
            if (currentCPMarkingPointIndex < centerPunchMarkingPoints.Count)
                CurrentMarkingPoint.gameObject.SetActive(true);
            else
            {
                CenterPunch.OnHammerHit -= OnHammerHit;
                Debug.Log("CuttingHummer puch done");
                GasCuttingManager.instance.checkStep5();
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
       /* if (other.transform.tag == "Job")
        {
        Debug.Log("other 1" + other.gameObject.name);

            other.transform.tag = "Untagged";
            GasCuttingManager.instance.CheckJobPlace();
        }
        else*/ if (other.transform.tag == "JobFlat")
        {
            Debug.Log("other 1" + other.gameObject.name);
            other.transform.tag = "Untagged";
            this.GetComponent<XRGrabInteractable>().enabled = false;
            GasCuttingManager.instance.CheckJobFlatPlace();
        }
    }
}