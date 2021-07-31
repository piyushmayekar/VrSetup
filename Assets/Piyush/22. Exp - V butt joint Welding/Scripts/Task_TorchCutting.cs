using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VWelding
{
    public class Task_TorchCutting : Task
    {
        [SerializeField] GameObject requiredPlatePrefab, bevelledPlatePrefab;
        [SerializeField] int platesCount = 0;
        [SerializeField] int knobIndex = -1;

        PiyushUtils.SMAW_MechanicalVise mechanicalVise;
        List<Knob> knobs;
        List<float> targetValues = new List<float>() { 0f, 0f };


        //1. Perform Cutting
        //2. Turn Off Torch
        //3. Perform Filing

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            knobIndex = -1;
            platesCount = 0;
            mechanicalVise = FindObjectOfType<MechanicalVise>();
            mechanicalVise.OnTaskBegin();
            knobs = new List<Knob>(CuttingTorch.Instance.GetComponentsInChildren<Knob>());
            JobPlate.jobPlates.ForEach(p => Debug.Log(p.name));
            JobPlate.jobPlates.ForEach(plate => plate.OnGasCuttingDone += () =>
            {
                OnGasCuttingDone();
            });
        }

        private void OnGasCuttingDone()
        {
            platesCount++;
            if (platesCount == JobPlate.jobPlates.Count)
            {
                platesCount = 0;
                JobPlate.jobPlates.ForEach(plate => plate.OnGasCuttingDone -= OnGasCuttingDone);
                TurnOffTorchStep();
            }
        }

        public void TurnOffTorchStep()
        {
            if (knobIndex >= 0) knobs[knobIndex].OnTargetValueReached.RemoveAllListeners();
            knobIndex++;
            if (knobIndex >= knobs.Count)
            {
                CuttingTorch.Instance.ToggleSwitch(false);
                mechanicalVise.ShouldAllowFiling = true;
                mechanicalVise.ToggleFilingCanvas();
                platesCount = 0;
                JobPlate.jobPlates.ForEach(p=>Debug.Log(p.name));
                JobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += ()=> { 
                    OnJobFilingDone(plate as JobPlate); 
                });
            }
            else
            {
                knobs[knobIndex].EnableTurning(targetValues[knobIndex], false);
                knobs[knobIndex].OnTargetValueReached.AddListener(TurnOffTorchStep);
            }
        }
        void OnJobFilingDone(JobPlate plate)
        {
            mechanicalVise.ShouldSwitchAttachTransforms = false;
            plate.gameObject.SetActive(false);
            GameObject prefabToSpawn = null;
            if (plate.PlateType == PlateType.Length || plate.PlateType == PlateType.Breadth)
                prefabToSpawn = requiredPlatePrefab;
            else if (plate.PlateType == PlateType.Required)
                prefabToSpawn = bevelledPlatePrefab;
            GameObject iPlate = Instantiate(prefabToSpawn, plate.transform.position, plate.transform.rotation);
            platesCount++;
            JobPlate.jobPlates.Remove(plate);

            if (platesCount == JobPlate.jobPlates.Count)
            {
                mechanicalVise.OnTaskCompleted();
                OnTaskCompleted();
            }
            Invoke(nameof(TurnOnViseSwitch), 1f);
        }

        void TurnOnViseSwitch() => mechanicalVise.ShouldSwitchAttachTransforms = true;
    }
}