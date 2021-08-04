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

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            knobIndex = -1;
            platesCount = 0;
            mechanicalVise = FindObjectOfType<SMAW_MechanicalVise>();
            mechanicalVise.OnTaskBegin();
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
                OnTaskCompleted();
            }
        }

        [ContextMenu("Debug Mode")]
        public void DebugMode()
        {
            FindObjectOfType<CuttingTorch>().ToggleSwitch();
            List<CuttingPoint> points = new List<CuttingPoint>(FindObjectsOfType<CuttingPoint>(true));
            points.ForEach(p => p.timer = .2f);
        }
        
    }
}