using CornerWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Semester2.Exp19
{
    public class Task_TagWelding : Task
    {
        [SerializeField] int plateIndex = 0;
        [SerializeField] ElectrodeType requiredElectrodeType;
        
        List<FusedPlates> fusedPlatesList;
        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            fusedPlatesList = new List<FusedPlates>(FindObjectsOfType<FusedPlates>(true));
            machine = WeldingMachine.Instance;
            machine.RequiredElectrodeType = requiredElectrodeType;
            bool electrodePlaced = machine.CheckIfRequiredElectrodePlaced(requiredElectrodeType);
            if (!electrodePlaced) machine.OnElectrodePlacedEvent += OnTaskDone;
        }

        void OnTaskDone()
        {
            plateIndex++;
            if (plateIndex >= fusedPlatesList.Count)
                OnTaskCompleted();
            else
            {
                fusedPlatesList[plateIndex].StartTagWelding();
                fusedPlatesList[plateIndex].OnTagWeldingDone.AddListener(OnTaskDone);
            }
        }

        [ContextMenu(nameof(SkipToThisStep))]
        public void SkipToThisStep()
        {
            fusedPlatesList = new List<FusedPlates>(FindObjectsOfType<FusedPlates>(true));
            fusedPlatesList.ForEach(fusedPlate =>
            {
                fusedPlate.finalPlates.ForEach(plate => plate.SetActive(true));
                fusedPlate.finalSpacers.ForEach(spacer => spacer.SetActive(true));
            });
            Transform electrodeSocket = FindObjectOfType<WeldingMachine>().socket_Electrode.attachTransform;
            Electrode electrode = null;
            var electrodes = FindObjectsOfType<Electrode>();
            foreach (var item in electrodes)
            {
                if (item.ElectrodeType == ElectrodeType._315mm)
                    electrode = item;
            }
            electrode.transform.SetPositionAndRotation(electrodeSocket.position, electrodeSocket.rotation);
        }

        [ContextMenu("Reduce Welding Points Timer")]
        public void ReduceWeldingTimer()
        {
            List<WeldingPoint> weldingPoints = new List<WeldingPoint>(FindObjectsOfType<WeldingPoint>(true));
            weldingPoints.ForEach(point => point.WeldingTimer = 0.1f);
        }
    }
}