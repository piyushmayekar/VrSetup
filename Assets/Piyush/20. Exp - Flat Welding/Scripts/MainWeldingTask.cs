using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    public class MainWeldingTask : Task
    {

        [SerializeField] GameObject tackingPointsParent, pointsParent;
        List<WeldingPoint> weldingPoints;

        int weldingDone = 0;
        public List<WeldingPoint> WeldingPoints { get => weldingPoints; set => weldingPoints = value; }

        public override void OnTaskBegin()
        {
            pointsParent.SetActive(true);
            WeldingPoints = new List<WeldingPoint>(pointsParent.GetComponentsInChildren<WeldingPoint>());
            WeldingPoints.ForEach(x =>
            {
                x.OnWeldingDone += OnPointWeldingDone;
            });
        }

        internal void OnPointWeldingDone()
        {
            weldingDone++;
            if (weldingDone >= WeldingPoints.Count)
            {
                tackingPointsParent.SetActive(false);
                OnTaskCompleted();
            }
        }
    }
}
