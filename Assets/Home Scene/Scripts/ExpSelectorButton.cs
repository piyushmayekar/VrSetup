using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Container for scene loading data
/// </summary>
public class ExpSelectorButton : MonoBehaviour
{
    public UnityEvent OnButtonClickEvent;
    TextLangManager textLangManager;

    public TMPro.TextMeshProUGUI buttonText;
    public TMPro.TMP_FontAsset[] languagesFont;
    public string sceneName;

    private void Awake()
    {
        HomeManager.OnLanguageChange += OnLanguageChange;
    }

    public void Initialize(TextLangManager manager, _Language currentLanguage)
    {
        textLangManager = manager;
        sceneName = manager.expSceneName;
        OnLanguageChange(currentLanguage);
    }

    void OnLanguageChange(_Language language)
    {
        int langIndex = (int)language;
        buttonText.font = languagesFont[langIndex];
        if (language == _Language.English) //Removing everything after \n
        {
            string str = textLangManager.readSteps[langIndex].ExperimentTitle;
            int indexOfN = str.IndexOf("\n");
            if (indexOfN > 0) //If \n is found
                str = str.Substring(0, indexOfN);
            buttonText.text = str;
        }
        else
        {
            buttonText.text = textLangManager.readSteps[langIndex].ExperimentTitle;
        }
    }
    
    public void OnButtonClick()
    {
        OnButtonClickEvent?.Invoke();
    }
}
