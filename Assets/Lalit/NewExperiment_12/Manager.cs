using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;



/// <summary>


///[Priority{high(0)---Low(n)}][Expected Time for work {H>N>L}]     Update and improvments
///
///[0][M**]      Improvemtns and fixes in Gas welding SetUP:- Pivots, Objects should not vanish after operation e.g. lighter, soap glass, water glass, Fix Gas Pipe[vivek], Torch hold animation, Flame control, Ristriction area for welding torch[vivek],dial UI
///         , solid highlights for objects placement etc.
///         
///[1][H]      Welding fix and improvments and Effects: Torch&Rod proper welding mechanism with highlight and intutive control, welding flame effect(job area flame effect, flame split, rod melting and burning flames (Sounds),
///
///[2][D]      Effects and animation for Gas welding setup: Gas welding setUp animation for object snap e.g. clips, arrestor, connector etc.
///
///[3][M]      Finishing and cleaning after welding: slag effects, chipping hammer sound, Welding area cleaning brush effects and mech,
///
///[4][M]      Sounds for welding Setup and Welding(Fusion Run):- lighter Sound, flames sound, welding sound,setup sounds 
///
///[5][M*]      Improvments: For Job Ready * Measurement With ruler, Add lines of job dimensions on ruler snap.
///
///[6][L]      Tools sound //For Job ready WireBrush,[Optional: Filling on Job],Steel Ruler, Scriber, Dot Punch, hammer,
///
///[6][L]      Tools Effects //For Job ready Scriber,[Optional: Filling ]
///
///[7][L]      Add ppt kit Sequence pick up

/// </summary>


public class Manager : MonoBehaviour
{
    public static Manager instance;

    public GameObject finishPanel;
    public ReadStepsAndVideoManager readSteps;

    [Header("Tools Highlight")]
    public Outline[] ToolsHighlight;
    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;
    public int countppekit;

    
    public GameObject WireBrushHL,SteelRulerHL, ScriberHL, CenterPunchHL, HammerHL,RodHL1, RodHL2, TorchHL1, TorchHL2,ChipHammerHL, WireBrushHL2;
    public GameObject ScriberHL1, ScriberHL2, ScriberHL3, ScriberHL4, ScriberHL5;
    public GameObject CenterPunchHL1, HammerHL1;
    #region Tools GameObject
    public GameObject WireBrushGo;
    public GameObject SteelRulerGo;
    public GameObject ScriberGo;
    public GameObject CenterPunchGo;
    public GameObject HammerGo;
    public GameObject ChippingHammerGo;
    #endregion

    private Transform SteelRuler_AttachedToJob;

    public Transform WorkPiece;
    private WireBrush_VL WireBrush;
    private SteelRuler_VL SteelRuler;
    private ScriberMarking_VL Scriber;
    private DotPunching_VL DotPunch;
    private ChippingHammer_VL ChippingHammer;
    private WeldingFlame_VL Weldingflame;


    

    [Header("Table uper gas kit")]
    public Collider[] GasTableObjectcolliders;

    [Header("outLine Object to HighLight")]
    public Outline[] ObjectOutlines;

    public GameObject lighter_Flame, blackSmoke, oxidizing_F, reduce_or_carb_F, neturel_F;
    public GameObject Step8flame, Step9flame, extraRedBol, oldRedBol;

    public bool isTurnOffFlame;

    public RotateNozzle[] rotateNozzles;

    public GameObject[] DotPuchLines;
    public GameObject[] WeldLines;
    public GameObject[] SlagLines;

    public GameObject WeldingSocket;
    public GameObject WeldingNuetralFlame;
    public GameObject[] FillerRod;
    public Material WeldFinishMat;

    public GameObject ZeroMeterred, ZeroMeterBlack;


    public GameObject Ruler1, Ruler2;
    public GameObject MeasureObj1, MeasureObj2;

