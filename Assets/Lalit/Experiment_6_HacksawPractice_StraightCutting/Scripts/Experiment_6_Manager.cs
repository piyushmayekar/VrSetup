using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Experiment_6_Manager : MonoBehaviour
{
    public static Experiment_6_Manager instance;

    [Header("Canvas ")]
    public GameObject finishPanel;

    [Header("Read step from json calss")]
    public ReadStepsFromJson readSteps;
    [Header("Steps audio clips")]
    public AudioSource stepAudioSource;
    public AudioClip[] stepsAudioClip;

    #region ToolsGameObject
    public GameObject BenchWiseGo;
    public GameObject FlatFileGo;
    public GameObject SteelRulerGo;
    public GameObject ScriberGo;
    public GameObject CenterPunchGo;
    public GameObject HammerGo;
    public GameObject HacksawGo;
    #endregion

    #region EmbeddedTool
    private Transform SteelRuler_AattachedToJob;
    #endregion

    #region WorkPieceObjects
    public Transform WorkPiece;
    public GameObject JobPlateOnStart;
    public GameObject JobPlateOnFirstCut;
    public GameObject JobPlateOnSecondCut;
    public GameObject FirstCut;
    public GameObject SecondCut;
    #endregion


    private FlatFile_VL FlatFile;
    private SteelRuler_VL SteelRuler;
    private ScriberMarking_VL Scriber;
    private DotPunching_VL DotPunch;
    private HackSaw_VL HackSaw;
    private BenchWise_VL BenchWise;

    [Header("PPE Collider")]
    public Collider[] ppekitcolliders;
    public int countppekit;

    [Header("Tools Highlight")]
    public Outline[] ToolsOutlines;

    public XRSocketInteractor JobSocketForFilling,JobSocketForMarking,JobSocketForCutting;
    public GameObject ScriberHL;


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

       // readSteps.panel.SetActive(true);
       // readSteps.AddClickConfirmbtnEvent(ConfirmStartBtn);
       // readSteps.confirmbtn.gameObject.SetActive(true);
    }

    private void Start()
    {
        FlatFile = FlatFileGo.GetComponent<FlatFile_VL>();
        SteelRuler = SteelRulerGo.GetComponent<SteelRuler_VL>();
        Scriber = ScriberGo.GetComponent<ScriberMarking_VL>();
        DotPunch = CenterPunchGo.GetComponent<DotPunching_VL>();
        HackSaw = HacksawGo.GetComponent<HackSaw_VL>();
        //BenchWise = BenchWiseGo.GetComponent<BechWise_VL>();
        SteelRuler_AattachedToJob = WorkPiece.Find("SteelRuler");
        EnableHoldJobOnBenchWiseForCutting();

        OnJobSnapForCutting();
    }

    public void ConfirmStartBtn()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnablePPEKitStep);
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
            Debug.Log("all kit catched");

            //readSteps.panel.SetActive(false);
            //finishPanel.SetActive(true);
            OnEnableHoldJobOnBenchWise();
        }
        selectGameObject.SetActive(false);
    }
    #endregion

    public void OnEnableHoldJobOnBenchWise()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableHoldJobOnBenchWise);
        
    }

    public void EnableHoldJobOnBenchWise()
    {
        readSteps.HideConifmBnt();
        JobPlateOnStart.GetComponent<Outline>().enabled = true;
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        JobSocketForFilling.gameObject.SetActive(true);
        
    }

    public void OnJobSnapForFilling()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableFillingOnJob);
    }

    public void EnableFillingOnJob()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[0].enabled = true;
        JobSocketForFilling.socketActive = false;
        
        FlatFile.SetFlatFileParams(3);
        FlatFile.AssignMethodOnFillingDone(OnEnableJobPlacemtnMarkingOnJob);
    }

    public void OnEnableJobPlacemtnMarkingOnJob()
    {
        ToolsOutlines[0].enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableJobPlacementMarkingOnJob);
    }

    public void EnableJobPlacementMarkingOnJob()
    {
        readSteps.HideConifmBnt();
        JobSocketForMarking.gameObject.SetActive(true);
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
    }

    public void OnJobSnapForMarking()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableMarkingOnJob);
        JobSocketForFilling.gameObject.SetActive(false);
    }

    public void EnableMarkingOnJob()
    {
        readSteps.HideConifmBnt();
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = false;
        ToolsOutlines[1].enabled = true;
        SteelRuler.AssignMethodOnSnapToJob(EnableFirstLineDrawingMech);
    }

    public void EnableFirstLineDrawingMech()
    {
        ToolsOutlines[2].enabled = true;
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AattachedToJob, 1,ScriberHL);
        Scriber.AssignMethodOnMarkingDone(EnableSecondLineDrawingMech);
    }

    public void EnableSecondLineDrawingMech()
    {
        //Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AattachedToJob, 2);
        Scriber.AssignMethodOnMarkingDone(OnEnablePunching);
    }

    public void OnEnablePunching()
    {
        ToolsOutlines[2].enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnablePunchingOnFirstLine);
    }

    public void EnablePunchingOnFirstLine()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[3].enabled = true;
        ToolsOutlines[4].enabled = true;
       // DotPunch.SetCenterPunchParams(WorkPiece, 1);
        DotPunch.AssignMethodOnPunchingDone(EnablePunchingOnSecondLine);
    }

    public void EnablePunchingOnSecondLine()
    {
        //DotPunch.SetCenterPunchParams(WorkPiece, 2);
        DotPunch.AssignMethodOnPunchingDone(OnEnableHoldJobONBenchWise);
    }

    public void OnEnableHoldJobONBenchWise()
    {
        ToolsOutlines[3].enabled = false;
        ToolsOutlines[4].enabled = false;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableHoldJobOnBenchWiseForCutting);

    }

    public void EnableHoldJobOnBenchWiseForCutting()
    {
        readSteps.HideConifmBnt();
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        JobSocketForCutting.gameObject.SetActive(true);
        
    }

    public void OnJobSnapForCutting()
    {
        ToolsOutlines[5].enabled = true; //HacksawHighlight
        JobPlateOnStart.SetActive(false);
        JobPlateOnFirstCut.SetActive(true);
        FirstCut.SetActive(true);
        WorkPiece.GetComponent<BoxCollider>().enabled = false;
        HackSaw.SetHackSawCuttingParams(WorkPiece, 1);
        HackSaw.AssignMethodOnCuttingDone(OnEnableSecondLineForCutting);
        //Add the rigidBody and othre component for cut piece to interact in HackSaw scripts 
        //or add them into oncomplete method     

        
    }

    public void OnEnableSecondLineForCutting()
    {

        Debug.Log("Enable second Line cutting Step");
    }






}
