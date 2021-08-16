using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.UI.Button;
using System;

public class HomeManager : MonoBehaviour
{
    [Header("ADD SCENE NAMES HERE")]
    [Tooltip("Add Scenes names here which will be loaded. DON'T FORGET TO ADD THOSE SCENES IN BUILD MENU")]
    public List<string> sceneNames;
    
    [Header("DON'T CHANGE THESE VARIABLES")]
    [SerializeField, Tooltip("The button that enables user to click on it & load an experiment scene")] 
    GameObject expSelectorButtonPrefab;

    [SerializeField] Transform selectorPanel;

    SceneLoadManager sceneLoadManager;


    void Start()
    {
        sceneLoadManager = GetComponent<SceneLoadManager>();
        for (int i = 0; i < sceneNames.Count; i++)
        {
            GameObject buttonGO = Instantiate(expSelectorButtonPrefab, selectorPanel);
            ExpSelectorButton expSelector = buttonGO.GetComponentInChildren<ExpSelectorButton>();
            expSelector.Initialize(sceneNames[i]);
            expSelector.OnButtonClickEvent.AddListener(() => OnExpSelectorButtonClick(expSelector));
        }
    }

    private void OnExpSelectorButtonClick(ExpSelectorButton expSelector)
    {
        sceneLoadManager.LoadSceneWithName(expSelector.sceneName);
    }
}
