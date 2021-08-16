using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ReadStepsAndVideoManager : MonoBehaviour
{
    public static ReadStepsAndVideoManager instance;
    /*   [Header("---------------------------------------------------")]
       [SerializeField, Header("Json file to read Steps, Attach diffrent languages file")] TextAsset[] languagesJson;
       public TextAsset jsonFile;*/
    /*[Header("---------------------------------------------------")]
    public ReadSteps englishSteps, gujaratiSteps;*/
    // ReadSteps readSteps;
    [Header("---------------------------------------------------")]
    public TextLangManager langManager;
    [Header("---------------------------------------------------")]
    public GameObject panel;
    public GameObject tablet, languageButton;
    [SerializeField] int countStep;

    public TextMeshProUGUI stepText, languageText;
    [SerializeField] public Button confirmbtn;
    public string sceneName;
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

    private string[] indexGuj = new string[] { "р", "с", "т", "у", "ж", "в", "ь", "ы", "з", "ш" };
    public _Language currentLanguage = _Language.Gujrati;


    //"ср", "сс","ст","су","сж","св","сь","сы","сз","сш","тр"};  // ***** ANKITA CHANGES *****


    void Awake()
    {
        instance = this;
        isStep = false;
        isChangeFont = true;
        OnClickLanguagesBtn();
        
    }
    void LoadNextStepText()
    {
        if (langManager._stepsText.Steps.Length > countStep)
        {
            isStep = true;
            stepText.text = GetStepIndex(countStep); // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
            stepText.text += langManager._stepsText.Steps[countStep]; // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
            countStep++;
        }
    }
    #region Load all Step on Tablet
    public void OnClickLanguagesBtn()
    {
        isChangeFont = !isChangeFont;
        if (isChangeFont) //english font load
        {
            stepText.font = languagesFont[0];
            langManager._stepsText = langManager.readSteps[0];
          //  stepText.fontSize = 15.5f;
            //stepText.autoSizeTextContainer = true;
        }
        else //Gujarati font load
        {
            stepText.font = languagesFont[1];
           // stepText.fontSize = 17.5f;
          //  stepText.autoSizeTextContainer = false;
            langManager._stepsText = langManager.readSteps[1];
        }
        if (!isStep)
        {
           
                stepText.text = langManager._stepsText.ExperimentTitle;
            


        }
        else
        {
            if (isBrushCleaning)
            {
                Debug.Log("call clean brush");
                CuttingBrush.instance.BrushFontChanage();
            }
            else
            {
                if (langManager._stepsText.Steps.Length >= countStep)
                {
                    Debug.Log("LM callimh;");
                    stepText.text = GetStepIndex(countStep - 1); // ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
                    stepText.text += langManager._stepsText.Steps[countStep - 1];// ***************************** CHANGES DONE HERE ***** ANKITA CHANGES *****
                }
            }
        }
    }
    // Get Step Number title... ***** ANKITA CHANGES *****
    private string GetStepIndex(int cntNum)
    {
        cntNum++;
        if (isChangeFont)
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
            return "pglu> Ю " + stepNumString + "\n";
        }
    }

    // END OF CHANGE ***** ANKITA CHANGES *****

    /// <summary>
    ///Load new text or msg on canvas.
    /// </summary>
    public void onClickConfirmbtn()
    {
        LoadNextStepText();
    }
    /// <summary>
    ///Add Canvas Button Click Event <paramref name="args"/>.
    /// </summary>
    /// <param name="callBtnclickEvent">Pass the method to set in button click.</param>
    public void AddClickConfirmbtnEvent(OnClickBtnEvent callBtnclickEvent)
    {

        tablet.SetActive(true);

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
[System.Serializable]
public class ReadSteps
{
    public string ExperimentTitle;
    public string[] Steps;

}