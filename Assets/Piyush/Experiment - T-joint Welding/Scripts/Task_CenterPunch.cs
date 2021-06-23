using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class Task_CenterPunch : Task
    {
        [SerializeField, Tooltip("Number of plates center punch marking is done on")]
        int plateCount = 0;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            plateCount = 0;
            JobPlate.jobPlates[plateCount].StartCenterPunchMarking();
            JobPlate.jobPlates.ForEach(plate => plate.OnCenterPunchMarkingDone += () =>
            {
                plateCount++;
                if (plateCount >= JobPlate.jobPlates.Count)
                    OnTaskCompleted();
                else
                    JobPlate.jobPlates[plateCount].StartCenterPunchMarking();
            });
        }
    }
}
