using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;

public class A_ReadStepsAndVideoManager : MonoBehaviour
{
    public static A_ReadStepsAndVideoManager instance;
    /*   [Header("---------------------------------------------------")]
       [SerializeField, Header("Json file to read Steps, Attach diffrent languages file")] TextAsset[] languagesJson;
       public TextAsset jsonFile;*/
    /*[Header("---------------------------------------------------")]
    public ReadSteps englishSteps, gujaratiSteps;*/
    // ReadSteps readSteps;
    public string sceneName;
    [Header("---------------------------------------------------")]
    public TextLangManager langManager;
    [Header("---------------------------------------------------")]
    public GameObject panel;
    public GameObject tablet, languageButton;
    [SerializeField] int countStep;

    public TextMeshProUGUI stepText, languageText;
    [SerializeField] public Button confirmbtn;
    public delegate void OnClickBtnEvent();
    [Header("---------------------------------------------------")]
    [SerializeField, Header("Attach TMP_FontAsset Refernce as index of languages json file")] TMP_FontAsset[] languagesFont;
    [Header("-------------------------video object--------------------------")]
    [SerializeField, Header("Attach VideoClip index of json steps")] VideoClip[] videoClips;
    public VideoPlayer videoPlayer;
    public GameObject videoPlayRawImage;
    public Button videoPlayBtn;
    [HideInInspector]
    public bool isChangeFont, isStep, isBrushCleaning;

    private string[] indexGuj = new string[] { "?", "?", "?", "?", "?", "?", "?", "?", "?", "?" };
    //"??", "??","??","??","??","??","??","??","??","??","??"};  // ***** ANKITA CHANGES *****
    [Header("Language")]
    public _Language currentLanguage;
    public int CurrentLangIndex => (int)currentLanguage;

    public TextMeshProUGUI stepTextWall;
    public TextLangManager namesText;
    public bool isOnlyText;
    public int newIndex;

    [Header("---------------------------------------------------")]
    public A_AudioManagerWithLanguage audioWithStep;
    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        instance = this;
        isStep = false;
        isOnlyText = false;
        // isStep = true;

