using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;


public class Experiment_12_Manager : MonoBehaviour
{
    public static Experiment_12_Manager instance;

    [Header("Canvas ")]
    public GameObject finishPanel;

    [Header("Read step from json calss")]
    public ReadStepsFromJson readSteps;
    [Header("Steps audio clips")]
    public AudioSource stepAudioSource;
    public AudioClip[] stepsAudioClip;

    #region Tools GameObject
    public GameObject WireBrushGo;
    public GameObject SteelRulerGo;
    private Transform SteelRulerGoStartPos;
    public GameObject ScriberGO;
    public GameObject CenterPunchGo;
    public GameObject HammerGo;
    public GameObject WeldingFlameGo;
    public GameObject CCMSFillerRodGo;
    public GameObject ChippingHammerGo;
    public XRGrabInteractable WeldingTorch;
    public Transform FlameControlPivot;
    public Transform WeldingPivot;
    #endregion

    #region EmbeddedTool
    private Transform SteelRuler_AttachedToJob;
    #endregion

    public Transform WorkPiece;
    private WireBrush_VL WireBrush;
    private SteelRuler_VL SteelRuler;
    private ScriberMarking_VL Scriber;
    private DotPunching_VL DotPunch;
    private WeldingFlame_VL WeldingFlame;
    private ChippingHammer_VL ChippingHammer;

    #region GasWeldingSetupVariables
    public GameObject PlacementSocketforWeldingGo;

    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;
    public int countppekit;

    [Header("Canvas")]
    public GameObject setPressureRegulatorCanvas, lighterCanvas, lighter_Flame;

    public RotateNozzle[] rotateNozzles;

    [Header("Tools Colliders")]
    public Collider[] GasTableKitColliders;

    [Header("Tools Highlight")]
    public Outline[] ToolsOutlines;

    [Header("Welding Highlight")]
    public Outline[] ObjectOutlines;

    public bool isPipeRedConnect, isPipeblueConnect;
    public GameObject nozzelSnapPoint;
    public GameObject ZeroMeterred, ZeroMeterBlack;
    public GameObject BluePipEndPoint, RedPipeEndPoint, ParentBluePipEndPoint, ParentRedPipeEndPoint;
    public GameObject HL_T_connectorBlack, HL_T_connectorRed, HL_T_ClipBlack, HL_T_ClipRed;
    public GameObject bluePipeRope, redPipeRope, blackSmoke, oxidizing_F, reduce_or_carb_F, neturel_F;

    [Header("          ")]
    public GameObject step8Flame;
    public GameObject Step9flame, extraRedBol, oldRedBol, redRotateSprite, BlueRotatesprite;

    public bool isTurnOffFlame;
    public bool slagRemoved;


    public LineRenderer[] LinesToDraw;
    public GameObject[] DotPunchLines;
    
    
    Vector3 pos = new Vector3(0.0317f, 0, 0);
    Quaternion ScriberRot = Quaternion.identity;
    public GameObject ScriberHL;
    public GameObject SteelRuleHL;
    public GameObject CenterPunchHL;
    public GameObject HammerHL;
    #endregion


    public void Awake()
    {
        Debug.Log("Awake");

        instance = this;
        Application.targetFrameRate = 60;
        nozzelSnapPoint.gameObject.SetActive(false);


        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = false;
            ppekitcolliders[i].GetComponent<Outline>().enabled = false;
        }
        for (int i = 0; i < GasTableKitColliders.Length; i++)
        {
            GasTableKitColliders[i].enabled = false;
        }
        for (int i = 0; i < ObjectOutlines.Length; i++)
        {
            ObjectOutlines[i].enabled = false;
        }

