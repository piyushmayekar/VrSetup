using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2Step2 : Step
{
    public bool jobGrabbed;
    public BenchWise benchwise;
    public GameObject snapPosForT3;

    public TaskBoardInfo StepOneInfo; //Hold the ms plate on wise
    public TaskBoardInfo StepTwoInfo; //Grab the file and do the filing

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

        TaskBoard.ins.ShowInformation(StepOneInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepTwo);

    }

    public void ShowStepTwo()
    {
        TaskBoard.ins.ShowInformation(StepTwoInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);

        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(OnDoneClick);
    }

    public void OnDoneClick()
    {
        done = true;
    }
    public void OnJobGrab()
    {
        if (isValid() && !benchwise.SecondFilingDone)
        {
            benchwise.ShowJobSnapPos2();

        }

    }

    private void Update()
    {

        if (done)
        {
            taskComplete = true;
            snapPosForT3.SetActive(true);
        }

        if (isValid() && benchwise.SecondFilingDone)
        {
            TaskBoard.ins.ProceedButton.enabled = true;
            TaskBoard.ins.ToggleButtonColor(true);


        }
        base.Update();
    }
}
