using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E24Step2 : Step
{
    public bool done;

    public TaskBoardInfo StepOneInfo;
    
   

    public GameObject Socket1, Socket2;
    public GameObject Job, Job2;

    public GameObject snapHL, snapHL1;



    public void ActivateSecondSocket()
    {
        Socket2.SetActive(true);
    }

    public void OnSecondJobPlaced()
    {
        TaskBoard.ins.ProceedButton.enabled = true;
        TaskBoard.ins.ToggleButtonColor(true);
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (done)
        {
            taskComplete = true;
        }
    }

    public void OnEnable()
    {
        base.OnEnable();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepOne);
        Socket1.SetActive(true);
    }

    public void ShowStepOne()
    {
        TaskBoard.ins.ShowInformation(StepOneInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(OnDoneClick);
    }

    

    public void OnDoneClick()
    {
        done = true;
        Socket1.SetActive(false);
        Socket2.SetActive(false);
        Job.SetActive(false);
        Job2.SetActive(false);
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
