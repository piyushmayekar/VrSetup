using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class Task_Hacksaw : Task
    {
        [SerializeField] int platesCount = 0;
        [SerializeField] MechanicalVise mechanicalVise;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            mechanicalVise.OnTaskBegin();
            JobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += () =>
            {
                string s = platesCount.ToString();
                platesCount++;
                if (platesCount == JobPlate.jobPlates.Count)
                    OnTaskCompleted();
            });
        }

        public override void OnTaskCompleted()
        {
            base.OnTaskCompleted();
            mechanicalVise.OnTaskCompleted();
        }

    }
}