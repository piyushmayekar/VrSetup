using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;


public class ManagerHackSawCutting : MonoBehaviour
{
    public static ManagerHackSawCutting instance;

    [Header("Canvas ")]
    public GameObject finishPanel;

    [Header("Read step from json calss")]
    public ReadStepsFromJson readSteps;
    [Header("Steps audio clips")]
    public AudioSource stepAudioSource;
    public AudioClip[] stepsAudioClip;

    [Header("PPE Collider")]
    public Collider[] ppekitcolliders;
    public int countppekit;

    public GameObject FlatFileHL,CSBottleHL,SteelRulerHL,BrushHL, ScriberHL, ScriberHL1, CenterPunchHL, CenterPunchHL1, HammerHL, HammerHL1, HackSawHL;

    #region WorkPieceObjects
    public Transform WorkPiece;
    public GameObject JobPlateOnStart;
    public GameObject JobPlateOnFirstCut;
    public GameObject JobPlateOnSecondCut;
    public GameObject FirstCut;
    public GameObject SecondCut;
    #endregion

    #region Tools GameObject
    public GameObject FlatFileGo;
    public GameObject CSBottleGo;
    public GameObject BrushGo;
    public GameObject SteelRulerGo;
    public GameObject ScriberGo;
    public GameObject CenterPunchGo;
    public GameObject HammerGo;
    public GameObject HackSawGo;
    public GameObject BenchWiseGo;
    #endregion

    [Header("Tools Highlight")]
    public Outline[] ToolsOutlines;

    #region EmbeddedTool
    private Transform SteelRuler_AattachedToJob;
    #endregion


    private FlatFile_VL FlatFile;
    private CopperSulphate_VL CSBottle;
    private SteelRuler_VL SteelRuler;
    private ScriberMarking_VL Scriber;
    private DotPunching_VL DotPunch;
    private HackSaw_VL HackSaw;
    private BenchWise_VL BenchWise;
    private MarkingBrush_VL MarkingBrush;

    #region SoketGameObject
    public GameObject FillingSocket, MarkingSocket, CuttingSocket , firstCutSocket, SecondCutSocket, endSocket;
    #endregion
    public GameObject markingLine1, markingLine2;

