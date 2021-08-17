using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMAW_Setup_4
{
    public class Task_SlagCleanup : Task
    {
        [SerializeField] List<CornerWelding.WeldingPoint> weldingPoints;
        [SerializeField] int count = 0;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            weldingPoints = new List<CornerWelding.WeldingPoint>(FindObjectsOfType<CornerWelding.WeldingPoint>());
            weldingPoints.ForEach(point => point.OnHitWithHammer += Point_OnHitWithHammer);
            count = weldingPoints.Count;
        }

        private void Point_OnHitWithHammer()
        {
            count--;
            if (count <= 0)
                OnTaskCompleted();
        }

        [ContextMenu(nameof(SkipToThisStep))]
        public void SkipToThisStep()
        {
            List<Task_WeldingRun> runs = new List<Task_WeldingRun>(FindObjectsOfType<Task_WeldingRun>());
            runs.ForEach(run =>
            {
                run.pointsParent.gameObject.SetActive(true);
                var points = new List<CornerWelding.WeldingPoint>(run.pointsParent.GetComponentsInChildren<CornerWelding.WeldingPoint>());
                points.ForEach(p => p.OnWeldingTimerFinish());
            });
        }
    }
}