        //if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
        //    || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
        //    || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
        //    || SceneManager.GetActiveScene().name.Contains("Quiz"))
        //{
        //    tablet.SetActive(false);
        //}
    }
    private void Start()
    {
        //countStep = 3;
        currentLanguage = FetchCurrentLanguage();
        //  Debug.Log("Current languages" + currentLanguage);
        TitleTextLoad();
    }
    void TitleTextLoad()
    {
        if (CurrentLangIndex == (int)_Language.English)//  if (isChangeFont) //english font load
        {
            if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
            || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
            || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
            {
                stepTextWall.font = languagesFont[0];
                namesText._stepsText = namesText.readSteps[0];
            }
            stepText.font = languagesFont[0];
            langManager._stepsText = langManager.readSteps[0];
        }
        else //Gujarati font load
        {
            if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
               || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
               || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
            {
                stepTextWall.font = languagesFont[1];
                namesText._stepsText = namesText.readSteps[1];
            }
            stepText.font = languagesFont[1];
            langManager._stepsText = langManager.readSteps[1];
        }
        // load audio with step title text 
        audioWithStep.PlayStepAudio(countStep);
        stepText.text = langManager._stepsText.ExperimentTitle; 
        
        if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
             || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
             || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
        {
            stepTextWall.text = langManager._stepsText.ExperimentTitle;
        }
    }

    public void PPEKit(int index)
    {
        newIndex = index;
        isOnlyText = true;
        if (CurrentLangIndex == (int)_Language.English)//  if (isChangeFont) //english font load
        {
            if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
                  || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
                  || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
                    || SceneManager.GetActiveScene().name.Contains("Quiz"))
            {
                stepTextWall.font = languagesFont[0];
                namesText._stepsText = namesText.readSteps[0];
            }
            stepText.font = languagesFont[0];
            langManager._stepsText = langManager.readSteps[0];
        }
        else //Gujarati font load
        {
            if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
                     || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
                     || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
                    || SceneManager.GetActiveScene().name.Contains("Quiz"))
            {
                stepTextWall.font = languagesFont[1];
                namesText._stepsText = namesText.readSteps[1];
            }
            stepText.font = languagesFont[1];
            langManager._stepsText = langManager.readSteps[1];
        }

        stepText.text = namesText._stepsText.Steps[index];
        stepTextWall.text = namesText._stepsText.Steps[index];
    }

    #region Save Language in memory

    _Language FetchCurrentLanguage()
    {
        return (_Language)PlayerPrefs.GetInt(nameof(_Language), (int)_Language.Gujrati);
    }

    void SaveCurrentLanguageToMemory()
    {
        PlayerPrefs.SetInt(nameof(_Language), (int)currentLanguage);
    }

    #endregion
    #region Load all Step on Tablet
    void LoadNextStepText()
    {
        isOnlyText = false;
        if (langManager._stepsText.Steps.Length > countStep)
        {
            if (SceneManager.GetActiveScene().name.Contains("15"))
            {
                Experiment15FlowManager.instance.cntSteps = countStep;
            }
            else if (SceneManager.GetActiveScene().name.Contains("14"))
            {
                Experiment14FlowManager.instance.cntSteps = countStep;
            }
            else if (SceneManager.GetActiveScene().name.Contains("13"))
            {
                ExperimentFlowManager.instance.cntSteps = countStep;
            }
            else if (SceneManager.GetActiveScene().name.Contains("Quiz"))
            {
                Exp2Quiz.instance.cntSteps = countStep;
            }
            else if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - "))
            {
                Exp1Manager.instance.cntSteps = countStep;
            }
            else if (SceneManager.GetActiveScene().name.Contains("Experiment 2 - "))
            {
                Exp2Manager.instance.cntSteps = countStep;
            }
            else if (SceneManager.GetActiveScene().name.Contains("Experiment 3 - "))
            {
                Exp3Manager.instance.cntSteps = countStep;
            }

            isStep = true;
            stepText.text = GetStepIndex(countStep); // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
            stepText.text += langManager._stepsText.Steps[countStep]; // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****

            if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
                  || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
                  || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
            {
                stepTextWall.text = GetStepIndex(countStep); // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
                stepTextWall.text += langManager._stepsText.Steps[countStep]; // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
            }

            // load audio with step text (for first is play title so heare +1 index pass)
            audioWithStep.PlayStepAudio(countStep + 1);
            // Debug.Log("**** Audio Index " + (countStep + 1));
            countStep++;
        }
    }
    public void OnClickLanguagesBtn()
    {
        int totalLanguagesCount = Enum.GetNames(typeof(_Language)).Length;
        int nextLanguageIndex = (CurrentLangIndex + 1) % totalLanguagesCount;
        currentLanguage = (_Language)nextLanguageIndex;
        stepText.font = languagesFont[(int)currentLanguage]; 
        
        if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
                   || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
                   || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
        {
            stepTextWall.font = languagesFont[(int)currentLanguage];
        }

        langManager._stepsText = langManager.readSteps[(int)currentLanguage];

        if (!isStep)
        {
            stepText.text = langManager._stepsText.ExperimentTitle; 
            if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
                    || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
                    || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
            {
                stepTextWall.text = langManager._stepsText.ExperimentTitle;
            }
        }
        else
        {
            if (langManager._stepsText.Steps.Length >= countStep)
            {
                stepText.text = GetStepIndex(countStep - 1); // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
                stepText.text += langManager._stepsText.Steps[countStep - 1];// ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
                if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
                    || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
                    || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
                {
                    stepTextWall.text = GetStepIndex(countStep - 1); // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
                    stepTextWall.text += langManager._stepsText.Steps[countStep - 1];// ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
                    
                }
            }
        }

        if (isOnlyText)
        {
            PPEKit(newIndex);
        }
        SaveCurrentLanguageToMemory();
    }
    // Get Step Number title... ***** ANKITA CHANGES *****
    private string GetStepIndex(int cntNum)
    {
        cntNum++;
        if (CurrentLangIndex == 0)// if (isChangeFont)
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
            return "pglu> ? " + stepNumString + "\n";
        }
    }

    // END OF CHANGE ***** ANKITA CHANGES *****

    /// <summary>
    ///Load new text or msg on canvas.
    /// </summary>
    public void onClickConfirmbtn()
    {
        videoPlayBtn.gameObject.SetActive(false);
        LoadNextStepText();
    }
    /// <summary>
    ///Add Canvas Button Click Event <paramref name="args"/>.
    /// </summary>
    /// <param name="callBtnclickEvent">Pass the method to set in button click.</param>
    public void AddClickConfirmbtnEvent(OnClickBtnEvent callBtnclickEvent)
    {
        if (SceneManager.GetActiveScene().name.Contains("Experiment 1 - ")
            || SceneManager.GetActiveScene().name.Contains("Experiment 2 - ")
            || SceneManager.GetActiveScene().name.Contains("Experiment 3 - ")
            || SceneManager.GetActiveScene().name.Contains("Quiz"))
        {
            //tablet.SetActive(false);
        }

        confirmbtn.gameObject.SetActive(true);
        confirmbtn.onClick.RemoveAllListeners();
        confirmbtn.onClick.AddListener(() => callBtnclickEvent());
    }
    /// <summary>
    ///Hide canvas button when you click.
    /// </summary>
    public void HideConifmBnt()
    {

        tablet.SetActive(false);

        confirmbtn.gameObject.SetActive(false);
    }

    #endregion
    #region Video Manager
    /// <summary>
    ///Video play using index of video clip button when you click.
    /// </summary>
    /// <param name="indexOfClip">Pass the index of video clip in button click.</param>
    void OnClickVideoPlayBtn(int indexOfClip)
    {
        languageButton.SetActive(false);
        videoPlayer.clip = videoClips[indexOfClip];
        videoPlayRawImage.SetActive(true);
    }
    /// <summary>
    ///Add Canvas Button Click Event <paramref name="args"/>.
    /// </summary>
    /// <param name="indexOfClip">Pass the index of video clip in button click.</param>
    public void AddClickEventVideoPlay(int indexOfClip)
    {
        videoPlayBtn.gameObject.SetActive(true);
        videoPlayBtn.onClick.RemoveAllListeners();
        videoPlayBtn.onClick.AddListener(() => OnClickVideoPlayBtn(indexOfClip));
    }
    #endregion
    /// <summary>
    ///Retry  scene. in this method the "sceneName" string variable load scene to pass in this variable.
    /// </summary>
    public void onClickRetryButton()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void onClickHomeButton(string homeScene)
    {
        SceneManager.LoadScene(homeScene);
    }
}
