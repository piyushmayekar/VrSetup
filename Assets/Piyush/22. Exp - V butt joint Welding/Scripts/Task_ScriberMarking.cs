using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VWelding
{
    public class Task_ScriberMarking : Task
    {
        [SerializeField] int platesScribed = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            IsTaskComplete = false;
            platesScribed = 0;
            JobPlate.jobPlates[platesScribed].StartScriberMarking();
            JobPlate.jobPlates.ForEach(x => x.OnScriberMarkingDone += () =>
            {
                platesScribed++;
                if (platesScribed >= JobPlate.jobPlates.Count)
                    OnTaskCompleted();
                else
                    JobPlate.jobPlates[platesScribed].StartScriberMarking();
            });
        }

    }
}
