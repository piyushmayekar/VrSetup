using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grinding
{
    public class Task_PlatesGrinding : Task
    {
        [SerializeField] int platesGrinded = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            platesGrinded = 0;
            IsTaskComplete = false;
        }

        public void OnGrindingComplete()
        {
            platesGrinded++;
            if (platesGrinded >= JobPlate.jobPlates.Count)
                OnTaskCompleted();
        }
    }
}