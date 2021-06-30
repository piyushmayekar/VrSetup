using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public ReadStepsFromJson readSteps;

    [Header("Extra objects")]
    public GameObject redPipeRop;
    public GameObject  bluePipeRop, ParentBluePipEndPoint, ParentRedPipeEndPoint, neturalFlameCube;
    public GameObject netural_flame, BlueRotatesprite, redRotateSprite, jointTackPoint, hummerhighlight, 
                      highlighttriSquare,torch35D,torch_m_35d,lighterFlame, supportPlat;
    public GameObject[] weldingLine1, weldingLine2;
    [Header("          ")]
    public GameObject HL_T_connectorRed;
    public GameObject HL_T_ClipRed, HL_T_connectorBlack, HL_T_ClipBlack, blacksmoke;
    [Header("          ")]
    public RotateNozzle redBol, blueBol, blackValve, redValve;

   // [Header("Counters")]
     int countppekit;
    public bool flameOff = false;
    public bool isPipeRedConnect, isPipeblueConnect;
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
    ///    Onclickbtn_s_2_confirm();
      //  Onclickbtn_s10_confirm();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPipeblueConnect)
        {
            bluePipeRop.transform.parent = ParentBluePipEndPoint.gameObject.transform; // new 22
            bluePipeRop.transform.localPosition = Vector3.zero;
            bluePipeRop.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        if (isPipeRedConnect)
        {
            redPipeRop.transform.parent = ParentRedPipeEndPoint.gameObject.transform;//red pipe sphere welding Tourch   //new 22
            redPipeRop.transform.localPosition = Vector3.zero;
            redPipeRop.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void ConfirmSatrtbtn()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_1_confirm);
        PlayStepAudio(3);// kit audio
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
        PlayStepAudio(4);//       
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
        Debug.Log("call brush");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);

        // Onclickbtn_s_4_confirm();
    }



    #endregion

    #region Step 3:  Keep the job on welding table in “T” Position.
    void Onclickbtn_s_3_confirm()
    {
        readSteps.HideConifmBnt();
        OnEnableStep3object();
    }
     void OnEnableStep3object()
    {
        /*GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        objectOutLines[9].enabled = true;
        GasTableObjectcolliders[1].enabled = true;// brush collider*/

        GasTableObjectcolliders[0].enabled = true;
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;// job plate material
        objectOutLines[1].gameObject.SetActive(true);// job flat  object position
                                                     //  objectOutLines[1].enabled = true;// job flat position
        objectOutLines[1].GetComponent<BoxCollider>().enabled = true;
    }
    public void PlaceJobPlate()
    {
        //job plat posion set
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-0.854f, 0.009f, 0.1383f);//objectOutLines[1].transform.position;// job plate material
        objectOutLines[1].gameObject.SetActive(false);// job flat position      
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false;// job plate material
        objectOutLines[1].enabled = false;// job flat position

    }
    public void CheckJobFlatPlace()
    {

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s4_confirm);
        PlayStepAudio(4);// 

   //     Onclickbtn_s_3_confirm();
    }
   
    #endregion

    #region Step 4:Fix hose connector and hose clip on Cutting torch and Hoses pipe to Gas Cutting torch.
    void Onclickbtn_s4_confirm()
    {
        onEnableStep4Object();
        readSteps.HideConifmBnt();
    }
    void onEnableStep4Object()
    {
        GasTableObjectcolliders[4].enabled = true; // cutting welding tourch
        GasTableObjectcolliders[4].GetComponent<Outline>().enabled = true;

        //connector of welding torch black
        GasTableObjectcolliders[13].enabled = true;
        GasTableObjectcolliders[13].GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[13].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorBlack.SetActive(true);
    }
    public void DoneConnector_T_Black()
    {
        //connector of welding torch red
        GasTableObjectcolliders[12].enabled = true;
        GasTableObjectcolliders[12].GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[12].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorRed.SetActive(true);
    }
    public void DoneConnector_T_Red()
    {
        GasTableObjectcolliders[5].enabled = true;// blue pipe sphere at tourch
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = true; // blue hose pipe end position capsule collider
        bluePipeRop.GetComponent<Outline>().enabled = true;// blue hose pipe end position out linecapsule collider
        GasTableObjectcolliders[5].GetComponent<Outline>().enabled = true;// blue pipe sphere outline
    }
    public void BluePipeConnectATTorch() // blue pipe obejct hide and red pipe object true
    {
        // Debug.Log("Red final");
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = false; // blue hose pipe end position capsule collider
        bluePipeRop.GetComponent<Outline>().enabled = false;// blue hose pipe end position out linecapsule collider
        GasTableObjectcolliders[5].GetComponent<Outline>().enabled = false;// blue pipe sphere outline
                                                                           // call at update
        bluePipeRop.GetComponent<SnapGrabbleObject>().enabled = false;

        bluePipeRop.transform.parent = ParentBluePipEndPoint.gameObject.transform;

        bluePipeRop.transform.localPosition = Vector3.zero;
        bluePipeRop.transform.localRotation = Quaternion.Euler(Vector3.zero);
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = false;
        isPipeblueConnect = true;
        //red pipe object 
        GasTableObjectcolliders[6].enabled = true;// red pipe sphere at tourch
        redPipeRop.GetComponent<CapsuleCollider>().enabled = true; // red hose pipe end position capsule collider
        redPipeRop.GetComponent<Outline>().enabled = true;// red hose pipe end position out linecapsule collider
        GasTableObjectcolliders[6].GetComponent<Outline>().enabled = true;// red pipe sphere outline
    }
    public void RedPipeConnectAtTorch()//  red pipe object hide
    {
        //red pipe object 
        GasTableObjectcolliders[6].enabled = false;// red pipe sphere at tourch
        redPipeRop.GetComponent<CapsuleCollider>().enabled = false; // red hose pipe end position capsule collider
        redPipeRop.GetComponent<Outline>().enabled = false;// red hose pipe end position out linecapsule collider
        GasTableObjectcolliders[6].GetComponent<Outline>().enabled = false;// red pipe sphere outline
        // call at update
        redPipeRop.GetComponent<SnapGrabbleObject>().enabled = false;
        redPipeRop.transform.parent = ParentBluePipEndPoint.gameObject.transform;

        redPipeRop.transform.localPosition = Vector3.zero;
        redPipeRop.transform.localRotation = Quaternion.Euler(Vector3.zero);
        redPipeRop.GetComponent<CapsuleCollider>().enabled = false;
        isPipeRedConnect = true;
        GasTableObjectcolliders[4].GetComponent<Outline>().enabled = false;
        //clip of torch red
        GasTableObjectcolliders[14].enabled = true;
        GasTableObjectcolliders[14].GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[14].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_ClipBlack.SetActive(true);
    }
    public void DoneBlackClip_T()
    {
        //clip of torch red
        GasTableObjectcolliders[15].enabled = true;
        GasTableObjectcolliders[15].GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[15].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_ClipRed.SetActive(true);
    }
    public void DoneRedClip_T()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s5_1_confirm);
    }
    #endregion
    #region Step 5_p:  set the pressure of oxygen at 1.6 kg/cm2 and for acetylene set it at 0.15 kg/cm2.
    public bool isTurnOffFlame;
    void Onclickbtn_s5_1_confirm()
    {
        onEnableStep5_1Object();
        readSteps.HideConifmBnt();
    }
    void onEnableStep5_1Object()
    {
        // enable blue Nozzel objects
        GasTableObjectcolliders[8].enabled = true;// blue valve nozzel
        blackValve.enabled = true;
        objectOutLines[2].enabled = true; // blue reguletor
        BlueRotatesprite.SetActive(true);

    }
    public void enableRedNozzelvalveObjects()
    {
        if (!isTurnOffFlame)
        {
            objectOutLines[2].enabled = false; // blue reguletor
            GasTableObjectcolliders[9].enabled = true;// red valve nozzel
            objectOutLines[3].enabled = true; // red reguletor
            redRotateSprite.SetActive(true);
            redValve.enabled = true;
            blackValve.enabled = false;
        }
    }
    public void Enablestep5_Water()
    {
        if (!isTurnOffFlame)
        {
            blackValve.enabled = false;
            objectOutLines[3].enabled = false; // red reguletor
            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_6_confirm_p2);
            Debug.Log("call water");
        }
    }
    #endregion

    #region step 6_p check leakage 

    void Onclickbtn_s_6_confirm_p2() //acetylene  shop water
    {
        GasTableObjectcolliders[2].enabled = true; //Glass object for red
        GasTableObjectcolliders[2].GetComponent<Outline>().enabled = true; //Glass object for red
        objectOutLines[3].enabled = true;//regulator red
        readSteps.HideConifmBnt();
    }

    public void Done_acetylene_shop_water() //oxygen  shop water
    {
        //  Debug.Log("call 6 ");
        GasTableObjectcolliders[2].enabled = false; //Glass object for red
        GasTableObjectcolliders[2].GetComponent<Outline>().enabled = false; //Glass object for red
        objectOutLines[3].enabled = false;//regulator red

        GasTableObjectcolliders[3].enabled = true; //Glass object for black
        GasTableObjectcolliders[3].GetComponent<Outline>().enabled = true; //Glass object for black
        objectOutLines[2].enabled = true; //regulator black
    }
    public void Done_oxygen_shop_water() //oxygen  shop water
    {
        //  Debug.Log("call 6 ");
        GasTableObjectcolliders[3].enabled = false; //Glass object for black
        GasTableObjectcolliders[3].GetComponent<Outline>().enabled = false; //Glass object for black
        objectOutLines[2].enabled = false;//regulator black
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s7_confirm);
    }

    #endregion
    #region Step 7_p: Show Fixing orifice nozzle of 1.2 mm on gas cutting torch(cutting blow pipe).
    void Onclickbtn_s7_confirm()
    {
        onEnableStep7Object();
        readSteps.HideConifmBnt();
    }
    void onEnableStep7Object()
    {
        GasTableObjectcolliders[7].enabled = true;
        GasTableObjectcolliders[7].GetComponent<Outline>().enabled = true;
    }
    public void CheckNozzelConnected()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s8_confirm);
    }
    #endregion


    #region Step 8  Open acetylene control valve and light the flame with spark lighter.
    public bool IsEnableFlame, step10Call;
    void Onclickbtn_s8_confirm()
    {
        onEnableStep8Object();
        readSteps.HideConifmBnt();
    }

    //carburing flame
    public void OpenFlameRedBol()
    {
        if (!isTurnOffFlame)
        {
            objectOutLines[8].enabled = true;
            GasTableObjectcolliders[18].enabled = true;
            GasTableObjectcolliders[18].GetComponent<Outline>().enabled = true;
            GasTableObjectcolliders[18].GetComponent<SnapGrabbleObject>().enabled = true;
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
        lighterFlame.SetActive(false);
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s9_confirm);
    }
    //netural flame
    //netural flame with oxygen cutting flame
    //oxidizing flame
    void onEnableStep8Object()
    {
        redBol.enabled = true; //RED  bol reduse or crbarn
        redBol.GetComponent<Outline>().enabled = true;
        GasTableObjectcolliders[17].enabled = true;

    }
    #endregion

    #region Step 9: Do tack welding on both ends and centre of the job.
     void Onclickbtn_s9_confirm()
    {
        flameOff = true;
        jointTackPoint.SetActive(true);
        neturalFlameCube.SetActive(true);
        step10Call = true;
        readSteps.HideConifmBnt();
    }
    public void EnableWeldingFlame()
    {
        if (flameOff)
        {
            netural_flame.SetActive(true);
        }
    }
    public void CheckTackPoint()
    {
        supportPlat.SetActive(false);
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
    }
    #endregion

    #region step10:  With the help of tri square, check the alignment of the job and clean the tag weld.
    void Onclickbtn_s10__confirm()
    {
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
        //   Onclickbtn_s8_confirm();
    }

    #endregion
    #region step 11 : Start Welding by Leftward Technique.
     void Onclickbtn_s11_confirm()
    {
        readSteps.HideConifmBnt();
        weldingLine1[0].SetActive(true);
        weldingLine1[0].transform.GetChild(0).gameObject.SetActive(true);
       
        GasTableObjectcolliders[19].enabled = true;
        GasTableObjectcolliders[19].GetComponent<Outline>().enabled = true;
        // JointWelding.instance.WeldingEnable();
        torch35D.SetActive(true);
    }
    public void weldingComplete()
    {
        GasTableObjectcolliders[19].GetComponent<Outline>().enabled =false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_12_1_confirm);

