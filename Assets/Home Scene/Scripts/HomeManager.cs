using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.UI.Button;
using System;

public class HomeManager : MonoBehaviour
{
    public static event Action<_Language> OnLanguageChange;

    [Header("ADD SCENE NAMES HERE")]
    [Tooltip("Add the scriptable object here of each experiment which will be loaded.\nDON'T FORGET TO ADD THOSE SCENES IN BUILD MENU")]
    public List<TextLangManager> textLangManagers = new List<TextLangManager>();

    public _Language currentLanguage = _Language.Gujrati;

    [Header("DON'T CHANGE THESE VARIABLES")]
    [SerializeField, Tooltip("The button that enables user to click on it & load an experiment scene")] 
    GameObject expSelectorButtonPrefab;

    [SerializeField] Transform selectorPanel;

    List<ExpSelectorButton> expSelectorButtons = new List<ExpSelectorButton>();
    SceneLoadManager sceneLoadManager;


    void Start()
    {
        sceneLoadManager = GetComponent<SceneLoadManager>();
        currentLanguage = PiyushUtils.TaskManager.FetchCurrentLanguage();
        for (int i = 0; i < textLangManagers.Count; i++)
        {
            GameObject buttonGO = Instantiate(expSelectorButtonPrefab, selectorPanel);
            ExpSelectorButton expSelector = buttonGO.GetComponentInChildren<ExpSelectorButton>();
            expSelector.Initialize(textLangManagers[i], currentLanguage);
            expSelector.OnButtonClickEvent.AddListener(() => OnExpSelectorButtonClick(expSelector));
            expSelectorButtons.Add(expSelector);
        }
    }

    private void OnExpSelectorButtonClick(ExpSelectorButton expSelector)
    {
        sceneLoadManager.LoadSceneWithName(expSelector.sceneName);
    }

    public void OnLanguageChangeButtonClick()
    {
        int totalLanguagesCount = Enum.GetNames(typeof(_Language)).Length;
        int CurrentLangIndex = (int)currentLanguage;
        int nextLanguageIndex = (CurrentLangIndex + 1) % totalLanguagesCount;
        currentLanguage = (_Language)nextLanguageIndex;

        PiyushUtils.TaskManager.SaveCurrentLanguageToMemory(currentLanguage);

        OnLanguageChange?.Invoke(currentLanguage);
    }
}
