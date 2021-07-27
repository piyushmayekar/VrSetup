using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class Task_Measurement : Task
    {
        [SerializeField] List<PiyushUtils.PlateMeasurement> plateMeasurements;
        [SerializeField] int index = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            plateMeasurements = new List<PiyushUtils.PlateMeasurement>(FindObjectsOfType<PiyushUtils.PlateMeasurement>());
            plateMeasurements[index].StartMeasurement();
            plateMeasurements[index].OnMeasurementDone.AddListener(OnPlateMeasurementDone);
        }

        void OnPlateMeasurementDone()
        {
            index++;
            if (index >= plateMeasurements.Count)
                OnTaskCompleted();
            else
            {
                plateMeasurements[index].StartMeasurement();
                plateMeasurements[index].OnMeasurementDone.AddListener(OnPlateMeasurementDone);
            }

        }

    }
}