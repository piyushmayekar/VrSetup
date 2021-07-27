using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{
    public class TurnKnobTask : Task
    {
        [SerializeField] List<Knob> knobs;
        [SerializeField] List<float> targetValues;
        [SerializeField] int index = -1;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            Step();
        }

        public void Step()
        {
            if (index > 0) knobs[index].OnTargetValueReached.RemoveAllListeners();
            index++;
            if (index >= knobs.Count)
            {
                OnTaskCompleted();
            }
            else
            {
                knobs[index].EnableTurning(targetValues[index], true);
                knobs[index].OnTargetValueReached.AddListener(Step);
            }
        }
    }
}