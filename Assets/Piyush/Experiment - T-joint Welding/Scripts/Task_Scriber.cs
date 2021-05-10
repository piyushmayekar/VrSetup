using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class Task_Scriber : Task
    {
        [SerializeField] int platesScribed = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            JobPlate.jobPlates.ForEach(x => x.StartScriberMarking());
            JobPlate.jobPlates.ForEach(x => x.OnScriberMarkingDone += () =>
            {
                platesScribed++;
                if (platesScribed >= JobPlate.jobPlates.Count)
                    OnTaskCompleted();
            });
        }

    }
}
