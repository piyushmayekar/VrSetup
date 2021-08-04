using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;

[System.Serializable]
public class ToolOrigin
{
    public Vector3 pos;
    public Vector3 rot;
}
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

    public GameObject SteelRulerHL, ScriberHL, ScriberHL1, ScriberHL2, CenterPunchHL, CenterPunchHL1, CenterPunchHL2, HammerHL, HammerHL1, HammerHL2, HackSawHL1, HackSawHL2,
        WireBrushHL, WireBrushHL1, WeldingTorchHL, FillerRodHL, ChippingHammerHL,socketFlat1_job,socketFlat2_job, lastScale,ChippingHammerHL1;

    #region WorkPieceObjects
    public Transform WorkPiece1;
    public Transform WorkPiece2;
    public Transform FinalWorkPiece;

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
    public GameObject TrySquareGo;
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
    private TrySquare_VL TrySquare;

    public GameObject Job1Socket, Job2Scoket, Job1Socket2, Job2Scoket2;
    public GameObject ClockWiseSprite, AntiClockWiseSprite;

    [Header("Table uper gas kit")]
    public Collider[] GasTableObjectcolliders;

    [Header("outLine Object to HighLight")]
    public Outline[] ObjectOutlines;

    public GameObject lighter_Flame, blackSmoke, oxidizing_F, reduce_or_carb_F, neturel_F;
    public GameObject Step8flame, Step9flame, extraRedBol, oldRedBol;

    public bool isTurnOffFlame;

    public RotateNozzle[] rotateNozzles;
    public GameObject ZeroMeterred, ZeroMeterBlack;

    public GameObject RodHL1, RodHL2, RodHL3, TorchHL1, TorchHL2, TorchHL3, WeldingTorch;
    public GameObject[] SlagLines;
    public GameObject RightAngleSocket, FinalWeldingSocket,TrySquareHL, FinalWeldingSocket1;
    public Transform FlipTranform;

    public Transform BrushOT, SteelRulerOT, ScriberOT, DotPunchOT, HammerOT, HackSawOT, ChippingHammerOT;
    public Vector3 StartPos;
    public Vector3 StartRot;

    public Dictionary<string, ToolOrigin> ToolsOrigin = new Dictionary<string, ToolOrigin>();
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

        
    }



    public ToolOrigin SetToolsOrigin(Transform T)
    {
        ToolOrigin To = new ToolOrigin();
        To.pos = T.position;
        To.rot = T.localEulerAngles;

        return To;
    }

    public void AssignToolOriginToDictionay()
    {
        ToolOrigin WTO = SetToolsOrigin(WireBrushGo.transform);
        ToolsOrigin.Add("WireBrush", WTO);

        ToolOrigin SRTO = SetToolsOrigin(SteelRulerGo.transform);
        ToolsOrigin.Add("SteelRuler", SRTO);

        ToolOrigin STO = SetToolsOrigin(ScriberGo.transform);
        ToolsOrigin.Add("Scriber", STO);

        ToolOrigin DPTO = SetToolsOrigin(CenterPunchGo.transform);
        ToolsOrigin.Add("CenterPunch", DPTO);

        ToolOrigin HTO = SetToolsOrigin(HammerGo.transform);
        ToolsOrigin.Add("Hammer", HTO);

        ToolOrigin HSTO = SetToolsOrigin(HackSawGo.transform);
        ToolsOrigin.Add("HackSaw", HSTO);

        ToolOrigin TSTO = SetToolsOrigin(TrySquareGo.transform);
        ToolsOrigin.Add("TrySquare", TSTO);

        ToolOrigin CHTO = SetToolsOrigin(ChippingHammer.transform);
        ToolsOrigin.Add("ChippingHammer", CHTO);

    }

    private void Start()
    {
        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmStartBtn);
        readSteps.confirmbtn.gameObject.SetActive(true);

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
        TrySquare = TrySquareGo.GetComponent<TrySquare_VL>();
        //EnableMarkingOnFirstJobSecongLine();
        ///  PlaceJobFlatePos1_socket1_job();
        //  PlaceJobFlatePos2_socket2_job();

        //OnCompleteWelding();

        AssignToolOriginToDictionay();

        //StartPos = WireBrushGo.transform.position;
        //StartRot = WireBrushGo.transform.localEulerAngles;

        
    }

    public void ConfirmStartBtn()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnablePPEKitStep);
        //readSteps.AddClickConfirmbtnEvent(GasWeldingSetUP.instance.Onclickbtn_s_3_confirm);
        //readSteps.AddClickConfirmbtnEvent(EnabletagWelding);

        
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
        //Debug.Log("call grab"+ selectGameObject.name);
        selectGameObject.SetActive(false);
    }
    #endregion

    #region JobReadyMethod
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
        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = false;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = true;
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(OnCleaningDone);
    }

    public void OnCleaningDone()
    {
        readSteps.HideConifmBnt();
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnEnableMarking);
        
    }

    public void OnEnableMarking()
    {
       
        readSteps.HideConifmBnt();
        ToolsOutlines[1].enabled = true;

        SteelRuler.AssignHighlight(SteelRulerHL);
        SteelRuler.readyForOperation = true;
        SteelRuler.AssignMethodOnSnapToJob(EnableMarkingOnFirstJobSecongLine);
        ResetToolPosition(WireBrushGo.transform,ToolsOrigin["WireBrush"]);
    }

    public void EnableMarkingOnFirstJobFirstLine()
    {
        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true;
        //Debug.Log("2");
        ToolsOutlines[2].enabled = true;
        Scriber.SetScriberMarkingParams(WorkPiece1, SteelRuler1_AattachedToJob1, 0, ScriberHL);
        Scriber.AssignMethodOnMarkingDone(EnableMarkingOnFirstJobSecongLine);
    }

    public void EnableMarkingOnFirstJobSecongLine()
    {
        ToolsOutlines[2].enabled = true;
        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true;
        ScriberHL1.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece1, SteelRuler2_AattachedToJob1, 1);
        Scriber.AssignMethodOnMarkingDone(OnMarkingDone);
    }

    public void EnableMarkingOnSecondJobFirstLine()
    {
        Debug.Log("3");
        ScriberHL2.SetActive(true);
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = false;
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = true;
        Scriber.SetScriberMarkingParams(WorkPiece2, SteelRuler2_AattachedToJob2, 0);
        Scriber.AssignMethodOnMarkingDone(OnMarkingDone);
    }

    public void OnMarkingDone()
    {
        //readSteps.onClickConfirmbtn();
        //readSteps.AddClickConfirmbtnEvent(EnablePunching);
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = false;
        //EnablePunching();
        EnablePunchingOnSecondLineFirstJob();
        lastScale.SetActive(true);
        SteelRulerGo.SetActive(false);
        Debug.Log("call last scale");
    }

    public void ResetScriberAndScalePos()
    {
        ResetToolPosition(ScriberGo.transform, ToolsOrigin["Scriber"]);
        //ResetToolPosition(SteelRulerGo.transform, ToolsOrigin["SteelRuler"]);
    }
    public void EnablePunching()
    {
        //readSteps.HideConifmBnt();
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = true;
        ToolsOutlines[3].enabled = true;
        ToolsOutlines[4].enabled = true;

        DotPunch.SetCenterPunchParams(WorkPiece1, 1, CenterPunchHL, HammerHL);
        DotPunch.AssignMethodOnPunchingDone(EnablePunchingOnSecondLineFirstJob);

        //ResetToolPosition(ScriberGo.transform, ToolsOrigin["Scriber"]);
        //ResetToolPosition(SteelRulerGo.transform, ToolsOrigin["SteelRuler"]);


    }

    public void EnablePunchingOnSecondLineFirstJob()
    {
        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true;
        ToolsOutlines[3].enabled = true;
        ToolsOutlines[4].enabled = true;
        CenterPunchHL1.SetActive(true);
        HammerHL1.SetActive(true);
        DotPunch.SetCenterPunchParams(WorkPiece1, 2);
        DotPunch.AssignMethodOnPunchingDone(OnJobReadyStepComplete);
    }

    public void EnablePunchingOnSecondJob()
    {
        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = false;
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = true;

        CenterPunchHL2.SetActive(true);
        HammerHL2.SetActive(true);
        DotPunch.SetCenterPunchParams(WorkPiece2, 1);
        DotPunch.AssignMethodOnPunchingDone(DonePunching);
    }

    public void DonePunching()
    {
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = false;
        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true;
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
        ToolsOutlines[5].enabled = true;
        HackSawHL1.SetActive(true);
        HackSaw.SetHackSawCuttingParams(WorkPiece1, 1);
        HackSaw.AssignMethodOnCuttingDone(EnableBenchWiseForUnMounting);
        ReadyJobForFirstCutting();

        ResetToolPosition(CenterPunchGo.transform, ToolsOrigin["CenterPunch"]);
        ResetToolPosition(HammerGo.transform, ToolsOrigin["Hammer"]);


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
        //ToolsOutlines[5].enabled = false;

        DropCutObj(Job1Cut);
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = true;
        WorkPiece1.GetComponent<BoxCollider>().enabled = true;
        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true;

        ClockWiseSprite.SetActive(false);
        AntiClockWiseSprite.SetActive(true);
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

        ResetToolPosition(HackSawGo.transform, ToolsOrigin["HackSaw"]);



        // BenchWise.handle.CanMove = false;
        // BenchWise.handle.SetHighDefaultMat();

        Job1Socket2.SetActive(false);
        Job2Scoket.SetActive(true);
        Job2Scoket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(EnableBenhWiseForCuttingOnSecondJob);


    }

    public void EnableBenhWiseForCuttingOnSecondJob()
    {
        Job2Scoket.SetActive(false);
        ClockWiseSprite.SetActive(true);
        AntiClockWiseSprite.SetActive(false);
        BenchWise.jaw.disable = false;
        BenchWise.SetRotationDirection(true);
        BenchWise.AssignMethodOnJobFit(EnableCuttingOnSecondJob);
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = false;


    }

    public void EnableCuttingOnSecondJob()
    {
        ReadySecondJobForCutting();
        ToolsOutlines[5].enabled = true;
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
        ClockWiseSprite.SetActive(false);
        AntiClockWiseSprite.SetActive(true);
        BenchWise.SetRotationDirection(false);
        BenchWise.jaw.disable = true;


        Job2Scoket2.SetActive(true);
        Job2Scoket2.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnJobReadyStepComplete);
    }


    public void OnJobReadyStepComplete()
    {
        Job2Scoket2.SetActive(false);
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = false;

        BenchWise.handle.CanMove = false;
        BenchWise.handle.SetHighDefaultMat();
        //ResetToolPosition(HackSawGo.transform, ToolsOrigin["HackSaw"]);

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnJobPlacementForWelding);
    }


    public void OnJobPlacementForWelding()
    {

        readSteps.HideConifmBnt();

        ResetToolPosition(CenterPunchGo.transform, ToolsOrigin["CenterPunch"]);
        ResetToolPosition(HammerGo.transform, ToolsOrigin["Hammer"]);

        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = false;
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = true;

        socketFlat1_job.SetActive(true);
        socketFlat1_job.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnSecondJobPlacementForWelding);


    }
    public void OnSecondJobPlacementForWelding()
    {


        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = false;
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = false;
        WorkPiece1.GetComponent<BoxCollider>().enabled = false;
        //WorkPiece1.transform.localPosition = socketFlat1_job.transform.localPosition;
        //WorkPiece1.transform.localRotation = socketFlat1_job.transform.localRotation;

        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = true;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = true;
        socketFlat1_job.SetActive(false);
        socketFlat2_job.SetActive(true);
        socketFlat2_job.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnCompleteJobPlacementForWelding);

    }
    public void OnCompleteJobPlacementForWelding()
    {
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = false;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = false;
        //WorkPiece2.transform.localPosition = socketFlat2_job.transform.localPosition;
        //WorkPiece2.transform.localRotation = socketFlat2_job.transform.localRotation;
        socketFlat2_job.SetActive(false);
        readSteps.onClickConfirmbtn();
        //readSteps.AddClickConfirmbtnEvent(Onclickbtn_s7_confirm);
        readSteps.AddClickConfirmbtnEvent(GasWeldingSetUP.instance.Onclickbtn_s_3_confirm);

    }
    #endregion


    public void callGasSetupEnd()
    {
        // readSteps.onClickConfirmbtn();
        //readSteps.AddClickConfirmbtnEvent(FlameControlStep);
        FlameControlStep();
    }

    public void FlameControlStep()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[0].enabled = true; //lighter
        GasTableObjectcolliders[0].GetComponent<SnapGrabbleObject_VL>().enabled = true;
        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = true;

        ObjectOutlines[0].enabled = true; // torch nozel part 
    }

    public void LighterSnap_true()
    {
        GasTableObjectcolliders[0].GetComponent<SnapGrabbleObject_VL>().enabled = false;
        ObjectOutlines[0].enabled = false; // torch nozel part 
        lighter_Flame.SetActive(false);
        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = false;

        GasTableObjectcolliders[1].enabled = true; //red bol at gas torch
        GasTableObjectcolliders[1].GetComponent<Outline>().enabled = true;
        StartCoroutine(lighterEnable());
    }

    IEnumerator lighterEnable()
    {
        //GasTableObjectcolliders[0].enabled = false;  //lighter
        lighter_Flame.SetActive(false);
        GasTableObjectcolliders[0].GetComponent<SnapGrabbleObject_VL>().enabled = false;
        //GasTableKitColliders[12].gameObject.SetActive(false);
        PlayFlamsParticle1();   //new 22
        yield return new WaitForSeconds(1.5f);
        PlayFlamsParticle2();  //new 22
    }

    void PlayFlamsParticle1()
    {
        blackSmoke.SetActive(true); // black smoke
        GasTableObjectcolliders[1].GetComponent<Outline>().enabled = false;
    }

    void PlayFlamsParticle2()
    {
        blackSmoke.SetActive(false);
        reduce_or_carb_F.SetActive(true); // simple reduce
        reduce_or_carb_F.GetComponent<AudioSource>().Play();
        GasTableObjectcolliders[2].GetComponent<Outline>().enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableFlameControls);
        //PlayStepAudio(12);// open flam audio

        //Need a ui step for setting up the flame

    }

    public void EnableFlameControls()
    {
        //readSteps.onClickConfirmbtn();

        Debug.Log("Enable flame control");

        reduce_or_carb_F.SetActive(true);
        Step8flame.SetActive(false);
        Step9flame.SetActive(true);
        reduce_or_carb_F.GetComponent<AudioSource>().Play();

        GasTableObjectcolliders[3].enabled = true; // red bol reduce carbon //15
        rotateNozzles[0].enabled = true; //red bol  reduce carbon

        ObjectOutlines[1].GetComponent<BoxCollider>().enabled = true; //red bol  reduce carbon
        ObjectOutlines[2].GetComponent<BoxCollider>().enabled = false; //green bol oxidizing
        ObjectOutlines[3].GetComponent<BoxCollider>().enabled = false; //red bol netural

        rotateNozzles[0].GetComponent<Outline>().enabled = true; //red bol reduce carbon
        readSteps.HideConifmBnt();
    }

    public void PlayFlamsParticle1_Flame() //first step flame green
    {
        if (!isTurnOffFlame)
        {
            // readSteps.onClickConfirmbtn();
            reduce_or_carb_F.SetActive(true);
            reduce_or_carb_F.GetComponent<AudioSource>().Play();
            GasTableObjectcolliders[4].enabled = true; //green bol oxidizing //16
            rotateNozzles[1].enabled = true; //green bol oxidizing 
            GasTableObjectcolliders[3].enabled = false; // red bol reduce carbon

            rotateNozzles[0].GetComponent<Outline>().enabled = false; //red bol reduce carbon

            ObjectOutlines[1].GetComponent<BoxCollider>().enabled = false; //red bol  reduce carbon
            ObjectOutlines[2].GetComponent<BoxCollider>().enabled = true; //green bol oxidizing
            ObjectOutlines[3].GetComponent<BoxCollider>().enabled = false; //red bol netural

            rotateNozzles[1].GetComponent<Outline>().enabled = true; //green bol oxidizing
            Debug.Log("flame 1");
        }
    }

    public void PlayFlameparticle2_Flam() //second step flame red
    {
        if (!isTurnOffFlame)
        {
            ObjectOutlines[1].GetComponent<BoxCollider>().enabled = false; //red bol  reduce carbon
            ObjectOutlines[2].GetComponent<BoxCollider>().enabled = false; //green bol oxidizing
            ObjectOutlines[3].GetComponent<BoxCollider>().enabled = true; //red bol netural

            //readSteps.onClickConfirmbtn();
            oxidizing_F.SetActive(true);
            oxidizing_F.GetComponent<AudioSource>().Play();
            reduce_or_carb_F.SetActive(false);
            GasTableObjectcolliders[4].enabled = false; //green bol oxidizing

            extraRedBol.SetActive(true);
            oldRedBol.SetActive(false);

            rotateNozzles[1].GetComponent<Outline>().enabled = false; //green bol oxidizing
            GasTableObjectcolliders[5].enabled = true; //red bol neutral red sphere sphere //17
            rotateNozzles[2].enabled = true; // neutral flame
            rotateNozzles[2].GetComponent<Outline>().enabled = true; // red bol neutral outline
            Debug.Log("flame 2");
        }
    }

    public void PlayFlamsParticle3_flame() // neutral flame
    {
        if (!isTurnOffFlame)
        {
            GasTableObjectcolliders[5].enabled = false;
            neturel_F.SetActive(true);
            neturel_F.GetComponent<AudioSource>().Play();
            oxidizing_F.SetActive(false);
            rotateNozzles[2].GetComponent<Outline>().enabled = false; // red bol neutral outline
            Debug.Log("end");

            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(EnabletagWelding);
            //readSteps.AddClickConfirmbtnEvent(OnTurningOffFlameStep1);
        }
    }



    #region GasSetUp turning off Procedure
    public void OnTurningOffFlameStep1()
    {
        Debug.Log("Starting turning off");
        readSteps.HideConifmBnt();
        neturel_F.SetActive(false);
        reduce_or_carb_F.SetActive(true); // redus true  1
        reduce_or_carb_F.GetComponent<AudioSource>().Play();
        isTurnOffFlame = true;
        rotateNozzles[0].enabled = true;
        rotateNozzles[0].RotateValue = 40;
        rotateNozzles[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
        rotateNozzles[0].OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0);
        rotateNozzles[0].isclockwise = false;

        GasTableObjectcolliders[3].transform.gameObject.SetActive(true);
        GasTableObjectcolliders[5].transform.parent.gameObject.SetActive(true);
        GasTableObjectcolliders[3].enabled = true;

        ObjectOutlines[1].enabled = true;
        ObjectOutlines[2].enabled = false;
    }

    public void TurnOffFlame_Step_2()
    {
        if (isTurnOffFlame)
        {
            reduce_or_carb_F.SetActive(false);
            oxidizing_F.SetActive(true);

            GasTableObjectcolliders[3].enabled = false;
            oxidizing_F.GetComponent<AudioSource>().Play();

            GasTableObjectcolliders[4].enabled = true; //Green bol oxidizing
            GasTableObjectcolliders[4].transform.gameObject.SetActive(true); //green bol oxidizing
            rotateNozzles[2].transform.localRotation = Quaternion.Euler(0, 0, 0);// Green bol oxidizing
            rotateNozzles[2].OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
            rotateNozzles[2].RotateValue = 40; //GREEN  bol oxidizing
            rotateNozzles[2].enabled = true; //GREEN  bol oxidizing
            rotateNozzles[2].isclockwise = false; //GREEN  bol oxidizing

            GasTableObjectcolliders[3].enabled = false;

            ObjectOutlines[1].enabled = false;
            ObjectOutlines[2].enabled = true;
        }
    }

    public void CallFlameOff()
    {
        if (isTurnOffFlame)
        {
            oxidizing_F.SetActive(false);
            neturel_F.SetActive(false);
            reduce_or_carb_F.SetActive(false);
            GasTableObjectcolliders[4].enabled = false;
            ObjectOutlines[1].enabled = false;
            ObjectOutlines[2].enabled = false;
            rotateNozzles[2].enabled = false; //Green bol oxidizing
            Debug.Log("Flame off");

            OnEnableTurnOfRegulator();
        }
    }

    //turnig of regulator Steps

    public void OnEnableTurnOfRegulator()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(CloseCylinderValves);
    }

    public void CloseCylinderValves()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[6].enabled = true; //blue valve nozzole(1)
        ObjectOutlines[6].enabled = true; //blue valve nozzole(1)

        ObjectOutlines[8].enabled = true; //Black regulator

        rotateNozzles[3].isclockwise = false;  //blue valve nozzole(1)
        rotateNozzles[3].enabled = true; //blue valve nozzole(1)
        rotateNozzles[3].otherMeterobject.SetActive(false); //blue valve nozzole(1)
        rotateNozzles[3].otherMeterobject = ZeroMeterBlack; //blue valve nozzole(1)
        rotateNozzles[3].MeterObject.SetActive(true); //blue valve nozzole(1)
        rotateNozzles[3].RotateValue = 10; //blue valve nozzole(1)
    }

    public void callBlackValve()
    {
        if (isTurnOffFlame)
        {
            GasTableObjectcolliders[6].enabled = false; //blue valve nozzole

            ObjectOutlines[8].enabled = false; //black regulator
            ObjectOutlines[9].enabled = true;  //red regulator 
            ObjectOutlines[6].enabled = false; //blue valve nozzole
            GasTableObjectcolliders[7].enabled = true; //red valve nozzole

            ObjectOutlines[7].enabled = true; //red valve nozzole

            rotateNozzles[4].isclockwise = false; // red valve nozzle
            rotateNozzles[4].enabled = true;
            rotateNozzles[4].otherMeterobject.SetActive(false);
            rotateNozzles[4].otherMeterobject = ZeroMeterred;
            rotateNozzles[4].MeterObject.SetActive(true);
            rotateNozzles[4].RotateValue = 10;

        }
    }

    public void CallRedValve()
    {
        if (isTurnOffFlame)
        {
            ObjectOutlines[9].enabled = false;  //red regulator 
            rotateNozzles[4].enabled = false; //red nozzole
            GasTableObjectcolliders[7].enabled = false; //red valve nozzole
            Debug.Log("Closed valve");
            ///finishPanel.SetActive(true);
            //readSteps.panel.SetActive(false);
            //OnTurnOffDone(); start cleaning method
            OnClenaingProcessStart();
        }
    }

    #endregion
    public void SetFillerRod(GameObject fillerRod)
    {
        if (Weldingflame)
        {
            Weldingflame.SetFillerRod(fillerRod);
        }
    }

    #region Welding Methods
    public void EnabletagWelding()
    {
        readSteps.HideConifmBnt();
        WorkPiece1.gameObject.SetActive(false);
        WorkPiece2.gameObject.SetActive(false);
        FinalWorkPiece.gameObject.SetActive(true);
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = false;

       // WeldingTorch.GetComponent<BoxCollider>().enabled = true;
        //WeldingTorch.GetComponent<CustomXRGrabInteractable>().enabled = true;

        WeldingNuetralFlame.SetActive(true);
        WeldingNuetralFlame.GetComponent<CapsuleCollider>().enabled = true;

        RodHL1.SetActive(true);
        TorchHL1.SetActive(true);
        
        for (int i = 0; i < FillerRod.Length; i++)
        {
            FillerRod[i].GetComponent<Outline>().enabled = true;
            FillerRod[i].GetComponent<XRGrabInteractable>().enabled = true;
        }

        Weldingflame.SetWeldingFlameParams(FinalWorkPiece, 0);
        Weldingflame.AssignMethodOnFusionRunDone(OnTriSquareStep);


    }

    public void OnTriSquareStep()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(RemoveTagWeldSlag);

        //Weldingflame.DisableEffect();
        //WeldingNuetralFlame.SetActive(false);
    }

    public void RemoveTagWeldSlag()
    {

        readSteps.HideConifmBnt();
        WeldingNuetralFlame.SetActive(false);

        int count = 0;
        for (int i = 0; i < FillerRod.Length; i++)
        {
            FillerRod[i].GetComponent<Outline>().enabled = false;

        }

       
            for (int j = 0; j < SlagLines[0].transform.childCount; j++)
            {
                count++;
                SlagLines[0].transform.GetChild(j).gameObject.SetActive(true);
                SlagLines[0].transform.GetChild(j).GetComponent<CapsuleCollider>().enabled = true;
                SlagLines[0].transform.GetChild(j).tag = "Slag";
            }
        

        ToolsOutlines[7].enabled = true;
        ChippingHammerHL.SetActive(true);
        ChippingHammer.SetChippingHammerParams(count);
        ChippingHammer.AssignMethodOnSlagRemoveDone(CheckAngleWithTrySqure);
        //WeldingNuetralFlame.SetActive(false);
    }

    public void CheckAngleWithTrySqure()
    {
        FinalWorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = true;
        //Enable the oulline

        RightAngleSocket.SetActive(true);
        if (RightAngleSocket.GetComponent<CustomSocket>() != null)
        {
            RightAngleSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnJobSnapForCheckRightAngle);
        }
    }

    public void OnJobSnapForCheckRightAngle()
    {
        RightAngleSocket.SetActive(false);
        FinalWorkPiece.GetComponent<XRGrabInteractable>().enabled = false;
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = false;


        ToolsOutlines[8].enabled = true;
        TrySquareHL.SetActive(true);
        TrySquare.AssignMethodOnCheckRightAngle(OnWeldingFusionRunForLapJoint);
    }

    public void OnWeldingFusionRunForLapJoint()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(PlaceJobForFinalWelding);
    }

    public void PlaceJobForFinalWelding()
    {
        readSteps.HideConifmBnt();
        FinalWorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = true;

        FinalWeldingSocket.SetActive(true);
        FinalWeldingSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(JobSnapForWeldingFirstRun);
    }

    public void JobSnapForWeldingFirstRun()
    {
        WeldingNuetralFlame.SetActive(true);
        WeldingNuetralFlame.GetComponent<AudioSource>().Play();


        FinalWeldingSocket.SetActive(false);
        FinalWorkPiece.GetComponent<XRGrabInteractable>().enabled = false;
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = false;
        RodHL2.SetActive(true);
        TorchHL2.SetActive(true);
        Weldingflame.SetWeldingFlameParams(FinalWorkPiece, 1);
        Weldingflame.AssignMethodOnFusionRunDone(OnFirstSideWeldingComplete);
    }

    public void OnFirstSideWeldingComplete()
    {
        FinalWeldingSocket1.SetActive(true);
        FinalWeldingSocket1.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(EnableWeldingOnSecondLine);
        FinalWorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = true;
    }

    public void EnableWeldingOnSecondLine()
    {
        FinalWeldingSocket1.SetActive(false);

        FinalWorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = true;
        RodHL3.SetActive(true);
        TorchHL3.SetActive(true);
       // FinalWorkPiece.position = FlipTranform.position;
        //FinalWorkPiece.eulerAngles = FlipTranform.eulerAngles;
        Weldingflame.SetWeldingFlameParams(FinalWorkPiece, 2);
        Weldingflame.AssignMethodOnFusionRunDone(OnCompleteWelding);
    }

    public void OnCompleteWelding()
    {
        FinalWorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        FinalWorkPiece.GetComponent<Rigidbody>().useGravity = true;
        FinalWorkPiece.GetComponent<Rigidbody>().isKinematic = false;
        int count = 0;
        for (int i = 0; i < FillerRod.Length; i++)
        {
            FillerRod[i].GetComponent<Outline>().enabled = false;

        }

        for (int i = 0; i < SlagLines[0].transform.childCount; i++)
        {
            SlagLines[0].transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int j = 0; j < SlagLines[1].transform.childCount; j++)
        {
            count++;
            SlagLines[1].transform.GetChild(j).gameObject.SetActive(true);
            SlagLines[1].transform.GetChild(j).GetComponent<CapsuleCollider>().enabled = true;
            SlagLines[1].transform.GetChild(j).tag = "Slag";
        }

        for (int k = 0; k < SlagLines[2].transform.childCount; k++)
        {
            count++;
            SlagLines[2].transform.GetChild(k).gameObject.SetActive(true);
            SlagLines[2].transform.GetChild(k).GetComponent<CapsuleCollider>().enabled = true;
            SlagLines[2].transform.GetChild(k).tag = "Slag";
        }


        //OnTurningOffFlameStep1
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnTurningOffFlameStep1);

        

    }


    #endregion

    #region CleaningMethod

    public void OnClenaingProcessStart()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(StartSlagRemovalStep);

        
    }

    public void StartSlagRemovalStep()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[7].enabled = true;
        ChippingHammerHL1.SetActive(true);
        ChippingHammer.SetChippingHammerParams(88);
        ChippingHammer.AssignMethodOnSlagRemoveDone(CleanWeldPlaceWithBrush);
    }
    public void CleanWeldPlaceWithBrush()
    {
        FinalWorkPiece.GetComponent<BoxCollider>().enabled = true;
        ToolsOutlines[0].enabled = true;
        WireBrushHL1.SetActive(true);
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(CleanWeldingLine);
    }

    public void CleanWeldingLine()
    {
        

        for (int i = 0; i < SlagLines[1].transform.childCount; i++)
        {
            SlagLines[1].transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial = WeldFinishMat;
            SlagLines[1].transform.GetChild(i).GetComponent<CapsuleCollider>().enabled = false;
        }

        for (int j = 0; j < SlagLines[2].transform.childCount; j++)
        {
            SlagLines[2].transform.GetChild(j).GetComponent<MeshRenderer>().sharedMaterial = WeldFinishMat;
            SlagLines[2].transform.GetChild(j).GetComponent<CapsuleCollider>().enabled = false;
        }

        finishPanel.SetActive(true);
        readSteps.tablet.SetActive(true);
        readSteps.panel.SetActive(false);
    }
    #endregion

    public void ResetToolPosition(Transform tool,ToolOrigin TO)
    {
        
        tool.position = TO.pos;
        tool.localEulerAngles = TO.rot;
    }

}
