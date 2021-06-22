using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class IntroTask : Task
    {
        [SerializeField] GameObject button;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            button.SetActive(true);
            XRGrabInteractable interactable = button.GetComponent<XRGrabInteractable>();
            interactable.firstHoverEntered.RemoveAllListeners();
            interactable.firstHoverEntered.AddListener(new UnityEngine.Events.UnityAction<HoverEnterEventArgs>(OnStartButtonClicked));
        }

        public void OnStartButtonClicked(HoverEnterEventArgs arg)
        {
            OnTaskCompleted();
            button.SetActive(false);
        }

    }
}