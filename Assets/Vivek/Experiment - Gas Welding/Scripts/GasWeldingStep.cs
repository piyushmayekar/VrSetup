using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GasWeldingStep : MonoBehaviour
{
    public static GasWeldingStep instance;
    [Header("Canvas ")]
    public GameObject finishPanel;
    public GameObject setPressureRegulatorCanvas, LighterCanvas, lighter_Flame;
    public GameObject creacking_C_key1_canvas, creacking_C_key2_canvas;
    [Header("          ")]
    /* public GameObject step1Pnl;
     public GameObject step1Pnl_part_2;
     public GameObject step2Pnl, step3Pnl, step4Pnl, step4Pnl_part_2, step4Pnl_part_3, step5Pnl, step5Pnl_part2, step6Pnl, step7Pnl, step8Pnl, step9Pnl, step10Pnl,finishPanel;
    public GameObject step9Pnl_part1, step9Pnl_part2, step9Pnl_part3, SafetyNote;*/
    [Header("          ")]
    public GameObject ChainHighLight;
    public GameObject BluePipEndPoint, RedPipeEndPoint, GasParticleOrange, GasParticleBlue;
    public GameObject ParentBluePipEndPoint, ParentRedPipeEndPoint, nozzelSnapPoint;
    public GameObject RedRegulator, blackRegulator, redPipeRop, bluePipeRop, flameBlueParticle, flameYellowParticel, blacksmoke, oxidizing_F, reduce_or_carb_F, neturel_F;
    //  public GameObject[] HighLightObject;
    public GameObject step8Flame, Step9flame, extraRedBol, oldRedBol;

    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;
    [Header("Table uper gas kit")]
    public Collider[] GasTablekitcolliders;
    [Header("outLine Object to HighLight")]
    public Outline[] objectOutLines;
    [Header("SnapGrabbleObject")]
    public SnapGrabbleObject[] snapGrabbleObjects;
    public int countppekit, countCrackTab;

    public RotateNozzle redbol, greenbol;
   /* [Header("     ovr objects     ")]
    public GameObject C_hand_left;
    public GameObject C_hand_right;

    public GameObject n_C_hand_left;
    public GameObject n_C_hand_right;*/
    public bool isPipeRedConnect, isPipeblueConnect;
    [Header("Read step from json calss")]
    public ReadStepsFromJson readSteps;
    [Header("Steps audio clips")]
    public AudioSource stepAudioSource;
    public AudioClip[] stepsAudioClip;
    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        // StartCanvas.SetActive(true);
        for (int i = 0; i < ppekitcolliders.Length; i++)
        {
            ppekitcolliders[i].enabled = false;
            ppekitcolliders[i].GetComponent<Outline>().enabled = false;
        }
        for (int i = 0; i < GasTablekitcolliders.Length; i++)
        {
            GasTablekitcolliders[i].enabled = false;
            /*  if(GasTablekitcolliders[i].GetComponent<OVRGrabbable>())
              {
                  GasTablekitcolliders[i].GetComponent<OVRGrabbable>().enabled = false;
              }*/
        }
        for (int i = 0; i < objectOutLines.Length; i++)
        {
            objectOutLines[i].enabled = false;
        }
        for (int i = 0; i < snapGrabbleObjects.Length; i++)
        {
            snapGrabbleObjects[i].enabled = false;
        }
        redbol.enabled = false;
        greenbol.enabled = false;
        nozzelSnapPoint.gameObject.SetActive(false);
        GasTablekitcolliders[11].GetComponent<SnapGrabbleObject>().enabled = false;
        GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = false;
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = false;
        redPipeRop.GetComponent<CapsuleCollider>().enabled = false;
        // onEnableStep6Object();
        //  Onclickbtn_s_4_confirm_p3();
        // Onclickbtn_s_4_confirm_p2();
        //   OnEnableStep5object();
        //    StartCoroutine(lighterEnable());
        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmSatrtbtn);
        readSteps.confirmbtn.gameObject.SetActive(true);
    // StartCoroutine(lighterEnable());
    }
    public void Start()
    {
        StartCoroutine(PlayGasWeldingStartAudio());
    }
    IEnumerator PlayGasWeldingStartAudio()
    {
        PlayStepAudio(0);
        yield return new WaitForSeconds(0.2f+stepAudioSource.clip.length);
        PlayStepAudio(1);
        yield return new WaitForSeconds(0.2f + stepAudioSource.clip.length);
        PlayStepAudio(2);

    }
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
      /*  if(Input.GetKey(KeyCode.Alpha0))
        {
            OnEnableStep2object();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            OnEnableStep3object();
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            OnEnableStep4object();
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            onEnableStep_4_part2_object();
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            onEnableStep_4_part3_object();

        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {

            OnEnableStep5object();
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            EnableWeldingTouchCanvas();
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            onEnableStep6Object();
        }
        else if (Input.GetKey(KeyCode.Alpha7))
        {
            onEnableStep7Object();
        }
        else if (Input.GetKey(KeyCode.Alpha8))
        {
            onEnableStep8Object();
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            PlayFlamsParticle2();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            PlayFlamsParticle1_Flam();
        }
        else if (Input.GetKey(KeyCode.B))
        {
            PlayFlamsParticle2_Flam();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            PlayFlamsParticle3_flam();
        }*/
    }
    public void ConfirmSatrtbtn()
    {
        //  StartCanvas.SetActive(false);

        //  StartCoroutine(DealyEnableObject(1f, readSteps.panel));
     //   Debug.Log("start confirm");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_1_confirm);
        PlayStepAudio(3);// kit audio
    }
    #region step1 Step 1: Wear PPE Kit
    public void Onclickbtn_s_1_confirm()
    {
        /* step1Pnl.SetActive(false);
         step1Pnl_part_2.SetActive(true);*/
       // Debug.Log("PPE confirm");
        // readSteps.panel.SetActive(true);
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
        if (countppekit >= 6)
        {
            OnEnableStep2object();
        }
        selectObject.SetActive(false);

    }
    public void OnEnableStep2object() //get all ppe kit then this call
    {
        //   step1Pnl_part_2.SetActive(false);
        // StartCoroutine(DealyEnableObject(1f, step2Pnl));
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_2_confirm);
        PlayStepAudio(4);// Chain audio
    }
    #endregion
    #region step2 gas cylinder on the trolley with chain.
    public void Onclickbtn_s_2_confirm()
    {
        //step2Pnl.SetActive(false);
       // step2Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        ChainHighLight.SetActive(true);
        objectOutLines[17].enabled = true;
        GasTablekitcolliders[0].enabled = true; // chain true
                                                // GasTablekitcolliders[0].GetComponent<OVRGrabbable>().enabled = true;

        snapGrabbleObjects[0].enabled = true;
        readSteps.HideConifmBnt();
    }
    #endregion
    #region Step 3: Cracking of both the cylinder
    public void OnEnableStep3object()
    {
        objectOutLines[17].enabled = false;
        // step2Pnl.SetActive(false);
        //StartCoroutine(DealyEnableObject(0.5f, step3Pnl));
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);
        PlayStepAudio(5);// Cracking audio
    }

    public void Onclickbtn_s_3_confirm()
    {
        //step3Pnl.SetActive(false);
      //  step3Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        HighLightCylinderCrack();
        readSteps.HideConifmBnt();
    }
    public void HighLightCylinderCrack()
    {
        objectOutLines[0].enabled = true;
        GasTablekitcolliders[1].enabled = true;//red crecking  cylinder key
                                               //**   GasTablekitcolliders[1].GetComponent<OVRGrabbable>().enabled = true;

        creacking_C_key1_canvas.SetActive(true);
    }
    public void Onclickcreacking_C_key1_canvas_btn()
    {
        objectOutLines[0].enabled = false;
        objectOutLines[1].enabled = true;
        creacking_C_key1_canvas.SetActive(false);
        creacking_C_key2_canvas.SetActive(true);
        GasTablekitcolliders[2].enabled = true;//blue crecking cylinder key
                                               //**  GasTablekitcolliders[2].GetComponent<OVRGrabbable>().enabled = true;
        GasTablekitcolliders[1].gameObject.SetActive(false);
    }
    public void Onclickcreacking_C_key2_canvas_btn()
    {
     //   step3Pnl.SetActive(false);
        objectOutLines[1].enabled = false;
        objectOutLines[0].enabled = true;
        creacking_C_key2_canvas.SetActive(false);
        GasTablekitcolliders[2].gameObject.SetActive(false);
        OnEnableStep4object();
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
    public void OnEnableStep4object()
    {
        //  StartCoroutine(DealyEnableObject(0.5f, step4Pnl));
        PlayStepAudio(6);// regulators audio
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm);
    }

    public void Onclickbtn_s_4_confirm()
    {
     ///   step4Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        objectOutLines[2].enabled = true; //red cylinder key

        GasTablekitcolliders[3].enabled = true; // red gas regulators
                                                //    GasTablekitcolliders[3].GetComponent<OVRGrabbable>().enabled = true;


        snapGrabbleObjects[1].enabled = true; //red gas regulators

        GasTablekitcolliders[3].GetComponent<Outline>().enabled = true;
        readSteps.HideConifmBnt();
    }
    public void onEnableStep_4_2_object()
    {
        objectOutLines[3].enabled = true; //blue cylinder key

        objectOutLines[2].enabled = false; //red cylinder key

        GasTablekitcolliders[17].enabled = true; // blue gas regulators
        //GasTablekitcolliders[17].GetComponent<OVRGrabbable>().enabled = true;

        GasTablekitcolliders[17].GetComponent<Outline>().enabled = true;
    }
    public void onEnableStep_4_part2_object()
    {
        /*  step4Pnl.SetActive(false);
          step4Pnl_part_2.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm_p2);
        objectOutLines[3].enabled = false;
    }
    public void Onclickbtn_s_4_confirm_p2() //acetylene  shop water
    {
  //      step4Pnl_part_2.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        objectOutLines[4].enabled = true;

        GasTablekitcolliders[15].enabled = true; //Glass object for red
                                                 //  GasTablekitcolliders[15].GetComponent<OVRGrabbable>().enabled = true;

        objectOutLines[12].enabled = true; // glass red outline
        /*   GasTablekitcolliders[15].transform.GetChild(2).GetComponent<Rigidbody>().isKinematic = false;
          for (int i = 1; i < GasTablekitcolliders[15].transform.GetChild(1).transform.childCount; i++)
           {
            GasTablekitcolliders[15].transform.GetChild(1).transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
           }*/
        /*for (int i = 1; i < GasTablekitcolliders[15].transform.childCount; i++)
        {
            GasTablekitcolliders[15].transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
        }*/
        readSteps.HideConifmBnt();
    }
    public void onEnableStep_4_part3_object()
    {
        objectOutLines[12].enabled = false; // glass red outline

        objectOutLines[4].enabled = false;
        /*step4Pnl_part_2.SetActive(false);
        step4Pnl_part_3.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm_p3);
    }
    public void Onclickbtn_s_4_confirm_p3() //oxygen  shop water
    {
        //step4Pnl_part_3.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        objectOutLines[5].enabled = true;

        GasTablekitcolliders[16].enabled = true;//Glass object for blue
                                                // GasTablekitcolliders[16].GetComponent<OVRGrabbable>().enabled = true;

        objectOutLines[13].enabled = true; // glass object blue
        /*  GasTablekitcolliders[16].transform.GetChild(2).GetComponent<Rigidbody>().isKinematic = false;
         for (int i = 1; i < GasTablekitcolliders[16].transform.GetChild(1).transform.childCount; i++)
          {
              GasTablekitcolliders[16].transform.GetChild(1).transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
          }*/
        /* for (int i = 1; i < GasTablekitcolliders[16].transform.childCount; i++)
         {
             GasTablekitcolliders[16].transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
         }*/
        readSteps.HideConifmBnt();
    }

    #endregion
    #region Step 5: Hose connection to regulator and Welding torch.
    public void OnEnableStep5object()
    {

        objectOutLines[5].enabled = false;

        objectOutLines[2].enabled = false; //blue cylinder key

        objectOutLines[3].enabled = false; //red cylinder key

        /*step4Pnl_part_3.SetActive(false);
        step5Pnl.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_confirm);
        PlayStepAudio(7);// hose pipe connection audio
    }
    public void Onclickbtn_s_5_confirm()
    {
     //   step5Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        onEnableStep5Object_2();
        readSteps.HideConifmBnt();
    }
    public void onEnableStep5Object_2()
    {
        GasTablekitcolliders[4].enabled = true; //blue hose pipe
                                                // GasTablekitcolliders[4].GetComponent<OVRGrabbable>().enabled = true;

        snapGrabbleObjects[2].enabled = true;// blue hose pipe

        GasTablekitcolliders[4].transform.GetChild(0).gameObject.SetActive(true);
        GasTablekitcolliders[4].transform.GetChild(0).GetComponent<Outline>().enabled = true;

        objectOutLines[5].enabled = true;//blue regulators outline

    }
    public void onEnableStep5Object_3()
    {
        GasTablekitcolliders[5].enabled = true;//red hose pipe
                                               // GasTablekitcolliders[5].GetComponent<OVRGrabbable>().enabled = true;

        snapGrabbleObjects[3].enabled = true; // red hose pipe

        GasTablekitcolliders[5].transform.GetChild(0).gameObject.SetActive(true);
        GasTablekitcolliders[5].transform.GetChild(0).GetComponent<Outline>().enabled = true;

        objectOutLines[4].enabled = true;   //red regulators

        objectOutLines[5].enabled = false;  //blue   regulators outline

    }

    public void EnableWeldingTouchCanvas()
    {
        /*   step5Pnl.SetActive(false);
           step5Pnl_part2.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_part2_confirm);
        PlayStepAudio(8);// tourch connection with pipe audio
    }
    public void Onclickbtn_s_5_part2_confirm()
    {
        //step5Pnl_part2.SetActive(false);
      //  step5Pnl_part2.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        objectOutLines[5].enabled = false;
        objectOutLines[4].enabled = false;

        GasTablekitcolliders[6].enabled = true;//welding Tourch 
                                               // GasTablekitcolliders[6].GetComponent<OVRGrabbable>().enabled = true;


        GasTablekitcolliders[7].enabled = true;// blue pipe sphere welding Tourch 


        bluePipeRop.GetComponent<CapsuleCollider>().enabled = true;

        objectOutLines[6].enabled = true;// blue pipe sphere outline
        objectOutLines[18].enabled = true;
        readSteps.HideConifmBnt();
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
        isPipeblueConnect = true;
    }
    public void OnConnecteRedPipe()
    {
        objectOutLines[7].enabled = false;// red pipe sphere outline
        objectOutLines[19].enabled = false;

        RedPipeEndPoint.GetComponent<SnapGrabbleObject>().enabled = false;

        GasTablekitcolliders[8].enabled = false; // red pipe sphere welding Tourch 


        RedPipeEndPoint.transform.parent = ParentRedPipeEndPoint.gameObject.transform;//red pipe sphere welding Tourch 
        RedPipeEndPoint.transform.localPosition = Vector3.zero;
        RedPipeEndPoint.GetComponent<CapsuleCollider>().enabled = false;
        RedPipeEndPoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        onEnableStep6Object();
        isPipeRedConnect = true;
    }

    #endregion
    #region Step 6: Set the gas pressure on the regulator as per nozzle size.
    public void onEnableStep6Object()
    {
        /* step5Pnl_part2.SetActive(false);
         step6Pnl.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_6_confirm);
        PlayStepAudio(9);// Nozzle set audio
        //   isPipeConnect = true;           
    }
    public void Onclickbtn_s_6_confirm()
    {
        /*n_C_hand_left.SetActive(true);
        n_C_hand_right.SetActive(true);

        C_hand_left.SetActive(false);
        C_hand_right.SetActive(false);*/

        //step6Pnl.SetActive(false);
        //step6Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        setPressureRegulatorCanvas.SetActive(true);

        GasTablekitcolliders[9].enabled = true;// blue valve nozzel
        GasTablekitcolliders[9].transform.GetChild(1).gameObject.SetActive(true);
        //   GasTablekitcolliders[9].GetComponent<OVRGrabbable>().enabled = true;

        //   objectOutLines[5].enabled = true;
        objectOutLines[4].enabled = false;
        objectOutLines[9].enabled = true;
        readSteps.HideConifmBnt();
    }
    public void OnEnableRedValeNozzel()
    {
        objectOutLines[9].enabled = false;
        objectOutLines[10].enabled = true;

        GasTablekitcolliders[10].enabled = true;// red  valve nozzel
        GasTablekitcolliders[10].transform.GetChild(1).gameObject.SetActive(true);
        // GasTablekitcolliders[10].GetComponent<OVRGrabbable>().enabled = true;
    }

    #endregion
    #region Step 7: Fix nozzle on Welding Torch.
    public void onEnableStep7Object()
    {

        objectOutLines[10].enabled = false;
        setPressureRegulatorCanvas.SetActive(false);  //setPressureRegulator Canvas 
        /* step6Pnl.SetActive(false);
         step7Pnl.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_7_confirm);
        PlayStepAudio(10);// fix nozzle  torch audio
        /*C_hand_left.SetActive(true);
        C_hand_right.SetActive(true);


        n_C_hand_left.SetActive(false);
        n_C_hand_right.SetActive(false);*/
    }
    public void Onclickbtn_s_7_confirm()
    {

        //step7Pnl.SetActive(false);
   //     step7Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        GasTablekitcolliders[11].enabled = true; //welding nozzle collider
                                                 //   GasTablekitcolliders[11].GetComponent<OVRGrabbable>().enabled = true;

        snapGrabbleObjects[4].enabled = true;//welding nozzle snapgrabble

        GasTablekitcolliders[11].GetComponent<Outline>().enabled = true; //welding nozzle collider
                                                                         //  GasTablekitcolliders[11].GetComponent<SnapGrabbleObject>().enabled = true;
        objectOutLines[8].enabled = true;//red pipe sphere
        objectOutLines[11].enabled = true;
        nozzelSnapPoint.gameObject.SetActive(true);
        readSteps.HideConifmBnt();
    }
    public void onEnableStep8Object()
    {
        GasTablekitcolliders[11].GetComponent<Outline>().enabled = false; //welding nozzle collider

        objectOutLines[8].enabled = false;
        objectOutLines[11].enabled = false;
        /* step7Pnl.SetActive(false);
         step8Pnl.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_8_confirm);
        PlayStepAudio(11);// open flam audio
    }

    #endregion
    #region Step 8: Open acetylene control valve about 1/4th and light the flame with spark lighter.
    public void Onclickbtn_s_8_confirm()
    {
      /*  step8Pnl.SetActive(false);
        step8Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
*/
        GasTablekitcolliders[11].enabled = false;//welding nozzle collider  

        snapGrabbleObjects[5].enabled = true;// lighter snap

        //  GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = true;

        GasTablekitcolliders[14].enabled = true;//Light collider
                                                // GasTablekitcolliders[14].GetComponent<OVRGrabbable>().enabled = true;

        GasTablekitcolliders[14].GetComponent<Outline>().enabled = true;//lighter outline

        LighterCanvas.SetActive(true);
        objectOutLines[8].enabled = true;
        readSteps.HideConifmBnt();
    }
    public void LighterSnap_true()
    {
        //    lighter_Flame.SetActive(true);  new 22
        //lighter_Flame.SetActive(false);
        GasTablekitcolliders[14].GetComponent<Outline>().enabled = false;//lighter outline
      //  step8Pnl.SetActive(false);
        LighterCanvas.SetActive(false);
        objectOutLines[8].enabled = false;

        //objectOutLines[11].enabled = false;
        GasTablekitcolliders[12].enabled = true; // red bol at gas tourch
                                                 // GasTablekitcolliders[12].GetComponent<OVRGrabbable>().enabled = true;
        GasTablekitcolliders[12].GetComponent<Outline>().enabled = true;

       /* redbol.enabled = true;
        greenbol.enabled = true;*/
        /*  step8Pnl.SetActive(true);
          step8Pnl.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);*/
       
        /*n_C_hand_left.SetActive(true);
        n_C_hand_right.SetActive(true);

        C_hand_left.SetActive(false);
        C_hand_right.SetActive(false);*/
        StartCoroutine(lighterEnable());
    }
    IEnumerator lighterEnable()
    {
        GasTablekitcolliders[14].enabled = false; // red bol at gas tourch
                                                  //  yield return new WaitForSeconds(1f);
        lighter_Flame.SetActive(false);
        GasTablekitcolliders[14].GetComponent<SnapGrabbleObject>().enabled = false;
        GasTablekitcolliders[14].gameObject.SetActive(false); // red bol at gas tourch
        PlayFlamsParticle1();   //new 22
        yield return new WaitForSeconds(1.5f);
        PlayFlamsParticle2();  //new 22
    }

    public void PlayFlamsParticle1()
    {
        Debug.Log("Orange");

        //        GasParticleOrange.SetActive(true);
        blacksmoke.SetActive(true); // black smoke

        GasTablekitcolliders[12].GetComponent<Outline>().enabled = false;
        //  GasTablekitcolliders[13].enabled = true;  // new 22
        //   GasTablekitcolliders[13].GetComponent<Outline>().enabled = true;// new 22
     //   step8Pnl.SetActive(false);
        //step9Pnl.SetActive(true);
       // SafetyNote.SetActive(true);
    }
    public void PlayFlamsParticle2()
    {
        Debug.Log("Blue");
        // GasParticleOrange.SetActive(false);
        // GasParticleBlue.SetActive(true);

        blacksmoke.SetActive(false);
        reduce_or_carb_F.SetActive(true); // simple reduce
        reduce_or_carb_F.GetComponent<AudioSource>().Play();
        GasTablekitcolliders[13].GetComponent<Outline>().enabled = false;
        //  SafetyNote.SetActive(false);

        //step9Pnl.SetActive(true);

        /*  C_hand_left.SetActive(true);
          C_hand_right.SetActive(true);

          n_C_hand_left.SetActive(false);
          n_C_hand_right.SetActive(false);*/
        /* step8Pnl.SetActive(false);
         step9Pnl.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_9_confirm);
        PlayStepAudio(12);// open flam audio
        //  Onclickbtn_s_9_confirm();
    }
    #endregion
    #region Step 9: Flame Setting.
    public void Onclickbtn_s_9_confirm()
    {
        /* step9Pnl.SetActive(false);
         step9Pnl_part1.SetActive(true);*/
        readSteps.onClickConfirmbtn();

        oldRedBol.GetComponent<Outline>().enabled = true;

        reduce_or_carb_F.SetActive(false);

        step8Flame.SetActive(false);
        Step9flame.SetActive(true);
        //step10Pnl.SetActive(true);
        GasTablekitcolliders[18].enabled = true;
        GasTablekitcolliders[18].transform.parent.GetComponent<RotateNozzle>().enabled = true;
        // GasTablekitcolliders[18].GetComponent<OVRGrabbable>().enabled = true;

        GasTablekitcolliders[14].GetComponent<Outline>().enabled = true;
        readSteps.HideConifmBnt();
    }


    public void PlayFlamsParticle1_Flam()//17   13  //first step flame  green
    {
        oldRedBol.GetComponent<Outline>().enabled = false;

        /* step9Pnl_part1.SetActive(false);
         step9Pnl_part2.SetActive(true);*/
        
        readSteps.onClickConfirmbtn();


        reduce_or_carb_F.SetActive(true);
        reduce_or_carb_F.GetComponent<AudioSource>().Play();
        //  GasTablekitcolliders[12].GetComponent<Outline>().enabled = false;
        GasTablekitcolliders[18].enabled = false;
        GasTablekitcolliders[18].transform.parent.gameObject.SetActive(false);
        GasTablekitcolliders[20].transform.parent.gameObject.SetActive(true);
        GasTablekitcolliders[14].GetComponent<Outline>().enabled = false;

        GasTablekitcolliders[19].enabled = true;  //green
        GasTablekitcolliders[19].transform.parent.GetComponent<RotateNozzle>().enabled = true;
        //   GasTablekitcolliders[19].GetComponent<OVRGrabbable>().enabled = true;

        GasTablekitcolliders[19].transform.parent.GetComponent<Outline>().enabled = true; //green

    }
    public void PlayFlamsParticle2_Flam() //second step flame   red 
    {
        /*step9Pnl_part2.SetActive(false);
        step9Pnl_part3.SetActive(true);*/
        readSteps.onClickConfirmbtn();

        oxidizing_F.SetActive(true);
        oxidizing_F.GetComponent<AudioSource>().Play();
        reduce_or_carb_F.SetActive(false);

        GasTablekitcolliders[19].enabled = false;
        GasTablekitcolliders[19].transform.parent.GetComponent<Outline>().enabled = false;

        extraRedBol.SetActive(true);
        oldRedBol.SetActive(true);

        GasTablekitcolliders[20].enabled = true;
        GasTablekitcolliders[20].transform.parent.GetComponent<RotateNozzle>().enabled = true;
        //   GasTablekitcolliders[20].GetComponent<OVRGrabbable>().enabled = true;

        GasTablekitcolliders[20].transform.parent.GetComponent<Outline>().enabled = true;

    }
    public void PlayFlamsParticle3_flam() // netural flame
    {

        GasTablekitcolliders[20].enabled = false;
        GasTablekitcolliders[20].transform.parent.GetComponent<Outline>().enabled = false;

        neturel_F.SetActive(true);
        neturel_F.GetComponent<AudioSource>().Play();
        oxidizing_F.SetActive(false);

        /* C_hand_left.SetActive(true);
         C_hand_right.SetActive(true);

         n_C_hand_left.SetActive(false);
         n_C_hand_right.SetActive(false);*/

        /*step9Pnl_part3.SetActive(false);
        step10Pnl.SetActive(true);*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_10_confirm);
    }

    #endregion
    #region Step 10: Turning off Flame
    public void Onclickbtn_s_10_confirm()
    {
        //     step10Pnl.SetActive(false);
        readSteps.panel.SetActive(false);
        oxidizing_F.SetActive(false);
        neturel_F.SetActive(false);
        reduce_or_carb_F.SetActive(false);

        finishPanel.SetActive(true);
    }
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
    void PlayStepAudio(int index)
    {
        if (stepAudioSource.clip != null)
        {
            stepAudioSource.Stop();
            stepAudioSource.PlayOneShot(stepsAudioClip[index]);
        }
    }
   
}