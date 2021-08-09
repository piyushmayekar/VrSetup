using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GasJointweldingManager : MonoBehaviour
{
    public static GasJointweldingManager instance;
    [Header("outLine Object to HighLight")]
    public Outline[] objectOutLines;

    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;

    [Header("Table uper gas kit")]
    public Collider[] GasTableObjectcolliders;

    [Header("Canvas ")]
    public GameObject finishPanel;

    [Header("Read step from json calss")]
    public ReadStepsAndVideoManager readSteps;

    [Header("Extra objects")]
    public GameObject neturalFlameCube;
    public GameObject netural_flame, jointTackPoint, hummerhighlight, blacksmoke,
                      highlighttriSquare, torch35D, torch_m_35d, lighterFlame, supportPlat, supportCube1, supportcube2,jobSupport1,jobSupport2;
    public GameObject[] weldingLine1, weldingLine2;
    [Header("          ")]
    public RotateNozzle redBol, blueBol, blackValve, redValve;

    // [Header("Counters")]
    int countppekit;
    public bool flameOff = false, step10Call, isTurnOffFlame, IsEnableFlame;
    [Header("Object Position Resetter ")]
    public Transform[] toolToReset;
    public List<Vector3> toolToResetPosition, toolToResetRotate;
    [Header("xr hand Object")]
    public GameObject leftHand, RightHand;
    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = false;
            ppekitcolliders[i].GetComponent<Outline>().enabled = false;
        }
        for (int i = 0; i < objectOutLines.Length; i++)
        {
            objectOutLines[i].enabled = false;
        }
        for (int i = 0; i < GasTableObjectcolliders.Length; i++)
        {
            GasTableObjectcolliders[i].enabled = false;
        }
        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmSatrtbtn);
        readSteps.confirmbtn.gameObject.SetActive(true);
        for (int i = 0; i < toolToReset.Length; i++)
        {
            toolToResetPosition.Add(toolToReset[i].localPosition);
            toolToResetRotate.Add(toolToReset[i].localEulerAngles);
        }
    //    Onclickbtn_s14_confirm();
    }
    public void ConfirmSatrtbtn()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_1_confirm);
    }
    #region step1 Step 1: Wear PPE Kit
    void Onclickbtn_s_1_confirm()
    {
        readSteps.HideConifmBnt();
        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = true;
            ppekitcolliders[i].GetComponent<Outline>().enabled = true;
        }
    }

    public void CheckStep1(GameObject selectObject)
    {
        countppekit++;
        if (countppekit >= ppekitcolliders.Length)
        {
            OnEnableStep2object();
        }
        selectObject.SetActive(false);

    }
    public void OnEnableStep2object() //get all ppe kit then this call
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_2_confirm);
    }
    #endregion
    #region  Step2 : Clean the job surface with wire brush and remove burrs by filing.
    void Onclickbtn_s_2_confirm()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;// job plate material

        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        objectOutLines[9].enabled = true;
        GasTableObjectcolliders[1].enabled = true;// brush collider
    }

    public void checkBrushStep()
    {
        objectOutLines[9].enabled = false;
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false; // job plate outline
                                                                                                  //  GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);
    }
    #endregion

    #region Step 3:  Keep the job on welding table in “T” Position.
    void Onclickbtn_s_3_confirm()
    {
        SetObjectRestPos_Rotate(0); //Brush tool
        readSteps.HideConifmBnt();
        OnEnableStep3object();
    }
    void OnEnableStep3object()
    {

        GasTableObjectcolliders[0].enabled = true;
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;// job plate material
        objectOutLines[1].gameObject.SetActive(true);// job flat  object position
                                                     //  objectOutLines[1].enabled = true;// job flat position
        objectOutLines[1].GetComponent<BoxCollider>().enabled = true;
    }
    public void PlaceJobPlate()
    {
        //job plat posion set
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.4475f, 0.0112f, -0.1002f);//objectOutLines[1].transform.position;// job plate material
                                                                                                      //     GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 180f,0f);//objectOutLines[1].transform.position;// job plate material
        GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 0f, 0f);//objectOutLines[1].transform.position;// job plate material

        objectOutLines[1].gameObject.SetActive(false);// job flat position      
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false;// job plate material
        objectOutLines[1].enabled = false;// job flat position

    }
    public void CheckJobFlatPlace()
    {

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(SetUpTrolley.instance.Onclickbtn_s_3_confirm);

    }

    #endregion
    #region Step 10Open acetylene control valve and light the flame with spark lighter.

    public void Onclickbtn_s10_confirm()
    {
        onEnableStep10Object();
        readSteps.HideConifmBnt();
    }
    public void EndMethodAudio()
    {
        //EXP Open acetylene control valve lighter  audio  clip (4)
        //PlayStepAudio(4);

    }

    //carburing flame
    public void OpenFlameRedBol()
    {
        if (!isTurnOffFlame)
        {
            objectOutLines[8].enabled = true;
            GasTableObjectcolliders[2].enabled = true;
            GasTableObjectcolliders[2].GetComponent<Outline>().enabled = true;
            GasTableObjectcolliders[2].GetComponent<SnapGrabbleObject>().enabled = true;
        }
    }
    public void LighterSnap_true()
    {
        objectOutLines[8].enabled = false;
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
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s10_2_confirm);

        GasTableObjectcolliders[2].GetComponent<Outline>().enabled = false;
    }
    void Onclickbtn_s10_2_confirm()
    {
        readSteps.HideConifmBnt();
        openOxgenValve();

    }
    public void onDoneBlueBol_2_oxygen()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s9_confirm);
    }
    void openOxgenValve()
    {
        GasTableObjectcolliders[9].GetComponent<RotateNozzle>().enabled = true; //blue 2  bol reduse or crbarn

        GasTableObjectcolliders[9].enabled = true;
        GasTableObjectcolliders[9].GetComponent<Outline>().enabled = true;
    }
    //netural flame
    //netural flame with oxygen cutting flame
    //oxidizing flame
    void onEnableStep10Object()
    {
        redBol.enabled = true; //RED  bol reduse or crbarn
        redBol.GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[3].enabled = true;

    }
    #endregion


    #region Step 9: Do tack welding on both ends and centre of the job.
    void Onclickbtn_s9_confirm()
    {
        flameOff = true;
        jointTackPoint.SetActive(true);
        neturalFlameCube.SetActive(true);
        step10Call = true;

        SetObjectRestPos_Rotate(1); //lighter tool

        GasTableObjectcolliders[8].enabled = true;
        objectOutLines[10].GetComponent<Outline>().enabled = true;       
      

        supportCube1.SetActive(false);
        supportcube2.SetActive(false);
        RotateJobPlate_FirstSide();

        readSteps.HideConifmBnt();
    }

    public void CheckTackPoint()
    {
        ResetRotateJobPlate();

       
        // GasTableObjectcolliders[8].enabled = false;
        objectOutLines[10].GetComponent<Outline>().enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s10__confirm);

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
        SetObjectRestPos_Rotate(2); //filler  tool

        readSteps.HideConifmBnt();
        highlighttriSquare.SetActive(true);

        objectOutLines[5].enabled = true;
        objectOutLines[6].enabled = true;
    }
    public void checkTriSquare()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s11_confirm);
        highlighttriSquare.SetActive(false);

        objectOutLines[5].enabled = false;
        objectOutLines[6].enabled = false;

    }

    #endregion
    #region step 11 : Start Welding by Leftward Technique.
    void Onclickbtn_s11_confirm()
    {
        SetObjectRestPos_Rotate(3); //try squre  tool

        readSteps.HideConifmBnt();

        weldingLine1[0].SetActive(true);
        weldingLine1[0].transform.GetChild(0).gameObject.SetActive(true);

        supportCube1.SetActive(false);
        supportcube2.SetActive(false);
        RotateJobPlate_FirstSide();

          GasTableObjectcolliders[8].enabled = true;
        objectOutLines[10].GetComponent<Outline>().enabled = true;
        // JointWelding.instance.WeldingEnable();
        torch35D.SetActive(true);
    }
    public void RotateJobPlate_SecondSide()
    {
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.4709f, -0.0044f, -0.0978f);//objectOutLines[1].transform.position;// job plate material
        GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0, -90, 10);
        jobSupport1.SetActive(false);
        jobSupport2.SetActive(true);
    }
    public void RotateJobPlate_FirstSide()
    {
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.4209f, -0.0036f, -0.0978f);//objectOutLines[1].transform.position;// job plate material
        GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0,90, -10);
        jobSupport1.SetActive(true);
        jobSupport2.SetActive(false);
    }
    public void ResetRotateJobPlate()
    {   //job plat posion set
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.4475f, 0.0112f, -0.1002f);//objectOutLines[1].transform.position;// job plate material
                                                                                                      //     GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 180f,0f);//objectOutLines[1].transform.position;// job plate material
        GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 0f, 0f);//objectOutLines[1].transform.position;// job plate material

        supportCube1.SetActive(true);
        supportcube2.SetActive(true);
        jobSupport1.SetActive(false);
        jobSupport2.SetActive(false);
    }
    public void weldingComplete()
    {
        ResetRotateJobPlate();    

        objectOutLines[10].GetComponent<Outline>().enabled = false;
        GasTableObjectcolliders[8].GetComponent<XRGrabInteractable>().selectEntered = null;
        GasTableObjectcolliders[8].GetComponent<XRGrabInteractable>().selectExited = null;
        //  Destroy(  GasTableObjectcolliders[8].GetComponent<FreezeRotation>());
        GasTableObjectcolliders[5].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GasTableObjectcolliders[5].GetComponent<CustomXRGrabInteractable>().trackRotation = true;

        leftHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        RightHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_12_1_confirm);

    }
    public Transform tempTransform;
    public Quaternion temprotation;
    public bool temptrue;
    public GameObject rightTempobject, handPresistRight;
    public void CheckWelding_rot(GameObject weldingTorch)
    {

        weldingTorch.SetActive(false);
        //  GasTableObjectcolliders[5].GetComponent<FreezeRotation>().Freezeangle = weldingTorch.transform;

        // GasTableObjectcolliders[5].GetComponent<FreezeRotation>().isFreeze = true;

        Vector3 pos = GasTableObjectcolliders[5].transform.localPosition;
        pos = weldingTorch.transform.localPosition;

        GasTableObjectcolliders[5].transform.localPosition = pos;

        // GasTableObjectcolliders[5].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;

        GasTableObjectcolliders[5].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        GasTableObjectcolliders[5].transform.localEulerAngles = weldingTorch.transform.localEulerAngles;

        neturalFlameCube.GetComponent<JointWelding>().isWelding = true;
    }
    #endregion

    private void Update()
    {

    }
    #region Step 12.1: Turning off Flame
    public GameObject dummyRedBol;
    void Onclickbtn_s_12_1_confirm()
    {
        SetObjectRestPos_Rotate(2); //filler  tool

        GasTableObjectcolliders[8].transform.GetChild(2).tag = "Untagged";
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
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[6].enabled = true;// blue valve nozzel

        objectOutLines[2].enabled = true;// blue valve nozzel

        blackValve.enabled = true;                                // black valve
        blackValve.isclockwise = true;                           // black valve
        blackValve.otherMeterobject.SetActive(false);             // black valve
        blackValve.otherMeterobject = ZeroMeterBlack;               // black valve
        blackValve.MeterObject.SetActive(true);                   // black valve
        blackValve.RotateValue = 15;                              // black valve
                                                                  //    blackValve.speed = 
        SetUpTrolley.instance.blackRotatespriteOn.SetActive(false);
        SetUpTrolley.instance.blackRotatespriteOff.SetActive(true);
    }
    public void callBlackValve_Bol()
    {
        if (isTurnOffFlame)
        {
            GasTableObjectcolliders[6].enabled = false;// blue valve nozzel
                                                       //   GasTableObjectcolliders[9].GetComponent<Outline>().enabled = true;// blue valve nozzel
            objectOutLines[2].enabled = false;// blue regulator

            GasTableObjectcolliders[7].enabled = true;// red valve nozzel
                                                      //   GasTableObjectcolliders[8].GetComponent<Outline>().enabled = false;// blue valve nozzel
            objectOutLines[3].enabled = true;// red regulator

            redValve.enabled = true;                          // red valve
            redValve.isclockwise = true;  // red valve
            redValve.otherMeterobject.SetActive(false);       // red valve
            redValve.otherMeterobject = ZeroMeterred;       // red valve
            redValve.MeterObject.SetActive(true);             // red valve
            redValve.RotateValue = 10;                         // red valve

            SetUpTrolley.instance.redRotateSpriteOn.SetActive(false);
            SetUpTrolley.instance.redRotateSpriteOff.SetActive(true);
        }
    }
    public void CallRedValve_bol()
    {
        if (isTurnOffFlame)
        {
            objectOutLines[3].enabled = false;// red regulator
            GasTableObjectcolliders[7].enabled = false;// blue valve nozzel
            GasTableObjectcolliders[7].GetComponent<Outline>().enabled = false;// blue valve nozzel

            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s13_confirm);
        }
    }

    #endregion


    #region step 13 Clean the job surface with wire brush and remove distortion.
    void Onclickbtn_s13_confirm()
    {
        readSteps.HideConifmBnt();
        objectOutLines[9].enabled = true;
        CuttingBrush.instance.cleanPointCount = 15;
        CuttingBrush.instance.isStop = false;
        //   Debug.Log("Call bursh out line");
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        netural_flame.SetActive(false);
    }
    public void cleanBrushFinish()
    {
        objectOutLines[9].enabled = false;
        GasTableObjectcolliders[1].enabled = true;
        Debug.Log("Call bursh out line");
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        objectOutLines[9].enabled = true;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s14_confirm);

    }
    #endregion
    #region step 14 Check the defects.
    void Onclickbtn_s14_confirm()
    {
        SetObjectRestPos_Rotate(0); //brush tool

        hummerhighlight.SetActive(true);
        objectOutLines[4].enabled = true;
        objectOutLines[4].transform.parent.GetComponent<JointWelding>().isWelding = true;
        objectOutLines[4].transform.parent.GetComponent<JointWelding>().isFiller = true;
        for (int i = 0; i < weldingLine1.Length; i++)
        {
            weldingLine1[i].SetActive(false);
            weldingLine2[i].SetActive(true);
        }
        weldingLine2[0].transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
        weldingLine2[0].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
        objectOutLines[9].enabled = false;
        readSteps.HideConifmBnt();
    }
    public void checkChappingHummer()
    {
        finishPanel.SetActive(true);
        readSteps.tablet.SetActive(true);
        readSteps.panel.SetActive(false);
        objectOutLines[4].enabled = false;
        SetObjectRestPos_Rotate(4); //chapping hummer tool
        Debug.Log("call end");
    }

    #endregion
    #region Others methods
    public void SetObjectRestPos_Rotate(int indexOfReset)
    {
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = false;
        toolToReset[indexOfReset].transform.localPosition = toolToResetPosition[indexOfReset];
        toolToReset[indexOfReset].transform.localEulerAngles = toolToResetRotate[indexOfReset];
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = true;
    }
    #endregion
}