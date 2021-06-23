using System.Collections;
using System.Collections.Generic;
using CornerWelding;
using UnityEngine;
using UnityEngine.Events;

namespace LapWelding
{
    public class Task_TagWelding : Task
    {
        public UnityEvent OnTagWeldingDoneOnASide;
        [SerializeField] int pointCounter = 0, parentCounter = 0;
        [SerializeField] List<Transform> pointsParents;
        [SerializeField] WeldingMachine machine;
        [SerializeField] ElectrodeType electrodeType;
        List<WeldingPoint> weldingPoints;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            if (machine.CheckIfRequiredElectrodePlaced(electrodeType))
                TogglePointsParent();
            else
            {
                machine.RequiredElectrodeType = electrodeType;
                machine.OnElectrodePlacedEvent += TogglePointsParent;
            }
        }

        void OnPointWeldingDone(WeldingPoint point)
        {
            pointCounter++;
            if (pointCounter >= weldingPoints.Count)
            {
                parentCounter++;
                if (parentCounter < pointsParents.Count)
                {
                    OnTagWeldingDoneOnASide?.Invoke();
                    pointCounter = 0;
                    TogglePointsParent();
                }
                else
                    OnTaskCompleted();
            }
        }

        void TogglePointsParent()
        {
            pointsParents[parentCounter].gameObject.SetActive(true);
            weldingPoints = new List<WeldingPoint>(
                pointsParents[parentCounter].GetComponentsInChildren<WeldingPoint>());
            weldingPoints.ForEach(point => point.OnWeldingDone += OnPointWeldingDone);
            machine.OnElectrodePlacedEvent -= TogglePointsParent;
        }
    }
}