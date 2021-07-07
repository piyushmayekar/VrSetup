using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class BrushTask : Task
    {
        [SerializeField, Tooltip("Total no of cleaning points")]
        int cleanPointCount = 15;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            List<CleanPoint> points = new List<CleanPoint>(FindObjectsOfType<CleanPoint>());
            points.ForEach(point => point.OnBrushBristleEnter += EdgeBrushed);
            cleanPointCount = points.Count;
        }

        internal void EdgeBrushed()
        {
            cleanPointCount--;
            if (cleanPointCount <= 0)
            {
                OnTaskCompleted();
            }

        }

    }
}