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
        WireBrushHL, WeldingTorchHL, FillerRodHL, ChippingHammerHL,socketFlat1_job,socketFlat2_job, lastScale;

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
        //EnableMarkingOnFirstJobSecongLine();
      ///  PlaceJobFlatePos1_socket1_job();
      //  PlaceJobFlatePos2_socket2_job();
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
        Debug.Log("call grab"+ selectGameObject.name);
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
        Debug.Log("2");
        ToolsOutlines[2].enabled = true;
        Scriber.SetScriberMarkingParams(WorkPiece1, SteelRuler1_AattachedToJob1, 0,ScriberHL);
        Scriber.AssignMethodOnMarkingDone(EnableMarkingOnFirstJobSecongLine);
    } 

    public void EnableMarkingOnFirstJobSecongLine()
    {
        Debug.Log("!");
        Scriber.SetScriberMarkingParams(WorkPiece1, SteelRuler2_AattachedToJob1, 1);
        Scriber.AssignMethodOnMarkingDone(EnableMarkingOnSecondJobFirstLine);
    }

    public void EnableMarkingOnSecondJobFirstLine()
    {
        Debug.Log("3");
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
        lastScale.SetActive(true);
        Debug.Log("call last scale");
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

        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = true; //2
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled =true;

        socketFlat1_job.SetActive(true);
        socketFlat1_job.transform.GetChild(0).GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(PlaceJobFlatePos1_socket1_job);

        BenchWise.handle.CanMove = false;
        BenchWise.handle.SetHighDefaultMat();

        Debug.Log("Cutting Done");
    }
    public void PlaceJobFlatePos1_socket1_job()
    {
        socketFlat1_job.SetActive(false);

        WorkPiece1.GetChild(1).GetComponent<Outline>().enabled = false; 
        WorkPiece1.GetComponent<XRGrabInteractable>().enabled = false;
        WorkPiece1.transform.localPosition = socketFlat1_job.transform.localPosition;
        WorkPiece1.transform.localRotation = socketFlat1_job.transform.localRotation;

        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = true;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = true;
        socketFlat2_job.SetActive(true);
        socketFlat2_job.transform.GetChild(0).GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(PlaceJobFlatePos2_socket2_job);

    }
    public void PlaceJobFlatePos2_socket2_job()
    {
        WorkPiece2.GetComponent<XRGrabInteractable>().enabled = false;
        WorkPiece2.GetChild(1).GetComponent<Outline>().enabled = false;
        WorkPiece2.transform.localPosition = socketFlat2_job.transform.localPosition;
        WorkPiece2.transform.localRotation = socketFlat2_job.transform.localRotation;
        socketFlat2_job.SetActive(false);
        readSteps.onClickConfirmbtn();
        //readSteps.AddClickConfirmbtnEvent(Onclickbtn_s7_confirm);
        readSteps.AddClickConfirmbtnEvent(GasWeldingSetUP.instance.Onclickbtn_s_3_confirm);

    }
    public void callGasSetupEnd()
    {
        Debug.Log("Called the end");
    }
    public Collider[] GasTableObjectcolliders;
    public GameObject blacksmoke, lighterFlame, jointTackPoint, neturalFlameCube, netural_flame;
    public GameObject highlighttriSquare, hummerhighlight;
    public GameObject[] weldingLine1, weldingLine2;
    public RotateNozzle redBol, blueBol, blackValve, redValve;
    bool isTurnOffFlame, IsEnableFlame, flameOff, step10Call;
    #region Step 10Open acetylene control valve and light the flame with spark lighter.
    public void Onclickbtn_s10_confirm()
    {
        onEnableStep10Object();
        readSteps.HideConifmBnt();
    }

    //carburing flame
    public void OpenFlameRedBol()
    {
        if (!isTurnOffFlame)
        {
            ToolsOutlines[8].enabled = true;
            GasTableObjectcolliders[0].enabled = true;
            GasTableObjectcolliders[0].GetComponent<Outline>().enabled = true;
            GasTableObjectcolliders[0].GetComponent<SnapGrabbleObject>().enabled = true;
        }

    }
    public void LighterSnap_true()
    {
        ToolsOutlines[8].enabled = false;
        StartCoroutine(lighterEnable());
    }
    IEnumerator lighterEnable()
    {
        blacksmoke.SetActive(true); // black smoke
        yield return new WaitForSeconds(0.5f);
        blacksmoke.SetActive(false); // black smoke

        IsEnableFlame = true;
        lighterFlame.SetActive(true);
        lighterFlame.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s9_confirm);

        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = false;
    }
    //netural flame
    //netural flame with oxygen cutting flame
    //oxidizing flame
    void onEnableStep10Object()
    {
        redBol.enabled = true; //RED  bol reduse or crbarn
        redBol.GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[1].enabled = true;

    }
    #endregion
    #region Step 9: Do tack welding on both ends and centre of the job.
    void Onclickbtn_s9_confirm()
    {
        flameOff = true;
        jointTackPoint.SetActive(true);
        neturalFlameCube.SetActive(true);
        step10Call = true;

     //   SetObjectRestPos_Rotate(1); //lighter tool

        GasTableObjectcolliders[2].enabled = true;
        ToolsOutlines[9].GetComponent<Outline>().enabled = true;

        readSteps.HideConifmBnt();
    }

    public void CheckTackPoint()
    {
   //     supportPlat.SetActive(false);
        // GasTableObjectcolliders[8].enabled = false;
        ToolsOutlines[9].GetComponent<Outline>().enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s10__confirm);

        //   Onclickbtn_s8_0_confirm();
    }
    public void OnPressLever()
    {
        if (step10Call)
        {
            netural_flame.SetActive(true);
            netural_flame.GetComponent<AudioSource>().Play();
        }
        else
        {
            if (IsEnableFlame)
            {
                lighterFlame.SetActive(true);
                lighterFlame.GetComponent<AudioSource>().Play();
            }
        }
    }
    #endregion

    #region step10:  With the help of tri square, check the alignment of the job and clean the tag weld.
    void Onclickbtn_s10__confirm()
    {
      //  SetObjectRestPos_Rotate(2); //filler  tool

        readSteps.HideConifmBnt();
        highlighttriSquare.SetActive(true);

        ToolsOutlines[10].enabled = true;
        //ToolsOutlines[6].enabled = true;
    }
    public void checkTriSquare()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s11_confirm);
        highlighttriSquare.SetActive(false);

        ToolsOutlines[10].enabled = false;
        //ToolsOutlines[6].enabled = false;
        //   Onclickbtn_s8_confirm();
    }

    #endregion
    #region step 11 : Start Welding by Leftward Technique.
    void Onclickbtn_s11_confirm()
    {
       // SetObjectRestPos_Rotate(3); //try squre  tool

        readSteps.HideConifmBnt();

        weldingLine1[0].SetActive(true);
        weldingLine1[0].transform.GetChild(0).gameObject.SetActive(true);

       // GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.4209f, -0.0036f, -0.0978f);//ToolsOutlines[1].transform.position;// job plate material
      //  GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0, 0, -10);
       // supportCube1.SetActive(true);
      //  supportcube2.SetActive(false);

        GasTableObjectcolliders[2].enabled = true;
        ToolsOutlines[9].GetComponent<Outline>().enabled = true;
        // JointWelding.instance.WeldingEnable();
    //    torch35D.SetActive(true);
    }
    public void SecondTourchPlateRotate()
    {
      /*  GasTableObjectcolliders[5].GetComponent<FreezeRotation>().isFreeze = false;

        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.4709f, -0.0044f, -0.0978f);//ToolsOutlines[1].transform.position;// job plate material
        GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0, 0, 10);
        supportCube1.SetActive(false);
        supportcube2.SetActive(true);*/
    }
    public void weldingComplete()
    {
        GasTableObjectcolliders[5].GetComponent<FreezeRotation>().isFreeze = false;
        //  GasTableObjectcolliders[8].GetComponent<FreezeRotation>().isFreeze = false;
        //job plat posion set
        /*GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.4475f, 0.0112f, -0.1002f);//ToolsOutlines[1].transform.position;// job plate material
                                                                                                      //     GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 180f,0f);//ToolsOutlines[1].transform.position;// job plate material
        GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 0f, 0f);//ToolsOutlines[1].transform.position;// job plate material

        supportCube1.SetActive(true);
        supportcube2.SetActive(true);*/

        ToolsOutlines[9].GetComponent<Outline>().enabled = false;
        GasTableObjectcolliders[2].GetComponent<XRGrabInteractable>().selectEntered = null;
        GasTableObjectcolliders[2].GetComponent<XRGrabInteractable>().selectExited = null;
        //  Destroy(  GasTableObjectcolliders[8].GetComponent<FreezeRotation>());

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_12_1_confirm);

        //       Onclickbtn_s9_confirm();
    }
    public void CheckWelding_rot(GameObject weldingTorch)
    {

        weldingTorch.SetActive(false);
        GasTableObjectcolliders[5].GetComponent<FreezeRotation>().Freezeangle = weldingTorch.transform;

        GasTableObjectcolliders[5].GetComponent<FreezeRotation>().isFreeze = true;
        // FreezeRotation.instance.isFreeze = true;
        neturalFlameCube.GetComponent<JointWelding>().isWelding = true;
    }
    #endregion

    #region Step 12.1: Turning off Flame
    public GameObject dummyRedBol;
    void Onclickbtn_s_12_1_confirm()
    {
        //SetObjectRestPos_Rotate(2); //filler  tool

        Debug.Log("Call end flame");
        readSteps.HideConifmBnt();
        redBol.gameObject.SetActive(false);
        dummyRedBol.gameObject.SetActive(true);
        redBol = dummyRedBol.GetComponent<RotateNozzle>();

        isTurnOffFlame = true;
        SetUpTrolley.instance.isTurnOffFlame = true;
        redBol.enabled = true; //RED  bol reduse or crbarn
        redBol.RotateValue = 30; //RED  bol reduse or crbarn
                                 // redBol.speed = 25;
        redBol.transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        redBol.OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        redBol.isclockwise = false; //RED  bol reduse or crbarn

        redBol.GetComponent<Outline>().enabled = true;

    }
    public void callTurnOff2_flame()//oxidiz
    {
        if (isTurnOffFlame)
        {
            redBol.GetComponent<BoxCollider>().enabled = false;
            redBol.enabled = false; //RED  bol reduse or crbarn
            GasTableObjectcolliders[4].enabled = true;  //GREEN  bol oxidizing

            blueBol.enabled = true; //GREEN  bol oxidizing
            blueBol.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
            blueBol.OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
            blueBol.RotateValue = 30; //GREEN  bol oxidizing
            blueBol.isclockwise = false; //GREEN  bol oxidizing

            blueBol.GetComponent<Outline>().enabled = true;
        }
    }
    public void callFalmeOff()
    {
        if (isTurnOffFlame)
        {
            step10Call = false;
            netural_flame.SetActive(false);
            IsEnableFlame = false;
            lighterFlame.SetActive(false);
            blueBol.GetComponent<Outline>().enabled = false;
            callCloseCylinderValves();

        }
    }
    //close valve
    public void callCloseCylinderValves()
    {

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_12_2_1confirm);

    }
    public GameObject ZeroMeterred, ZeroMeterBlack;
    void Onclickbtn_s_12_2_1confirm()
    {
        Debug.Log("10");
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[1].enabled = true;// blue valve nozzel

       ToolsOutlines[13].enabled = true;// blue valve nozzel
      /* WorkPiece1
            workPiece2*/

        blackValve.enabled = true;                                // black valve
        blackValve.isclockwise = false;                           // black valve
        blackValve.otherMeterobject.SetActive(false);             // black valve
        blackValve.otherMeterobject = ZeroMeterBlack;               // black valve
        blackValve.MeterObject.SetActive(true);                   // black valve
        blackValve.RotateValue = 15;                              // black valve
                                                                  //    blackValve.speed = 20;
    }
    public void callBlackValve_Bol()
    {
        if (isTurnOffFlame)
        {
            Debug.Log("10eer");
            GasTableObjectcolliders[1].enabled = false;// blue valve nozzel
                                                       //   GasTableObjectcolliders[9].GetComponent<Outline>().enabled = true;// blue valve nozzel
            ToolsOutlines[13].enabled = false;// blue regulator

            GasTableObjectcolliders[4].enabled = true;// red valve nozzel
                                                      //   GasTableObjectcolliders[8].GetComponent<Outline>().enabled = false;// blue valve nozzel
            ToolsOutlines[11].enabled = true;// red regulator

            redValve.enabled = true;                          // red valve
            redValve.isclockwise = false;  // red valve
            redValve.otherMeterobject.SetActive(false);       // red valve
            redValve.otherMeterobject = ZeroMeterred;       // red valve
            redValve.MeterObject.SetActive(true);             // red valve
            redValve.RotateValue = 10;                         // red valve
        }
    }
    public void CallRedValve_bol()
    {
        if (isTurnOffFlame)
        {
            ToolsOutlines[11].enabled = false;// red regulator
            GasTableObjectcolliders[4].enabled = false;// blue valve nozzel
            GasTableObjectcolliders[4].GetComponent<Outline>().enabled = false;// blue valve nozzel
            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s13_confirm);
        }
    }

    #endregion


    #region step 13 Clean the job surface with wire brush and remove distortion.
    void Onclickbtn_s13_confirm()
    {
        readSteps.HideConifmBnt();
        ToolsOutlines[0].enabled = true;
        /*CuttingBrush.instance.cleanPointCount = 15;
        CuttingBrush.instance.isStop = false;*/

        WorkPiece1.GetChild(0).GetComponent<Outline>().enabled = true;
        WorkPiece2.GetChild(0).GetComponent<Outline>().enabled = true;


        //   Debug.Log("Call bursh out line");
        //   GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        netural_flame.SetActive(false);

        WireBrushHL.SetActive(true);
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(cleanBrushFinish);
    }
    public void cleanBrushFinish()
    {
        ToolsOutlines[0].enabled = false;

    

        ToolsOutlines[0].enabled = true;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s14_confirm);

        ///     Onclickbtn_s10_confirm();

    }
    #endregion
    #region step 14 Check the defects.
    void Onclickbtn_s14_confirm()
    {
     //   SetObjectRestPos_Rotate(0); //brush tool

        hummerhighlight.SetActive(true);
        ToolsOutlines[12].enabled = true;
        ToolsOutlines[12].transform.parent.GetComponent<JointWelding>().isWelding = true;
        ToolsOutlines[12].transform.parent.GetComponent<JointWelding>().isFiller = true;
        for (int i = 0; i < weldingLine1.Length; i++)
        {
            weldingLine1[i].SetActive(false);
            weldingLine2[i].SetActive(true);
        }
        weldingLine2[0].transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
        weldingLine2[0].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
        ToolsOutlines[0].enabled = false;
        readSteps.HideConifmBnt();
    }
    public void checkChappingHummer()
    {
        finishPanel.SetActive(true);
        readSteps.tablet.SetActive(true);
        readSteps.panel.SetActive(false);
        ToolsOutlines[12].enabled = false;
      //  SetObjectRestPos_Rotate(4); //chapping hummer tool
        Debug.Log("call end");
    }

    #endregion
   
}
