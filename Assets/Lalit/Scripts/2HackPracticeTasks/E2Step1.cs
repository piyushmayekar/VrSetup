using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2Step1 : Step
{
    public bool rulerGrabbed;
    public bool jobGrabbed;

    public TaskBoardInfo StepOneInfo;
    public bool done;
    public void OnEnable()
    {
        base.OnEnable();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepOne);
    }

    public void ShowStepOne()
    {
        TaskBoard.ins.ShowInformation(StepOneInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);
        TaskBoard.ins.ProceedButton.onClick.AddListener(OnDoneClick);
    }

    public void OnDoneClick()
    {
        done = true;
    }
    public void OnRulerGrab()
    {
        if (isValid())
        {
            rulerGrabbed = true;
        }

    }

    public void OnJobGrabbed()
    {
        if (isValid())
        {
            jobGrabbed = true;
        }

    }

    private void Update()
    {
        base.Update();
        if (rulerGrabbed && jobGrabbed)
        {
            TaskBoard.ins.ProceedButton.enabled = true;
            TaskBoard.ins.ToggleButtonColor(true);
        }
        if (rulerGrabbed && jobGrabbed && done)
        {
            taskComplete = true;

        }
    }
}
