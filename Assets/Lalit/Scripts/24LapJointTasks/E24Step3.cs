using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class E24Step3 : Step
{
    public bool done;
    public GameObject WeldedJob, WeldSocket, WeldSocket2;
    public bool tagWeld1, tagWeld2;
    public WeldedJob job;
    public XRSocketInteractor socket;
    public Transform flipAnchor;

    public TaskBoardInfo StepOneInfo, StepTwoInfo,PPIKitInfo;

    public GameObject snapHL, snapHL1;

    public bool flip;
    public void OnEnable()
    {
        base.OnEnable();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowWearPPIKit);
    }
    public void ShowWearPPIKit()
    {
        TaskBoard.ins.ShowInformation(PPIKitInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        // TaskBoard.ins.ProceedButton.enabled = false;
        // TaskBoard.ins.ToggleButtonColor(false);
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepOne);
    }
    public void ShowStepOne()
    {
        TaskBoard.ins.ShowInformation(StepOneInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
       // TaskBoard.ins.ProceedButton.enabled = false;
       // TaskBoard.ins.ToggleButtonColor(false);
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

    void Start()
    {
        WeldedJob.SetActive(true);
        WeldSocket.SetActive(true);
        WeldSocket2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (job.measuredAngle)
        {
            TaskBoard.ins.ProceedButton.enabled = true;
            TaskBoard.ins.ToggleButtonColor(true);
        }

        if (done)
        {
            taskComplete = true;
        }
    }

    public void ChangeSocketAnchor()
    {
        flip = !flip;
        if (flip)
        {
            socket.attachTransform = flipAnchor;
        }
        else
        {
            socket.attachTransform = null;
        }
    }


    public void ToggleSnapHighlight(bool show)
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

    public void ToggleSnapHighlight1(bool show)
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

}
