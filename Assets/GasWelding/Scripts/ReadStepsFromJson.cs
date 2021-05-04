using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadStepsFromJson : MonoBehaviour
{
    [SerializeField, Header("Json file to read Steps")] TextAsset jsonFile;
     public ReadSteps readSteps;
    [SerializeField] int countStep;
    [SerializeField]TextMeshProUGUI stepText;
    // Start is called before the first frame update
    void Start()
    {
      /*  filePath = Application.dataPath + "/StreamingAssets/GasWelding.json";
        if (File.Exists(jsonFile))
        {*/
      if(jsonFile!=null)
        { 

            readSteps = JsonUtility.FromJson<ReadSteps>(jsonFile.text);
        }
        stepText.text = readSteps.ExperimentTitle;
    }
    public void LoadNextStepText()
    {if (readSteps.Steps.Length > countStep)
        {
            stepText.text = readSteps.Steps[countStep];
            countStep++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class ReadSteps
{
    public string ExperimentTitle;
    public string[] Steps;
 
}