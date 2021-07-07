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
    public void LoadSceneWithIndex(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);

    }
    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void onClickToQuit()
    {
        Application.Quit();
    }
  
}

