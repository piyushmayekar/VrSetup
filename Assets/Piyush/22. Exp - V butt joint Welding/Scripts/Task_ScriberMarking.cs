using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VWelding
{
    public class Task_ScriberMarking : Task
    {
        [SerializeField] int platesScribed = 0;
        List<PiyushUtils.SMAWJobPlate> jobPlates;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            IsTaskComplete = false;
            platesScribed = 0;
            jobPlates = JobPlate.jobPlates;
            jobPlates[platesScribed].StartScriberMarking();
            jobPlates.ForEach(x => x.OnScriberMarkingDone+=OnScriberMarkingDone);
        }

        public void OnScriberMarkingDone()
        {
                platesScribed++;
                if (platesScribed >= jobPlates.Count)
                    OnTaskCompleted();
                else
                    jobPlates[platesScribed].StartScriberMarking();
        }

    }
}
