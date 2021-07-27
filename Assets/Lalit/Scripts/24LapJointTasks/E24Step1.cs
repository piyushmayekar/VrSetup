using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class E24Step1 : Step
{
    public WeldJob JobA, JobB;

    public TaskBoardInfo StepOneInfo;
    public TaskBoardInfo StepTwoInfo;
    public TaskBoardInfo StepThreeInfo;
    public GameObject snapHL,snapHL1, snapHL2;
    
    public bool done;

    public bool measurementDone;
    public bool fillingDone;
    public bool markingdone;

    public GameObject CuttingSocket;
     
    public void OnEnable()
    {
        base.OnEnable();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepOne);
    }

    private void Update()
    {
        base.Update();
       
        if (JobA.measeured && JobB.measeured && !measurementDone)
        {
            TaskBoard.ins.ProceedButton.enabled = true;
            TaskBoard.ins.ToggleButtonColor(true);
            CuttingSocket.SetActive(true);
            measurementDone = true;

        }

        if (JobA.filled && JobB.filled && !fillingDone)
        {
            TaskBoard.ins.ProceedButton.enabled = true;
            TaskBoard.ins.ToggleButtonColor(true);
            fillingDone = true;
        }

        if (JobA.markingDone && JobB.markingDone && !markingdone)
        {
            TaskBoard.ins.ProceedButton.enabled = true;
            TaskBoard.ins.ToggleButtonColor(true);
            markingdone = true;
        }

        if (done)
        {
            taskComplete = true;
        }

       
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
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepThree);
    }

    public void ShowStepThree()
    {
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

    public void ToggleFillingHighlight1(bool show)
    {
        if (show)
        {
            snapHL1.SetActive(true);
        }
        else
        {
            snapHL1.SetActive(false);
        }
    }

    public void ToggleMarkingHighlight(bool show)
    {
        if (show)
        {
            snapHL.SetActive(true);
        }
        else
        {
            snapHL.SetActive(false);
        }
    }


    public void ToggleCuttingHighlight(bool show)
    {
        if (show)
        {
            snapHL2.SetActive(true);
        }
        else
        {
            snapHL2.SetActive(false);
        }
    }

    
}
