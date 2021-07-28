using UnityEngine;
using UnityEngine.Events;
using System;
using FlatWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Script created by Piyush M.
/// </summary>
public class Task : MonoBehaviour
{
    [SerializeField, Tooltip("A flag to check if the task is complete")]
    bool isTaskComplete = false;

    [SerializeField, Tooltip("The text that will be displayed on the start of the task")]
    string title, taskDetails;

    [SerializeField, Tooltip("A list of the outlines to turn on & off at the tast start & end respectively.")]
    internal List<Outline> outlines;
    public UnityEvent EventsOnTaskBegin, EventsOnTaskComplete;
    public bool IsTaskComplete { get => isTaskComplete; set => isTaskComplete = value; }
    public string Title { get => title; set => title = value; }
    public string TaskDetails { get => taskDetails; set => taskDetails = value; }

    public virtual void OnTaskBegin()
    {
        //Turning on highlights
        for (int i = 0; i < outlines.Count; i++)
            if (outlines[i] != null)
                outlines[i].enabled = true;
        EventsOnTaskBegin?.Invoke();
    }

    public virtual void OnTaskCompleted()
    {
        if (!isTaskComplete)
        {
            isTaskComplete = true;
            //Turning off highlights
            for (int i = 0; i < outlines.Count; i++)
                if (outlines[i] != null)
                    outlines[i].enabled = false;
            EventsOnTaskComplete?.Invoke();
            EnableButton();
        }
    }

    public void EnableButton()
    {
        Button button = PiyushUtils.TaskManager.Instance.confirmButton;
        button.gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        PiyushUtils.TaskManager.Instance.OnTaskCompleted(this);
    }
}
