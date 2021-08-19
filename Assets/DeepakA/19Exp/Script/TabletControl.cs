using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FlatWelding
{
  
    public class TabletControl : Task
    {

        [SerializeField]  public GameObject tablet;
        [SerializeField]  public float tim;
        [SerializeField]  public bool tabON;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            tabON = true;

        }
        private void Update()
        {
            if (tabON == true)
            {
                tim += 1 * Time.deltaTime;
                if (tim > 1)
                {
                    tim = 0;
                    tablet.transform.localScale = new Vector3(0,0,0);
                    tabON = false;
                }
            }

        }
    }
}