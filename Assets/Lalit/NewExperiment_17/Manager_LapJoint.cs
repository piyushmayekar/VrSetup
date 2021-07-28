using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Manager_LapJoint : MonoBehaviour
{
    public static Manager_LapJoint instance;
    public GameObject finishPanel;
    public ReadStepsAndVideoManager readSteps;

    [Header("Steps audio clips")]
    public AudioSource stepAudioSource;
    public AudioClip[] stepsAudioClip;

    [Header("PPE Collider")]
    public Collider[] ppekitcolliders;
    public int countppekit;

    public GameObject SteelRulerHL,FlatFileHL, ScriberHL, CenterPunchHL, HammerHL, HackSawHL1, HackSawHL2,
        WireBrushHL, WeldingTorchHL, FillerRodHL, ChippingHammerHL;

    #region WorkPieceObjects
    public Transform WorkPiece1;
    public Transform WorkPiece2;

    public GameObject Job1PlateOnStart;
    public GameObject Job1PlateOnFirstCut;
    public GameObject Job2PlateOnStart;
    public GameObject Job2PlateOnFirstCut;

    public GameObject Job1Cut;
    public GameObject Job2Cut;

    public GameObject Job1MarkingLine, Job2MarkingLine;

    public Material WeldFinishMat;
    #endregion



    #region Tools GameObject
    public GameObject FlatFileGo;
    public GameObject SteelRulerGo;
    public GameObject ScriberGo;
    public GameObject CenterPunchGo;
    public GameObject HammerGo;
    public GameObject HackSawGo;
    public GameObject BenchWiseGo;
    public GameObject WireBrushGo;
    public GameObject ChippingHammerGo;
    public GameObject WeldingNuetralFlame;
    public GameObject[] FillerRod;

    #endregion

    [Header("Tools Highlight")]
    public Outline[] ToolsOutlines;

    #region EmbeddedTool
    private Transform SteelRuler1_AattachedToJob1;
    private Transform SteelRuler2_AattachedToJob1;
    private Transform SteelRuler2_AattachedToJob2;
    #endregion

    private FlatFile_VL FlatFile;
    private SteelRuler_VL SteelRuler;
    private ScriberMarking_VL Scriber;
    private DotPunching_VL DotPunch;
    private HackSaw_VL HackSaw;
    private BenchWise_VL BenchWise;
    private WireBrush_VL WireBrush;
    private ChippingHammer_VL ChippingHammer;
    private WeldingFlame_VL Weldingflame;

    public GameObject Job1Socket, Job2Scoket, Job1Socket2, Job2Scoket2;

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

    private void Start()
    {
        WireBrush = WireBrushGo.GetComponent<WireBrush_VL>();
        SteelRuler = SteelRulerGo.GetComponent<SteelRuler_VL>();
        SteelRuler1_AattachedToJob1 = WorkPiece1.transform.Find("SteelRuler1");
        SteelRuler2_AattachedToJob1 = WorkPiece1.transform.Find("SteelRuler2");

        SteelRuler2_AattachedToJob2 = WorkPiece2.transform.Find("SteelRuler");
        Scriber = ScriberGo.GetComponent<ScriberMarking_VL>();
        DotPunch = CenterPunchGo.GetComponent<DotPunching_VL>();
        HackSaw = HackSawGo.GetComponent<HackSaw_VL>();
        BenchWise = BenchWiseGo.GetComponent<BenchWise_VL>();
        Weldingflame = WeldingNuetralFlame.GetComponent<WeldingFlame_VL>();
        ChippingHammer = ChippingHammerGo.GetComponent<ChippingHammer_VL>();

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
            OnEnableCleaningStep();

        }
        selectGameObject.SetActive(false);
    }
    #endregion

    public void OnEnableCleaningStep()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableCleaningJobWithWireBrush);
    }
    public void EnableCleaningJobWithWireBrush()
    {

        readSteps.HideConifmBnt();
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = true;
        WireBrushHL.SetActive(true);
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(EnableCleaningOnSecondJob);

        ToolsOutlines[0].enabled = true;
    }

    public void EnableCleaningOnSecondJob()
    {
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = false;
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = true;
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(OnCleaningDone);
    }

    public void OnCleaningDone()
    {
        readSteps.HideConifmBnt();
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnEnableMarking);
    }

    public void OnEnableMarking()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[1].enabled = true;

        SteelRuler.AssignHighlight(SteelRulerHL);
        SteelRuler.readyForOperation = true;
        SteelRuler.AssignMethodOnSnapToJob(EnableMarkingOnFirstJobFirstLine);
    }

    public void EnableMarkingOnFirstJobFirstLine()
    {
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = true;

        ToolsOutlines[2].enabled = true;
        Scriber.SetScriberMarkingParams(WorkPiece1, SteelRuler1_AattachedToJob1, 0,ScriberHL);
        Scriber.AssignMethodOnMarkingDone(EnableMarkingOnFirstJobSecongLine);
    } 

    public void EnableMarkingOnFirstJobSecongLine()
    {
        Scriber.SetScriberMarkingParams(WorkPiece1, SteelRuler2_AattachedToJob1, 1);
        Scriber.AssignMethodOnMarkingDone(EnableMarkingOnSecondJobFirstLine);
    }

    public void EnableMarkingOnSecondJobFirstLine()
    {
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = false;
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = true;
        Scriber.SetScriberMarkingParams(WorkPiece2, SteelRuler2_AattachedToJob2, 0);
        Scriber.AssignMethodOnMarkingDone(OnMarkingDone);
    }

    public void OnMarkingDone()
    {
        //readSteps.onClickConfirmbtn();
        //readSteps.AddClickConfirmbtnEvent(EnablePunching);
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = false;
        EnablePunching();


    }

    public void EnablePunching()
    {
        //readSteps.HideConifmBnt();
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = true;
        ToolsOutlines[3].enabled = true;
        ToolsOutlines[4].enabled = true;

        DotPunch.SetCenterPunchParams(WorkPiece1, 1, CenterPunchHL, HammerHL);
        DotPunch.AssignMethodOnPunchingDone(EnablePunchingOnSecondLineFirstJob);


    }

    public void EnablePunchingOnSecondLineFirstJob()
    {
        DotPunch.SetCenterPunchParams(WorkPiece1, 2);
        DotPunch.AssignMethodOnPunchingDone(EnablePunchingOnSecondJob);
    }

    public void EnablePunchingOnSecondJob()
    {
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = false;
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = true;

        DotPunch.SetCenterPunchParams(WorkPiece2, 1);
        DotPunch.AssignMethodOnPunchingDone(DonePunching);
    }

    public void DonePunching()
    {
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = false;
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = true;
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = true;

        Job1Socket.SetActive(true);
        Job1Socket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(ActivateBenchWise);

    }

    public void ActivateBenchWise()
    {
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = false;

        Job1Socket.SetActive(false);
        BenchWise.jaw.disable = false;
        BenchWise.SetRotationDirection(true);
        BenchWise.AssignMethodOnJobFit(EnableCuttingOnJob1);
       
    }

    public void EnableCuttingOnJob1()
    {
        ToolsOutlines[6].enabled = true;
        HackSawHL1.SetActive(true);
        HackSaw.SetHackSawCuttingParams(WorkPiece1, 1);
        HackSaw.AssignMethodOnCuttingDone(EnableBenchWiseForUnMounting);
        ReadyJobForFirstCutting();
    }

    public void DropCutObj(GameObject cutobj)
    {
        cutobj.transform.parent = null;
        cutobj.AddComponent<Rigidbody>();
        

        cutobj.AddComponent<XRGrabInteractable>();
        cutobj.GetComponent<Outline>().enabled = true;

        if (cutobj.GetComponent<Rigidbody>())
        {
            cutobj.GetComponent<Rigidbody>().drag = 8;
        }
    }
    public void ReadyJobForFirstCutting()
    {
        WorkPiece1.GetComponent<BoxCollider>().enabled = false;
        Job1PlateOnStart.SetActive(false);
        Job1PlateOnFirstCut.SetActive(true);
        Job1Cut.SetActive(true);
        Job1MarkingLine.SetActive(false);
    }

    public void ReadySecondJobForCutting()
    {
        WorkPiece2.GetComponent<BoxCollider>().enabled = false;
        Job2PlateOnStart.SetActive(false);
        Job2PlateOnFirstCut.SetActive(true);
        Job2Cut.SetActive(true);
        Job2MarkingLine.SetActive(false);
    }
    public void EnableBenchWiseForUnMounting()
    {

        DropCutObj(Job1Cut);
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = true;
        WorkPiece1.GetComponent<BoxCollider>().enabled = true;
        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true;


        BenchWise.SetRotationDirection(false);
        BenchWise.jaw.disable = true;

        Job1Socket2.SetActive(true);
        Job1Socket2.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(ActivateBenchWiseForSecondCut);
    }

    public void ActivateBenchWiseForSecondCut()
    {
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = false;
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = true;

        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = false;
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = true;



       // BenchWise.handle.CanMove = false;
       // BenchWise.handle.SetHighDefaultMat();

        Job1Socket2.SetActive(false);
        Job2Scoket.SetActive(true);
        Job2Scoket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(EnableBenhWiseForCuttingOnSecondJob);
        

    }

    public void EnableBenhWiseForCuttingOnSecondJob()
    {
        Job2Scoket.SetActive(false);
        BenchWise.jaw.disable = false;
        BenchWise.SetRotationDirection(true);
        BenchWise.AssignMethodOnJobFit(EnableCuttingOnSecondJob);
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = false;


    }

    public void EnableCuttingOnSecondJob()
    {
        ReadySecondJobForCutting();
        ToolsOutlines[6].enabled = true;
        HackSaw.EmptyParams();
        HackSawHL2.SetActive(true);
        HackSaw.SetHackSawCuttingParams(WorkPiece2, 1);
        HackSaw.AssignMethodOnCuttingDone(OnCuttingDone);
    }

    public void OnCuttingDone()
    {
        DropCutObj(Job2Cut);
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = true;
        WorkPiece2.GetComponent<BoxCollider>().enabled = true;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = true;
        BenchWise.SetRotationDirection(false);
        BenchWise.jaw.disable = true;


        Job2Scoket2.SetActive(true);
        Job2Scoket2.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(PlaceJobAfterCuttingDone);
    }

    public void PlaceJobAfterCuttingDone()
    {
        Job2Scoket2.SetActive(false);
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = false;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = true;
        BenchWise.handle.CanMove = false;
        BenchWise.handle.SetHighDefaultMat();
        Debug.Log("Cutting Done");

    }
}
