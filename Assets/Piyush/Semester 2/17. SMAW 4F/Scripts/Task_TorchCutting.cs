using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;

namespace Semester2
{
    public class Task_TorchCutting : Task
    {
        [SerializeField] int platesCount = 0;

        MechanicalVise mechanicalVise;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            platesCount = 0;
            mechanicalVise = FindObjectOfType<MechanicalVise>();
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