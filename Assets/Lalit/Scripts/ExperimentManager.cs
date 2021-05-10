using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class ExperimentManager : MonoBehaviour
{
    public Step[] Steps;


    public GameObject LeftHand;
    public GameObject RightHand;


    private InputDevice LeftController;
    private InputDevice RightController;

    public Transform start;
    public TaskBoardInfo Title;
    public TaskBoardInfo Equipments;
    public TaskBoardInfo JobMaterial;

    public int currentTaskIndex = 0;

    public Bottle bottle;
    public BenchWise benchWise;

    private void Start()
    {
        ShowTaskInfo(Title, start.position, start.localEulerAngles, false, "Next");
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowEquipmentList);
        TaskBoard.ins.ToggleButtonColor(true);
    }

    private void Update()
    {
        TryGetController();


        if (LeftController.isValid)
        {
            if (LeftController.TryGetFeatureValue(CommonUsages.trigger, out float value))
            {
                if (value > 0.2f)
                {
                    if (benchWise.HasControl)
                    {
                        //Move forward
                        benchWise.moveForward = true;
                        benchWise.moveBackward = false;
                    }
                }

            }

            if (LeftController.TryGetFeatureValue(CommonUsages.grip, out float value1))
            {
                if (value1 > 0.2f)
                {
                    if (benchWise.HasControl)
                    {
                        //Move Back
                        benchWise.moveForward = false;
                        benchWise.moveBackward = true;
                    }
                }

            }


        }

        if (RightController.isValid)
        {
            if (RightController.TryGetFeatureValue(CommonUsages.trigger, out float value))
            {
                if (value > 0.2f)
                {
                    if (bottle.enableWorking)
                    {
                        bottle.StartDropping();
                    }


                }
            }
        }
    }
    private void TryGetController()
    {
        HandPresence leftHandController = LeftHand.GetComponentInChildren<HandPresence>();
        HandPresence rightHandController = RightHand.GetComponentInChildren<HandPresence>();

        if (leftHandController)
        {
            LeftController = leftHandController.Controller;
        }

        if (rightHandController)
        {
            RightController = rightHandController.Controller;
        }

    }

    public void ShowTaskInfo(TaskBoardInfo info, Vector3 position, Vector3 rotation, bool showStep, string buttonText)
    {
        TaskBoard.ins.ShowInformation(info, position, rotation, showStep, buttonText);
    }

    public void ShowEquipmentList()
    {
        ShowTaskInfo(Equipments, start.position, start.localEulerAngles, false, "Next");
        TaskBoard.ins.ProceedButton.onClick.AddListener(ShowMaterialInfo);

    }

    public void ShowMaterialInfo()
    {
        ShowTaskInfo(JobMaterial, start.position, start.localEulerAngles, false, "Start");
        TaskBoard.ins.ProceedButton.onClick.AddListener(EnableTask);

    }

    public void EnableTask()
    {

        Step task = GetTask(currentTaskIndex);
        if (task)
        {
            task.enabled = true;
            currentTaskIndex++;

        }

    }

    public Step GetTask(int index)
    {
        if (Steps[index] != null)
        {
            return Steps[index];
        }
        else
        {
            Debug.Log("Task finished");
            return null;
        }
    }

    public void EnableNextTask()
    {
        Steps[currentTaskIndex - 1].enabled = false;
        EnableTask();



    }

}



