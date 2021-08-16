using FlatWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMAW_Setup_4
{
    /// <summary>
    /// And Attach electrode to welding gun
    /// </summary>
    public class Task_SetCurrent : Task
    {
        [SerializeField] CurrentKnob currentKnob;
        [SerializeField] float finalCurrent = 110f;

        [SerializeField] List<Outline> weldingHighlights;
        [SerializeField] WeldingMachine weldingMachine;
        [SerializeField] ElectrodeType requiredElectrodeType;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            currentKnob.TargetValue = finalCurrent;
            CurrentKnob.OnTargetValueSet += OnCurrentSet;
            currentKnob.GetComponent<Outline>().enabled = true;
        }

        void OnCurrentSet()
        {
            CurrentKnob.OnTargetValueSet -= OnCurrentSet;
            currentKnob.GetComponent<Outline>().enabled = false;
            if (weldingMachine.CheckIfRequiredElectrodePlaced(requiredElectrodeType))
                OnTaskCompleted();
            else
            {
                weldingHighlights.ForEach(h => h.enabled = true);
                weldingMachine.RequiredElectrodeType = requiredElectrodeType;
                weldingMachine.OnElectrodePlacedEvent += OnElectrodeAttached;
            }
        }

        void OnElectrodeAttached()
        {
            weldingHighlights.ForEach(h => h.enabled = false);
            weldingMachine.OnElectrodePlacedEvent -= OnElectrodeAttached;
            OnTaskCompleted();
        }
    }
}