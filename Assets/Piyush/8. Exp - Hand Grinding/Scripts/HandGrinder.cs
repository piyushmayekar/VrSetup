using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Grinding
{
    public class HandGrinder : GrindingMachine
    {
        [SerializeField] Vector3 grindingRotation;
        [SerializeField] Outline switchOutline;
        Rigidbody _rb;
        XRGrabInteractable grabInteractable;


        private void Start()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            _rb = wheels[0].GetComponent<Rigidbody>();
            grabInteractable.selectEntered.AddListener(OnGrinderSelectEnter);
            grabInteractable.selectExited.AddListener(OnGrinderSelectExit);
        }

        void OnGrinderSelectEnter(SelectEnterEventArgs args)
        {
            switchOutline.enabled = !IsOn;
        }

        void OnGrinderSelectExit(SelectExitEventArgs args)
        {
            switchOutline.enabled = IsOn;
        }
    }
}