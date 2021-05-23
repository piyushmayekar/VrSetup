using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCuttingManager : MonoBehaviour
{
    public static GasCuttingManager instance;

    [Header("outLine Object to HighLight")]
    public Outline[] objectOutLines;

    [Header("Canvas ")]
    public GameObject finishPanel;

    [Header("Extra objects")]
    public GameObject redPipeRop;
    public GameObject bluePipeRop, ParentBluePipEndPoint, ParentRedPipeEndPoint,neturalFlameCube;
    public GameObject /*carburing_flame, */netural_flame/*, netural__oxygen_cutting_flame, oxidizing_flame*/;
    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;

    [Header("Table uper gas kit")]
    public Collider[] GasTableObjectcolliders;

    [Header("Read step from json calss")]
    public ReadStepsFromJson readSteps;
    [Header("Counters")]
    public int countppekit;

    public bool isPipeRedConnect, isPipeblueConnect;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
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
       
    }
    void Start()
    {
        Onclickbtn_s_2_confirm();
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
    #region Step 2: Keep raw material ready as per the given drawing.
    public void Onclickbtn_s_2_confirm()
    {
        readSteps.HideConifmBnt();
        CuttingJobMaterial.instance.StartScriberMarking();
      //  GasTableObjectcolliders[0].enabled = true;// job plate material 
     //   GasTableObjectcolliders[0].GetComponent<Outline>().enabled = true;// job plate material
     //   objectOutLines[0].enabled = true; // job plate mateial place position
    }
    public void CheckJobPlace()
    {
        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = false;// job plate material
        objectOutLines[0].enabled = false; // job plate mateial place position

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_3_confirm);
        PlayStepAudio(4);//   
    }

    #endregion
    #region Step 3: Keep the job on the Cutting table in flat position.
    public void Onclickbtn_s_3_confirm()
    {
        readSteps.HideConifmBnt();
        OnEnableStep3object();
    }
    public void OnEnableStep3object()
    {
        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = true;// job plate material
        objectOutLines[1].gameObject.SetActive(true);// job flat  object position
        objectOutLines[1].enabled = true;// job flat position
        objectOutLines[1].GetComponent<BoxCollider>().enabled = true;
    }
    public void CheckJobFlatPlace()
    {
        //job plat posion set
        GasTableObjectcolliders[0].transform.localPosition = new Vector3(-1.310f, 0, -0.076f);//objectOutLines[1].transform.position;// job plate material
        objectOutLines[1].gameObject.SetActive(false);// job flat position      
        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = false;// job plate material
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_4_confirm);
        PlayStepAudio(4);// 
    }
    #endregion
    #region Step 4: Clean the job surface with wire brush and remove burrs by filing.
    public void OnEnableStep4object()
    {
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        GasTableObjectcolliders[1].GetComponent<Outline>().enabled = true;// brush out line
        GasTableObjectcolliders[1].enabled = true;// brush collider

    }

    public void Onclickbtn_s_4_confirm()
    {

        readSteps.HideConifmBnt();
        OnEnableStep4object();
    }
    public void checkBrushStep()
    {
        GasTableObjectcolliders[1].GetComponent<Outline>().enabled = false;// Brushout line
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_5_confirm);
        GasTableObjectcolliders[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;

    }
    #endregion
    #region Step 5: As per the drawing, the surfaces of the job should be marked and punched.
    public void MarkingPointTwoEnable()
    {
        GasTableObjectcolliders[10].enabled = true;
    }
    public void OnEnableStep5object()
    {
        // CuttingJobMaterial.instance.StartScriberMarking();
        CuttingJobMaterial.instance.StartCenterPunchMarking();
    }
    public void Onclickbtn_s_5_confirm()
    {
        Debug.Log("Onclickbtn_s_5_confirm");
        OnEnableStep5object();
        readSteps.HideConifmBnt();
    }
    public void checkStep5()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s_6_confirm);
    }
    #endregion
    #region Step 6: Set up Gas Cutting Table and Connection
    public void onEnableStep6Object()
    {
        // chain highlight
        /*Debug.Log("aa");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(gasWelding.Onclickbtn_s_2_confirm);*/
    }
    public void Onclickbtn_s_6_confirm()
    {
        /*readSteps.HideConifmBnt();
        onEnableStep6Object();*/
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s7_confirm);
    }
    #endregion
    #region Step 7: Show Gas cutting torch and hoses connection and hoses to pressure regulator mounted on cylinders
    public void onEnableStep7Object()
    {
        // chain highlight
        /*Debug.Log("aa");
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(gasWelding.Onclickbtn_s_2_confirm);*/
        GasTableObjectcolliders[2].enabled = true;//blue hose pipe
        GasTableObjectcolliders[2].GetComponent<Outline>().enabled = true;//blue hose pipe at table outline
        objectOutLines[2].enabled = true;         // blue hose pipe outline regulator
    }
    public void Onclickbtn_s7_confirm()
    {
        onEnableStep7Object();
        readSteps.HideConifmBnt();
    }
    public void EnableRedHosPipe()
    {
        objectOutLines[2].enabled = false;         // blue hose pipe outline regulator
        GasTableObjectcolliders[3].enabled = true;//red hose pipe

        GasTableObjectcolliders[3].GetComponent<Outline>().enabled = true;//red hose pipe at table outline
        objectOutLines[3].enabled = true;         // red hose pipe outline regulator
    }
    public void EnableWeldingCuttingtorch() //cutting tourch and blue pipe rop
    {
        objectOutLines[3].enabled = false;         // red hose pipe outline regulator

        GasTableObjectcolliders[4].enabled = true; // cutting welding tourch
        GasTableObjectcolliders[5].enabled = true;// blue pipe sphere at tourch
        bluePipeRop.GetComponent<CapsuleCollider>().enabled = true; // blue hose pipe end position capsule collider
        bluePipeRop.GetComponent<Outline>().enabled = true;// blue hose pipe end position out linecapsule collider
        GasTableObjectcolliders[5].GetComponent<Outline>().enabled = true;// blue pipe sphere outline

    }
    public void BluePipeConnectATTorch() // blue pipe obejct hide and red pipe object true
    {
        Debug.Log("Red final");
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
        Debug.Log("Red final");
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
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s8_confirm);

    }

    #endregion
    #region Step 8: Show Fixing orifice nozzle of 1.2 mm on gas cutting torch(cutting blow pipe).
    public void Onclickbtn_s8_confirm()
    {
        onEnableStep8Object();
        readSteps.HideConifmBnt();
    }
    public void onEnableStep8Object()
    {
        GasTableObjectcolliders[7].enabled = true;
        GasTableObjectcolliders[7].GetComponent<Outline>().enabled = true;

    }
    public void CheckNozzelConnected()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s9_confirm);
    }

    #endregion
    #region Step 9: Need to set the pressure of oxygen at 1.6 kg/cm2 and for acetylene set it at 0.15 kg/cm2.
    public void Onclickbtn_s9_confirm()
    {
        onEnableStep9Object();
        readSteps.HideConifmBnt();
    }
    public void onEnableStep9Object()
    {
        // enable blue Nozzel objects
        GasTableObjectcolliders[8].enabled = true;// blue valve nozzel
        GasTableObjectcolliders[8].transform.GetChild(1).gameObject.SetActive(true);
        objectOutLines[2].enabled = true; // blue reguletor

    }
    public void enableRedNozzelvalveObjects()
    {
        objectOutLines[2].enabled = false; // blue reguletor
        // enable red Nozzel objects
        GasTableObjectcolliders[9].enabled = true;// red valve nozzel
        GasTableObjectcolliders[9].transform.GetChild(1).gameObject.SetActive(true);
        objectOutLines[3].enabled = true; // red reguletor
    }
    public void Enablestep10()
    {
        objectOutLines[3].enabled = false; // red reguletor
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s10_confirm);
    }
    #endregion
    #region Step 10: Set the neutral flame.
    public void Onclickbtn_s10_confirm()
    {
        onEnableStep10Object();
        readSteps.HideConifmBnt();
    }
    //carburing flame
    public void NeturalFlameFinish()
    {
        netural_flame.SetActive(true);
        netural_flame.GetComponent<AudioSource>().Play();
        GasTableObjectcolliders[11].transform.parent.GetComponent<Outline>().enabled = false;
        GasTableObjectcolliders[11].enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s11_confirm);
    }
    //netural flame
    //netural flame with oxygen cutting flame
    //oxidizing flame
    public void onEnableStep10Object()
    {
        GasTableObjectcolliders[11].enabled = true;
        GasTableObjectcolliders[11].transform.parent.GetComponent<Outline>().enabled = true;
    }
   

    #endregion
    #region Step 11: Keep the gas cutting torch (blow pipe) at 90� on job surface and the cutting line.
    public void Onclickbtn_s11_confirm()
    {
        onEnableStep11Object();
        readSteps.HideConifmBnt();
    }
    public void onEnableStep11Object()
    {
        neturalFlameCube.SetActive(true);
        
    }
    public void CheckCuttingLine()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(Onclickbtn_s12_confirm);
    }
    #endregion
    #region Step 12: Heat one end of the marking line till it turns cherry red.Keep a distance of 5mm between the job and the nozzle.
    public void Onclickbtn_s12_confirm()
    {
        onEnableStep12Object();
        readSteps.HideConifmBnt();
    }
    public void onEnableStep12Object()
    {
        

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
