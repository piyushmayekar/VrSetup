using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grinding
{
    public class Task_CenterPunchMarking : Task
    {
        [SerializeField] int platesMarked = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            platesMarked = 0;
            IsTaskComplete = false;
            JobPlate.jobPlates[platesMarked].StartCenterPunchMarking();
            JobPlate.jobPlates.ForEach(x =>
            {
                x.OnCenterPunchMarkingDone += () =>
                {
                    platesMarked++;
                    if (platesMarked >= JobPlate.jobPlates.Count)
                        OnTaskCompleted();
                    else
                        JobPlate.jobPlates[platesMarked].StartCenterPunchMarking();
                };
            });
        }
    }
}
