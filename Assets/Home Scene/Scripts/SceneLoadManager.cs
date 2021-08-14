using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField, Tooltip("Please enter scene name with match of experiment buttons number indexes")] string[] sceneNames;
    public GameObject quitPanel, menuButtonPanel;
    public void LoadSceneWithIndex(int SceneIndex)
    {

        SceneManager.LoadScene(sceneNames[SceneIndex]);

    }
    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OnPressXbutton()
    {
        quitPanel.SetActive(true);
        menuButtonPanel.SetActive(false);

    }
    public void onClickYesbutton()
    {
        Application.Quit();
    }
    public void onClickNoButton()
    {
        quitPanel.SetActive(false);
        menuButtonPanel.SetActive(true);
    }
}

