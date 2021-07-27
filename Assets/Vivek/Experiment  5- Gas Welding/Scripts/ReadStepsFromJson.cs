using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ReadStepsFromJson : MonoBehaviour
{
    public static ReadStepsFromJson instance;
    [Header("---------------------------------------------------")]
    [SerializeField, Header("Json file to read Steps, Attach diffrent languages file")] TextAsset[] languagesJson;
    public TextAsset jsonFile;
    [Header("---------------------------------------------------")]
    public ReadSteps englishSteps, gujaratiSteps;
    ReadSteps readSteps;
    [Header("---------------------------------------------------")]
    public GameObject panel;
    [SerializeField] int countStep;
    public TextMeshProUGUI stepText;
    [SerializeField] public Button confirmbtn;
    public string sceneName;
    public delegate void OnClickBtnEvent();
    public GameObject tablet;
    [Header("---------------------------------------------------")]
    [SerializeField, Header("Attach TMP_FontAsset Refernce as index of languages json file")] TMP_FontAsset[] languagesFont;
    [Header("-------------------------video object--------------------------")]
    [SerializeField, Header("Attach VideoClip index of json steps")] VideoClip[] videoClips;
    public VideoPlayer videoPlayer;
    public GameObject videoPlayRawImage;
    public Button videoPlayBtn;

    public bool isChangeFont;
    public bool isStep;
    void Awake()
    {
        instance = this;
        /*if (jsonFile != null)
        {
            readSteps = JsonUtility.FromJson<ReadSteps>(jsonFile.text);
        }*/
        isStep = false;
        OnClickLanguagesBtn();
       // stepText.text = readSteps.ExperimentTitle;
    }
    void LoadNextStepText()
    {
        if (readSteps.Steps.Length > countStep)
        {
            isStep = true;
            stepText.text = readSteps.Steps[countStep];
            countStep++;
        }
    }   
    #region Json manager and Load all Step on Tablet
    public void OnClickLanguagesBtn()
    {
        isChangeFont = !isChangeFont;
        if (isChangeFont) //english font load
        {
           /// jsonFile = languagesJson[0];
           stepText.font = languagesFont[0];
            readSteps = englishSteps;
        }
        else //Gujarati font load
        {
        //    jsonFile = languagesJson[1];
            stepText.font = languagesFont[1];
            readSteps = gujaratiSteps;
        }
        if (!isStep)
        {
            stepText.text = readSteps.ExperimentTitle;
        }
        else
        {
            if (CuttingBrush.instance.isCleaning)
            {
                CuttingBrush.instance.BrushFontChanage();
            }


            else
            {
                if (readSteps.Steps.Length > countStep)
                {
                    stepText.text = readSteps.Steps[countStep - 1];
                }
            }
        }
        /* if (jsonFile != null)
         {
             readSteps = JsonUtility.FromJson<ReadSteps>(jsonFile.text);
         }*/

        // stepText.text = readSteps.ExperimentTitle;
        /* jsonFile = languagesJson[index];
         stepText.font = languagesFont[index];

         if (jsonFile != null)
         {
             readSteps = JsonUtility.FromJson<ReadSteps>(jsonFile.text);
         }*/
    }
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
    public void onClickRetryButton()
    {
        SceneManager.LoadScene(sceneName);
    }

}
[System.Serializable]
public class ReadSteps
{
    public string ExperimentTitle;
    public string[] Steps;

}