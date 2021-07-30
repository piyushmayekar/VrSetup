using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class Task_TorchCutting : Task
    {
        [SerializeField] int platesCount = 0;
        [SerializeField] int knobIndex = -1;

        PiyushUtils.SMAW_MechanicalVise mechanicalVise;
        List<Knob> knobs;
        List<float> targetValues=new List<float>() { 0f, 0f };

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
            if (knobIndex > 0) knobs[knobIndex].OnTargetValueReached.RemoveAllListeners();
            knobIndex++;
            if (knobIndex >= knobs.Count)
            {
                CuttingTorch.Instance.ToggleSwitch(false);
                mechanicalVise.ShouldAllowFiling = true;
                mechanicalVise.ToggleFilingCanvas();
                platesCount = 0;
                JobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += OnPlateFilingDone);
            }
            else
            {
                knobs[knobIndex].EnableTurning(targetValues[knobIndex], false);
                knobs[knobIndex].OnTargetValueReached.AddListener(TurnOffTorchStep);
            }
        }

        void OnPlateFilingDone()
        {
            platesCount++;
            if (platesCount == JobPlate.jobPlates.Count)
            {
                mechanicalVise.OnTaskCompleted();
                OnTaskCompleted();   
            }
        }
    }
}