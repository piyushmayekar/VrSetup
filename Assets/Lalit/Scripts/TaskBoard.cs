using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TaskBoardInfo
{
    public string taskHeading;
    public int Step;
    public int SubStep;
    public string info;

}

public class TaskBoard : MonoBehaviour
{
    public static TaskBoard ins;

    public Text taskHeadingText;
    public Button ProceedButton;
    public Text StepText;
    public Text infoText;

    public Color activeColor;
    public Color startColor;

    public Text ButtonText;

    public void Awake()
    {

        ins = this;
    }

    private void Start()
    {
        ButtonText = ProceedButton.GetComponentInChildren<Text>();

    }

    public void ShowInformation(TaskBoardInfo info, Vector3 position, Vector3 rotation, bool showStep, string buttonText)
    {
        transform.position = position;
        transform.localEulerAngles = rotation;

        taskHeadingText.text = info.taskHeading;
        if (showStep)
        {
            StepText.text = string.Format("Step {0}.{1}", info.Step, info.SubStep);
        }
        else
        {
            StepText.text = "";
        }
        infoText.text = info.info;
        ProceedButton.GetComponentInChildren<Text>().text = buttonText;

    }

    public void ToggleButtonColor(bool active)
    {
        if (active)
        {
            ProceedButton.image.color = activeColor;
            ButtonText.color = Color.white;
        }
        else
        {
            ProceedButton.image.color = startColor;
            ButtonText.color = Color.black;

        }




    }
}
