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

    public string sceneName;

    public void Initialize(string _sceneName)
    {
        sceneName = _sceneName;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = sceneName;
    }
    
    public void OnButtonClick()
    {
        OnButtonClickEvent?.Invoke();
    }
}
