using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Written by Piyush Mayekar
/// </summary>
namespace PiyushUtils
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField, Header("Json file to read Steps")] TextAsset jsonFile;
        [SerializeField] List<Task> tasks;
        [SerializeField, Range(0, 15)] int currentTaskIndex = 0;
        [SerializeField] TMPro.TextMeshProUGUI taskDetailsText;
        [SerializeField] public Button confirmButton;
        [SerializeField] Tablet tablet;
        ReadSteps readSteps;
        public static TaskManager Instance => instance;

        public int CurrentTaskIndex { get => currentTaskIndex; set => currentTaskIndex = value; }
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
        IEnumerator Start()
        {
            tablet = FindObjectOfType<Tablet>();
            taskDetailsText = tablet.taskDetailText;
            confirmButton = tablet.confirmButton;
            confirmButton.gameObject.SetActive(false);
            tasks = new List<Task>(GetComponentsInChildren<Task>());
            readSteps = JsonUtility.FromJson<ReadSteps>(jsonFile.text);
            yield return new WaitForSeconds(1f);
            StartTask(CurrentTaskIndex);
        }

        private void StartTask(int currentTaskIndex)
        {
            if (currentTaskIndex < tasks.Count) //List range check
            {
                Task task = tasks[currentTaskIndex];

                //Assigning the task title & details to the task displayer.
                taskDetailsText.text = currentTaskIndex==0 ? readSteps.ExperimentTitle : readSteps.Steps[currentTaskIndex-1];
                task.OnTaskBegin();
            }
        }

        internal void OnTaskCompleted(Task task)
        {
            confirmButton.gameObject.SetActive(false);
            CurrentTaskIndex++;
            if (CurrentTaskIndex < tasks.Count)
                StartTask(CurrentTaskIndex);
            else //Current Task index reached out of the task list index. ie. Tasks are done
            {
                // taskTitleText.text = string.Empty;
                taskDetailsText.text = "Congratulations you finished the job";
            }
        }
        [ContextMenu("Rename Tasks")]
        public void RenameChildTasks()
        {
            tasks = new List<Task>(GetComponentsInChildren<Task>());
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].gameObject.name = nameof(Task) + " " + i + " - " + tasks[i].Title;
            }
        }

        [ContextMenu("Complete current Task")]
        public void CompleteCurrentTask()
        {
            tasks[currentTaskIndex].OnButtonClick();
        }

    }
}