    public GameObject SteelRulePrefab;
    public GameObject markingPlane;
    public GameObject ClockWiseSprite, AntiClockWiseSprite;
    public GameObject MeasureHL1, MeasureHL2;
    public GameObject MeasureObj;
    public GameObject[] JobEdges;
    public void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;

        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = false;
            ppekitcolliders[i].GetComponent<Outline>().enabled = false;
        }

        for (int i = 0; i < ToolsOutlines.Length; i++)
        {
            ToolsOutlines[i].enabled = false;
        }

        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmStartBtn);
        readSteps.confirmbtn.gameObject.SetActive(true);
    }

    public void ConfirmStartBtn()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnablePPEKitStep);
    }
    private void Start()
    {
        FlatFile = FlatFileGo.GetComponent<FlatFile_VL>();
        CSBottle = CSBottleGo.GetComponent<CopperSulphate_VL>();
        MarkingBrush = BrushGo.GetComponent<MarkingBrush_VL>();
        SteelRuler = SteelRulerGo.GetComponent<SteelRuler_VL>();
        Scriber = ScriberGo.GetComponent<ScriberMarking_VL>();
        DotPunch = CenterPunchGo.GetComponent<DotPunching_VL>();
        HackSaw = HackSawGo.GetComponent<HackSaw_VL>();
        BenchWise = BenchWiseGo.GetComponent<BenchWise_VL>();
        SteelRuler_AattachedToJob = WorkPiece.Find("SteelRuler");
        
        //FillingSocket.SetActive(true);
        //FillingSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnJobFittedForCutting);

        //FlatFile.SetFlatFileParams(5);
        //FlatFile.AssignMethodOnFillingDone(OnFitUnlock);

        //SteelRuler.AssignHighlight(SteelRulerHL);
        //SteelRuler.AssignMethodOnSnapToJob(OnFitUnlock);

        //Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AattachedToJob, 1,ScriberHL);
        //Scriber.AssignMethodOnMarkingDone(EnableSecondLineMarking);

        //DotPunch.SetCenterPunchParams(WorkPiece, 1, CenterPunchHL, HammerHL);
        //DotPunch.AssignMethodOnPunchingDone(EnableSecondLinePunching);

        //ReadyJobForFirstCutting();
        //EnableFirstLineCutting();
        //CSBottle.AssignMethodOnPoringDone(OnPoringDone);

    }

    
    public void ReadyJobForFirstCutting()
    {
        WorkPiece.GetComponent<BoxCollider>().enabled = false;
        JobPlateOnStart.SetActive(false);
        JobPlateOnFirstCut.SetActive(true);
        FirstCut.SetActive(true);
        markingLine1.SetActive(false);
    }

    public void ReadyJobForSecondCutting()
    {
        JobPlateOnFirstCut.SetActive(false);
        JobPlateOnSecondCut.SetActive(true);
        SecondCut.SetActive(true);
        markingLine2.SetActive(false);


    }
    

    

    public void DropCutObj(GameObject cutobj)
    {
        cutobj.transform.parent = null;
        cutobj.AddComponent<Rigidbody>();
        cutobj.AddComponent<XRGrabInteractable>();
        cutobj.GetComponent<Outline>().enabled = true;
    }

    
   

   
    public void OnJobSnap()
    {
        BenchWise.jaw.disable = false;
        BenchWise.SetRotationDirection(true);
        BenchWise.AssignMethodOnJobFit(OnFitUnlock);
        FillingSocket.SetActive(false);
    }
    public void OnFitUnlock()
    {
        Debug.Log("Job Fitted");
        
    }

    public void OnJobFittedForCutting()
    {
        BenchWise.jaw.disable = false;
        BenchWise.SetRotationDirection(true);
        BenchWise.AssignMethodOnJobFit(OnFitUnlock);
        FillingSocket.SetActive(false);
    }

   

    #region PPE kit methods
    public void EnablePPEKitStep()
    {
        readSteps.HideConifmBnt();
        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = true;
            ppekitcolliders[i].GetComponent<Outline>().enabled = true;
        }
    }

    public void CheckPPEKitStep(GameObject selectGameObject)
    {
        countppekit++;
        if (countppekit >= ppekitcolliders.Length)
        {
            OnEnableHoldJobOnBenchWise();
            
            
        }
        selectGameObject.SetActive(false);
    }
    #endregion

    public void OnEnableHoldJobOnBenchWise()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableHoldBenchWiseStep);
       
    }

    public void EnableHoldBenchWiseStep()
    {
        readSteps.HideConifmBnt();
        FillingSocket.SetActive(true);
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        JobPlateOnStart.GetComponent<Outline>().enabled = true;

        FillingSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(EnableBenchwiseToHoldJob);
    }
    public void EnableBenchwiseToHoldJob()
    {
        FillingSocket.SetActive(false);
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = false;
        JobPlateOnStart.GetComponent<Outline>().enabled = false;
        ClockWiseSprite.SetActive(true);
        BenchWise.jaw.disable = false;
        BenchWise.SetRotationDirection(true);
        BenchWise.AssignMethodOnJobFit(OnJobFittedForFillling);

    }

    public void OnJobFittedForFillling()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableFilling);
        
    }

    public void EnableFilling()
    {
        readSteps.HideConifmBnt();
        FlatFileHL.SetActive(true);
        ToolsOutlines[0].enabled = true;
        
        FlatFile.SetFlatFileParams(10);
        FlatFile.AssignMethodOnFillingDone(EnableBenchWiseToUnmount);
    }

    public void EnableBenchWiseToUnmount()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnClickConfirmForMarkingPlacing);
        for (int i = 0; i < JobEdges.Length; i++)
        {
            JobEdges[i].SetActive(false);
        }
        //markingPlane.SetActive(false);
        JobPlateOnStart.GetComponent<Outline>().enabled = true;
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        ClockWiseSprite.SetActive(false);
        AntiClockWiseSprite.SetActive(true);
        BenchWise.SetRotationDirection(false);
        BenchWise.jaw.disable = true;
        MarkingSocket.SetActive(true);
        MarkingSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnJobSanpForMarking);
    }

    public void OnClickConfirmForMarkingPlacing()
    {
        readSteps.HideConifmBnt();
    }
    public void OnJobSanpForMarking()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableMarkingMediaStep);
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = false;
        MarkingSocket.SetActive(false);
        BenchWise.handle.CanMove = false;
        BenchWise.handle.SetHighDefaultMat();
        
    }

    

    public void EnableMarkingMediaStep()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[7].enabled = true;
        CSBottleHL.SetActive(true);
        //markingPlane.SetActive(true);
        markingPlane.tag = "Job";
        CSBottle.AssignMethodOnPoringDone(OnPoringDone);
    }
    public void OnPoringDone()
    {
        ToolsOutlines[7].enabled = false;
        ToolsOutlines[6].enabled = true;
        BrushHL.SetActive(true);
        MarkingBrush.SetBrushParams(10, "Job");
        MarkingBrush.AssignMethodOnBrushJobDone(EnableOperationBeforeMarking);
        markingPlane.GetComponent<BoxCollider>().isTrigger = true;
        markingPlane.tag = "MarkingPlate";

    }

    public void EnableOperationBeforeMarking()
    {
        ToolsOutlines[6].enabled = false;
        ToolsOutlines[1].enabled = true;
        MeasureHL1.SetActive(true);
        SteelRuler.isMeasuring = true;
        SteelRuler.AssignMethodOnSnapForMeasurement(EnableSecondSteelMeasuring);

    }

    public void EnableSecondSteelMeasuring()
    {
        ToolsOutlines[1].enabled = true;
        SteelRuler.GetComponent<CustomXRGrabInteractable>().enabled = true;
        SteelRuler.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        MeasureHL2.SetActive(true);
        SteelRuler.AssignMethodOnSnapForMeasurement(EnableScriberForMeasuring);
    }

    public void EnableScriberForMeasuring()
    {
        ToolsOutlines[2].enabled = true;
        Scriber.isMeasuring = true;
        Debug.Log("Update Workpiece");
        Scriber.SetScriberForMeasuremetn(WorkPiece, 2, 1);
        Scriber.AssignMethodOnMeasuringDone(EnableMarking);
    }


    public void DryMarkingPlane()
    {
        Material mat =  markingPlane.GetComponent<MeshRenderer>().sharedMaterial;
        

        mat.color = new Color(1, 0, 0);
        markingPlane.GetComponent<MeshRenderer>().sharedMaterial = mat;
    }
    public void EnableMarking()
    {
        Scriber.isMeasuring = false;
        SteelRuler.isMeasuring = false;
        SteelRuler.GetComponent<CustomXRGrabInteractable>().enabled = true;
        SteelRuler.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Scriber.EmptyParamsForMeasuring();

        DryMarkingPlane();
        
        ToolsOutlines[1].enabled = true;
        SteelRuler.readyForOperation = true;

        SteelRuler.AssignHighlight(SteelRulerHL);
        SteelRuler.AssignMethodOnSnapToJob(EnableMarkingOnFirstLine);    
    }

    public void EnableMarkingOnFirstLine()
    {
        ToolsOutlines[2].enabled = true;
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AattachedToJob, 1,ScriberHL);
        Scriber.AssignMethodOnMarkingDone(EnableSecondLineMarking);
    }

    public void EnableSecondLineMarking()
    {
        ScriberHL1.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AattachedToJob, 2);
        Scriber.AssignMethodOnMarkingDone(OnMarkingDone);
    }

    public void OnMarkingDone()
    {
        GameObject steelR = Instantiate(SteelRulePrefab);
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnablePunching);
        JobPlateOnStart.GetComponent<Outline>().enabled = false;
    }

    public void EnablePunching()
    {
        readSteps.HideConifmBnt();
       
        ToolsOutlines[3].enabled = true;
        ToolsOutlines[4].enabled = true;
        DotPunch.SetCenterPunchParams(WorkPiece, 1, CenterPunchHL, HammerHL);
        DotPunch.AssignMethodOnPunchingDone(EnableSecondLinePunching);

    }

    public void EnableSecondLinePunching()
    {
        CenterPunchHL1.SetActive(true);
        HammerHL1.SetActive(true);
        DotPunch.SetCenterPunchParams(WorkPiece, 2);
        DotPunch.AssignMethodOnPunchingDone(OnPunchingdone);
    }

    public void OnPunchingdone()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableCuttingSocket);
        markingPlane.SetActive(false);
        MeasureObj.SetActive(false);
    }

    public void EnableCuttingSocket()
    {
        readSteps.HideConifmBnt();
        FillingSocket.SetActive(true);
        FillingSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(EnableBenchWiseForHoldJob);
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        JobPlateOnStart.GetComponent<Outline>().enabled = true;
    }

    public void EnableBenchWiseForHoldJob()
    {
        ClockWiseSprite.SetActive(true);
        AntiClockWiseSprite.SetActive(false);
        BenchWise.jaw.disable = false;
        BenchWise.SetRotationDirection(true);
        BenchWise.AssignMethodOnJobFit(OnEnableCutting);
        FillingSocket.SetActive(false);
        

    }

    public void OnEnableCutting()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableFirstLineCutting);
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = false;
    }
    public void EnableFirstLineCutting()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[5].enabled = true;
        HackSawHL.SetActive(true);
        HackSaw.SetHackSawCuttingParams(WorkPiece, 1);
        HackSaw.AssignMethodOnCuttingDone(OnFirstCutDone);
        ReadyJobForFirstCutting();
    }


    public void OnFirstCutDone()
    {
        DropCutObj(FirstCut);
        firstCutSocket.SetActive(true);
        firstCutSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(GetReadyForSecondCutting);
        
    }

    public void GetReadyForSecondCutting()
    {
        firstCutSocket.SetActive(false);
        FirstCut.GetComponent<Outline>().enabled = false;
        FirstCut.GetComponent<XRGrabInteractable>().enabled = false;
       
        
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableSecondLineCutting);
    }
    public void EnableSecondLineCutting()
    {

        readSteps.HideConifmBnt();
        ToolsOutlines[5].enabled = true;
        HackSaw.EmptyParams();
        HackSaw.SetHackSawCuttingParams(WorkPiece, 2);
        HackSaw.AssignMethodOnCuttingDone(OnSecondLineCutDone);
        ReadyJobForSecondCutting();
        markingLine1.SetActive(false);
    }

    public void OnSecondLineCutDone()
    {
        DropCutObj(SecondCut);
        SecondCutSocket.SetActive(true);
        SecondCutSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnSecondCutSnap);
        WorkPiece.GetComponent<BoxCollider>().enabled = true;
    }

    public void OnSecondCutSnap()
    {
        AntiClockWiseSprite.SetActive(true);
        ClockWiseSprite.SetActive(false);
        BenchWise.SetRotationDirection(false);
        BenchWise.jaw.disable = true;
        SecondCut.GetComponent<Outline>().enabled = false;
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        JobPlateOnSecondCut.GetComponent<Outline>().enabled = true;
        SecondCutSocket.SetActive(false);
        endSocket.SetActive(true);
        endSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(EndExperiment);

    }

    public void EndExperiment()
    {
        JobPlateOnSecondCut.GetComponent<Outline>().enabled = false;
        endSocket.SetActive(false);
        finishPanel.SetActive(true);
        readSteps.tablet.SetActive(true);
        readSteps.panel.SetActive(false);
        BenchWise.handle.CanMove = false;
        BenchWise.handle.SetHighDefaultMat();
    }

    //Bottle and sound
}



