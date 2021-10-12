using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Written by Piyush Mayekar
/// </summary>
namespace PiyushUtils
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] float startDelay = 1f;
        [SerializeField, Range(0, 25)] int currentTaskIndex = 0;
        [SerializeField] List<Task> tasks;
        [SerializeField] TMPro.TextMeshProUGUI taskDetailsText;
        [SerializeField] public Button confirmButton;
        [SerializeField] internal Tablet tablet;

        [Header("Language")]
        [SerializeField] _Language currentLanguage;
        [SerializeField, Header("Json file to read Steps")] List<TextAsset> jsonFiles;
        [SerializeField] TextLangManager textLangManager;
        [SerializeField] VoiceOverDataHolder voiceOverData;
        
        AudioSource _voAudioSource;
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
            Task.taskManager = this;
            tasks = new List<Task>(GetComponentsInChildren<Task>());
            currentLanguage = FetchCurrentLanguage();
            SetTaskInfoFont();
            readSteps = new List<ReadSteps>(textLangManager.readSteps);
            tablet.OnLanguageButtonClick.AddListener(OnLanguageButtonClick);
            _voAudioSource = gameObject.AddComponent<AudioSource>();
            _voAudioSource.spatialBlend = 0f;
            yield return new WaitForSeconds(startDelay);
            StartTask(CurrentTaskIndex);
        }

        private void StartTask(int currentTaskIndex)
        {
            if (currentTaskIndex < tasks.Count) //List range check
            {
                Task task = tasks[currentTaskIndex];

                //Assigning the task title & details to the task displayer.
                SetTaskInfoTextAccToLanguage();

                PlayStepVO();
                confirmButton.gameObject.SetActive(true);
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(new UnityEngine.Events.UnityAction(OnConfirmButtonClick));
                task.OnTaskBegin();
            }
        }

        void OnConfirmButtonClick()
        {
            confirmButton.gameObject.SetActive(false);
            tablet.SetTabletOnState(false);
        }

        internal void OnTaskCompleted(Task task)
        {
            CurrentTaskIndex++;
            if (CurrentTaskIndex < tasks.Count)
                StartTask(CurrentTaskIndex);
            else //Current Task index reached out of the task list index. ie. Tasks are done
            {
                tablet.confirmButton.gameObject.SetActive(false);
                tablet.relearnButton.gameObject.SetActive(true);
                tablet.homeButton.gameObject.SetActive(true);
                tablet.relearnButton.onClick.AddListener(OnRelearnButtonClick);
                tablet.homeButton.onClick.AddListener(OnHomeButtonClick);
                SetTaskInfoTextAccToLanguage();
            }
            tablet.SetTabletOnState(true);
        }

        private void SetTaskInfoTextAccToLanguage()
        {
            if (CurrentTaskIndex >= tasks.Count) //All tasks finish
            {
                taskDetailsText.text = CurrentLangIndex == 0 ? "Congratulations you finished the job" : "Aiwn>dn tme p/yog smaPt kyoR";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                if (CurrentTaskIndex == 0)
                    sb.Append(readSteps[CurrentLangIndex].ExperimentTitle);
                else
                {
                    sb.Append(GetStepIndex(CurrentTaskIndex));
                    sb.AppendLine(readSteps[CurrentLangIndex].Steps[CurrentTaskIndex - 1]);
                }
                taskDetailsText.text = sb.ToString();
            }
        }

        public void PlayStepVO()
        {
            if (CurrentTaskIndex < 0 || CurrentTaskIndex >= tasks.Count) return;
            if (voiceOverData != null)
            {
                if (_voAudioSource != null)
                {
                    if (_voAudioSource.isPlaying) 
                        _voAudioSource.Stop();
                    if (voiceOverData.voDatas[CurrentLangIndex] != null && voiceOverData.voDatas[CurrentLangIndex].data.Count>0)
                    {
                        List<AudioClip> currentLanguageVO = voiceOverData.voDatas[CurrentLangIndex].data;
                        if (currentLanguageVO != null && CurrentTaskIndex < currentLanguageVO.Count)
                            _voAudioSource.PlayOneShot(currentLanguageVO[CurrentTaskIndex]);
                    }
                }
            }
            else
            {
                Debug.Log("Voice over data not assigned");
            }
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
                //return "";
            }
        }


        public static _Language FetchCurrentLanguage()
        {
            return (_Language)PlayerPrefs.GetInt(nameof(_Language), (int)_Language.Gujrati);
        }

        public static void SaveCurrentLanguageToMemory(_Language currentLang)
        {
            PlayerPrefs.SetInt(nameof(_Language), (int)currentLang);
        }

        void OnLanguageButtonClick()
        {
            int totalLanguagesCount = Enum.GetNames(typeof(_Language)).Length;
            int nextLanguageIndex = (CurrentLangIndex + 1) % totalLanguagesCount;
            currentLanguage = (_Language)nextLanguageIndex;
            SaveCurrentLanguageToMemory(currentLanguage);
            SetTaskInfoFont();
            SetTaskInfoTextAccToLanguage();
            PlayStepVO();
        }

        void OnRelearnButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void OnHomeButtonClick()
        {
            SceneManager.LoadScene("Home Scene");
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