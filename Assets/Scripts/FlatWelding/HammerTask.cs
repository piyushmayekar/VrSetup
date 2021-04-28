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
                if (point.IsExteriorWeldingPoint)
                {
                    exteriorPointsCount++;
                    point.OnHitWithHammer += OnExteriorPointHit;
                }
            });
        }

        public void OnExteriorPointHit()
        {
            highlights.ForEach(x => x.gameObject.SetActive(false));
            pointsHit++;
            if (pointsHit >= exteriorPointsCount)
                OnTaskCompleted();
        }

    }
}