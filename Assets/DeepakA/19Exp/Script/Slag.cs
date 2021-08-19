using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FlatWelding
{
  
    public class Slag : Task
    {
        [SerializeField] public GameObject Task1;
        [SerializeField] public GameObject Task2;
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
            if (Task1.transform.name == "Done" && Task2.transform.name == "Done")
            {
                OnTaskCompleted();
            }
        }
    }
}