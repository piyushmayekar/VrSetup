using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    public int index;
    protected ExperimentManager expManager;

    public bool taskComplete = false;
    public TaskBoardInfo taskInfo;

    public Transform taskBoardTransform;
    protected void Awake()
    {
        expManager = GetComponent<ExperimentManager>();
    }

    protected void OnEnable()
    {
        if (expManager)
        {
            expManager.ShowTaskInfo(taskInfo, taskBoardTransform.position, taskBoardTransform.localEulerAngles, true, "Confirm");
            TaskBoard.ins.ToggleButtonColor(true);

        }

    }
    protected void Update()
    {
        if (taskComplete)
        {
            expManager.EnableNextTask();


        }
    }

    protected bool isValid()
    {
        if (index == expManager.currentTaskIndex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
