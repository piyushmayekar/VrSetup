using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Written by Piyush Mayekar
/// </summary>
namespace PiyushUtils
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField, Range(0, 20)] int currentTaskIndex = 0;
        [SerializeField] List<Task> tasks;
        [SerializeField] TMPro.TextMeshProUGUI taskDetailsText;
        [SerializeField] public Button confirmButton;
        [SerializeField] Tablet tablet;

        [Header("Language")]
        [SerializeField] _Language currentLanguage;
        [SerializeField, Header("Json file to read Steps")] List<TextAsset> jsonFiles;
        [SerializeField] TextLangManager textLangManager;
        List<ReadSteps> readSteps;
        public static TaskManager Instance => instance;

        public int CurrentTaskIndex { get => currentTaskIndex; set => currentTaskIndex = value; }
        public int CurrentLangIndex => (int)currentLanguage;

        private string[] indexGuj = new string[] { "Ò", "Ó", "Ô", "Õ", "Ö", "×", "Ø", "Ù", "Ú", "Û" };

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
            currentLanguage = FetchCurrentLanguage();
            SetTaskInfoFont();
            readSteps = new List<ReadSteps>(textLangManager.readSteps);
            tablet.OnLanguageButtonClick.AddListener(OnLanguageButtonClick);
            yield return new WaitForSeconds(1f);
            StartTask(CurrentTaskIndex);
        }

        private void StartTask(int currentTaskIndex)
        {
            if (currentTaskIndex < tasks.Count) //List range check
            {
                Task task = tasks[currentTaskIndex];

                //Assigning the task title & details to the task displayer.
                SetTaskInfoTextAccToLanguage(currentTaskIndex);
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
                taskDetailsText.text = CurrentLangIndex==0 ? "Congratulations you finished the job" : "Aiwn>dn tme p/yog smaPt kyoR";
            }
        }

        private void SetTaskInfoTextAccToLanguage(int currentTaskIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (currentTaskIndex == 0)
                sb.Append(readSteps[CurrentLangIndex].ExperimentTitle);
            else
            {
                sb.Append(GetStepIndex(currentTaskIndex));
                sb.AppendLine(readSteps[CurrentLangIndex].Steps[currentTaskIndex - 1]);
            }
            taskDetailsText.text = sb.ToString();
        }
        private string GetStepIndex(int cntNum)
        {
            //cntNum++;
            if (CurrentLangIndex==0)
            {
                return "Step - " + cntNum + "\n";
            }
            else
            {
                string stepNumString = "";
                if (cntNum > 9)
                {
                    List<int> temp = new List<int>();
                    while (cntNum > 0)
                    {
                        temp.Add(cntNum % 10);
                        cntNum = cntNum / 10;
                    }
                    temp.Reverse();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        stepNumString += indexGuj[temp[i]];
                    }
                }
                else
                {
                    stepNumString = indexGuj[cntNum];
                }
                return "pglu> à " + stepNumString + "\n";
            }
        }


        _Language FetchCurrentLanguage()
        {
            return (_Language)PlayerPrefs.GetInt(nameof(_Language), (int)_Language.English);
        }

        void SaveCurrentLanguageToMemory()
        {
            PlayerPrefs.SetInt(nameof(_Language), (int)currentLanguage);
        }

        void OnLanguageButtonClick()
        {
            int totalLanguagesCount = Enum.GetNames(typeof(_Language)).Length;
            int nextLanguageIndex = (CurrentLangIndex + 1) % totalLanguagesCount;
            currentLanguage = (_Language)nextLanguageIndex;

            SaveCurrentLanguageToMemory();
            SetTaskInfoFont();
            SetTaskInfoTextAccToLanguage(currentTaskIndex);
        }

        private void SetTaskInfoFont()
        {
            taskDetailsText.font = tablet.languagesFont[CurrentLangIndex];
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
            tasks[currentTaskIndex].EventsOnTaskComplete?.Invoke();
            tasks[currentTaskIndex].OnButtonClick();
        }

        [ContextMenu("Import English Steps To SO")]
        public void ImportEnglishSteps() => ImportStepsToSO((int)(_Language.English));

        [ContextMenu("Import Gujrati Steps To SO")]
        public void ImportGujratiSteps() => ImportStepsToSO((int)(_Language.Gujrati));

        public void ImportStepsToSO(int languageIndex = 0)
        {
            ReadSteps _readSteps = JsonUtility.FromJson<ReadSteps>(jsonFiles[languageIndex].text);

            for (int i = 0; i < _readSteps.Steps.Length; i++)
            {
                string info = _readSteps.Steps[i];
                int index = info.IndexOf("\n");
                info = info.Substring(index + 2);
                _readSteps.Steps[i] = info;
            }
            textLangManager.readSteps[languageIndex] = _readSteps;
        }
    }
}

[Serializable]
public enum _Language
{
    English, Gujrati
}