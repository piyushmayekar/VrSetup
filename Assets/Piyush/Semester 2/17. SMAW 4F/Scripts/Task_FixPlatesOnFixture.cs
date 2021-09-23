using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2
{
    /// <summary>
    /// Place the fused plates structure on the fixture & close the screw
    /// </summary>
    public class Task_FixPlatesOnFixture : Task
    {
        [SerializeField] XRSocketInteractor socket;
        [SerializeField] GameObject highlight;
        [SerializeField] XRGrabInteractable platesGrabbable;
        [SerializeField] WeldPositionerScrew screw;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            socket.socketActive = true;
            highlight.SetActive(true);
            platesGrabbable.enabled = true;
            socket.selectEntered.AddListener(OnPlatesPlaced);
        }

        void OnPlatesPlaced(SelectEnterEventArgs args)
        {
            if (args.interactable == platesGrabbable)
            {
                highlight.SetActive(false);
                platesGrabbable.transform.SetPositionAndRotation(socket.transform.position, socket.transform.rotation);
                socket.socketActive = false;
                platesGrabbable.enabled = false;
                screw.TurnTheScrew(false);
                screw.OnTargetValueReach.AddListener(OnScrewClosed);
            }
        }

        void OnScrewClosed()
        {
            OnTaskCompleted();
        }
    }
}