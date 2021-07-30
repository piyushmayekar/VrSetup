using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Grinding
{
    public class HandGrinder : GrindingMachine
    {
        [SerializeField] Vector3 grindingRotation;
        Rigidbody _rb;
        XRGrabInteractable grabInteractable;


        private void Start()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            _rb = wheels[0].GetComponent<Rigidbody>();
        }
        public void FreezeGrinder()
        {
            grabInteractable.trackRotation = false;
            grabInteractable.movementType = XRBaseInteractable.MovementType.Kinematic;
            transform.rotation = Quaternion.Euler(grindingRotation);
            //_rb.isKinematic = true;
        }

        public void UnFreezeGrinder()
        {
            grabInteractable.trackRotation = true;
            grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        }
    }
}