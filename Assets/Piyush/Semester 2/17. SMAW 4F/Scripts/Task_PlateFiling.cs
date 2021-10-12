using PiyushUtils;
using UnityEngine;

namespace Semester2
{
    public class Task_PlateFiling : Task
    {
        [SerializeField] int platesCount = 0;
        MechanicalVise mechanicalVise;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            mechanicalVise = FindObjectOfType<MechanicalVise>();
            mechanicalVise.ToggleFilingCanvas(true);
            JobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += OnPlateFilingDone);
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