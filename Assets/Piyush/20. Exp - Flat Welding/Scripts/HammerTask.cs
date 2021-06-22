using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class HammerTask : Task
    {
        [SerializeField] MainWeldingTask weldingTask;
        [SerializeField] int exteriorPointsCount = 0, pointsHit = 0;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            weldingTask.WeldingPoints.ForEach(point =>
            {
                point.OnHitWithHammer += OnWeldingPointHit;
            });
        }

        public void OnWeldingPointHit()
        {
            highlights.ForEach(x => x.gameObject.SetActive(false));
            pointsHit++;
            if (pointsHit >= weldingTask.WeldingPoints.Count)
                OnTaskCompleted();
        }

    }
}