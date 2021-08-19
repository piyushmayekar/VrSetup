using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class ElectrodeTask1 : Task
    {
        [SerializeField] WeldingMachine machine;
        [SerializeField] public GameObject tablet;
        [SerializeField] public float tim;
        [SerializeField] public bool tabON;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            tabON = true;
            if (machine.IsElectrodePlaced) //If electrode is already placed
                OnTaskCompleted();
            else
                machine.OnElectrodePlacedEvent += OnTaskCompleted;
        }
        private void Update()
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
        }
    }
}