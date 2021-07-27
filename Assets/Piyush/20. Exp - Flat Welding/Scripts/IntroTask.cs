using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FlatWelding
{
    public class IntroTask : Task
    {
        
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            OnTaskCompleted();
        }
    }
}