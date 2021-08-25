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
    public GameObject quitPanel, menuButtonPanel;

    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void OnPressXbutton()
    {
        quitPanel.SetActive(true);
        menuButtonPanel.SetActive(false);

    }
    public void OnClickYesbutton()
    {
        Application.Quit();
    }
    public void OnClickNoButton()
    {
        quitPanel.SetActive(false);
        menuButtonPanel.SetActive(true);
    }
}

