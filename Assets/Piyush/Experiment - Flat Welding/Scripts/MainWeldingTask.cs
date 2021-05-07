using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    public class MainWeldingTask : Task
    {

        public static event Action OnTaskDone;
        [SerializeField] List<WeldingPoint> weldingPoints;

        public List<WeldingPoint> WeldingPoints { get => weldingPoints; set => weldingPoints = value; }

        public override void OnTaskBegin()
        {
            WeldingPoints.ForEach(x =>
            {
                x.gameObject.SetActive(true);
                x.OnWeldingDone += OnPointWeldingDone;
            });
        }

        internal void OnPointWeldingDone()
        {
            if (WeldingPoints.TrueForAll(t => t.IsWeldingDone))
            {
                OnTaskCompleted();
                OnTaskDone?.Invoke();
            }
        }
    }
}
