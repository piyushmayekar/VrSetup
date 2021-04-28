using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class Task_Hacksaw : Task
    {
        [SerializeField] XRSocketInteractor socketInteractor;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            socketInteractor.gameObject.SetActive(true);
        }
    }
}