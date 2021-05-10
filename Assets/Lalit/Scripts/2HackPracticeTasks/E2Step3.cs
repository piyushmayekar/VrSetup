using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2Step3 : Step
{

    public GameObject BottleHighlight;
    public GameObject BrushHighlight;
    public GameObject ScriberHighlight;
    public GameObject CenterPuchHighlight;
    public GameObject HammerHighlight;

    public Job Job;

    public bool showHighlight;
    public GameObject snapHighlight;

    public GameObject snapPosForT4;

    public TaskBoardInfo StepOneInfo;
    public TaskBoardInfo StepTwoInfo;
    public TaskBoardInfo StepThreeInfo;

    public bool done;
    public void OnEnable()
    {
        base.OnEnable();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepOne);
        TaskBoard.ins.ToggleButtonColor(true);
    }

    public void ShowStepOne()
    {
        //copper sulphate
        TaskBoard.ins.ShowInformation(StepOneInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepTwo);

    }

    public void ShowStepTwo()
    {
        //scriber
        TaskBoard.ins.ShowInformation(StepTwoInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);

        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepThree);
    }

    public void ShowStepThree()
    {
        //dot punch
        TaskBoard.ins.ShowInformation(StepThreeInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);

        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(OnDoneClick);
    }


    public void OnDoneClick()
    {
        done = true;
    }
    private void Update()
    {

        base.Update();

        if (done)
        {
            taskComplete = true;
        }
        else if (Job.UseCS && Job.UseBrush && Job.UseScriber && Job.UseDotPunch)
        {
            //hintText.text = "Marking Done && Fit the Job into BenchWise";
            Job.doneMarking = true;
            Job.markingQuad.SetActive(false);


        }
        else if (Job.UseCS && Job.UseBrush && Job.UseScriber && !Job.UseDotPunch)
        {
            // hintText.text = "Grab The Dot punch";

        }
        else if (Job.UseCS && Job.UseBrush && !Job.UseScriber)
        {
            // hintText.text = "Grab The Scriber";
            // hintImage.SetActive(true);


        }
        else if (Splash.count >= 1 && !Job.UseCS)
        {
            Job.UseCS = true;
            // hintText.text = "Grab small Brush";

        }
    }

    public void OnJobSnapForMarking()
    {
        if (isValid())
        {
            // hintText.text = "Grab the CopperSulphate Bottle";
            snapHighlight.SetActive(false);
        }


    }

    public void OnBottleGrab()
    {
        if (isValid() && !Job.UseCS)
        {
            BottleHighlight.SetActive(true);
           // hintText.text = "put some drop on job Surface using trigger";
        }
    }

    public void OnBrushGrab()
    {
        if (isValid() && Job.UseCS && !Job.UseBrush)
        {
            BrushHighlight.SetActive(true);
            //hintText.text = "Use Brush to spread Chemical";

        }
    }

    public void OnScriberGrab()
    {
        if (isValid() && Job.UseCS && Job.UseBrush && !Job.UseScriber)
        {
            ScriberHighlight.SetActive(true);
            Job.Line1.SetActive(true);
            Job.Line2.SetActive(true);
            // hintText.text = "Do the Marking as Shown here";

        }
    }

    public void OnCenterPunchGrab()
    {
        if (isValid() && Job.UseCS && Job.UseBrush && Job.UseScriber && !Job.UseDotPunch)
        {
            CenterPuchHighlight.SetActive(true);

            // hintText.text = "Grab Hammer for punching";

        }
    }

    public void OnHammerGrab()
    {
        if (isValid() && Job.UseCS && Job.UseBrush && Job.UseScriber && !Job.UseDotPunch)
        {
            HammerHighlight.SetActive(true);
            //hintText.text = "Do punching on job line by hammering on scriber slowly";
        }
    }
}
