using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class Task_CheckByGrabbing : Task
    {
        [SerializeField] List<XRGrabInteractable> grabbables;
        [SerializeField] int _index = 0;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            StartChecking();
        }

        void StartChecking()
        {
            grabbables[_index].enabled = true;
            grabbables[_index].GetComponent<Outline>().enabled = true;
            grabbables[_index].selectEntered.AddListener(OnSelectEnter);
        }

        private void OnSelectEnter(SelectEnterEventArgs args)
        {
            grabbables[_index].GetComponent<Outline>().enabled = false;
            grabbables[_index].selectEntered.RemoveListener(OnSelectEnter);
            _index++;
            if (_index >= grabbables.Count)
                OnTaskCompleted();
            else
                StartChecking();
        }
    }
}