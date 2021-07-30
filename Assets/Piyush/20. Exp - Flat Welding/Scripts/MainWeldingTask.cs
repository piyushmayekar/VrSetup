using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    public class MainWeldingTask : Task
    {

        [SerializeField] GameObject tackingPointsParent, pointsParent;
        List<CornerWelding.WeldingPoint> weldingPoints;
        [SerializeField] GameObject weldingGunHighlight;
        int weldingDone = 0;
        public List<CornerWelding.WeldingPoint> WeldingPoints { get => weldingPoints; set => weldingPoints = value; }

        public override void OnTaskBegin()
        {
            pointsParent.SetActive(true);
            WeldingPoints = new List<CornerWelding.WeldingPoint>(pointsParent.GetComponentsInChildren<CornerWelding.WeldingPoint>());
            weldingGunHighlight.SetActive(true);
            WeldingPoints.ForEach(x =>
            {
                x.OnWeldingDone += OnPointWeldingDone;
            });
        }

        internal void OnPointWeldingDone(CornerWelding.WeldingPoint point)
        {
            weldingDone++;
            weldingGunHighlight.SetActive(false);
            if (weldingDone >= WeldingPoints.Count)
            {
                tackingPointsParent.SetActive(false);
                OnTaskCompleted();
            }
        }
    }
}
