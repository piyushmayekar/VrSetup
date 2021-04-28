using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class TackingTask : Task
    {
        [SerializeField] List<WeldingPoint> tacks;


        public override void OnTaskBegin()
        {
            tacks.ForEach(x =>
            {
                x.gameObject.SetActive(true);
                x.OnWeldingDone += OnTackDone;
            });
            MainWeldingTask.OnTaskDone += () =>
                tacks.ForEach(t => t.gameObject.SetActive(false));
        }

        internal void OnTackDone()
        {
            if (tacks.TrueForAll(t => t.IsWeldingDone))
                OnTaskCompleted();
        }
    }
}