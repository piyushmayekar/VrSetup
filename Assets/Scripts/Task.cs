using UnityEngine;
using System;
using FlatWelding;
using System.Collections;
using System.Collections.Generic;

public class Task : MonoBehaviour
{
    [SerializeField, Tooltip("A flag to check if the task is complete")]
    bool isTaskComplete = false;

    [SerializeField, Tooltip("The text that will be displayed on the start of the task")]
    string title, taskDetails;

    [SerializeField, Tooltip("A list of the highlight gameobjects to turn on at the task start")]
    internal List<GameObject> highlights;

    public bool IsTaskComplete { get => isTaskComplete; set => isTaskComplete = value; }
    public string Title { get => title; set => title = value; }
    public string TaskDetails { get => taskDetails; set => taskDetails = value; }

    public virtual void OnTaskBegin()
    {
        //Turning on highlights
        highlights.ForEach(x => x.gameObject.SetActive(true));
    }

    public virtual void OnTaskCompleted()
    {
        isTaskComplete = true;
        FlatWelding.TaskManager.Instance.OnTaskCompleted(this);
        //Turning off highlights
        highlights.ForEach(x => x.gameObject.SetActive(false));
    }
}
