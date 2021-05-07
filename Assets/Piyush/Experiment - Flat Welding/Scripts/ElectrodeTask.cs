using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class ElectrodeTask : Task
    {
        [SerializeField] WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            if (machine.IsElectrodePlaced) //If electrode is already placed
                OnTaskCompleted();
            else
                machine.OnElectrodePlacedEvent += OnTaskCompleted;
        }

    }
}