    public GameObject ClockWiseSprite, AntiClockWiseSprite;
    public Transform BrushOT, SteelRulerOT, ScriberOT, DotPunchOT, HammerOT, ChippingHammerOT;
    public Dictionary<string, ToolOrigin> ToolsOrigin = new Dictionary<string, ToolOrigin>();

    public void PlayVoiceOver(int index)
    {
        VoiceOverManager_VL.instance.PlayVOForStepIndex(index);
    }
    public void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmSatrtbtn);
        readSteps.confirmbtn.gameObject.SetActive(true);

        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = false;
            ppekitcolliders[i].GetComponent<Outline>().enabled = false;
        }

        for (int i = 0; i < GasTableObjectcolliders.Length; i++)
        {
            GasTableObjectcolliders[i].enabled = false;
        }

        for (int i = 0; i < ObjectOutlines.Length; i++)
        {
            ObjectOutlines[i].enabled = false;
        }

        for (int i = 0; i < rotateNozzles.Length; i++)
        {
            rotateNozzles[i].enabled = false;
        }

        for (int i = 0; i < ToolsHighlight.Length; i++)
        {
            ToolsHighlight[i].enabled = false;
        }

    }

    public void Start()
    {
        //PlayVoiceOver(0);
        WireBrush = WireBrushGo.GetComponent<WireBrush_VL>();
        SteelRuler = SteelRulerGo.GetComponent<SteelRuler_VL>();
        SteelRuler_AttachedToJob = WorkPiece.transform.Find("SteelRuler");
        Scriber = ScriberGo.GetComponent<ScriberMarking_VL>();
        DotPunch = CenterPunchGo.GetComponent<DotPunching_VL>();
        Weldingflame = WeldingNuetralFlame.GetComponent<WeldingFlame_VL>();
        ChippingHammer = ChippingHammerGo.GetComponent<ChippingHammer_VL>();
        //Hammer = HammerGo.GetComponent<Hammer_VL>();

        //EnableJobPlacingForWelding();

        // PrepareJobforRemovingSlag();

        AssignToolOriginToDictionay();

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

        ToolOrigin CHTO = SetToolsOrigin(ChippingHammer.transform);
        ToolsOrigin.Add("ChippingHammer", CHTO);

    }

    public void ResetToolPosition(Transform tool, ToolOrigin TO)
    {

        tool.position = TO.pos;
        tool.localEulerAngles = TO.rot;
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
            Debug.Log("Done ppe step");
        }
        selectGameObject.SetActive(false);
    }
    #endregion

    public void OnEnableCleanJobWithWireBrush()
    {
        PlayVoiceOver(2);
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableCleanJobWithWireBrush);
    }

    public void EnableCleanJobWithWireBrush()
    {
        readSteps.HideConifmBnt();
        WorkPiece.GetComponentInChildren<Outline>().enabled = true;
        ToolsHighlight[0].enabled = true;
        WireBrushHL.SetActive(true);
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(CleaningDone);
        //Assign On Clean job done method to brush
    }

    public void CleaningDone()
    {
        ToolsHighlight[0].enabled = false;
        WorkPiece.GetComponentInChildren<Outline>().enabled = false;
        Debug.Log("Cleaning Done");
        
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableMeasuring);

        PlayVoiceOver(3);
        //EnableJobPlacingForWelding();
    }

    #region MarkingMethods

    public void EnableMeasuring()
    {
        readSteps.HideConifmBnt();
        Ruler1.SetActive(true);
        ToolsHighlight[1].enabled = true;
        WorkPiece.GetComponentInChildren<Outline>().enabled = true;
        SteelRuler.isMeasuring = true;
        SteelRuler.AssignMethodOnSnapForMeasurement(EnableScriberForMeasureing);
        ResetToolPosition(WireBrushGo.transform, ToolsOrigin["WireBrush"]);


    }

    public void EnableScriberForMeasureing()
    {
        ToolsHighlight[2].enabled = true;
        Scriber.isMeasuring = true;
        Scriber.SetScriberForMeasuremetn(WorkPiece, 6,1);
        Scriber.AssignMethodOnMeasuringDone(EnableMeasuringAgain);
    }

    public void EnableMeasuringAgain()
    {
        ToolsHighlight[1].enabled = true;
        SteelRuler.GetComponent<CustomXRGrabInteractable>().enabled = true;
        SteelRuler.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Ruler2.SetActive(true);
        SteelRuler.AssignMethodOnSnapForMeasurement(EnableScriberForMeasureingAgain);
        Scriber.EmptyParamsForMeasuring();

    }

    public void EnableScriberForMeasureingAgain()
    {
        ToolsHighlight[2].enabled = true;
        
        Scriber.SetScriberForMeasuremetn(WorkPiece,6, 2);
        Scriber.AssignMethodOnMeasuringDone(EnableMarking);


    }
    public void EnableMarking()
    {
        Scriber.isMeasuring = false;
        SteelRuler.isMeasuring = false;
        SteelRuler.GetComponent<CustomXRGrabInteractable>().enabled = true;
        SteelRuler.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Scriber.EmptyParamsForMeasuring();
        SteelRuler.GetComponent<CustomXRGrabInteractable>().enabled = true;
        ToolsHighlight[1].enabled = true;
        WorkPiece.GetComponentInChildren<Outline>().enabled = true;
        SteelRuler.readyForOperation = true;
        SteelRuler.AssignHighlight(SteelRulerHL);
        SteelRuler.AssignMethodOnSnapToJob(EnableCenterLineDrawingMech);
        //Set the method on done snap
    }

    public void EnableCenterLineDrawingMech()
    {
        Scriber.EmptyParamsForMeasuring();
        ToolsHighlight[2].enabled = true;
        SteelRuler_AttachedToJob.gameObject.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 0, ScriberHL);
        Scriber.AssignMethodOnMarkingDone(EnableFirstLineDrawing);

    }

    public void EnableFirstLineDrawing()
    {
        ScriberHL1.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 1);
        Scriber.AssignMethodOnMarkingDone(EnableSecondLineDrawing);
    }

    public void EnableSecondLineDrawing()
    {
        ScriberHL2.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 2);
        Scriber.AssignMethodOnMarkingDone(EnableThirdLineDrawing);
    }

    public void EnableThirdLineDrawing()
    {
        ScriberHL3.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 3);
        Scriber.AssignMethodOnMarkingDone(EnableFourthLineDrawing);
    }

    public void EnableFourthLineDrawing()
    {
        ScriberHL4.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 4);
        Scriber.AssignMethodOnMarkingDone(EnableFifthLineDrawing);
    }

    public void EnableFifthLineDrawing()
    {
        ScriberHL5.SetActive(true);
        Scriber.SetScriberMarkingParams(WorkPiece, SteelRuler_AttachedToJob, 5);
        Scriber.AssignMethodOnMarkingDone(DoneMarking);
    }
    #endregion

    public void DoneMarking()
    {
        ToolsHighlight[2].enabled = false;
        WorkPiece.GetComponentInChildren<Outline>().enabled = false;

        readSteps.onClickConfirmbtn();
         readSteps.AddClickConfirmbtnEvent(EnablePunching);

        PlayVoiceOver(4);
        //EnablePunching();
    }

    public void EnablePunching()
    {
        readSteps.HideConifmBnt();
        ToolsHighlight[3].enabled = true;
        ToolsHighlight[4].enabled = true;

        DotPunch.SetCenterPunchParams(WorkPiece, 1,CenterPunchHL,HammerHL);
        DotPunch.AssignMethodOnPunchingDone(EnablePunchingForSecondLine);

        ResetToolPosition(ScriberGo.transform, ToolsOrigin["Scriber"]);
    }

    public void EnablePunchingForSecondLine()
    {
        CenterPunchHL1.SetActive(true);
        HammerHL1.SetActive(true);
        DotPunch.SetCenterPunchParams(WorkPiece, 2);
        DotPunch.AssignMethodOnPunchingDone(CompleteAllPunchingLine);
    }

    public void CompleteAllPunchingLine()
    {
        DotPuchLines[0].SetActive(true);
        DotPuchLines[1].SetActive(true);
        DotPuchLines[2].SetActive(true);
        ToolsHighlight[3].enabled = false;
        ToolsHighlight[4].enabled = false;
        MeasureObj1.SetActive(false);
        MeasureObj2.SetActive(false);
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableJobPlacingForWelding);

        PlayVoiceOver(5);
    }

    public void EnableJobPlacingForWelding()
    {
        readSteps.HideConifmBnt();
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        WorkPiece.GetComponentInChildren<Outline>().enabled = true;
        WeldingSocket.SetActive(true);
        WeldingSocket.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnJobSnapForWelding);
        ResetToolPosition(CenterPunchGo.transform, ToolsOrigin["CenterPunch"]);
        ResetToolPosition(HammerGo.transform, ToolsOrigin["Hammer"]);
    }

    public void OnJobSnapForWelding()
    {
        readSteps.onClickConfirmbtn();
        WorkPiece.GetComponentInChildren<Outline>().enabled = false;

        WeldingSocket.SetActive(false);
        readSteps.AddClickConfirmbtnEvent(GasWeldingSetUP.instance.Onclickbtn_s_3_confirm);

        PlayVoiceOver(6);
    }



    #region WeldingTorchFlamecontrolStep
    public void startFromNozzleWithTorch()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnSetUpDone);
    }
    public void ConfirmSatrtbtn()
    {
        readSteps.onClickConfirmbtn();
       // readSteps.AddClickConfirmbtnEvent(GasWeldingSetUP.instance.Onclickbtn_s_3_confirm);
        //readSteps.AddClickConfirmbtnEvent(FlameControlStep);

        readSteps.AddClickConfirmbtnEvent(EnablePPEKitStep);
        PlayVoiceOver(1);
        //PlayStepAudio(3);// kit audio
    }


    public void OnSetUpDone()
    {
        Debug.Log("SetUp Done");
        readSteps.HideConifmBnt();
        // FlameControlStep();
    }

    public void FlameControlStep()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[0].enabled = true; //lighter
        GasTableObjectcolliders[0].GetComponent<SnapGrabbleObject_VL>().enabled = true;
        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = true;

        ObjectOutlines[0].enabled = true; // torch nozel part 
       // PlayVoiceOver(15);
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

        PlayVoiceOver(16);

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
            readSteps.AddClickConfirmbtnEvent(EnableWelding);

            PlayVoiceOver(17);
        }
    }

    #endregion

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
        PlayVoiceOver(19);
    }

    public void enableRotateSpriteForClockWiseRotation(GameObject sprite)
    {
        Vector3 rot = sprite.transform.localEulerAngles;
        rot.y = 180;
        sprite.transform.localEulerAngles = rot;
    }
    public void CloseCylinderValves()
    {
        AntiClockWiseSprite.SetActive(true);
        enableRotateSpriteForClockWiseRotation(AntiClockWiseSprite);
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[6].enabled = true; //blue valve nozzole(1)
        ObjectOutlines[6].enabled = true; //blue valve nozzole(1)

        ObjectOutlines[8].enabled = true; //Black regulator

        rotateNozzles[3].isclockwise = true;  //blue valve nozzole(1)
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
            ClockWiseSprite.SetActive(true);
            enableRotateSpriteForClockWiseRotation(ClockWiseSprite);

            GasTableObjectcolliders[6].enabled = false; //blue valve nozzole

            ObjectOutlines[8].enabled = false; //black regulator
            ObjectOutlines[9].enabled = true;  //red regulator 
            ObjectOutlines[6].enabled = false; //blue valve nozzole
            GasTableObjectcolliders[7].enabled = true; //red valve nozzole

            ObjectOutlines[7].enabled = true; //red valve nozzole

            rotateNozzles[4].isclockwise = true; // red valve nozzle
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
            OnTurnOffDone();
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
    public void EnableWelding()
    {
        readSteps.HideConifmBnt();
        RodHL1.SetActive(true);
        TorchHL1.SetActive(true);
        WeldingNuetralFlame.SetActive(true);
        WeldingNuetralFlame.GetComponent<CapsuleCollider>().enabled = true;
        for (int i = 0; i < FillerRod.Length; i++)
        {
            FillerRod[i].GetComponent<Outline>().enabled = true;
            FillerRod[i].GetComponent<XRGrabInteractable>().enabled = true;
           // FillerRod[i].transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
        }

        Weldingflame.SetWeldingFlameParams(WorkPiece, 1);
        Weldingflame.AssignMethodOnFusionRunDone(EnableSecondLineWelding);

    }

    public void EnableSecondLineWelding()
    {
        RodHL2.SetActive(true);
        TorchHL2.SetActive(true);
        Weldingflame.SetWeldingFlameParams(WorkPiece, 2);
        Weldingflame.AssignMethodOnFusionRunDone(CompleteOtherWeldingLine);
    }

    public void CompleteOtherWeldingLine()
    {
        WeldLines[0].SetActive(true);
        WeldLines[1].SetActive(true);
        WeldLines[2].SetActive(true);
        //Debug.Log("Welding Completed add chipping hammer step");
        //Add close flame step and close regulator

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnTurningOffFlameStep1);
        PlayVoiceOver(18);

    }



    #endregion
    #region Cleaning Methods

    public void OnTurnOffDone()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(PrepareJobforRemovingSlag);
        PlayVoiceOver(20);
    }
    public void PrepareJobforRemovingSlag()
    {
        readSteps.HideConifmBnt();
        int count = 0;
        for (int i = 0; i < FillerRod.Length; i++)
        {
            FillerRod[i].GetComponent<Outline>().enabled = false;

        }

        for (int i = 0; i < SlagLines.Length; i++)
        {
            Transform line = SlagLines[i].transform;
            line.gameObject.SetActive(true);
            for (int j = 0; j < line.childCount; j++)
            {
                count++;
                line.GetChild(j).gameObject.SetActive(true);
                line.GetChild(j).GetComponent<CapsuleCollider>().enabled = true;
                line.GetChild(j).tag = "Slag";
            }
        }

        ToolsHighlight[5].enabled = true;
        ChipHammerHL.SetActive(true);
        ChippingHammer.SetChippingHammerParams(count);
        ChippingHammer.AssignMethodOnSlagRemoveDone(SlagRemoved);
    }

    public void SlagRemoved()
    {
       // readSteps.onClickConfirmbtn();
       // readSteps.AddClickConfirmbtnEvent(PrepareJobforRemovingSlag);
        ToolsHighlight[5].enabled = false;
        ToolsHighlight[0].enabled = true;
        WireBrushHL2.SetActive(true);
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(CleanedWeldLines);

    }



    public void CleanedWeldLines()
    {
        int count = 0;
        for (int i = 0; i < SlagLines.Length; i++)
        {
            Transform line = SlagLines[i].transform;
            line.gameObject.SetActive(true);
            for (int j = 0; j < line.childCount; j++)
            {
                count++;
                line.GetChild(j).GetComponent<MeshRenderer>().sharedMaterial = WeldFinishMat;
                line.GetChild(j).GetComponent<CapsuleCollider>().enabled = false;

            }
        }
        WorkPiece.GetComponent<XRGrabInteractable>().enabled = true;
        WorkPiece.GetComponent<Rigidbody>().isKinematic = false;
        WorkPiece.GetComponent<Rigidbody>().useGravity = true;

        ToolsHighlight[0].enabled = false;
        finishPanel.SetActive(true);
        readSteps.tablet.SetActive(true);
        readSteps.panel.SetActive(false);
    }

    public void RestBrushPos()
    {
        ResetToolPosition(WireBrushGo.transform, ToolsOrigin["WireBrush"]);
    }

    public void ResetChippingHammerPos()
    {
        ResetToolPosition(ChippingHammerGo.transform, ToolsOrigin["ChippingHammer"]);
    }
    #endregion












}
