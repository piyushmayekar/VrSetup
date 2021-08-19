using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FlatWelding
{
  
    public class checkBrush : Task
    {
        [SerializeField]  public GameObject brush_Task;
        [SerializeField] public GameObject tablet;
        [SerializeField] public float tim;
        [SerializeField] public bool tabON;
        [SerializeField] public GameObject PrevScene;
        [SerializeField] public GameObject NextScene;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            tabON = true;
            tim = 0;
            CheckFinalTask.Tasknum = 0;
        }
        void Update()
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
            if (brush_Task.transform.name == "Done")
            {
                NextScene.SetActive(true);
                PrevScene.SetActive(false);
                //brush_Task.transform.name = "Over";
            }
            if (NextScene.transform.name == "Done")
            {
                OnTaskCompleted();
                NextScene.transform.name = "Over";
            }
        }
    }
}