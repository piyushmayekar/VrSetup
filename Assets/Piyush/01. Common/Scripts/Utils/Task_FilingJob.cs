using System.Collections;
using UnityEngine;

namespace PiyushUtils
{
    public class Task_FilingJob : Task
    {
        [SerializeField] int platesCount = 0;
        SMAW_MechanicalVise mechanicalVise;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            mechanicalVise = FindObjectOfType<SMAW_MechanicalVise>();
            mechanicalVise.ToggleFilingCanvas(true);
            SMAWJobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += OnPlateFilingDone);
        }

        void OnPlateFilingDone()
        {
            platesCount++;
            if (platesCount == SMAWJobPlate.jobPlates.Count)
            {
                mechanicalVise.OnTaskCompleted();
                OnTaskCompleted();
            }
        }
    }
}