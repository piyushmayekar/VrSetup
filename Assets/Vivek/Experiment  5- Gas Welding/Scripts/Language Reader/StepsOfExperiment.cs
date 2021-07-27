using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class only contais the languages that will be on the game

public class Language
{
    private Language(string value) { Value = value; }

    public string Value { get; private set; }

    public static Language English { get { return new Language("English"); } }
    public static Language Gujarati { get { return new Language("Gujarati"); } }
  
}

[CreateAssetMenu(fileName = "Experiment Steps", menuName = "Scriptable Objects/ExperimetntSteps")]
public class StepsOfExperiment : ScriptableObject
{
   // public ReadSteps readSteps;

}