//       Onclickbtn_s9_confirm();
    }
    public void CheckWelding_rot(GameObject weldingTorch)
    {
        weldingTorch.SetActive(false);
        FreezeRotation.instance.isFreeze = true;
        neturalFlameCube.GetComponent<JointWelding>().isWelding = true;
    }
    #endregion

    #region Step 12.1: Turning off Flame
    public GameObject dummyRedBol;
    void Onclickbtn_s_12_1_confirm()
    {
        Debug.Log("Call end flame");
        readSteps.HideConifmBnt();
        redBol.gameObject.SetActive(false);
        dummyRedBol.gameObject.SetActive(true);
        redBol = dummyRedBol.GetComponent<RotateNozzle>();

        isTurnOffFlame = true;
        redBol.enabled = true; //RED  bol reduse or crbarn
        redBol.RotateValue = 30; //RED  bol reduse or crbarn
        redBol.speed = 25;
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
            GasTableObjectcolliders[16].enabled = true;  //GREEN  bol oxidizing

            blueBol.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
            blueBol.OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
            blueBol.RotateValue = 30; //GREEN  bol oxidizing
            blueBol.speed = 25;
            blueBol.enabled = true; //GREEN  bol oxidizing
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
        GasTableObjectcolliders[8].enabled = true;// blue valve nozzel

        objectOutLines[2].enabled = true;// blue valve nozzel

        blackValve.enabled = true;                                // black valve
        blackValve.isclockwise = true;                           // black valve
        blackValve.otherMeterobject.SetActive(false);             // black valve
        blackValve.otherMeterobject = ZeroMeterred;               // black valve
        blackValve.MeterObject.SetActive(true);                   // black valve
        blackValve.RotateValue = 30;                              // black valve
        blackValve.speed = 20;
    }
    public void callBlackValve_Bol()
    {
        if (isTurnOffFlame)
        {
            Debug.Log("10eer");
            GasTableObjectcolliders[8].enabled = false;// blue valve nozzel
                                                       //   GasTableObjectcolliders[9].GetComponent<Outline>().enabled = true;// blue valve nozzel
            objectOutLines[2].enabled = false;// blue regulator

            GasTableObjectcolliders[9].enabled = true;// blue valve nozzel
                                                      //   GasTableObjectcolliders[8].GetComponent<Outline>().enabled = false;// blue valve nozzel
            objectOutLines[3].enabled = true;// red regulator

            redValve.isclockwise = false;  // red valve
            redValve.enabled = true;                          // red valve
            redValve.otherMeterobject.SetActive(false);       // red valve
            redValve.otherMeterobject = ZeroMeterBlack;       // red valve
            redValve.MeterObject.SetActive(true);             // red valve
            redValve.RotateValue = 10;                         // red valve
        }
    }
    public void CallRedValve_bol()
    {
        if (isTurnOffFlame)
        {
            objectOutLines[3].enabled = false;// red regulator
            GasTableObjectcolliders[9].enabled = false;// blue valve nozzel
            GasTableObjectcolliders[9].GetComponent<Outline>().enabled = false;// blue valve nozzel
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

   ///     Onclickbtn_s10_confirm();

    }
    #endregion
    #region step 14 Check the defects.
     void Onclickbtn_s14_confirm()
    {
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
        readSteps.panel.SetActive(false);
        objectOutLines[4].enabled = false;
        Debug.Log("call end");
    }

    #endregion
    #region Others methods
    void PlayStepAudio(int index)
    {
        /*if (stepAudioSource.clip != null)
        {
            stepAudioSource.Stop();
            stepAudioSource.PlayOneShot(stepsAudioClip[index]);
        }*/
    }
    #endregion
}
