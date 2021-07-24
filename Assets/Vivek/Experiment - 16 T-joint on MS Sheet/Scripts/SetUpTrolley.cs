using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SetUpTrolley : MonoBehaviour
{
    public static SetUpTrolley instance;

    [Header("Table uper gas kit")]
    public Collider[] GasTablekitcolliders;
    [Header("outLine Object to HighLight")]
    public Outline[] objectOutLines;
    [Header("          ")]
    public RotateNozzle[] rotateNozzles;
    [Header("          ")]
    public GameObject bluePipEndPoint;
    public GameObject RedPipeEndPoint, ParentBluePipEndPoint, ParentRedPipeEndPoint, nozzelSnapPoint;
    public GameObject redPipeRop, bluePipeRop, blacksmoke, redRotateSprite, BlueRotatesprite;
    [Header("          ")]
    public GameObject HL_flashbackred;
    public GameObject HL_connectorRed, HL_R_ClipRed, HL_flashbackBlack, HL_connectorBlack,
                        HL_R_ClipBlack, HL_T_connectorRed, HL_T_ClipRed,
                        HL_T_connectorBlack, HL_T_ClipBlack;
    public bool isPipeRedConnect, isPipeblueConnect, isTurnOffFlame;
    [Header("Read step from json calss")]
    public ReadStepsFromJson readSteps;
    [Header("Steps audio clips")]
    public AudioSource stepAudioSource;
    public AudioClip[] stepsAudioClip;
    public AudioClip creckykeyClip;
    [Header("Step End Method ")]
    public UnityEvent CallEndMethod;
    int countCrackTab;
    [Header("Object Position Resetter ")]
    public Transform[] toolToReset;
    public List<Vector3> toolToResetPosition, toolToResetRotate;
    public Vector3 blueStartpos, redStartpos;

    public bool isRedCrecking, isBlackCrecking;
    public void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 0; i < toolToReset.Length; i++)
        {
            toolToResetPosition.Add(toolToReset[i].localPosition);
            toolToResetRotate.Add(toolToReset[i].localEulerAngles);
        }
      //  Onclickbtn_s_3_confirm();
    }
    public void Update()
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
    #region Step 3: Cracking of both the cylinder
     void OnEnableStep3object()
    {
       
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);
        PlayStepAudio(5);// Cracking audio
    }

  public void Onclickbtn_s_3_confirm()
    {
      
        HighLightCylinderCrack();
        readSteps.HideConifmBnt();
    }
    void HighLightCylinderCrack()
    {
        objectOutLines[0].enabled = true;
        GasTablekitcolliders[0].enabled = true;//red crecking  cylinder key
        rotateNozzles[0].enabled = true; //red crecking  cylinder key

    }
    public void Onclickcreacking_C_key1_canvas_btn()
    {
        if (!isRedCrecking)
        {
            isRedCrecking = true;
            GasTablekitcolliders[0].enabled = true;//red crecking  cylinder key
            rotateNozzles[0].enabled = true; //red crecking  cylinder key
            rotateNozzles[0].isclockwise = false;
            rotateNozzles[0].RotateValue = 20;

            stepAudioSource.PlayOneShot(creckykeyClip);
        }
        else
        {
            objectOutLines[0].enabled = false;
            objectOutLines[1].enabled = true;
            GasTablekitcolliders[1].enabled = true;//black crecking cylinder key
            rotateNozzles[1].enabled = true; //black crecking  cylinder key
        }
    }
    public void Onclickcreacking_C_key2_canvas_btn()
    {
        if (!isBlackCrecking)
        {
            isBlackCrecking = true;
            GasTablekitcolliders[1].enabled = true;//black crecking cylinder key
            rotateNozzles[1].enabled = true; //black crecking  cylinder key
            rotateNozzles[1].isclockwise = false; //black crecking  cylinder key
            rotateNozzles[1].RotateValue = 20;
            stepAudioSource.PlayOneShot(creckykeyClip);
        }
        else
        {
            objectOutLines[1].enabled = false;
            objectOutLines[0].enabled = false;

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
        PlayStepAudio(6);// regulators audio
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm);
    }

    void Onclickbtn_s_4_confirm()
    {
        objectOutLines[2].enabled = true; //red cylinder key attech
        GasTablekitcolliders[2].enabled = true; // red gas regulators
        GasTablekitcolliders[2].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[2].GetComponent<SnapGrabbleObject>().enabled = true;//red gas regulators
        readSteps.HideConifmBnt();

    }
    public void onEnableStep_4_2_object()
    {
        objectOutLines[2].enabled = false; //red cylinder key
        objectOutLines[3].enabled = true; //black cylinder key

        GasTablekitcolliders[3].enabled = true; // blue gas regulators
        GasTablekitcolliders[3].GetComponent<Outline>().enabled = true;
    }
    public void CallRegulatorDone()
    {
        objectOutLines[3].enabled = false; //black cylinder key
        objectOutLines[4].enabled = false; //red cylinder key
    //    Debug.Log("call end  of regulator");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_1_confirm);
    }

    void Onclickbtn_s_5_1_confirm()//Flashback Arrestor confirm
    {
        objectOutLines[3].enabled = false; //black cylinder key
        GasTablekitcolliders[5].enabled = true;// black flash beck
        GasTablekitcolliders[5].GetComponent<Outline>().enabled = true;// black flash beck
        GasTablekitcolliders[5].GetComponent<SnapGrabbleObject>().enabled = true;// black flash beck
        HL_flashbackBlack.SetActive(true);
        readSteps.HideConifmBnt();
    }
    public void DoneFlashBlack()
    {

        GasTablekitcolliders[4].enabled = true;// red flash beck
        GasTablekitcolliders[4].GetComponent<Outline>().enabled = true;// red flash beck
        GasTablekitcolliders[4].GetComponent<SnapGrabbleObject>().enabled = true;// red flash beck
        HL_flashbackred.SetActive(true);
    }
    public void DoneFlashRed()
    {
        //black connector and outline
        GasTablekitcolliders[6].enabled = true;
        GasTablekitcolliders[6].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[6].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_connectorBlack.SetActive(true);
    }
    public void DoneConnector_R_red()
    {
        OnEnableStep5object();

    }
    public void DoneConnector_R_Black()
    {
        //black connector and outline
        GasTablekitcolliders[7].enabled = true;
        GasTablekitcolliders[7].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[7].GetComponent<SnapGrabbleObject>().enabled = true;

        HL_connectorRed.SetActive(true);
    }
    #endregion
    #region Step 5: Hose connection to regulator and Welding torch.
     void OnEnableStep5object()
    {
      
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_confirm);
        PlayStepAudio(7);// hose pipe connection audio
    }
    void Onclickbtn_s_5_confirm()
    {
        onEnableStep5Object_2();
        readSteps.HideConifmBnt();
    }
    void onEnableStep5Object_2()
    {
        GasTablekitcolliders[10].enabled = true; //blue hose pipe
        GasTablekitcolliders[10].GetComponent<SnapGrabbleObject>().enabled = true; // blue hose pipe
        GasTablekitcolliders[10].transform.GetChild(0).gameObject.SetActive(true);
        GasTablekitcolliders[10].GetComponent<Outline>().enabled = true;
        objectOutLines[5].enabled = true;//blue regulators outline
    }
    public void onEnableStep5Object_3()
    {
        GasTablekitcolliders[11].enabled = true;//red hose pipe
        GasTablekitcolliders[11].GetComponent<SnapGrabbleObject>().enabled = true; // red hose pipe
        GasTablekitcolliders[11].transform.GetChild(0).gameObject.SetActive(true);
        GasTablekitcolliders[11].GetComponent<Outline>().enabled = true;
        objectOutLines[4].enabled = true;   //red regulators

        objectOutLines[5].enabled = false;  //blue   regulators outline
       // Debug.Log("Red turn tiyare");
    }

    void Onclickbtn_s_5_2_confirm()//hose clip confirm
    {
        //clip of regulators black
        GasTablekitcolliders[8].enabled = true;
        GasTablekitcolliders[8].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[8].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_R_ClipBlack.SetActive(true);
        //     Debug.Log("2222");
        readSteps.HideConifmBnt();
    }
    public void Onclickbtn_s_5_3_clip()

    {
        objectOutLines[4].enabled = false;
        //  Debug.Log("*");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_2_confirm);
        // readSteps.HideConifmBnt();

    }
    public void DoneBlackClip_R() //hose clip 
    {
        //clip of regulators red
        GasTablekitcolliders[9].enabled = true;
        GasTablekitcolliders[9].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[9].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_R_ClipRed.SetActive(true);

    }
    public void DoneRedClip_R()
    {
        //   Debug.Log("*342342");
        EnableWeldingTouchCanvas();
    }

    void EnableWeldingTouchCanvas()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_part2_confirm);
        PlayStepAudio(8);// tourch connection with pipe audio
    }
    void Onclickbtn_s_5_part2_confirm()
    {
        objectOutLines[5].enabled = false;
        objectOutLines[4].enabled = false;

        GasTablekitcolliders[12].enabled = true;//welding Tourch  
        objectOutLines[6].enabled = true;

        //connector of welding torch black
        GasTablekitcolliders[13].enabled = true;
        GasTablekitcolliders[13].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[13].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorBlack.SetActive(true);
        readSteps.HideConifmBnt();
    }
    public void DoneConnector_T_Black()
    {
        //connector of welding torch red
        GasTablekitcolliders[14].enabled = true;
        GasTablekitcolliders[14].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_connectorRed.SetActive(true);
    }
    public void DoneConnector_T_Red()
    {

        GasTablekitcolliders[17].enabled = true;// blue pipe sphere welding Tourch 
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = true;
        bluePipEndPoint.GetComponent<SnapGrabbleObject>().enabled = true;

        objectOutLines[7].enabled = true;// blue pipe sphere outline
        objectOutLines[9].enabled = true;// blue end capsule

    }
    public void OnConnecteblackPipe()
    {
        objectOutLines[7].enabled = false;// blue pipe sphere outline
        objectOutLines[9].enabled = false; //blue end capsule

        objectOutLines[8].enabled = true;// red pipe sphere outline
        objectOutLines[10].enabled = true;

        GasTablekitcolliders[17].enabled = false; // blue pipe sphere welding Tourch 
        GasTablekitcolliders[18].enabled = true; // red pipe sphere welding Tourch 
        RedPipeEndPoint.GetComponent<SnapGrabbleObject>().enabled = true;

        redPipeRop.GetComponent<CapsuleCollider>().enabled = true;

        bluePipEndPoint.GetComponent<SnapGrabbleObject>().enabled = false;

        bluePipEndPoint.transform.parent = ParentBluePipEndPoint.gameObject.transform;

        bluePipEndPoint.transform.localPosition = Vector3.zero;
        bluePipEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        bluePipEndPoint.GetComponent<CapsuleCollider>().enabled = false;
        bluePipEndPoint.transform.localScale = new Vector3(0.044504f, 0.03f, 0.03f);
        bluePipEndPoint.transform.GetChild(0).transform.localPosition = blueStartpos;//new Vector3(0, 600, 0);
        //13 ,-250,105

        //  BluePipEndPoint.transform.GetChild(1).gameObject.SetActive(false);
        isPipeblueConnect = true;
    }
    public void OnConnecteRedPipe()
    {
        objectOutLines[8].enabled = false;// red pipe sphere outline
        objectOutLines[10].enabled = false;// red end spheare

        RedPipeEndPoint.GetComponent<SnapGrabbleObject>().enabled = false;
        objectOutLines[6].enabled = false;

        GasTablekitcolliders[18].enabled = false; // red pipe sphere welding Tourch 
        RedPipeEndPoint.transform.parent = ParentRedPipeEndPoint.gameObject.transform;//red pipe sphere welding Tourch 
        RedPipeEndPoint.transform.localPosition = Vector3.zero;
        RedPipeEndPoint.GetComponent<CapsuleCollider>().enabled = false;
        RedPipeEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        RedPipeEndPoint.transform.GetChild(0).transform.localPosition = redStartpos;// new Vector3(0, 13, 0);
        //-0.3,1.79,3
        //RedPipeEndPoint.transform.GetChild(1).gameObject.SetActive(false);
        // RedPipeEndPoint.transform.localScale = new Vector3(0.044504f, 0.03f, 0.03f);
        isPipeRedConnect = true;

        //clip of torch black
        GasTablekitcolliders[15].enabled = true;
        GasTablekitcolliders[15].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[15].GetComponent<SnapGrabbleObject>().enabled = true;
        HL_T_ClipBlack.SetActive(true);
    }
    public void DoneBlackClip_T()
    {
        //clip of torch red
        GasTablekitcolliders[16].enabled = true;
        GasTablekitcolliders[16].GetComponent<Outline>().enabled = true;
        GasTablekitcolliders[16].GetComponent<SnapGrabbleObject>().enabled = true;
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
        PlayStepAudio(9);// Nozzle set audio
    }
    void Onclickbtn_s_6_confirm()
    {
       // setPressureRegulatorCanvas.SetActive(true);
        GasTablekitcolliders[19].enabled = true;// blue valve nozzel

        rotateNozzles[2].enabled = true;// blue valve nozzel

        // GasTablekitcolliders[9].transform.GetChild(1).gameObject.SetActive(true);
        objectOutLines[4].enabled = false;
        objectOutLines[11].enabled = true;// blue valve
        readSteps.HideConifmBnt();
        BlueRotatesprite.SetActive(true);
    }
    public void OnEnableRedValeNozzel()
    {
        if (!isTurnOffFlame)
        {
            redRotateSprite.SetActive(true);
            objectOutLines[11].enabled = false;
            objectOutLines[12].enabled = true;
            GasTablekitcolliders[20].enabled = true;// red  valve nozzel
                                                    //  GasTablekitcolliders[10].transform.GetChild(1).gameObject.SetActive(true);
            rotateNozzles[3].enabled = true;// red valve nozzel
        }

    }
    public void Enablestep8_Water()
    {
        if (!isTurnOffFlame)
        {
            rotateNozzles[3].enabled = false;// blue valve nozzel
            objectOutLines[12].enabled = false; // red reguletor
            readSteps.onClickConfirmbtn();
            readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_8_confirm_p2);
          //  Debug.Log("call water");
        }
    }
    #endregion

    #region step 8_p check leakage 

    void Onclickbtn_s_8_confirm_p2() //acetylene  shop water
    {
        GasTablekitcolliders[21].enabled = true; //Glass object for red
        GasTablekitcolliders[21].GetComponent<Outline>().enabled = true; //Glass object for red
        objectOutLines[4].enabled = true;//regulator red
        readSteps.HideConifmBnt();
    }

    public void Done_acetylene_shop_water() //oxygen  shop water
    {
        //  Debug.Log("call 6 ");
        GasTablekitcolliders[21].enabled = false; //Glass object for red
        GasTablekitcolliders[21].GetComponent<Outline>().enabled = false; //Glass object for red
        objectOutLines[4].enabled = false;//regulator red
        GasTablekitcolliders[21].gameObject.SetActive(true);
        SetObjectRestPos_Rotate(0);

        GasTablekitcolliders[22].enabled = true; //Glass object for black
        GasTablekitcolliders[22].GetComponent<Outline>().enabled = true; //Glass object for black
        objectOutLines[5].enabled = true; //regulator black
       

    }
    public void Done_oxygen_shop_water() //oxygen  shop water
    {
        //  Debug.Log("call 6 ");
        GasTablekitcolliders[22].enabled = false; //Glass object for black
        GasTablekitcolliders[22].GetComponent<Outline>().enabled = false; //Glass object for black
        objectOutLines[5].enabled = false;//regulator black
        GasTablekitcolliders[22].gameObject.SetActive(true);
        SetObjectRestPos_Rotate(1);

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s9_confirm);
    }

    #endregion
    #region Step 9_p: Show Fixing orifice nozzle of 1.2 mm on gas cutting torch(cutting blow pipe).
    void Onclickbtn_s9_confirm()
    {
      /*  GasTablekitcolliders[21].gameObject.SetActive(true);
        SetObjectRestPos_Rotate(0);

        GasTablekitcolliders[22].gameObject.SetActive(true);
        SetObjectRestPos_Rotate(1);*/

        onEnableStep9Object();
        readSteps.HideConifmBnt();
    }
    void onEnableStep9Object()
    {
        GasTablekitcolliders[23].enabled = true;
        GasTablekitcolliders[23].GetComponent<Outline>().enabled = true;
    }
    public void CheckNozzelConnected()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(CallEndMethod.Invoke);
    }
    #endregion
    void PlayStepAudio(int index)
    {
        if (stepAudioSource.clip != null)
        {
            stepAudioSource.Stop();
            stepAudioSource.PlayOneShot(stepsAudioClip[index]);
        }
    }
    public void SetObjectRestPos_Rotate(int indexOfReset)
    {
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = false;
        toolToReset[indexOfReset].transform.localPosition = toolToResetPosition[indexOfReset];
        toolToReset[indexOfReset].transform.localEulerAngles = toolToResetRotate[indexOfReset];
        toolToReset[indexOfReset].GetComponent<XRGrabInteractable>().enabled = true;
    }
}