        for (int i = 0; i < rotateNozzles.Length; i++)
        {
            rotateNozzles[i].enabled = false;
        }

        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmStartBtn);
        readSteps.confirmbtn.gameObject.SetActive(true);
    }
    void Start()
    {
        WireBrush = WireBrushGo.GetComponent<WireBrush_VL>();
        SteelRuler = SteelRulerGo.GetComponent<SteelRuler_VL>();
        SteelRulerGoStartPos = SteelRulerGo.transform;
        Scriber = ScriberGO.GetComponent<ScriberMarking_VL>();
        DotPunch = CenterPunchGo.GetComponent<DotPunching_VL>();
        ChippingHammer = ChippingHammerGo.GetComponent<ChippingHammer_VL>();
        SteelRuler_AttachedToJob = WorkPiece.Find("SteelRuler");
        //EnableCenterLineDrawingMech();


        //ConnectWeldingTorchToGasPipe_Step_1();
        //EnablePPEKitStep();

        //WeldingFlame = WeldingFlameGo.GetComponent<WeldingFlame_VL>();
        //WeldingFlameGo.GetComponent<CapsuleCollider>().enabled = true;

        //if (WeldingFlame)
        //{
        // WeldingFlame.SetWeldingFlameParams(WorkPiece, 1);
        // WeldingFlame.AssignMethodOnFusionRunDone(OnDoneFusionRun);
        // EnableWeldingOnFirstLine();
        //}
        //EnableMarkingOnJob();


    }
    private void Update()
    {
        if (isPipeblueConnect)
        {
            BluePipEndPoint.transform.parent = ParentBluePipEndPoint.gameObject.transform; // new 22
            BluePipEndPoint.transform.localPosition = Vector3.zero;
            BluePipEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        if (isPipeRedConnect)
        {
            RedPipeEndPoint.transform.parent = ParentRedPipeEndPoint.gameObject.transform;//red pipe sphere welding Tourch   //new 22
            RedPipeEndPoint.transform.localPosition = Vector3.zero;
            RedPipeEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void LockRotation(GameObject tool)
    {
        StartCoroutine(SetScriberRotationInTime(tool));
    }

    IEnumerator SetScriberRotationInTime(GameObject tool)
    {
        yield return new WaitForSeconds(1.5f);
        ScriberRot = tool.transform.localRotation;
       
    }

    public void SetWeldingTorchPivot(Transform pivot)
    {
        WeldingTorch.attachTransform = pivot;
    }
    public void SetFillerRod(GameObject fillerRod)
    {
        if (WeldingFlame)
        {
            WeldingFlame.SetFillerRod(fillerRod);
        }
    }
    public void OnDoneFusionRun()
    {
        Debug.Log("FusionRun Done Start ChippingHammer");
    }
   
    public void ConfirmStartBtn()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnablePPEKitStep);
    }

    #region PPE Kit Methods
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
            OnEnableCleanJobWithWireBrush();
            //readSteps.panel.SetActive(false);
            //finishPanel.SetActive(true);
        }
        selectGameObject.SetActive(false);
    }
    #endregion

    public void OnEnableCleanJobWithWireBrush()
    {
       
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableCleanJobWithWireBrush);
        //Play audio
    }

    public void EnableCleanJobWithWireBrush()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[0].enabled = true; //JobPlateHighlight
        ToolsOutlines[1].enabled = true; //WireBrushHighlight
        WireBrush.SetWireBrushParams(4, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(OnEnableMarkingOnJob);
        //Debug.Log("Highligh the Job placement and Job hightligh and Wire brush highlight");
    }

    public void OnEnableMarkingOnJob()
    {
        ToolsOutlines[1].enabled = false; //WireBrushHighlight

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableMarkingOnJob);
    }
    #region DrawingLineMethods

    public void EnableMarkingOnJob()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[2].enabled = true;
        SteelRuler.readyForOperation = true;
        SteelRuler.AssignMethodOnSnapToJob(EnableCenterLineDrawingMech);
    }
    public void EnableCenterLineDrawingMech()
    {

       // SteelRuleHL.SetActive(true);
        ScriberGO.GetComponent<Outline>().enabled = true;
        ScriberGO.GetComponent<BoxCollider>().enabled = true;
        SteelRuler_AttachedToJob.gameObject.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 0, ScriberHL);
        Scriber.AssignMethodOnMarkingDone(DrawAllLines);
    }

    public void EnableFirstLineDrawingMech()
    {
        //Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 1);
        Scriber.AssignMethodOnMarkingDone(EnableSecondLineDrawingMech);
    }

    public void EnableSecondLineDrawingMech()
    {
        //Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 2);
        Scriber.AssignMethodOnMarkingDone(EnableThirdLineDrawingMech);
    }

    public void EnableThirdLineDrawingMech()
    {
        //Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 3);
        Scriber.AssignMethodOnMarkingDone(EnableFourthLineDrawingMech);
    }

    public void EnableFourthLineDrawingMech()
    {
       // Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 4);
        Scriber.AssignMethodOnMarkingDone(EnableFifthLineDrawingMech);
    }

    public void EnableFifthLineDrawingMech()
    {
        //Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 5);
        Scriber.AssignMethodOnMarkingDone(OnEnableDotPunchingOnJob);
    }

    public void DrawAllLines()
    {

        LinesToDraw[0].SetPosition(0, pos);
        LinesToDraw[1].SetPosition(0, pos);
        LinesToDraw[2].SetPosition(0, pos);
        LinesToDraw[3].SetPosition(0, pos);
        LinesToDraw[4].SetPosition(0, pos);




        OnEnableDotPunchingOnJob();
    }

    #endregion

    public void OnEnableDotPunchingOnJob()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableDotPunchingOnFirstLine);
    }

    #region PunchingMethods
    public void EnableDotPunchingOnFirstLine()
    {
        readSteps.HideConifmBnt();

        CenterPunchGo.GetComponent<Outline>().enabled = true;
        HammerGo.GetComponent<Outline>().enabled = true;
        DotPunch.SetCenterPunchParams(WorkPiece, 1, CenterPunchHL, HammerHL);
        DotPunch.AssignMethodOnPunchingDone(DrawAllDotPunchLines);

    }

    public void EnableDotPuchingOnSecondLine()
    {
        //DotPunch.SetCenterPunchParams(WorkPiece, 2);
        DotPunch.AssignMethodOnPunchingDone(EnableDotPuchingOnThirdLine);
    }

    public void EnableDotPuchingOnThirdLine()
    {
       // DotPunch.SetCenterPunchParams(WorkPiece, 3);
        DotPunch.AssignMethodOnPunchingDone(EnableDotPuchingOnFourthLine);
    }

    public void EnableDotPuchingOnFourthLine()
    {
        //DotPunch.SetCenterPunchParams(WorkPiece, 4);
         DotPunch.AssignMethodOnPunchingDone(EnableDotPuchingOnFifthLine);
    }

    public void EnableDotPuchingOnFifthLine()
    {
        //DotPunch.SetCenterPunchParams(WorkPiece, 5);
        DotPunch.AssignMethodOnPunchingDone(OnEnableGasWeldingSetUp);
    }

    public void DrawAllDotPunchLines()
    {
        DotPunchLines[0].SetActive(true);
        DotPunchLines[1].SetActive(true);
        DotPunchLines[2].SetActive(true);
        DotPunchLines[3].SetActive(true);
        OnEnableGasWeldingSetUp();
    }

    public void OnEnableGasWeldingSetUp()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(ConnectWeldingTorchToGasPipe_Step_1);
    }
    #endregion


    #region WeldingTorchConnection
    public void ConnectWeldingTorchToGasPipe_Step_1()
    {
        readSteps.HideConifmBnt();


        GasTableKitColliders[0].enabled = true;
        ObjectOutlines[0].enabled = true;

        GasTableKitColliders[1].enabled = true;
        GasTableKitColliders[1].GetComponent<Outline>().enabled = true;
        GasTableKitColliders[1].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorBlack.SetActive(true);
    }

    public void DoneConnector_T_Black()
    {
        GasTableKitColliders[2].enabled = true;
        GasTableKitColliders[2].GetComponent<Outline>().enabled = true;
        GasTableKitColliders[2].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorRed.SetActive(true);
    }

    public void DoneConnector_T_Red()
    {

        GasTableKitColliders[3].enabled = true;// blue pipe sphere welding Tourch 
        bluePipeRope.GetComponent<CapsuleCollider>().enabled = true;
        ObjectOutlines[2].enabled = true;// blue pipe sphere outline
        ObjectOutlines[3].enabled = true;
    }

    public void OnConnectBlackPipe()
    {
        ObjectOutlines[2].enabled = false;// blue pipe sphere outline
        ObjectOutlines[3].enabled = false;

        ObjectOutlines[4].enabled = true;// red pipe sphere outline
        ObjectOutlines[5].enabled = true;

        GasTableKitColliders[3].enabled = false;// blue pipe sphere welding Tourch 
        GasTableKitColliders[4].enabled = true;// red pipe sphere welding Tourch 

        redPipeRope.GetComponent<CapsuleCollider>().enabled = true;
        BluePipEndPoint.GetComponent<SnapGrabbleObject>().enabled = false;

        BluePipEndPoint.transform.localPosition = Vector3.zero;
        BluePipEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        BluePipEndPoint.GetComponent<CapsuleCollider>().enabled = false;
        BluePipEndPoint.transform.localScale = new Vector3(0.044504f, 0.03f, 0.03f);

        BluePipEndPoint.transform.GetChild(1).gameObject.SetActive(false);
        isPipeblueConnect = true;
    }

    public void OnConnecteRedPipe()
    {
        ObjectOutlines[4].enabled = false;// red pipe sphere outline
        ObjectOutlines[5].enabled = false;

        RedPipeEndPoint.GetComponent<SnapGrabbleObject>().enabled = false;
        ObjectOutlines[0].enabled = false;
        GasTableKitColliders[4].enabled = false; // red pipe sphere welding Tourch 
        RedPipeEndPoint.transform.parent = ParentRedPipeEndPoint.gameObject.transform;//red pipe sphere welding Tourch 
        RedPipeEndPoint.transform.localPosition = Vector3.zero;
        RedPipeEndPoint.GetComponent<CapsuleCollider>().enabled = false;
        RedPipeEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);

        RedPipeEndPoint.transform.GetChild(1).gameObject.SetActive(false);
        // RedPipeEndPoint.transform.localScale = new Vector3(0.044504f, 0.03f, 0.03f);
        isPipeRedConnect = true;

        //clip of torch black
        GasTableKitColliders[5].enabled = true;
        GasTableKitColliders[5].GetComponent<Outline>().enabled = true;
        GasTableKitColliders[5].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_ClipBlack.SetActive(true);
    }

    public void DoneBlackClip_T()
    {
        //clip of torch red
        GasTableKitColliders[6].enabled = true;
        GasTableKitColliders[6].GetComponent<Outline>().enabled = true;
        GasTableKitColliders[6].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_ClipRed.SetActive(true);
    }

    public void DoneRedClip_T()
    {
        Debug.Log("Torch connect done");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(SettingGasPressure_Step_1);
        
    }

    #endregion

    #region Setting gas pressure as per the nozzole size
    public void SettingGasPressure_Step_1()
    {
        readSteps.HideConifmBnt();


        setPressureRegulatorCanvas.SetActive(true);
        GasTableKitColliders[7].enabled = true; //blue valve nozzel

        rotateNozzles[0].enabled = true; //blue valve nozzel
        ObjectOutlines[6].enabled = true; //blue valve nozzel outline
        BlueRotatesprite.SetActive(true);
        

    }


    public void OnEnableRedValeNozzel()
    {
        if (!isTurnOffFlame)
        {
            redRotateSprite.SetActive(true);
            ObjectOutlines[6].enabled = false;
            ObjectOutlines[7].enabled = true;
            GasTableKitColliders[8].enabled = true;
            //  GasTablekitcolliders[10].transform.GetChild(1).gameObject.SetActive(true);
            rotateNozzles[1].enabled = true;// red valve nozzel
        }
    }

    #endregion

    public void OnEnableCheckLeakage()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(CheckLeakageWithSoapWater);

        ObjectOutlines[7].enabled = false;
        setPressureRegulatorCanvas.SetActive(false);
    }

    #region Check leakage
    public void CheckLeakageWithSoapWater()
    {

        readSteps.HideConifmBnt();
        ObjectOutlines[8].enabled = true; //red regulator outline
        GasTableKitColliders[9].enabled = true; // glass object for red
        ObjectOutlines[10].enabled = true; //red glass outline
    }

    public void CheckLeakageWithWater()
    {
        ObjectOutlines[8].enabled = false;//red regulator outline
        ObjectOutlines[10].enabled = true; //red glass outline

        GasTableKitColliders[10].enabled = true; // glass object for red
        ObjectOutlines[11].enabled = true; //red glass outline

        ObjectOutlines[9].enabled = true; //blue regulator outline

    }
    
    public void OnEnableAttachNozzle()
    {
        ObjectOutlines[9].enabled = false;//blue regulator outline
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(AttachNozzleToTorch);
    }
    public void AttachNozzleToTorch()
    {
        readSteps.HideConifmBnt();

        GasTableKitColliders[11].enabled = true;//Welding nozzle collider
        GasTableKitColliders[11].GetComponent<SnapGrabbleObject>().enabled = true;
        GasTableKitColliders[11].GetComponent<Outline>().enabled = true;

        ObjectOutlines[12].enabled = true;
        ObjectOutlines[13].enabled = true;

        nozzelSnapPoint.gameObject.SetActive(true);
    }
    #endregion

    #region Placing Job on WeldingTable
    public void OnEnableJobPlacingForWelding()
    {
        GasTableKitColliders[11].GetComponent<SnapGrabbleObject>().enabled = false;
        ObjectOutlines[12].enabled = false;
        ObjectOutlines[13].enabled = false;
        GasTableKitColliders[11].enabled = false;//Welding nozzle collider

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableJobPlacingforWelding);
    }

    public void EnableJobPlacingforWelding()
    {
        readSteps.HideConifmBnt();
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        PlacementSocketforWeldingGo.SetActive(true);
    }

    public void OnJobPlacedForWelding()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(flameControlStep);
    }
    #endregion

    #region lighter/flame control step

    public void flameControlStep()
    {
        readSteps.HideConifmBnt();
        PlacementSocketforWeldingGo.SetActive(false);
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = false;


        GasTableKitColliders[12].enabled = true; //lighter
        GasTableKitColliders[12].GetComponent<SnapGrabbleObject_VL>().enabled = true;
        GasTableKitColliders[12].GetComponent<Outline>().enabled = true;

        lighterCanvas.SetActive(true);
        ObjectOutlines[12].enabled = true;
    }

    public void LighterSnap_true()
    {
        GasTableKitColliders[12].GetComponent<SnapGrabbleObject_VL>().enabled = false;
        lighterCanvas.SetActive(false);
        ObjectOutlines[12].enabled = false;

        GasTableKitColliders[13].enabled = true; //red bol at gas torch
        GasTableKitColliders[13].GetComponent<Outline>().enabled = true;
        StartCoroutine(lighterEnable());

    }

    IEnumerator lighterEnable()
    {
        GasTableKitColliders[12].enabled = false;  //lighter
        lighter_Flame.SetActive(false);
        GasTableKitColliders[12].GetComponent<SnapGrabbleObject_VL>().enabled = false;
        GasTableKitColliders[12].gameObject.SetActive(false); 
        PlayFlamsParticle1();   //new 22
        yield return new WaitForSeconds(1.5f);
        PlayFlamsParticle2();  //new 22
    }

    void PlayFlamsParticle1()
    {
        blackSmoke.SetActive(true); // black smoke
        GasTableKitColliders[13].GetComponent<Outline>().enabled = false;
    }
    void PlayFlamsParticle2()
    {
        blackSmoke.SetActive(false);
        reduce_or_carb_F.SetActive(true); // simple reduce
        reduce_or_carb_F.GetComponent<AudioSource>().Play();
        GasTableKitColliders[14].GetComponent<Outline>().enabled = false;
        //readSteps.onClickConfirmbtn();
        //readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_9_confirm);
        //PlayStepAudio(12);// open flam audio
        
        //Need a ui step for setting up the flame
        EnableFlameControls();
    }
    #endregion

    #region Flame setting
    public void EnableFlameControls()
    {
        reduce_or_carb_F.SetActive(false);
        step8Flame.SetActive(false);
        Step9flame.SetActive(true);

        GasTableKitColliders[15].enabled = true; // red bol reduce carbon
        rotateNozzles[2].enabled = true; //red bol  reduce carbon

        ObjectOutlines[14].GetComponent<BoxCollider>().enabled = true; //red bol  reduce carbon
        ObjectOutlines[15].GetComponent<BoxCollider>().enabled = false; //green bol oxidizing
        ObjectOutlines[16].GetComponent<BoxCollider>().enabled = false; //red bol netural

        rotateNozzles[2].GetComponent<Outline>().enabled = true; //red bol reduce carbon

    }

    public void PlayFlamsParticle1_Flame() //first step flame green
    {
        if (!isTurnOffFlame)
        {
            reduce_or_carb_F.SetActive(true);
            //reduce_or_carb_F.GetComponent<AudioSource>().Play();
            GasTableKitColliders[16].enabled = true; //green bol oxidizing
            rotateNozzles[3].enabled = true; //green bol oxidizing
            GasTableKitColliders[15].enabled = false; // red bol reduce carbon

            rotateNozzles[2].GetComponent<Outline>().enabled = false; //red bol reduce carbon

            ObjectOutlines[14].GetComponent<BoxCollider>().enabled = false; //red bol  reduce carbon
            ObjectOutlines[15].GetComponent<BoxCollider>().enabled = true; //green bol oxidizing
            ObjectOutlines[16].GetComponent<BoxCollider>().enabled = false; //red bol netural

            rotateNozzles[3].GetComponent<Outline>().enabled = true; //green bol oxidizing

        }
    }

    public void PlayFlameparticle2_Flam() //second step flame red
    {
        if (!isTurnOffFlame)
        {
            ObjectOutlines[14].GetComponent<BoxCollider>().enabled = false; //red bol  reduce carbon
            ObjectOutlines[15].GetComponent<BoxCollider>().enabled = false; //green bol oxidizing
            ObjectOutlines[16].GetComponent<BoxCollider>().enabled = true; //red bol netural

            reduce_or_carb_F.SetActive(false);
            GasTableKitColliders[16].enabled = false; //green bol oxidizing

            extraRedBol.SetActive(true);
            oldRedBol.SetActive(false);

            rotateNozzles[3].GetComponent<Outline>().enabled = true; //green bol oxidizing
            GasTableKitColliders[17].enabled = true; //red bol neutral red sphere sphere
            rotateNozzles[4].enabled = true; // neutral flame
            rotateNozzles[4].GetComponent<Outline>().enabled = true; // red bol neutral outline
        }
    }

    public void PlayFlamsParticle3_flame() // neutral flame
    {
        if (!isTurnOffFlame)
        {
            GasTableKitColliders[17].enabled = true;
            neturel_F.SetActive(true);
            //neturel_F.GetComponent<AudioSource>().Play();
            oxidizing_F.SetActive(false);
            rotateNozzles[4].GetComponent<Outline>().enabled = true; // red bol neutral outline


            OnEnableWeldingWithLeftwardTech();   
        }
    }
    #endregion

    public void OnEnableWeldingWithLeftwardTech()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableWeldingOnFirstLine);
        WeldingFlameGo.SetActive(true);
        WeldingFlame = WeldingFlameGo.GetComponent<WeldingFlame_VL>();
        WeldingFlameGo.GetComponent<CapsuleCollider>().enabled = true;
        ToolsOutlines[6].enabled = true;

       
        
    }

    #region WeldingMethods
    public void EnableWeldingOnFirstLine()
    {
        readSteps.HideConifmBnt();
        WeldingFlame.SetWeldingFlameParams(WorkPiece, 1);
        WeldingFlame.AssignMethodOnFusionRunDone(EnableWeldingOnSecondLine);
        SetWeldingTorchPivot(WeldingPivot);
    }

    public void EnableWeldingOnSecondLine()
    {
        WeldingFlame.SetWeldingFlameParams(WorkPiece, 2);
        WeldingFlame.AssignMethodOnFusionRunDone(EnableWeldingOnThirdLine);
    }

    public void EnableWeldingOnThirdLine()
    {
        WeldingFlame.SetWeldingFlameParams(WorkPiece, 3);
        WeldingFlame.AssignMethodOnFusionRunDone(EnableWeldingOnFourthLine);
    }

    public void EnableWeldingOnFourthLine()
    {
        WeldingFlame.SetWeldingFlameParams(WorkPiece, 4);
        WeldingFlame.AssignMethodOnFusionRunDone(EnableWeldingOnFifthLine);
    }

    public void EnableWeldingOnFifthLine()
    {
        WeldingFlame.SetWeldingFlameParams(WorkPiece, 5);
        WeldingFlame.AssignMethodOnFusionRunDone(OnEnableTurnOffFlame); //OnEnableRemoveSlagStep
    }

   

    public void OnEnableTurnOffFlame()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(TurnOffFlame_Step_1);
    }
    #endregion

    #region Turning off flame

    public void TurnOffFlame_Step_1()
    {

        readSteps.HideConifmBnt();
        SetWeldingTorchPivot(FlameControlPivot);
        neturel_F.SetActive(false);
        reduce_or_carb_F.SetActive(true); // redus true  1
        reduce_or_carb_F.GetComponent<AudioSource>().Play();

        isTurnOffFlame = true;
        GasTableKitColliders[15].enabled = true;

        rotateNozzles[2].enabled = true; //RED  bol reduse or crbarn
        rotateNozzles[2].RotateValue = 40; //RED  bol reduse or crbarn
        rotateNozzles[2].transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        rotateNozzles[2].OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        rotateNozzles[2].isclockwise = false; //RED  bol reduse or crbarn

        GasTableKitColliders[15].transform.gameObject.SetActive(true);
        GasTableKitColliders[17].transform.parent.gameObject.SetActive(true);
        GasTableKitColliders[15].enabled = true;

        ObjectOutlines[14].enabled = true;
        ObjectOutlines[15].enabled = false;

    }

    public void TurnOffFlame_Step_2()
    {
        if (isTurnOffFlame)
        {
            reduce_or_carb_F.SetActive(false);
            oxidizing_F.SetActive(true);

            GasTableKitColliders[15].enabled = false;
            oxidizing_F.GetComponent<AudioSource>().Play();

            rotateNozzles[2].enabled = false; //red bol reduse or carbon
            GasTableKitColliders[16].enabled = true; //Green bol oxidizing
            GasTableKitColliders[16].transform.gameObject.SetActive(true); //green bol oxidizing
            rotateNozzles[3].transform.localRotation = Quaternion.Euler(0, 0, 0);// Green bol oxidizing
            rotateNozzles[3].OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
            rotateNozzles[3].RotateValue = 40; //GREEN  bol oxidizing
            rotateNozzles[3].enabled = true; //GREEN  bol oxidizing
            rotateNozzles[3].isclockwise = false; //GREEN  bol oxidizing

            GasTableKitColliders[15].enabled = false;

            ObjectOutlines[14].enabled = false;
            ObjectOutlines[15].enabled = true;


        }

    }

    public void CallFlameOff()
    {
        if (isTurnOffFlame)
        {
            oxidizing_F.SetActive(false);
            neturel_F.SetActive(false);
            reduce_or_carb_F.SetActive(false);
            GasTableKitColliders[16].enabled = false;
            ObjectOutlines[14].enabled = false;
            ObjectOutlines[15].enabled = false;
            rotateNozzles[3].enabled = false; //Green bol oxidizing
            Debug.Log("Flame off");

            //CloseCylinderValves();
            OnEnableTurnOfRegulator();
        }
    }

    #endregion

    #region Turning of Regulator

    public void OnEnableTurnOfRegulator()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(CloseCylinderValves);
    }
    public void CloseCylinderValves()
    {
        readSteps.HideConifmBnt();
        GasTableKitColliders[7].enabled = true; //blue valve nozzole(1)
        ObjectOutlines[6].enabled = true; //blue valve nozzole(1)

        ObjectOutlines[9].enabled = true; //Black regulator

        rotateNozzles[0].isclockwise = false;  //blue valve nozzole(1)
        rotateNozzles[0].enabled = true; //blue valve nozzole(1)
        rotateNozzles[0].otherMeterobject.SetActive(false); //blue valve nozzole(1)
        rotateNozzles[0].otherMeterobject = ZeroMeterBlack; //blue valve nozzole(1)
        rotateNozzles[0].MeterObject.SetActive(true); //blue valve nozzole(1)
        rotateNozzles[0].RotateValue = 10; //blue valve nozzole(1)

    }

    public void callBlackValve()
    {
        if (isTurnOffFlame)
        {
            GasTableKitColliders[6].enabled = false; //blue valve nozzole
            
            ObjectOutlines[9].enabled = false; //black regulator
            ObjectOutlines[8].enabled = true;  //red regulator 
            ObjectOutlines[6].enabled = false; //blue valve nozzole
            GasTableKitColliders[8].enabled = true; //red valve nozzole

            ObjectOutlines[7].enabled = true; //red valve nozzole

            rotateNozzles[1].isclockwise = false; // red valve nozzle
            rotateNozzles[1].enabled = true;
            rotateNozzles[1].otherMeterobject.SetActive(false);
            rotateNozzles[1].otherMeterobject = ZeroMeterred;
            rotateNozzles[1].MeterObject.SetActive(true);
            rotateNozzles[1].RotateValue = 10;

        }
    }

    public void CallRedValve()
    {
        if (isTurnOffFlame)
        {
            ObjectOutlines[8].enabled = true;  //red regulator 
            rotateNozzles[1].enabled = false; //red nozzole
            GasTableKitColliders[8].enabled = false; //red valve nozzole
            Debug.Log("Closed valve");
            OnEnableRemoveSlagStep();
        }
    }


    #endregion

    public void OnEnableRemoveSlagStep()
    {
        ObjectOutlines[8].enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableChippingHammerProcedure);
    }
    public void EnableChippingHammerProcedure()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[7].enabled = true;
        ChippingHammer.SetChippingHammerParams(220);
        ChippingHammer.AssignMethodOnSlagRemoveDone(OnSlagRemoved);

    }

    public void OnSlagRemoved()
    {
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        ToolsOutlines[7].enabled = false;
        readSteps.panel.SetActive(false);
        finishPanel.SetActive(true);
    }





}
