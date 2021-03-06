using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public enum CuttingType { Gascut, CircularCut, BevelCut };
public enum experimentType { GasWelding, GasCutting, GasJointPlate }
public class CuttingJobMaterial : MonoBehaviour
{
    public static CuttingJobMaterial instance;
    public CuttingType cuttingType;

    [SerializeField] public List<GameObject> markingPoints;
    [SerializeField] List<MarkingLinePoint> markingLinePoints;
    [SerializeField] public LineRenderer markingLine, markingLine2, markingLine3;
    [SerializeField] int currentMarking = 0;
    [SerializeField] List<GameObject> scriberHighlights;
    public int totalLineMarkingPoints = 0;
    public int lineMarkingPoint = 0;
    public int LineMarkingPoint { get => lineMarkingPoint; set => lineMarkingPoint = value; }

    [Header("-------Hammer punch-------")]
    //public GameObject SCriberHitpoint;
    [SerializeField] List<PunchMarkingPoint> centerPunchMarkingPoints;
    [SerializeField] List<GameObject> cpDotPoint;
    [SerializeField] int currentCPMarkingPointIndex = 0;
    [SerializeField] SoundPlayer hummerCenterPunchsound, Scibersound;
    PunchMarkingPoint CurrentMarkingPoint => centerPunchMarkingPoints[currentCPMarkingPointIndex];
    public GameObject SteelScale, SnapScale1, SnapScale2, SnapScale3;
    public int CountLine;
    public List<Outline> outlines;
    public FreezeRotation centerPunch, scribal;
    void Awake()
    {
        instance = this;
    }

    //SCRIBER MARKING
    public void StartScriberMarking()
    {
        Debug.Log("StartScriberMarking");
        TurnOnMarkingPoint();
        //   totalLineMarkingPoints = markingLine.transform.childCount;
    }

    void TurnOnMarkingPoint()
    {
        Debug.Log("CALLINTurnOnMarkingPoint");
        markingPoints[currentMarking].SetActive(true);
        if (currentMarking < outlines.Count)
        {
            Debug.Log("CALLINGsdsa");
            outlines[currentMarking].enabled = true;
        }

        if (markingPoints[currentMarking].GetComponentInChildren<MarkingPointGet>())
            markingPoints[currentMarking].GetComponentInChildren<MarkingPointGet>().OnMarkingDone += OnMarkingDone;
    }
    public int countpoint;

