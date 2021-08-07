using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using FlatWelding;
using UnityEngine;
using UnityEngine.UI;

namespace TWelding
{
    public class Task_TackWelding : Task
    {
        [SerializeField] WeldingMachine machine;
        [SerializeField] ElectrodeType requireElectrodeType = ElectrodeType._315mm;
        [SerializeField] XRGrabInteractable jobPlatesGrabInteractable;
        [SerializeField] List<WeldingPoint> weldingPoints;
        [SerializeField] int weldingDoneOnPoints = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            weldingPoints[0].transform.parent.gameObject.SetActive(true);
            weldingPoints.ForEach(point => point.OnWeldingDone += () =>
            {
                weldingDoneOnPoints++;
                if (weldingDoneOnPoints >= weldingPoints.Count)
                {
                    OnTaskCompleted();
                    jobPlatesGrabInteractable.enabled = true;
                }
            });
            machine.CheckIfRequiredElectrodePlaced(requireElectrodeType);
        }

    }
}