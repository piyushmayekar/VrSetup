using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
public class E24Step4 : Step
{
    public bool done;
    public GameObject WeldSocket;
    public bool rootRun1, rootRun2;
    public WeldedJob job;
    public XRSocketInteractor socket;
    public Transform flipAnchor;
    public Transform FirstRootRunObject1, FirstRootRunObject2, SecondRootRunObject1, SecondRootRunObject2,SecondSlag1,SecondSlag2;
    public TaskBoardInfo StepOneInfo, StepTwoInfo,CompleteInfo;
    
    public bool flip;
    public void OnEnable()
    {
        base.OnEnable();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepOne);
    }

    public void ShowStepOne()
    {
        TaskBoard.ins.ShowInformation(StepOneInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        // TaskBoard.ins.ProceedButton.enabled = false;
        // TaskBoard.ins.ToggleButtonColor(false);
        ActivateFirstRootRunPoints();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepTwo);
    }

    private void ActivateFirstRootRunPoints()
    {
        foreach (Transform item in FirstRootRunObject1)
        {
            item.gameObject.SetActive(true);

        }

        foreach (Transform item in FirstRootRunObject2)
        {
            item.gameObject.SetActive(true);
        }
    }

    private void ActivateSecondRootPoints()
    {
        SecondSlag1.gameObject.SetActive(true);
        SecondSlag2.gameObject.SetActive(true);

        foreach (Transform item in SecondRootRunObject1)
        {
            item.gameObject.SetActive(true);
        }

        foreach (Transform item in SecondRootRunObject2)
        {
            item.gameObject.SetActive(true);
        }
    }

    public void ShowStepTwo()
    {
        TaskBoard.ins.ShowInformation(StepTwoInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        // TaskBoard.ins.ProceedButton.enabled = false;
        // TaskBoard.ins.ToggleButtonColor(false);
        ActivateSecondRootPoints();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(OnDoneClick);
    }

    public void OnDoneClick()
    {
        //done = true;
        TaskBoard.ins.ShowInformation(CompleteInfo, expManager.start.position, expManager.start.localEulerAngles, false, "Restart");
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(Restart);

        Debug.Log("Complete"); 
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        //if (job.measuredAngle)
        //{
        //    TaskBoard.ins.ProceedButton.enabled = true;
        //    TaskBoard.ins.ToggleButtonColor(true);
        //}

        //if (done)
        //{
        //    taskComplete = true;
        //}
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
}
