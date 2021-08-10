using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using PiyushUtils;

public class GasCuttingManager : MonoBehaviour
{
    public static GasCuttingManager instance;

    [Header("outLine Object to HighLight")]
    public Outline[] objectOutLines;
    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;

    [Header("Table uper gas kit")]
    public Collider[] GasTableObjectcolliders;
    [Header("Canvas ")]
    public GameObject finishPanel;

    [Header("Extra objects")]
    public GameObject neturalFlameCube;
    public GameObject netural_flame, lighterFlame;
    public GameObject torch90degree, blacksmoke;
    [Header("          ")]
    public RotateNozzle redBol, blueBol, blackValve, redValve;


    [Header("Read step from json calss")]
    public ReadStepsAndVideoManager readSteps;

    int countppekit;

    public bool flameOff = false;

    [Header("Object Position Resetter ")]
    public Transform[] toolToReset;

    public List<Vector3> toolToResetPosition, toolToResetRotate;
    public void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;

    }
    public void Start()
    {
        //Hide all the collider and outline
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
     //   CheckJobFlatPlace();
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
    void OnEnableStep2object() //get all ppe kit then this call
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_2_confirm);
    }
    #endregion
    #region Step 2: Keep raw material ready as per the given drawing.
    void Onclickbtn_s_2_confirm()
    {
        readSteps.HideConifmBnt();
        CuttingJobMaterial.instance.StartScriberMarking();
    }
    public void CheckJobPlace()
    {

        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false;// job plate material
        objectOutLines[0].enabled = false; // job plate mateial place position

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);
    }

    #endregion
    #region Step 3: Keep the job on the Cutting table in flat position.
    void Onclickbtn_s_3_confirm()
    {
        SetObjectRestPos_Rotate(1);// Scribal tool
        readSteps.HideConifmBnt();
        OnEnableStep3object();
    }
    void OnEnableStep3object()
    {
        GasTableObjectcolliders[0].enabled = true;
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;// job plate material
        objectOutLines[1].gameObject.SetActive(true);// job flat  object position
        objectOutLines[1].enabled = true;// job flat position
        objectOutLines[1].GetComponent<BoxCollider>().enabled = true;
    }
    public void CheckJobFlatPlace()
    {
        GasTableObjectcolliders[0].enabled = false;
        //job plat position set
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.338f, 0.016f, -0.094f);//objectOutLines[1].transform.position;// job plate material
                                                                                                   //     GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 180f,0f);//objectOutLines[1].transform.position;// job plate material
        GasTableObjectcolliders[0].transform.localEulerAngles = new Vector3(0f, 0f, 0f);//objectOutLines[1].transform.position;// job plate material

        objectOutLines[1].gameObject.SetActive(false);// job flat position      
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false;// job plate material
        objectOutLines[1].enabled = false;// job flat position

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm);

    }
    #endregion
    #region Step 4: Clean the job surface with wire brush and remove burrs by filing.
    void OnEnableStep4object()
    {
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        GasTableObjectcolliders[1].enabled = true;// brush collider
        objectOutLines[2].enabled = true;
    }

    void Onclickbtn_s_4_confirm()
    {
        readSteps.HideConifmBnt();
        OnEnableStep4object();
    }
    public void checkBrushStep()
    {
        objectOutLines[2].enabled = false;
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false; // job plate outline
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_confirm);
    }
    #endregion
    #region Step 5: As per the drawing, the surfaces of the job should be marked and punched.
    public void MarkingPointTwoEnable()
    {
        GasTableObjectcolliders[2].enabled = true;
    }
    void OnEnableStep5object()
    {
        CuttingJobMaterial.instance.StartCenterPunchMarking();
        objectOutLines[3].enabled = true;
        objectOutLines[4].enabled = true;
    }
    void Onclickbtn_s_5_confirm()
    {
        SetObjectRestPos_Rotate(0); //Brush tool
        OnEnableStep5object();
        readSteps.HideConifmBnt();
    }
    public void checkStep5()
    {
        readSteps.onClickConfirmbtn();
        // SetUpTrolley.instance.PlayCrackingKeyAudio();
        readSteps.AddClickConfirmbtnEvent(SetUpTrolley.instance.Onclickbtn_s_3_confirm);
        readSteps.confirmbtn.onClick.AddListener(() => checkCenterPunchHummer());
        objectOutLines[3].enabled = false;
        objectOutLines[4].enabled = false;
    }
    void checkCenterPunchHummer()
    {
        SetObjectRestPos_Rotate(2); //centar punch tool
        SetObjectRestPos_Rotate(3); //Hummar tool
    }
    #endregion    
    #region Step 10 Open acetylene control valve and light the flame with spark lighter.
    public bool IsEnableFlame, step10Call;
    void TempTorchStep()
    {
        GasTableObjectcolliders[6].enabled = true;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s10_confirm);
    }

    public void Onclickbtn_s10_confirm()
    {
          onEnableStep10Object();
        readSteps.HideConifmBnt();
    }    //netural flame
    //netural flame with oxygen cutting flame
    //oxidizing flame
    void onEnableStep10Object()
    {
        redBol.enabled = true; //RED  bol reduse or crbarn
        redBol.GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[4].enabled = true;
    }
    public void EndMethodAudio()
    {
        //EXP Open acetylene control valve lighter  audio  clip (6)
        //PlayStepAudio(6);
    }
    //carburing flame
    public void OpenFlameRedBol()
    {
        if (!isTurnOffFlame)
        {
            objectOutLines[5].enabled = true;
            GasTableObjectcolliders[3].enabled = true; //Lighter
            GasTableObjectcolliders[3].GetComponent<Outline>().enabled = true;
            GasTableObjectcolliders[3].GetComponent<SnapGrabbleObject>().enabled = true;
        }
    }

    public void LighterSnap_true()
    {
        objectOutLines[5].enabled = false;
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
        GasTableObjectcolliders[3].GetComponent<Outline>().enabled = false;
        SetObjectRestPos_Rotate(4); //lighter tool

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s10_2_confirm);
     //   Onclickbtn_s10_2_confirm();
    }
    //Open oxygen control valve 
    void Onclickbtn_s10_2_confirm()
    {
        //Debug.Log("clickConfirm ");
        readSteps.HideConifmBnt();
        openOxgenValve();

    }
    void openOxgenValve()
    {
        GasTableObjectcolliders[9].GetComponent<RotateNozzle>().enabled = true; //blue 2  bol reduse or crbarn
        GasTableObjectcolliders[9].enabled = true;
        GasTableObjectcolliders[9].GetComponent<Outline>().enabled = true;
    }
    public void onDoneBlueBol_2_oxygen()
    {
        Destroy(GasTableObjectcolliders[9].GetComponent<RotateNozzle>());
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s11_confirm);
    //    Onclickbtn_s11_confirm();
    }

    #endregion
    #region Step 11: Keep the gas cutting torch (blow pipe) at 90° on job surface and the cutting line.
    void Onclickbtn_s11_confirm()
    {
        onEnableStep11Object();
        readSteps.HideConifmBnt();
    }
    void onEnableStep11Object()
    {
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;

        torch90degree.SetActive(true);
    }
    public void Checktourch90degree()
    {
        torch90degree.SetActive(false);
        Vector3 pos = GasTableObjectcolliders[6].transform.localPosition;
        pos = torch90degree.transform.localPosition;
        GasTableObjectcolliders[6].transform.localPosition = pos;


        GasTableObjectcolliders[6].transform.localEulerAngles = torch90degree.transform.localEulerAngles;
        GasTableObjectcolliders[6].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        GasTableObjectcolliders[6].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        GasTableObjectcolliders[6].GetComponent<CustomXRGrabInteractable>().trackRotation = false;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s12_confirm);
    }
    #endregion
    #region Step 12: Heat one end of the marking line till it turns cherry red.Keep a distance of 5mm between the job and the nozzle.
    void Onclickbtn_s12_confirm()
    {
        GasTableObjectcolliders[6].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GasTableObjectcolliders[6].GetComponent<CustomXRGrabInteractable>().trackRotation = true;

        readSteps.HideConifmBnt();
        onEnableStep12Object();
    }
    void onEnableStep12Object()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s13_confirm);
    }

    #endregion
    #region Step 13: Press oxygen lever and slowly proceed in the direction of cutting
    void Onclickbtn_s13_confirm()
    {
        objectOutLines[6].enabled = true;
        step10Call = true;
        onEnableStep13Object();
        readSteps.HideConifmBnt();
    }

    void onEnableStep13Object()
    {
        neturalFlameCube.SetActive(true);
        flameOff = true;
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
    public void CheckCuttingLine()
    {
        if (flameOff)
        {
            objectOutLines[6].enabled = false;
            GasTableObjectcolliders[6].GetComponent<FreezeRotation>().isFreeze = false;
            CuttingBrush.instance.cleanPointCount = 15;
            CuttingBrush.instance.isStop = false;

            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_14_1_confirm);
        }
    }
    #endregion

    #region Step 14: Turning off Flame
    public GameObject dummyRedBol;
    void Onclickbtn_s_14_1_confirm()
    {
        Debug.Log("Call end flame");
        readSteps.HideConifmBnt();
        redBol.gameObject.SetActive(false);
        dummyRedBol.gameObject.SetActive(true);
        redBol = dummyRedBol.GetComponent<RotateNozzle>();

        isTurnOffFlame = true;
        SetUpTrolley.instance.isTurnOffFlame = true;
        redBol.enabled = true; //RED  bol reduse or crbarn
        redBol.RotateValue = 40; //RED  bol reduse or crbarn
        redBol.transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        redBol.OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        redBol.isclockwise = true; //RED  bol reduse or crbarn

        redBol.GetComponent<Outline>().enabled = true;

    }
    public void callTurnOff2_flame()//oxidiz
    {
        if (isTurnOffFlame)
        {
            redBol.GetComponent<BoxCollider>().enabled = false;
            redBol.enabled = false; //RED  bol reduse or crbarn
            GasTableObjectcolliders[5].enabled = true;  //GREEN  bol oxidizing

           // blueBol.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
           // blueBol.OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
        blueBol.RotateValue = 310; //GREEN  bol oxidizing
            blueBol.enabled = true; //GREEN  bol oxidizing
            blueBol.isclockwise = true; //GREEN  bol oxidizing

            blueBol.GetComponent<Outline>().enabled = true;
        }
    }
    public void callFalmeOff()
    {
        if (isTurnOffFlame)
        {
            IsEnableFlame = false;
            lighterFlame.SetActive(false);
            step10Call = false;
            netural_flame.SetActive(false);
            blueBol.GetComponent<Outline>().enabled = false;

            callCloseCylinderValves();

        }
    }
    //close valve
    public void callCloseCylinderValves()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_10_1confirm);

    }
    public GameObject ZeroMeterred, ZeroMeterBlack;
    void Onclickbtn_s_10_1confirm()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[7].enabled = true;// blue valve nozzel

        objectOutLines[9].enabled = true;// black regulator

        blackValve.isclockwise = true;                           // black valve
        blackValve.enabled = true;                                // black valve
        blackValve.otherMeterobject.SetActive(false);             // black valve
        blackValve.otherMeterobject = ZeroMeterBlack;               // black valve
        blackValve.MeterObject.SetActive(true);                   // black valve
        blackValve.RotateValue = 15;                              // black valve
                                                                  //   blackValve.speed = 20;

        SetUpTrolley.instance.blackRotatespriteOn.SetActive(false);
        SetUpTrolley.instance.blackRotatespriteOff.SetActive(true);
    }
    public void callBlackValve_Bol()
    {
        if (isTurnOffFlame)
        {
            Debug.Log("10eer");
            GasTableObjectcolliders[7].enabled = false;// blue valve nozzel
                                                       //   GasTableObjectcolliders[9].GetComponent<Outline>().enabled = true;// blue valve nozzel
            objectOutLines[9].enabled = false;// blue regulator

            GasTableObjectcolliders[8].enabled = true;// blue valve nozzel
                                                      //   GasTableObjectcolliders[8].GetComponent<Outline>().enabled = false;// blue valve nozzel
            objectOutLines[10].enabled = true;// red regulator

            redValve.isclockwise = true;  // red valve
            redValve.enabled = true;                          // red valve
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
            objectOutLines[10].enabled = false;// red regulator

            GasTableObjectcolliders[8].enabled = false;// blue valve nozzel
            GasTableObjectcolliders[8].GetComponent<Outline>().enabled = false;// blue valve nozzel

            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s14_confirm);
        }
    }

    public bool isTurnOffFlame;
    #endregion


    #region Step 15: Pick up C.S. brush and clean the surface
    void Onclickbtn_s14_confirm()
    {
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = false;
        onEnableStep14Object();
        readSteps.HideConifmBnt();
    }
    void onEnableStep14Object()
    {
        GasTableObjectcolliders[1].enabled = true;
        Debug.Log("Call bursh out line");
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        objectOutLines[2].enabled = true;

    }
    public void cleanBrushFinish()
    {
        objectOutLines[2].enabled = false;
        finishPanel.SetActive(true);
        readSteps.panel.SetActive(false);
    }
    #endregion

    #region Others methods
    /* void //PlayStepAudio(int index)
     {
         AudioManagerWithLanguage.Instance.//PlayStepAudio(index);
     }*/
    public void SetObjectRestPos_Rotate(int indexOfReset)
    {
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = false;
        toolToReset[indexOfReset].transform.localPosition = toolToResetPosition[indexOfReset];
        toolToReset[indexOfReset].transform.localEulerAngles = toolToResetRotate[indexOfReset];
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = true;
    }

    #endregion
}