    public void OnMarkingDone()
    {
        countpoint++;
        if (cuttingType == CuttingType.Gascut)
        {
            if (countpoint == 2 || countpoint == 4 || countpoint == 6)
            {
                Debug.Log("    " + currentMarking);

                return;
            }
            else
            {
                currentMarking++;
                Debug.Log(currentMarking);

                TurnOnMarkingPoint();

            }
        }
        else
        {

            currentMarking++;
            if (cuttingType == CuttingType.BevelCut && currentMarking == 1)
            {
                {
                    Debug.Log(currentMarking + "1111");
                    TurnOnMarkingPoint();
                    cuttingType = CuttingType.CircularCut;
                    return;
                }

            }
        }
        if (currentMarking >= 1)
        {

            if (cuttingType == CuttingType.CircularCut)
            {
                Debug.Log(currentMarking + "###");
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);

                markingLinePoints[0].GetComponent<Outline>().enabled = true;
                markingLinePoints[0].GetComponent<MeshRenderer>().enabled = true;

            }
            else if (cuttingType == CuttingType.BevelCut)
            {
                Debug.Log(currentMarking + "#@@@@#");
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);

                markingLinePoints[0].GetComponent<Outline>().enabled = true;
                markingLinePoints[0].GetComponent<MeshRenderer>().enabled = true;

            }
            else
            {
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);



            }

        }

    }
    public void onAddMarkingPoints()
    {

    }
    public int countIndex;
    void OnMarkingPointScriberEnter(int index, Vector3 position)
    {
        if (index == LineMarkingPoint)
        {
            Debug.Log("Play sound");
            Scibersound.PlayClip(0);
            // if (cuttingType == CuttingType.CircularCut)
            {
                if (index < markingLinePoints.Count)
                {
                    if (index + 1 != markingLinePoints.Count)
                    {
                        if (markingLinePoints[index + 1].GetComponent<Outline>())
                            markingLinePoints[index + 1].GetComponent<Outline>().enabled = true;

                        markingLinePoints[index + 1].GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
            countIndex = index;

            OnMarkingDone(position);
        }
    }
    void OnMarkingDone(Vector3 position)
    {
        lineMarkingPoint++;
        markingLine.positionCount++;
        markingLinePoints[countIndex].gameObject.SetActive(false);
        markingLine.SetPosition(markingLine.positionCount - 1, position);

        if (lineMarkingPoint >= totalLineMarkingPoints)
        {
            if (cuttingType == CuttingType.Gascut)
            {
                CountLine += 1;

                Enable_2_LinePoint();
            }
            else
            {
                /* scribal.GetComponent<XRGrabInteractable>().selectEntered = null;
                 Destroy(scribal);*/
                GasCuttingManager.instance.CheckJobPlace();
                for (int i = 0; i < outlines.Count; i++)
                {
                    outlines[i].enabled = false;
                }
                for (int i = 0; i < markingPoints.Count - 1; i++)
                {

                    markingPoints[i].SetActive(false);
                }
            }
        }
        scriberHighlights.ForEach(highlight => highlight.SetActive(false));
    }
    public void Enable_2_LinePoint()
    {
        if (CountLine == 3)
        {
            GasCuttingManager.instance.CheckJobPlace();

            /*    scribal.GetComponent<XRGrabInteractable>().selectEntered = null;
                Destroy(scribal);*/
            for (int i = 0; i < markingPoints.Count - 1; i++)
            {
                markingPoints[i].SetActive(false);
            }
        }
        else
        {
            if (CountLine == 1)
            {

                // totalLineMarkingPoints = markingLine2.transform.childCount;
                markingLine = markingLine2;
                currentMarking++;

                SnapScale1.GetComponent<BoxCollider>().enabled = true;
                outlines[1].enabled = false;
                TurnOnMarkingPoint();
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);
                lineMarkingPoint = 0;

            }
            else
            {
                markingPoints[currentMarking].SetActive(false);
                currentMarking++;
                if (currentMarking < markingPoints.Count)
                {
                    TurnOnMarkingPoint();
                }
                markingLine = markingLine3;
                //  totalLineMarkingPoints = markingLine3.transform.childCount;
                SnapScale2.GetComponent<BoxCollider>().enabled = true;
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);
                lineMarkingPoint = 0;
                TurnOnMarkingPoint();


            }
        }
    }
    //CENTER PUNCH
    public void StartCenterPunchMarking()
    {
        Debug.Log("StartCenterPunchMarking");
        CurrentMarkingPoint.gameObject.SetActive(true);
        CenterPunch.OnHammerHit += OnHammerHit;
    }
    public void PlayhummerCenterPunchsound()
    {
        hummerCenterPunchsound.PlayClip(hummerCenterPunchsound.Clips[0]);
    }
    private void OnHammerHit()
    {
        Debug.Log("OnHammerHit call");
        if (CurrentMarkingPoint.IsCenterPunchInside)
        {
            Debug.Log("OnHammerHit call  IsCenterPunchInside");
            CurrentMarkingPoint.gameObject.SetActive(false);
            currentCPMarkingPointIndex++;
            PlayhummerCenterPunchsound();
            if (currentCPMarkingPointIndex < centerPunchMarkingPoints.Count)
            {
                CurrentMarkingPoint.gameObject.SetActive(true);
                cpDotPoint[currentCPMarkingPointIndex - 1].SetActive(true);
            }

            else
            {
                Debug.Log("OnHammerHit call  checkStep5");
                CenterPunch.OnHammerHit -= OnHammerHit;
                cpDotPoint[currentCPMarkingPointIndex - 1].SetActive(true);
                /*  centerPunch.GetComponent<XRGrabInteractable>().selectEntered = null;
                  Destroy(centerPunch);*/
                GasCuttingManager.instance.checkStep5();
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "JobFlat")
        {
            Debug.Log("other 1" + other.gameObject.name);
            other.transform.tag = "Untagged";
            this.GetComponent<XRGrabInteractable>().enabled = false;
            GasCuttingManager.instance.CheckJobFlatPlace();
        }
    }

}