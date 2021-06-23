using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grinding
{
    public class Task_ScriberMarking : Task
    {
        [SerializeField] int platesScribed = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
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