using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class Task_Scriber : Task
    {
        [SerializeField] int plateIndex = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            JobPlate.jobPlates[plateIndex].StartScriberMarking();
            JobPlate.jobPlates.ForEach(plate => plate.OnScriberMarkingDone += OnScriberMarkingDone);
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
