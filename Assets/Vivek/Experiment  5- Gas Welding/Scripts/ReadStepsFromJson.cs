using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ReadStepsFromJson : MonoBehaviour
{
    public static ReadStepsFromJson instance;
    [SerializeField, Header("Json file to read Steps")] TextAsset jsonFile;
    public ReadSteps readSteps;
    public GameObject panel;
    [SerializeField] int countStep;
    public TextMeshProUGUI stepText;
    [SerializeField] public Button confirmbtn;
    public string sceneName;
    public delegate void OnClickBtnEvent();
    public GameObject tablet;

    void Awake()
    {
        instance = this;
        if (jsonFile != null)
        {
            readSteps = JsonUtility.FromJson<ReadSteps>(jsonFile.text);
        }
        stepText.text = readSteps.ExperimentTitle;
    }
    void LoadNextStepText()
    {
        if (readSteps.Steps.Length > countStep)
        {
            stepText.text = readSteps.Steps[countStep];
            countStep++;
        }
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
        //   if (jsonFile.name == "GasWelding")
        {
            tablet.SetActive(true);
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
        //  if (jsonFile.name == "GasWelding")
        {
            tablet.SetActive(false);
        }
        confirmbtn.gameObject.SetActive(false);
    }


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