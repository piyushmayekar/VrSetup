using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class IntroTask : Task
    {
        [SerializeField] GameObject startButton;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            startButton.SetActive(true);
        }

        public void OnStartButtonClicked()
        {
            OnTaskCompleted();
            startButton.SetActive(false);
        }

    }
}