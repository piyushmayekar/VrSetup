using CornerWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2.Exp19
{
    public class Task_WeldingRun : Task
    {
        public FusedPlates platesToPerformWeldingOn;
        [SerializeField] FlatWelding.CurrentKnob knob;
        [SerializeField] float targetCurrentValue = 116f;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] bool currentSet = false, electrodePlaced = false;
        [SerializeField] GameObject highlight_WeldingMachine, diagram;
        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            diagram.SetActive(true);
            
            //If welding machine does not have the required electrode attached, wait for it to be attached
            machine = WeldingMachine.Instance;
            machine.RequiredElectrodeType = requiredElectrodeType;
            electrodePlaced = machine.CheckIfRequiredElectrodePlaced(requiredElectrodeType);
            if (!electrodePlaced) machine.OnElectrodePlacedEvent += OnElectrodePlaced;

            //If current as per required, wait for it to be set
            knob.TargetValue = targetCurrentValue;
            if (knob.CurrentValue != targetCurrentValue)
            {
                FindObjectOfType<FlatWelding.CurrentKnob>().ToggleOutline();
                FlatWelding.CurrentKnob.OnTargetValueSet += OnKnobTargetValueSet;
            }
            else
                currentSet = true;

            //If all conditions are met OnTaskBegin, start the welding
            CheckIfMiniTasksCompleted();
        }

        void OnElectrodePlaced()
        {
            electrodePlaced = true;
            machine.OnElectrodePlacedEvent -= OnElectrodePlaced;
            CheckIfMiniTasksCompleted();
        }

        void OnKnobTargetValueSet()
        {
            currentSet = true;
            FlatWelding.CurrentKnob.OnTargetValueSet -= OnKnobTargetValueSet;
            CheckIfMiniTasksCompleted();
        }

        /// <summary>
        /// Check if the current & electrode is appropriate
        /// </summary>
        void CheckIfMiniTasksCompleted()
        {
            if (currentSet && electrodePlaced) InitializeEverything();
        }

        void InitializeEverything()
        {
            platesToPerformWeldingOn.StartRootWelding();
            platesToPerformWeldingOn.OnRootRunWeldingDone.AddListener(OnRootRunWeldingDone);
            highlight_WeldingMachine.SetActive(true);
            platesToPerformWeldingOn.OnRootRunPointWeldingDone.AddListener(OnRootRunPointWeldingDone);
        }

        void OnRootRunPointWeldingDone()
        {
            highlight_WeldingMachine.SetActive(false);
        }

        void OnRootRunWeldingDone()
        {
            diagram.SetActive(false);
            OnTaskCompleted();
        }

        public void SetWeldingGunElectrodeObtuse()
        {
            FindObjectOfType<WeldingMachine>().fauxElectrode.transform.localEulerAngles = new Vector3(30f, 0f, 0f);
        }

        public void SetWeldingGunElectrodeAcute()
        {
            FindObjectOfType<WeldingMachine>().fauxElectrode.transform.localEulerAngles = new Vector3(-15f, 0f, 0f);
        }

        //Debug Functions

        [ContextMenu(nameof(SkipToThisStep))]
        public void SkipToThisStep()
        {
            var fusedPlatesList = new List<FusedPlates>(FindObjectsOfType<FusedPlates>(true));
            fusedPlatesList.ForEach(fusedPlate =>
            {
                fusedPlate.finalPlates.ForEach(plate => plate.SetActive(true));
                fusedPlate.plateState = FusedPlates.PlateState.ReadyForRootRunWelding;
                fusedPlate.GetComponent<XRGrabInteractable>().enabled = true;
            });

            if (platesToPerformWeldingOn == null)
                platesToPerformWeldingOn = fusedPlatesList[0];
            machine = WeldingMachine.Instance;
            Transform electrodeSocket = FindObjectOfType<WeldingMachine>().socket_Electrode.attachTransform;
            Electrode electrode = null;
            var electrodes = FindObjectsOfType<Electrode>();
            foreach (var item in electrodes)
            {
                if (item.ElectrodeType == requiredElectrodeType)
                    electrode = item;
            }
            electrode.transform.SetPositionAndRotation(electrodeSocket.position, electrodeSocket.rotation);

            OnElectrodePlaced();
            OnKnobTargetValueSet();
            CheckIfMiniTasksCompleted();
            ReduceWeldingTimer();
        }

        [ContextMenu("Reduce Welding Points Timer")]
        public void ReduceWeldingTimer()
        {
            List<WeldingPoint> weldingPoints = new List<WeldingPoint>(FindObjectsOfType<WeldingPoint>(true));
            weldingPoints.ForEach(point => point.WeldingTimer = 0.1f);
        }
    }
}