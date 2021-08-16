using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using PiyushUtils;

namespace FlatWelding
{
    public class IntroTask : Task
    {
        
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            taskManager.confirmButton.onClick.RemoveAllListeners();
            taskManager.confirmButton.onClick.AddListener(OnTaskCompleted);
        }
    }
}