using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Piyush Mayekar
/// </summary>
namespace FlatWelding
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] List<Task> tasks;
        [SerializeField, Range(0, 10)] int currentTaskIndex = 0;
        [SerializeField] TMPro.TextMeshProUGUI taskTitleText, taskDetailsText;
        public static TaskManager Instance => instance;
        #region SINGLETON
        static TaskManager instance = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null && instance != this)
                Destroy(gameObject);
        }
        #endregion

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            tasks = new List<Task>(GetComponentsInChildren<Task>());
            StartTask(currentTaskIndex);
        }

        private void StartTask(int currentTaskIndex)
        {
            if (currentTaskIndex < tasks.Count) //List range check
            {
                Task task = tasks[currentTaskIndex];

                //Assigning the task title & details to the task displayer.
                taskTitleText.text = task.Title;
                taskDetailsText.text = task.TaskDetails;
                task.OnTaskBegin();
            }
        }

        internal void OnTaskCompleted(Task task)
        {
            currentTaskIndex++;
            if (currentTaskIndex < tasks.Count)
                StartTask(currentTaskIndex);
            else //Current Task index reached out of the task list index. ie. Tasks are done
            {
                taskTitleText.text = string.Empty;
                taskDetailsText.text = "Congratulations you finished the job";
            }
        }
    }
}