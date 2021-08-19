using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FlatWelding
{
  
    public class Marking : Task
    {
        [SerializeField] public GameObject Task;
        [SerializeField] public int WeldingTask;
        [SerializeField] public GameObject tablet;
        [SerializeField] public float tim;
        [SerializeField] public bool tabON;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            CheckFinalTask.Tasknum = WeldingTask;
            tabON = true;
        }
        public void Update()
        {
            if (tabON == true)
            {
                tim++;
                if (tim == 50)
                {
                    tim = 0;
                    //tablet.transform.localScale = new Vector3(0, 0, 0);
                    tabON = false;
                }
            }
            if (Task.transform.name == "Done")
            {
                OnTaskCompleted();
                Task.transform.name = "Over";
            }
        }
    }
}