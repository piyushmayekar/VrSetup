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
    public GameObject redPipeRopend;
    public GameObject bluePipeRopend, redpipeRop, bluepipeRop, ParentBluePipEndPoint, ParentRedPipeEndPoint, neturalFlameCube;
    public GameObject netural_flame, BlueRotatesprite, redRotateSprite, jointTackPoint, hummerhighlight, highlighttriSquare,torch35D,torch_m_35d;
    public GameObject[] weldingLine1, weldingLine2;
    [Header("Counters")]
    public int countppekit;
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

        readSteps.panel.SetActive(true);
        readSteps.AddClickConfirmbtnEvent(ConfirmSatrtbtn);
        readSteps.confirmbtn.gameObject.SetActive(true);
        //   Onclickbtn_s_2_confirm();//  Onclickbtn_s8_confirm();
        //  Onclickbtn_s8_confirm();
      //  Onclickbtn_s10_confirm();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPipeblueConnect)
        {
            bluePipeRopend.transform.parent = ParentBluePipEndPoint.gameObject.transform; // new 22
            bluePipeRopend.transform.localPosition = Vector3.zero;
            bluePipeRopend.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        if (isPipeRedConnect)
        {
            redPipeRopend.transform.parent = ParentRedPipeEndPoint.gameObject.transform;//red pipe sphere welding Tourch   //new 22
            redPipeRopend.transform.localPosition = Vector3.zero;
            redPipeRopend.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void ConfirmSatrtbtn()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_1_confirm);
        PlayStepAudio(3);// kit audio
    }
    #region step1 Step 1: Wear PPE Kit
    public void Onclickbtn_s_1_confirm()
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
    #region Step 2:  Keep the job on welding table in “T” Position.
    public void Onclickbtn_s_2_confirm()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = true;// job plate material
        objectOutLines[1].gameObject.SetActive(true);// job flat  object position
        objectOutLines[1].enabled = true;// job flat position
        objectOutLines[1].GetComponent<BoxCollider>().enabled = true;
    }
    public void PlaceJobPlate()
    {
        //job plat posion set
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-1.310f, 0.023f, -0.076f);//objectOutLines[1].transform.position;// job plate material
        objectOutLines[1].gameObject.SetActive(false);// job flat position      
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false;// job plate material
        objectOutLines[1].enabled = false;// job flat position

    }
    public void CheckJobFlatPlace()
    {

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);
        PlayStepAudio(4);// 

   //     Onclickbtn_s_3_confirm();
    }

    #endregion

    #region Step 3: Clean the job surface with wire brush and remove burrs by filing.
    public void Onclickbtn_s_3_confirm()
    {
        readSteps.HideConifmBnt();
        OnEnableStep3object();
    }
    public void OnEnableStep3object()
    {
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        GasTableObjectcolliders[1].transform.GetChild(0).GetComponent<Outline>().enabled = true;// brush out line
        GasTableObjectcolliders[1].enabled = true;// brush collider
    }
    public void checkBrushStep()
    {
        GasTableObjectcolliders[1].transform.GetChild(0).GetComponent<Outline>().enabled = false;// Brushout line
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<Outline>().enabled = false; // job plate outline
                                                                                                  //  GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        Debug.Log("call brush");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm);

       // Onclickbtn_s_4_confirm();
    }
    #endregion

    #region Step 4: Hoses connection to Gas Cutting torch.
    public void Onclickbtn_s_4_confirm()
    {
        GasTableObjectcolliders[2].GetComponent<Outline>().enabled = true; //cutting tourch     
        GasTableObjectcolliders[2].enabled = true; //cuting torch collider
        redpipeRop.SetActive(true);
        bluepipeRop.SetActive(true);

        GasTableObjectcolliders[4].GetComponent<Outline>().enabled = true;// blue pipe sphere outline
        bluePipeRopend.GetComponent<CapsuleCollider>().enabled = true; // blue hose pipe end position capsule collider
        bluePipeRopend.GetComponent<Outline>().enabled = true;// blue hose pipe end position out linecapsule collider
        readSteps.HideConifmBnt();
    }

    public void BluePipeConnectATTorch() // blue pipe obejct hide and red pipe object true
    {
        // Debug.Log("Red final");
        bluePipeRopend.GetComponent<CapsuleCollider>().enabled = false; // blue hose pipe end position capsule collider
        bluePipeRopend.GetComponent<Outline>().enabled = false;// blue hose pipe end position out linecapsule collider
        GasTableObjectcolliders[4].GetComponent<Outline>().enabled = false;// blue pipe sphere outline

        // call at update
        bluePipeRopend.GetComponent<SnapGrabbleObject>().enabled = false;

        bluePipeRopend.transform.parent = ParentBluePipEndPoint.gameObject.transform;

        bluePipeRopend.transform.localPosition = Vector3.zero;
        bluePipeRopend.transform.localRotation = Quaternion.Euler(Vector3.zero);
        bluePipeRopend.GetComponent<CapsuleCollider>().enabled = false;
        isPipeblueConnect = true;


        //red pipe object 
        GasTableObjectcolliders[5].enabled = true;// red pipe sphere at tourch
        redPipeRopend.GetComponent<CapsuleCollider>().enabled = true; // red hose pipe end position capsule collider
        redPipeRopend.GetComponent<Outline>().enabled = true;// red hose pipe end position out linecapsule collider
        GasTableObjectcolliders[5].GetComponent<Outline>().enabled = true;// red pipe sphere outline
    }
    public void RedPipeConnectAtTorch()//  red pipe object hide
    {

        //red pipe object 
        GasTableObjectcolliders[5].enabled = false;// red pipe sphere at tourch
        redPipeRopend.GetComponent<CapsuleCollider>().enabled = false; // red hose pipe end position capsule collider
        redPipeRopend.GetComponent<Outline>().enabled = false;// red hose pipe end position out linecapsule collider
        GasTableObjectcolliders[5].GetComponent<Outline>().enabled = false;// red pipe sphere outline

        // call at update
        redPipeRopend.GetComponent<SnapGrabbleObject>().enabled = false;
        redPipeRopend.transform.parent = ParentBluePipEndPoint.gameObject.transform;

        redPipeRopend.transform.localPosition = Vector3.zero;
        redPipeRopend.transform.localRotation = Quaternion.Euler(Vector3.zero);
        redPipeRopend.GetComponent<CapsuleCollider>().enabled = false;
        isPipeRedConnect = true;

        GasTableObjectcolliders[2].GetComponent<Outline>().enabled = false;  //welding tourch outline

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s5_confirm);

       // Onclickbtn_s5_confirm();

    }


    #endregion
    #region step 5:Fix nozzle on Welding Torch.
    public void Onclickbtn_s5_confirm()
    {
        onEnableStep5Object();
        readSteps.HideConifmBnt();
    }
    public void onEnableStep5Object()
    {
        GasTableObjectcolliders[3].enabled = true;// welding nozzel
        GasTableObjectcolliders[3].GetComponent<Outline>().enabled = true;// welding nozzel

    }
    public void CheckNozzelConnected()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s6_confirm);


       // Onclickbtn_s6_confirm();
    }
    #endregion
    #region Step 6: Set the gas pressure on the regulator as per nozzle size.
    public void Onclickbtn_s6_confirm()
    {
        onEnableStep6Object();
        readSteps.HideConifmBnt();
    }
    public void onEnableStep6Object()
    {
        // enable blue Nozzel objects
        GasTableObjectcolliders[6].enabled = true;// blue valve nozzel
        GasTableObjectcolliders[6].transform.GetChild(1).gameObject.SetActive(true);
        objectOutLines[2].enabled = true; // blue reguletor
        BlueRotatesprite.SetActive(true);

    }
    public void enableRedNozzelvalveObjects()
    {
        objectOutLines[2].enabled = false; // blue reguletor
        // enable red Nozzel objects
        GasTableObjectcolliders[7].enabled = true;// red valve nozzel
        GasTableObjectcolliders[7].transform.GetChild(1).gameObject.SetActive(true);
        objectOutLines[3].enabled = true; // red reguletor
        redRotateSprite.SetActive(true);
    }
    public void Enablestep6()
    {
        objectOutLines[3].enabled = false; // red reguletor
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s7_confirm);

      //  Onclickbtn_s7_confirm();
    }

    #endregion
    #region Step 7: Do tack welding on both ends and centre of the job.
    public void Onclickbtn_s7_confirm()
    {
        flameOff = true;
        jointTackPoint.SetActive(true);
        neturalFlameCube.SetActive(true);
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
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s8_0_confirm);

     //   Onclickbtn_s8_0_confirm();
    }
    #endregion

    #region  With the help of tri square, check the alignment of the job and clean the tag weld.
    public void Onclickbtn_s8_0_confirm()
    {
        readSteps.HideConifmBnt();
        highlighttriSquare.SetActive(true);

        objectOutLines[5].enabled = true;
        objectOutLines[6].enabled = true;
    }
    public void checkTriSquare()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s8_confirm);
        highlighttriSquare.SetActive(false);

        objectOutLines[5].enabled = false;
        objectOutLines[6].enabled = false;
        //   Onclickbtn_s8_confirm();
    }

    #endregion
    #region step 9 : Start Welding by Leftward Technique.
    public void Onclickbtn_s8_confirm()
    {
        readSteps.HideConifmBnt();
        weldingLine1[0].SetActive(true);
        weldingLine1[0].transform.GetChild(0).gameObject.SetActive(true);
        // JointWelding.instance.WeldingEnable();
        torch35D.SetActive(true);
    }
    public void weldingComplete()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s9_confirm);

