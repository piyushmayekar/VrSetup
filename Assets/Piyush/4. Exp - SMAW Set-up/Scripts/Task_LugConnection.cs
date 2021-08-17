using FlatWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMAW_Setup_4
{
    public class Task_LugConnection : Task
    {
        [SerializeField] List<PlugConnection> plugs;
        [SerializeField] int plugIndex = 0;
        [SerializeField] List<PositionResetter> spannerResetters;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            StartPlugConnections();
        }

        private void StartPlugConnections()
        {
            plugs[plugIndex].StartConnecting();
            plugs[plugIndex].OnConnectionDone.AddListener(OnConnectionDone);
        }

        public void OnConnectionDone()
        {
            plugs[plugIndex].OnConnectionDone.RemoveListener(OnConnectionDone);
            plugIndex++;
            if (plugIndex < plugs.Count)
            {
                StartPlugConnections();
            }
            else
            {
                spannerResetters.ForEach(s => s.ResetPos());
                OnTaskCompleted();
            }
        }

    }
}