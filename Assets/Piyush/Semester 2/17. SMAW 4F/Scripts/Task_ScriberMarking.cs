using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Semester2
{
    public class Task_ScriberMarking : Task
    {
        [SerializeField] int plateIndex = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            List<JobPlate> plates = JobPlate.jobPlates;
            plates[plateIndex].StartScriberMarking();
            plates.ForEach(plate => plate.OnScriberMarkingDone += OnScriberMarkingDone);
        }

        void OnScriberMarkingDone()
        {
            plateIndex++;
            if (plateIndex >= JobPlate.jobPlates.Count)
                OnTaskCompleted();
            else
                JobPlate.jobPlates[plateIndex].StartScriberMarking();
        }
    }
}