//       Onclickbtn_s9_confirm();
    }
    public void CheckWelding_rot(GameObject weldingTorch)
    {
        weldingTorch.SetActive(false);
        FreezeRotation.instance.isFreeze = true;
        neturalFlameCube.GetComponent<JointWelding>().isWelding = true;
    }
    #endregion
    #region step 10 Clean the job surface with wire brush and remove distortion.
    public void Onclickbtn_s9_confirm()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[1].transform.GetChild(0).GetComponent<Outline>().enabled = true;// brush out line
        CuttingBrush.instance.cleanPointCount = 15;
        CuttingBrush.instance.isStop = false;
     //   Debug.Log("Call bursh out line");
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        netural_flame.SetActive(false);
    }
    public void cleanBrushFinish()
    {
        GasTableObjectcolliders[1].transform.GetChild(0).GetComponent<Outline>().enabled = false;// brush out line

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s10_confirm);

   ///     Onclickbtn_s10_confirm();

    }
    #endregion
    #region step 11 Check the defects.
    public void Onclickbtn_s10_confirm()
    {
        readSteps.HideConifmBnt();
        hummerhighlight.SetActive(true);
        objectOutLines[4].enabled = true;
        objectOutLines[4].transform.parent.GetComponent<JointWelding>().isWelding = true;
        for (int i = 0; i < weldingLine1.Length; i++)
        {
            weldingLine1[i].SetActive(false);
            weldingLine2[i].SetActive(true);
        }
        weldingLine2[0].transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = true;
        weldingLine2[0].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;

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
