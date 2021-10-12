using CornerWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Semester2
{
    public class Task_TagWelding : Task
    {
        [SerializeField] WeldingMachine machine;
        [SerializeField] ElectrodeType requireElectrodeType = ElectrodeType._315mm;
        [SerializeField] Transform pointsParent;
        [SerializeField] GameObject weldingArea;
        [SerializeField] int weldingDoneOnPoints = 0;

        List<WeldingPoint> weldingPoints;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            machine.RequiredElectrodeType = requireElectrodeType;
            if (machine.CheckIfRequiredElectrodePlaced(requireElectrodeType))
                OnCorrectElectrodePlaced();
            else
                machine.OnElectrodePlacedEvent += OnCorrectElectrodePlaced;
        }

        void OnCorrectElectrodePlaced()
        {
            machine.OnElectrodePlacedEvent -= OnCorrectElectrodePlaced;
            weldingArea.SetActive(true);
            StartWeldingOnAPointsSet();
        }

        void StartWeldingOnAPointsSet()
        {
            pointsParent.gameObject.SetActive(true);
            weldingPoints = new List<WeldingPoint>(pointsParent.GetComponentsInChildren<WeldingPoint>());
            weldingPoints.ForEach(point => point.OnWeldingDone += OnWeldingDoneOnPoint);
        }

        void OnWeldingDoneOnPoint(WeldingPoint point)
        {
            weldingDoneOnPoints++;
            if (weldingDoneOnPoints >= weldingPoints.Count)
            {
                OnTaskCompleted();
            }
        }

        //Debug helper functions. Don't use anywhere else

        [ContextMenu("Skip To this step")]
        public void SkipToThisStep()
        {
            ToggleFinalPlates();
        }

        [ContextMenu(nameof(TurnOffFinalPlates))]
        public void TurnOffFinalPlates()
        {
            ToggleFinalPlates(false);
        }

        void ToggleFinalPlates(bool on=true)
        {
            const string platesFinal = "Final Plates";
            GameObject finalPlatesParent = GameObject.Find(platesFinal);
            for (int i = 0; i < finalPlatesParent.transform.childCount; i++)
                finalPlatesParent.transform.GetChild(i).gameObject.SetActive(on);
        }
    }
}