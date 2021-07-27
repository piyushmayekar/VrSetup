using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class TackingTask : Task
    {
        [SerializeField] GameObject pointsParent;
        List<WeldingPoint> weldingPoints;
        int weldingDone = 0;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            pointsParent.SetActive(true);
            weldingPoints = new List<WeldingPoint>(pointsParent.GetComponentsInChildren<WeldingPoint>());
            weldingPoints.ForEach(x =>
            {
                x.OnWeldingDone += OnWeldingDone;
            });
        }

        internal void OnWeldingDone()
        {
            weldingDone++;
            if (weldingDone >= weldingPoints.Count)
                OnTaskCompleted();
        }
    }
}