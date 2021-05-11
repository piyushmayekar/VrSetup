using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E2Step4 : Step
{
    public BenchWise benchwise;
    public Job job;

    public TaskBoardInfo StepOneInfo;
    public TaskBoardInfo StepTwoInfo;
    public TaskBoardInfo CompleteInfo;

    public bool done;

    public void OnEnable()
    {
        base.OnEnable();
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepOne);
        TaskBoard.ins.ToggleButtonColor(false);
        TaskBoard.ins.ProceedButton.enabled = false;
    }

    public void ShowStepOne()
    {
        //First Cut
        TaskBoard.ins.ShowInformation(StepOneInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);
        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowStepTwo);

    }

    public void ShowStepTwo()
    {
        //SecondCut
        TaskBoard.ins.ShowInformation(StepTwoInfo, expManager.start.position, expManager.start.localEulerAngles, true, "Done");
        TaskBoard.ins.ProceedButton.enabled = false;
        TaskBoard.ins.ToggleButtonColor(false);

        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowOnComplete);
    }

    public void ShowOnComplete()
    {
        TaskBoard.ins.ShowInformation(CompleteInfo, expManager.start.position, expManager.start.localEulerAngles, false, "Restart");
        TaskBoard.ins.ProceedButton.enabled = true;
        TaskBoard.ins.ToggleButtonColor(true);

        TaskBoard.ins.ProceedButton.onClick.RemoveAllListeners();
        TaskBoard.ins.ProceedButton.onClick.AddListener(Restart);
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
       // expManager.currentTaskIndex = 4;
       // Job.doneMarking = true;
       //  benchwise.SetJob(job);
        // job.SetJobStateForTaskCutting();
    }


    public void OnJobGrabForCut()
    {
        if (isValid() && Job.doneMarking && !Job.firstCutDone)
        {
            benchwise.ShowJobSnapForHackSaw();
            Debug.Log("Job grabbed for First cut");

        }
    }

    private void Update()
    {
        if (Job.SecondCutDone)
        {
            //benchwise.HintText.text = "Collect the pieces and Remove the Job";
            // taskManager.ShowTaskInfo("Experiment Completed");
        }


        base.Update();
    }
}
