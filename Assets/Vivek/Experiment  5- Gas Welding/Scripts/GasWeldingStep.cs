using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GasWeldingStep : MonoBehaviour
{
    public static GasWeldingStep instance;
    [Header("Canvas ")]
    public GameObject finishPanel;
    public GameObject setPressureRegulatorCanvas, LighterCanvas, lighter_Flame;
    public GameObject creacking_C_key1_canvas, creacking_C_key2_canvas;

    [Header("          ")]
    public GameObject ChainHighLight;
    public GameObject BluePipEndPoint, RedPipeEndPoint;
    public GameObject ParentBluePipEndPoint, ParentRedPipeEndPoint, nozzelSnapPoint;
    public GameObject RedRegulator, blackRegulator, redPipeRop, bluePipeRop, blacksmoke, oxidizing_F, reduce_or_carb_F, neturel_F;
    [Header("    HighLight Objects      ")]
    public GameObject HL_flashbackred;
    public GameObject HL_connectorRed, HL_R_ClipRed, HL_flashbackBlack, HL_connectorBlack,
                        HL_R_ClipBlack, HL_T_connectorRed, HL_T_ClipRed, HL_T_connectorBlack, HL_T_ClipBlack;
    [Header("     bol     ")]
    public GameObject step8Flame;
    public GameObject Step9flame, extraRedBol, oldRedBol;

    [Header("     Roatate vales at regulator     ")]
    public GameObject redRotateSpriteOn;
    public GameObject BlueRotatespriteOn,redRotateSpriteOff, BlueRotatespriteOff;
    public GameObject snapGlassRed, snapGlassBlack;

    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;
    [Header("Table uper gas kit")]
    public Collider[] GasTablekitcolliders;
    [Header("outLine Object to HighLight")]
    public Outline[] objectOutLines;
    [Header("SnapGrabbleObject")]
    public SnapGrabbleObject chainStartSphere;
    public int countppekit, countCrackTab;

    public RotateNozzle[] rotateNozzles;
    public bool isPipeRedConnect, isPipeblueConnect;
    [Header("Read step from json calss")]
    public ReadStepsAndVideoManager readSteps;
    [Header("Steps audio clips")]
    public AudioSource stepAudioSource;
    public AudioClip creckykeyClip;

    public Vector3 blueStartpos, redStartpos;

    [Header("Object Position Resetter ")]
    public Transform[] toolToReset;
    public List<Vector3> toolToResetPosition, toolToResetRotate;
    public bool isRedCrecking, isBlackCrecking;
    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
       
        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = false;
            ppekitcolliders[i].GetComponent<Outline>().enabled = false;
        }
        for (int i = 0; i < GasTablekitcolliders.Length; i++)
        {
            GasTablekitcolliders[i].enabled = false;
        }
        for (int i = 0; i < objectOutLines.Length; i++)
        {
            objectOutLines[i].enabled = false;
        }

        for (int i = 0; i < rotateNozzles.Length; i++)
        {
            rotateNozzles[i].enabled = false;
        }

        nozzelSnapPoint.gameObject.SetActive(false);
        GasTablekitcolliders[11].GetComponent<SnapGrabbleObject>().enabled = false;
        GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = false;
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = false;
        redPipeRop.GetComponent<CapsuleCollider>().enabled = false;

        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmSatrtbtn);
        readSteps.confirmbtn.gameObject.SetActive(true);

    }
    public void Start()
    {
        for (int i = 0; i < toolToReset.Length; i++)
        {
            toolToResetPosition.Add(toolToReset[i].localPosition);
            toolToResetRotate.Add(toolToReset[i].localEulerAngles);
        }
       // Onclickbtn_s_9_confirm();
    }
  /*  IEnumerator PlayGasWeldingStartAudio()
    {
        yield return new WaitForSeconds(4f);
        //EXP title audio clip (0)
        //PlayStepAudio(0);
        Onclickbtn_s_1_confirm();
        yield return new WaitForSeconds(1f);
        ReadStepsAndVideoManager.instance.OnClickLanguagesBtn();
        OnEnableStep2object();
        yield return new WaitForSeconds(5f);
        Onclickbtn_s_2_confirm();
        yield return new WaitForSeconds(5f);
        OnEnableStep3object();
        yield return new WaitForSeconds(5f);
        Onclickbtn_s_3_confirm();
    }*/
    public void Update()
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

    void ConfirmSatrtbtn()
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
    #region step2 gas cylinder on the trolley with chain.
    void Onclickbtn_s_2_confirm()
    {
        ChainHighLight.SetActive(true);
        objectOutLines[17].enabled = true;
        chainStartSphere.enabled = true; // chain true chainStartSphere
        GasTablekitcolliders[0].enabled = true;
        readSteps.HideConifmBnt();
    }
    #endregion
    #region Step 3: Cracking of both the cylinder
    public void OnEnableStep3object()
    {
        objectOutLines[17].enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);
    }

    void Onclickbtn_s_3_confirm()
    {
        HighLightCylinderCrack();
        readSteps.HideConifmBnt();
    }
    void HighLightCylinderCrack()
    {
        objectOutLines[0].enabled = true;
        GasTablekitcolliders[1].enabled = true;//red crecking  cylinder key
        rotateNozzles[0].enabled = true; //red crecking  cylinder key
    }
    public void Onclickcreacking_C_key1_canvas_btn()
    {
        if (!isRedCrecking)
        {
            isRedCrecking = true;
            GasTablekitcolliders[1].enabled = true;//red crecking  cylinder key
            rotateNozzles[0].enabled = true; //red crecking  cylinder key
            rotateNozzles[0].isclockwise = true;
            stepAudioSource.PlayOneShot(creckykeyClip);
        }
        else
        {
            objectOutLines[0].enabled = false;
            objectOutLines[1].enabled = true;

            GasTablekitcolliders[2].enabled = true;//blue crecking cylinder key
            GasTablekitcolliders[1].gameObject.SetActive(false);

            rotateNozzles[1].enabled = true; //black crecking  cylinder key
        }
    }
    public void Onclickcreacking_C_key2_canvas_btn()
    {
        if (!isBlackCrecking)
        {
            isBlackCrecking = true;
            GasTablekitcolliders[2].enabled = true;//blue crecking cylinder key
            rotateNozzles[1].enabled = true; //black crecking  cylinder key
            rotateNozzles[1].isclockwise = true;
            stepAudioSource.PlayOneShot(creckykeyClip);
        }
        else
        {
            objectOutLines[1].enabled = false;
            objectOutLines[0].enabled = true;
            GasTablekitcolliders[2].gameObject.SetActive(false);
            Debug.Log("VideoComming");

            OnEnableStep4object();
        }
    }
    public void CheckCylinderCrack(GameObject go)
    {
        go.gameObject.GetComponent<Outline>().enabled = false;
        countCrackTab++;
        if (countCrackTab == 2)
        {

            OnEnableStep4object();
        }
        go.SetActive(false);
    }
    #endregion
    #region Step 4: Fix regulators on both the cylinders.
    void OnEnableStep4object()
    {
        readSteps.AddClickEventVideoPlay(0); // regulators video vlip animation

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm);
    }

    void Onclickbtn_s_4_confirm()
    {

        objectOutLines[21].enabled = true; //red cylinder key
        GasTablekitcolliders[3].enabled = true; // red gas regulators
        GasTablekitcolliders[3].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[3].GetComponent<SnapGrabbleObject>().enabled = true;//red gas regulators
        readSteps.HideConifmBnt();
    }
    public void onEnableStep_4_2_object()
    {
        objectOutLines[21].enabled = false; //red cylinder key
        objectOutLines[20].enabled = true; //red cylinder key
        objectOutLines[3].enabled = true; //blue cylinder key
        objectOutLines[2].enabled = false; //red cylinder key
        GasTablekitcolliders[17].enabled = true; // blue gas regulators
        GasTablekitcolliders[17].GetComponent<Outline>().enabled = true;
    }
    public void Onclickbtn_s_5_1_confirm()//Flashback Arrestor confirm
    {
        objectOutLines[20].enabled = false; //red cylinder key
        GasTablekitcolliders[21].enabled = true;
        GasTablekitcolliders[21].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[21].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_flashbackBlack.SetActive(true);
        readSteps.HideConifmBnt();
    }
    public void DoneFlashBlack()
    {
        GasTablekitcolliders[22].enabled = true;
        GasTablekitcolliders[22].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[22].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_flashbackred.SetActive(true);
    }
    public void DoneFlashRed()
    {
        readSteps.AddClickEventVideoPlay(2); // hose connector video vlip animation

        //black connector and outline
        GasTablekitcolliders[23].enabled = true;
        GasTablekitcolliders[23].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[23].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_connectorBlack.SetActive(true);
    }
    public void DoneConnector_R_red()
    {
        readSteps.videoPlayBtn.gameObject.SetActive(false);
        OnEnableStep5object();
    }
    public void DoneConnector_R_Black()
    {
        //black connector and outline
        GasTablekitcolliders[24].enabled = true;
        GasTablekitcolliders[24].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[24].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_connectorRed.SetActive(true);
    }

    public void CallRegulatorDone()
    {
        objectOutLines[20].enabled = false; //red cylinder key
        Debug.Log("call end  of regulator");
        readSteps.onClickConfirmbtn();

        readSteps.AddClickEventVideoPlay(1); // Flashback Arrestor video vlip animation

        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_1_confirm);
    }

    #endregion
    #region Step 5: Hose connection to regulator and Welding torch.
    public void OnEnableStep5object()
    {
        objectOutLines[5].enabled = false;

        objectOutLines[2].enabled = false; //blue cylinder key

        objectOutLines[3].enabled = false; //red cylinder key
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_confirm);
    }
    void Onclickbtn_s_5_confirm()
    {
        onEnableStep5Object_2();
        readSteps.HideConifmBnt();
    }
    void onEnableStep5Object_2()
    {
        GasTablekitcolliders[4].enabled = true; //blue hose pipe
        GasTablekitcolliders[4].GetComponent<SnapGrabbleObject>().enabled = true; // blue hose pipe
        GasTablekitcolliders[4].transform.GetChild(0).gameObject.SetActive(true);
        GasTablekitcolliders[4].transform.GetChild(0).transform.GetChild(0).GetComponent<Outline>().enabled = true;
        objectOutLines[5].enabled = true;//blue regulators outline
    }
    public void onEnableStep5Object_3()
    {
        GasTablekitcolliders[5].enabled = true;//red hose pipe
        GasTablekitcolliders[5].GetComponent<SnapGrabbleObject>().enabled = true; // red hose pipe
        GasTablekitcolliders[5].transform.GetChild(0).gameObject.SetActive(true);
        GasTablekitcolliders[5].transform.GetChild(0).transform.GetChild(0).GetComponent<Outline>().enabled = true;
        objectOutLines[4].enabled = true;   //red regulators

        objectOutLines[5].enabled = false;  //blue   regulators outline
        Debug.Log("Red turn tiyare");
    }

    void Onclickbtn_s_5_2_confirm()//hose clip confirm
    {
        //clip of regulators black
        GasTablekitcolliders[25].enabled = true;
        GasTablekitcolliders[25].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[25].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_R_ClipBlack.SetActive(true);
        //     Debug.Log("2222");
        readSteps.HideConifmBnt();
    }
    public void Onclickbtn_s_5_3_clip()

    {
        objectOutLines[4].enabled = false;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_2_confirm);
    }
    public void DoneBlackClip_R() //hose clip 
    {
        //clip of regulators red
        GasTablekitcolliders[26].enabled = true;
        GasTablekitcolliders[26].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[26].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_R_ClipRed.SetActive(true);
    }
    public void DoneRedClip_R()
    {
        EnableWeldingTouchCanvas();
    }
    void EnableWeldingTouchCanvas()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_part2_confirm);
    }
    void Onclickbtn_s_5_part2_confirm()
    {
        objectOutLines[5].enabled = false;
        objectOutLines[4].enabled = false;
        GasTablekitcolliders[6].enabled = true;//welding Tourch  
        objectOutLines[22].enabled = true;

        //connector of welding torch black
        GasTablekitcolliders[29].enabled = true;
        GasTablekitcolliders[29].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[29].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorBlack.SetActive(true);
        readSteps.HideConifmBnt();
    }
    public void DoneConnector_T_Black()
    {
        //connector of welding torch red
        GasTablekitcolliders[30].enabled = true;
        GasTablekitcolliders[30].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[30].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorRed.SetActive(true);
    }
    public void DoneConnector_T_Red()
    {
        GasTablekitcolliders[7].enabled = true;// blue pipe sphere welding Tourch 
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = true;
        objectOutLines[6].enabled = true;// blue pipe sphere outline
        objectOutLines[18].enabled = true;
    }
    public void OnConnecteblackPipe()
    {
        objectOutLines[6].enabled = false;// blue pipe sphere outline
        objectOutLines[18].enabled = false;

        objectOutLines[7].enabled = true;// red pipe sphere outline
        objectOutLines[19].enabled = true;

        GasTablekitcolliders[7].enabled = false; // blue pipe sphere welding Tourch 
        GasTablekitcolliders[8].enabled = true; // red pipe sphere welding Tourch 

        redPipeRop.GetComponent<CapsuleCollider>().enabled = true;

        BluePipEndPoint.GetComponent<SnapGrabbleObject>().enabled = false;

        BluePipEndPoint.transform.parent = ParentBluePipEndPoint.gameObject.transform;

        BluePipEndPoint.transform.localPosition = Vector3.zero;
        BluePipEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        BluePipEndPoint.GetComponent<CapsuleCollider>().enabled = false;
        BluePipEndPoint.transform.localScale = new Vector3(0.044504f, 0.03f, 0.03f);
        BluePipEndPoint.transform.GetChild(1).transform.localPosition = blueStartpos;//new Vector3(0, 600, 0);

        isPipeblueConnect = true;
    }
    public void OnConnecteRedPipe()
    {
        objectOutLines[7].enabled = false;// red pipe sphere outline
        objectOutLines[19].enabled = false;

        RedPipeEndPoint.GetComponent<SnapGrabbleObject>().enabled = false;
        objectOutLines[22].enabled = false;
        GasTablekitcolliders[8].enabled = false; // red pipe sphere welding Tourch 
        RedPipeEndPoint.transform.parent = ParentRedPipeEndPoint.gameObject.transform;//red pipe sphere welding Tourch 
        RedPipeEndPoint.transform.localPosition = Vector3.zero;
        RedPipeEndPoint.GetComponent<CapsuleCollider>().enabled = false;
        RedPipeEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        RedPipeEndPoint.transform.GetChild(1).transform.localPosition = redStartpos;// new Vector3(0, 13, 0);

        isPipeRedConnect = true;

        //clip of torch black
        GasTablekitcolliders[27].enabled = true;
        GasTablekitcolliders[27].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[27].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_ClipBlack.SetActive(true);
    }
    public void DoneBlackClip_T()
    {
        //clip of torch red
        GasTablekitcolliders[28].enabled = true;
        GasTablekitcolliders[28].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[28].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_ClipRed.SetActive(true);
    }
    public void DoneRedClip_T()
    {
        onEnableStep6Object();
    }
    #endregion
    #region Step 6: Set the gas pressure on the regulator as per nozzle size.
    void onEnableStep6Object()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_6_confirm);
    }
    void Onclickbtn_s_6_confirm()
    {
        setPressureRegulatorCanvas.SetActive(true);
        GasTablekitcolliders[9].enabled = true;// blue valve nozzel

        rotateNozzles[5].enabled = true;// blue valve nozzel
        objectOutLines[4].enabled = false;
        objectOutLines[9].enabled = true;
        readSteps.HideConifmBnt();
        BlueRotatespriteOn.SetActive(true);
    }
    public void OnEnableRedValeNozzel()
    {
        if (!isTurnOffFlame)
        {
            redRotateSpriteOn.SetActive(true);
            objectOutLines[9].enabled = false;
            objectOutLines[10].enabled = true;
            GasTablekitcolliders[10].enabled = true;// red  valve nozzel
                                                    //  GasTablekitcolliders[10].transform.GetChild(1).gameObject.SetActive(true);
            rotateNozzles[6].enabled = true;// red valve nozzel
        }
    }
    #endregion
    #region Step 7: Fix nozzle on Welding Torch.
    void onEnableStep_7_part2_object() // enable glass objects
    {
        Onclickbtn_s_7_confirm_p2();
        objectOutLines[3].enabled = false;
    }

    void Onclickbtn_s_7_confirm_p2() //acetylene  shop water
    {
        //    Debug.Log("call 4");
        objectOutLines[4].enabled = true;
        GasTablekitcolliders[15].enabled = true; //Glass object for red

        objectOutLines[12].enabled = true; // glass red outline
        readSteps.HideConifmBnt();
    }
    public void Done_acetylene_shop_water() //oxygen  shop water
    {
        //  Snap red glass object true
        GasTablekitcolliders[15].GetComponent<SnapGrabbleObject>().enabled = true; //Glass object for red
        snapGlassRed.SetActive(true);

    }
    public void onEnableStep_7_part3_object()
    {
        GasTablekitcolliders[15].GetComponent<SnapGrabbleObject>().enabled = false; //Glass object for red
        GasTablekitcolliders[15].enabled = false;
        //red glass reset
        GasTablekitcolliders[15].gameObject.SetActive(true);
        SetObjectRestPos_Rotate(0);

        //   Debug.Log("call 5");
        objectOutLines[12].enabled = false; // glass red outline

        objectOutLines[4].enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm_p3);
    }
    void Onclickbtn_s_4_confirm_p3() //oxygen  shop water
    {
        //  Debug.Log("call 6 ");
        objectOutLines[5].enabled = true;

        GasTablekitcolliders[16].enabled = true;//Glass object for blue
        objectOutLines[13].enabled = true; // glass object blue
        readSteps.HideConifmBnt();
    }
    public void Done_oxygen_shop_water() //oxygen  shop water
    {
        //  Snap black glass object true
        GasTablekitcolliders[16].GetComponent<SnapGrabbleObject>().enabled = true; //Glass object for black
        snapGlassBlack.SetActive(true);
    }

    public void onEnableStep7Object()
    {
        if (!isTurnOffFlame)
        {
            objectOutLines[10].enabled = false;
            setPressureRegulatorCanvas.SetActive(false);  //setPressureRegulator Canvas 
            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_7_confirm);
        }
    }
    void Onclickbtn_s_7_confirm()
    {
        onEnableStep_7_part2_object();
        readSteps.HideConifmBnt();
    }

    public void OnEnableNozzleObject()
    {
        //blue glass reset
        GasTablekitcolliders[16].gameObject.SetActive(true);
        GasTablekitcolliders[16].enabled = false;
        GasTablekitcolliders[16].GetComponent<SnapGrabbleObject>().enabled = false; //Glass object for black

        SetObjectRestPos_Rotate(1);

        objectOutLines[5].enabled = false;  //blue   regulators outline
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_7_3_confirm);

    }
    public void Onclickbtn_s_7_3_confirm()
    {
        readSteps.HideConifmBnt();
        Debug.Log("call Nozzel step");
        GasTablekitcolliders[11].enabled = true; //welding nozzle collider
        GasTablekitcolliders[11].GetComponent<SnapGrabbleObject>().enabled = true;//welding nozzle snapgrabble
        GasTablekitcolliders[11].GetComponent<Outline>().enabled = true; //welding nozzle collider
        objectOutLines[8].enabled = true;//red pipe sphere
        objectOutLines[11].enabled = true;
        nozzelSnapPoint.gameObject.SetActive(true);

    }

    public void onEnableStep8Object()
    {
        GasTablekitcolliders[11].GetComponent<Outline>().enabled = false; //welding nozzle collider

        objectOutLines[8].enabled = false;
        objectOutLines[11].enabled = false;

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_8_confirm);

    }

    #endregion
    #region Step 8: Open acetylene control valve about 1/4th and light the flame with spark lighter.
    void Onclickbtn_s_8_confirm()
    {
        GasTablekitcolliders[11].enabled = false;//welding nozzle collider  

        GasTablekitcolliders[31].enabled = true;
        GasTablekitcolliders[31].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[31].GetComponent<RotateNozzle>().enabled = true;
        readSteps.HideConifmBnt();
    }
    public void LighterSnap_true()
    {

        GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = false;//lighter outline

        GasTablekitcolliders[14].GetComponent<Outline>().enabled = false;//lighter outline
        LighterCanvas.SetActive(false);
        objectOutLines[8].enabled = false;
        GasTablekitcolliders[12].enabled = true; // red bol at gas tourch
        GasTablekitcolliders[12].GetComponent<Outline>().enabled = true;
        StartCoroutine(lighterEnable());
    }
    IEnumerator lighterEnable()
    {
        GasTablekitcolliders[14].enabled = false; // red bol at gas tourch

        GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = false;

        Destroy(GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>());
        SetObjectRestPos_Rotate(2);

        //GasTablekitcolliders[14].gameObject.SetActive(false); // red bol at gas tourch
        PlayFlamsParticle1();   //new 22
        yield return new WaitForSeconds(1.5f);
        PlayFlamsParticle2();  //new 22

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_8_2_confirm);
    }
    public void OnDoneAcetyleneControlRedBol()
    {
        GasTablekitcolliders[31].enabled = false;
        GasTablekitcolliders[31].GetComponent<Outline>().enabled = false;
        Destroy(GasTablekitcolliders[31].GetComponent<RotateNozzle>());

        GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = true;// lighter snap
        GasTablekitcolliders[14].enabled = true;//Light collider
        GasTablekitcolliders[14].GetComponent<Outline>().enabled = true;//lighter outline

        LighterCanvas.SetActive(true);
        objectOutLines[8].enabled = true;
    }
    void Onclickbtn_s_8_2_confirm()
    {
        readSteps.HideConifmBnt();
        GasTablekitcolliders[32].enabled = true;
        GasTablekitcolliders[32].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[32].GetComponent<RotateNozzle>().enabled = true;
    }
    public void OnDoneOxyzenControlBlueBol()
    {
        Destroy(GasTablekitcolliders[32].GetComponent<RotateNozzle>());
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_9_confirm);
    }
    void PlayFlamsParticle1()
    {
        blacksmoke.SetActive(true); // black smoke
        GasTablekitcolliders[12].GetComponent<Outline>().enabled = false;
    }
    void PlayFlamsParticle2()
    {
        blacksmoke.SetActive(false);
        reduce_or_carb_F.SetActive(true); // simple reduce
        reduce_or_carb_F.GetComponent<AudioSource>().Play();
        GasTablekitcolliders[13].GetComponent<Outline>().enabled = false;
    }
    #endregion
    #region Step 9: Flame Setting.
    void Onclickbtn_s_9_confirm()
    {
        readSteps.onClickConfirmbtn();

        reduce_or_carb_F.SetActive(false);
        step8Flame.SetActive(false);
        Step9flame.SetActive(true);
        GasTablekitcolliders[18].enabled = true;
        //GasTablekitcolliders[18].transform.parent.GetComponent<RotateNozzle>().enabled = true;
        rotateNozzles[2].enabled = true; //RED  bol reduse or crbarn

        objectOutLines[14].GetComponent<BoxCollider>().enabled = true;
        objectOutLines[15].GetComponent<BoxCollider>().enabled = false;
        objectOutLines[16].GetComponent<BoxCollider>().enabled = false;

        rotateNozzles[2].GetComponent<Outline>().enabled = true; //RED  bol  out linereduse or crbarn

        readSteps.HideConifmBnt();
    }
    public void PlayFlamsParticle1_Flam()//17   13  //first step flame  green
    {
        if (!isTurnOffFlame)
        {
            readSteps.onClickConfirmbtn();
            reduce_or_carb_F.SetActive(true);
            reduce_or_carb_F.GetComponent<AudioSource>().Play();
            GasTablekitcolliders[19].enabled = true;  //green
            rotateNozzles[3].enabled = true;//green   bol oxidize
            GasTablekitcolliders[18].enabled = false;

            rotateNozzles[2].GetComponent<Outline>().enabled = false; //RED  bol  out linereduse or crbarn 

            objectOutLines[14].GetComponent<BoxCollider>().enabled = false;
            objectOutLines[15].GetComponent<BoxCollider>().enabled = true;
            objectOutLines[16].GetComponent<BoxCollider>().enabled = false;

            rotateNozzles[3].GetComponent<Outline>().enabled = true; //green  bol  out linereduse or crbarn 

        }
    }
    public void PlayFlamsParticle2_Flam() //second step flame   red 
    {
        if (!isTurnOffFlame)
        {
            objectOutLines[14].GetComponent<BoxCollider>().enabled = false;
            objectOutLines[15].GetComponent<BoxCollider>().enabled = false;
            objectOutLines[16].GetComponent<BoxCollider>().enabled = true;

            readSteps.onClickConfirmbtn();
            oxidizing_F.SetActive(true);
            oxidizing_F.GetComponent<AudioSource>().Play();
            reduce_or_carb_F.SetActive(false);
            GasTablekitcolliders[19].enabled = false;

            extraRedBol.SetActive(true);
            oldRedBol.SetActive(false);

            rotateNozzles[3].GetComponent<Outline>().enabled = false; //green  bol  out linereduse or crbarn 

            GasTablekitcolliders[20].enabled = true;
            //GasTablekitcolliders[20].transform.parent.GetComponent<RotateNozzle>().enabled = true;
            rotateNozzles[4].enabled = true; // netural flame 
            rotateNozzles[4].GetComponent<Outline>().enabled = true; //red  bol  out line natural

        }
    }
    public void PlayFlamsParticle3_flam() // netural flame
    {
        if (!isTurnOffFlame)
        {
            GasTablekitcolliders[20].enabled = false;
            neturel_F.SetActive(true);
            neturel_F.GetComponent<AudioSource>().Play();
            oxidizing_F.SetActive(false);
            rotateNozzles[4].GetComponent<Outline>().enabled = false; //red  bol  out line natural

            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_10_confirm);
          
        }
    }
    #endregion
    #region Step 10: Turning off Flame
    void Onclickbtn_s_10_confirm()
    {
        Debug.Log("Call end flame");
        readSteps.HideConifmBnt();

        neturel_F.SetActive(false);
        reduce_or_carb_F.SetActive(true); // redus true  1
        reduce_or_carb_F.GetComponent<AudioSource>().Play();


        isTurnOffFlame = true;
        GasTablekitcolliders[18].enabled = true;

        rotateNozzles[2].enabled = true; //RED  bol reduse or crbarn
        rotateNozzles[2].RotateValue = 50; //RED  bol reduse or crbarn
        rotateNozzles[2].transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        rotateNozzles[2].OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //RED  bol reduse or crbarn
        rotateNozzles[2].isclockwise = true; //RED  bol reduse or crbarn
                                              //
        GasTablekitcolliders[18].transform.gameObject.SetActive(true);
        GasTablekitcolliders[20].transform.parent.gameObject.SetActive(false);
        GasTablekitcolliders[18].enabled = true;

        objectOutLines[14].enabled = true;
        objectOutLines[15].enabled = false;
        objectOutLines[23].enabled = true;
    }
    public void callTurnOff2_flame()//oxidiz
    {
        if (isTurnOffFlame)
        {
            reduce_or_carb_F.SetActive(false);
            oxidizing_F.SetActive(true); // redus true  1

            GasTablekitcolliders[18].enabled = false;
            oxidizing_F.GetComponent<AudioSource>().Play();
            objectOutLines[23].enabled = false;
            rotateNozzles[2].enabled = false; //RED  bol reduse or crbarn
            GasTablekitcolliders[19].enabled = true;  //GREEN  bol oxidizing
            GasTablekitcolliders[19].transform.gameObject.SetActive(true);  //GREEN  bol oxidizing
            rotateNozzles[3].transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
            rotateNozzles[3].OtherRotate.transform.localRotation = Quaternion.Euler(0, 0, 0); //GREEN  bol oxidizing
         
            rotateNozzles[3].RotateValue = 320; //GREEN  bol oxidizing
            rotateNozzles[3].speed = -20; //GREEN  bol oxidizing
            rotateNozzles[3].enabled = true; //GREEN  bol oxidizing
            rotateNozzles[3].isclockwise = false; //GREEN  bol oxidizing false

            GasTablekitcolliders[18].enabled = false;

            objectOutLines[24].enabled = true;
            objectOutLines[14].enabled = false;
            objectOutLines[15].enabled = true;

        }
    }
    public void callFalmeOff()
    {
        if (isTurnOffFlame)
        {
            objectOutLines[24].enabled = false;
            oxidizing_F.SetActive(false);
            neturel_F.SetActive(false);
            reduce_or_carb_F.SetActive(false);
            GasTablekitcolliders[19].enabled = false;
            objectOutLines[14].enabled = false;
            objectOutLines[15].enabled = false;
            rotateNozzles[3].enabled = false; //GREEN  bol oxidizing
                                              //    Onclickbtn_s_10_1confirm();
            callCloseCylinderValves();

        }
    }
    public void callCloseCylinderValves()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_10_1confirm);

    }
    public GameObject ZeroMeterred, ZeroMeterBlack;
    public void Onclickbtn_s_10_1confirm()
    {
        readSteps.HideConifmBnt();
        GasTablekitcolliders[9].enabled = true;                  // blue valve nozzel
        objectOutLines[9].enabled = true;

        objectOutLines[5].enabled = true;
        rotateNozzles[5].isclockwise = true;                      // blue valve nozzel
        rotateNozzles[5].enabled = true;                          // blue valve nozzel
        rotateNozzles[5].otherMeterobject.SetActive(false);       // blue valve nozzel
        rotateNozzles[5].otherMeterobject = ZeroMeterBlack;       // blue valve nozzel
        rotateNozzles[5].MeterObject.SetActive(true);             // blue valve nozzel
        rotateNozzles[5].RotateValue = 10;                        // blue valve nozzel

        BlueRotatespriteOn.SetActive(false);
        BlueRotatespriteOff.SetActive(true);
            
    }
    public void callBlackValve()
    {
        if (isTurnOffFlame)
        {
            GasTablekitcolliders[9].enabled = false;// blue valve nozzel
                                                    // GasTablekitcolliders[9].transform.GetChild(1).gameObject.SetActive(false);
            objectOutLines[5].enabled = false;
            objectOutLines[4].enabled = true;
            objectOutLines[9].enabled = false;
            GasTablekitcolliders[10].enabled = true;

            objectOutLines[10].enabled = true;

            rotateNozzles[6].isclockwise = true;                            // Red valve nozzel
            rotateNozzles[6].enabled = true;                                // Red valve nozzel
            rotateNozzles[6].otherMeterobject.SetActive(false);             // Red valve nozzel
            rotateNozzles[6].otherMeterobject = ZeroMeterred;               // Red valve nozzel
            rotateNozzles[6].MeterObject.SetActive(true);                   // Red valve nozzel
            rotateNozzles[6].RotateValue = 10;                              // Red valve nozzel

            redRotateSpriteOn.SetActive(false);
            redRotateSpriteOff.SetActive(true);
        }
    }
    public void CallRedValve()
    {
        if (isTurnOffFlame)
        {
            objectOutLines[4].enabled = false;
            readSteps.panel.SetActive(false);
            readSteps.tablet.SetActive(true);
            finishPanel.SetActive(true);
            rotateNozzles[6].enabled = false;
            GasTablekitcolliders[10].enabled = false;
        }
    }

    public bool isTurnOffFlame;
    #endregion
    #region FinishPanel
    public void OnRestartbtn()
    {
        finishPanel.SetActive(false);
        SceneManager.LoadScene("Gas Welding");
    }
    #endregion
    IEnumerator DealyEnableObject(float time, GameObject enableObject)
    {
        yield return new WaitForSeconds(time);
        enableObject.SetActive(true);
    }
    public void SetObjectRestPos_Rotate(int indexOfReset)
    {
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = false;
        toolToReset[indexOfReset].transform.localPosition = toolToResetPosition[indexOfReset];
        toolToReset[indexOfReset].transform.localEulerAngles = toolToResetRotate[indexOfReset];
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = true;
    }
}

