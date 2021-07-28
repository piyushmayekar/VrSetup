using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class ManagerCornerJoint : MonoBehaviour
{
    public static ManagerCornerJoint instance;
    public GameObject finishPanel;
    public ReadStepsAndVideoManager readSteps;

    [Header("Tools Highlight")]
    public Outline[] ToolsHighlight;
    [Header("ppeCollider")]
    public Collider[] ppekitcolliders;
    public int countppekit;

    public GameObject WireBrushGo;
    public GameObject ChippingHammerGo;

    
    private WireBrush_VL WireBrush;
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

    public GameObject WeldingNuetralFlame;
    public GameObject[] FillerRod;
    public Material WeldFinishMat;

    public GameObject ZeroMeterred, ZeroMeterBlack;

    public Transform WorkPiece1;
    public Transform WorkPiece2;
    public GameObject WorkPieceSocket1, WorkPieceSocket2, FinalWorkPiece;

    public GameObject slagLine;

    public GameObject WireBrushHL1, WireBrushHL2, TorchHL, RodHL, ChipHammerHL;
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
    public void SetFillerRod(GameObject fillerRod)
    {
        if (Weldingflame)
        {
            Weldingflame.SetFillerRod(fillerRod);
        }
    }
    public void ConfirmSatrtbtn()
    {
        readSteps.onClickConfirmbtn();
        //readSteps.AddClickConfirmbtnEvent(GasWeldingSetUP.instance.Onclickbtn_s_3_confirm);
        readSteps.AddClickConfirmbtnEvent(EnablePPEKitStep);

        //PlayStepAudio(3);// kit audio
    }
    private void Start()
    {
        WireBrush = WireBrushGo.GetComponent<WireBrush_VL>();
        Weldingflame = WeldingNuetralFlame.GetComponent<WeldingFlame_VL>();
        ChippingHammer = ChippingHammerGo.GetComponent<ChippingHammer_VL>();
        //EnableWelding();
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

    public void OnEnableCleanJobWithWireBrush()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(EnableCleanJobWithWireBrush);
    }

    public void EnableCleanJobWithWireBrush()
    {
        readSteps.HideConifmBnt();
        WorkPiece1.GetComponentInChildren<Outline>().enabled = true;
        ToolsHighlight[0].enabled = true;
        WireBrushHL1.SetActive(true);
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(CleaningDoneForFirstPiece);
        //Assign On Clean job done method to brush
    }

    public void CleaningDoneForFirstPiece()
    {
        ToolsHighlight[0].enabled = false;
        WorkPiece1.GetComponentInChildren<Outline>().enabled = false;
        WorkPiece2.GetComponentInChildren<Outline>().enabled = true;

        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(CleaningDone);
        // readSteps.onClickConfirmbtn();
        // readSteps.AddClickConfirmbtnEvent();
        //EnableJobPlacingForWelding();
    }

    public void CleaningDone()
    {
        WorkPiece2.GetComponentInChildren<Outline>().enabled = false;
        ToolsHighlight[0].enabled = false;
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(PlaceWorkPiece1AtTable);

    }

    public void PlaceWorkPiece1AtTable()
    {
        readSteps.HideConifmBnt();
        WorkPieceSocket1.SetActive(true);
        WorkPiece1.GetComponentInChildren<Outline>().enabled = true;
        WorkPieceSocket1.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnWorkPiece1Snap);

    }

    public void OnWorkPiece1Snap()
    {
        WorkPieceSocket1.SetActive(false);
        WorkPiece1.GetComponentInChildren<Outline>().enabled = false;
        WorkPiece1.GetComponent<BoxCollider>().enabled = false;

        WorkPiece2.GetComponentInChildren<Outline>().enabled = true;
        WorkPieceSocket2.SetActive(true);
        WorkPieceSocket2.GetComponent<CustomSocket>().AssignMethodOnCleaningJobDone(OnBothJobSnapDone);
    }

    public void OnBothJobSnapDone()
    {
        WorkPieceSocket2.SetActive(false);
        WorkPiece2.GetComponentInChildren<Outline>().enabled = false;
        WorkPiece2.GetComponent<BoxCollider>().enabled = false;

        

        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(GasWeldingSetUP.instance.Onclickbtn_s_3_confirm);
        Debug.Log("Here");
    }

    public void FlameControlStep()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[0].enabled = true; //lighter
        GasTableObjectcolliders[0].GetComponent<SnapGrabbleObject_VL>().enabled = true;
        GasTableObjectcolliders[0].GetComponent<Outline>().enabled = true;

        ObjectOutlines[0].enabled = true; // torch nozel part 
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
        GasTableObjectcolliders[0].enabled = false;  //lighter
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
        }
    }

    #endregion

    public void EnableWelding()
    {
        readSteps.HideConifmBnt();
        WorkPiece1.gameObject.SetActive(false);
        WorkPiece2.gameObject.SetActive(false);
        FinalWorkPiece.SetActive(true);
        TorchHL.SetActive(true);
        RodHL.SetActive(true);
        WeldingNuetralFlame.SetActive(true);
        WeldingNuetralFlame.GetComponent<CapsuleCollider>().enabled = true;
        for (int i = 0; i < FillerRod.Length; i++)
        {
            FillerRod[i].GetComponent<Outline>().enabled = true;
            FillerRod[i].GetComponent<XRGrabInteractable>().enabled = true;
        }

        Weldingflame.SetWeldingFlameParams(FinalWorkPiece.transform, 1);
        Weldingflame.AssignMethodOnFusionRunDone(DoneWelding);

    }

    public void DoneWelding()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnTurningOffFlameStep1);
    }

    #region GasSetUp turning off Procedure
    public void OnTurningOffFlameStep1()
    {
        Debug.Log("Starting turning off");
        readSteps.HideConifmBnt();
        neturel_F.SetActive(false);
        reduce_or_carb_F.SetActive(true); // redus true  1
                                          //reduce_or_carb_F.GetComponent<AudioSource>().Play();
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
            //oxidizing_F.GetComponent<AudioSource>().Play();

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
    }

    public void CloseCylinderValves()
    {
        readSteps.HideConifmBnt();
        GasTableObjectcolliders[6].enabled = true; //blue valve nozzole(1)
        ObjectOutlines[6].enabled = true; //blue valve nozzole(1)

        ObjectOutlines[8].enabled = true; //Black regulator

        rotateNozzles[3].isclockwise = false;  //blue valve nozzole(1)
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
            GasTableObjectcolliders[6].enabled = false; //blue valve nozzole

            ObjectOutlines[8].enabled = false; //black regulator
            ObjectOutlines[9].enabled = true;  //red regulator 
            ObjectOutlines[6].enabled = false; //blue valve nozzole
            GasTableObjectcolliders[7].enabled = true; //red valve nozzole

            ObjectOutlines[7].enabled = true; //red valve nozzole

            rotateNozzles[4].isclockwise = false; // red valve nozzle
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

    public void OnTurnOffDone()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(PrepareJobforRemovingSlag);
    }
    public void PrepareJobforRemovingSlag()
    {
        readSteps.HideConifmBnt();
        int count = 0;
        for (int i = 0; i < FillerRod.Length; i++)
        {
            FillerRod[i].GetComponent<Outline>().enabled = false;

        }
        
        for (int i = 0; i < slagLine.transform.childCount; i++)
        {
            count++;
            slagLine.transform.GetChild(i).GetComponent<CapsuleCollider>().enabled = true;
            slagLine.transform.GetChild(i).tag = "Slag";

        }

        ChipHammerHL.SetActive(true);

        ToolsHighlight[1].enabled = true;
        ChippingHammer.SetChippingHammerParams(count);
        ChippingHammer.AssignMethodOnSlagRemoveDone(SlagRemoved);
    }

    public void SlagRemoved()
    {
         //readSteps.onClickConfirmbtn();
         //readSteps.AddClickConfirmbtnEvent(PrepareJobforRemovingSlag);
        ToolsHighlight[1].enabled = false;
        ToolsHighlight[0].enabled = true;
        WireBrushHL2.SetActive(true);
        WireBrush.SetWireBrushParams(10, "Job");
        WireBrush.AssignMethodOnCleaningJobDone(CleanedWeldLines);

    }

    public void CleanedWeldLines()
    {
       
        for (int i = 0; i < slagLine.transform.childCount; i++)
        {
            slagLine.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial = WeldFinishMat;
           slagLine.transform.GetChild(i).GetComponent<CapsuleCollider>().enabled = false;
            

        }

        ToolsHighlight[0].enabled = false;
        finishPanel.SetActive(true);
        readSteps.tablet.SetActive(true);
        readSteps.panel.SetActive(false);
        Debug.Log("Done");
    }



    #endregion
}
