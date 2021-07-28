using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "StepsMaker", order = 1)]
public class TextLangManager : ScriptableObject
{
 //   public string[] languages= new string[2] {"English", "Gujarati"};
 [Header("First add english text and second add gujarati text")]
    public ReadSteps[] readSteps;
    [HideInInspector]
    public ReadSteps _stepsText;
}
