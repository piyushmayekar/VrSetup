using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class PlugsTask : Task
    {
        [SerializeField] List<PlugConnection> plugs;
        [SerializeField] int plugIndex = 0;
        [SerializeField] CurrentKnob currentKnob;
        [SerializeField] List<PositionResetter> spannerResetters;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            plugs[plugIndex].StartConnecting();
            plugs[plugIndex].OnConnectionDone.AddListener(new UnityEngine.Events.UnityAction(OnConnectionDone));
        }

        public void OnConnectionDone()
        {
            plugIndex++;
            if (plugIndex < plugs.Count)
            {
                plugs[plugIndex].StartConnecting();
                plugs[plugIndex].OnConnectionDone.AddListener(new UnityEngine.Events.UnityAction(OnConnectionDone));
            }
            else if (plugIndex == plugs.Count)
            {
                if (currentKnob.CurrentValue == currentKnob.TargetValue)
                {
                    OnTaskCompleted();
                }
                else
                {
                    CurrentKnob.OnTargetValueSet += OnTaskCompleted;
                    currentKnob.GetComponent<Outline>().enabled = true;
                }
            }
        }

        public override void OnTaskCompleted()
        {
            spannerResetters.ForEach(s => s.ResetPos());
            base.OnTaskCompleted();
        }

    }
}