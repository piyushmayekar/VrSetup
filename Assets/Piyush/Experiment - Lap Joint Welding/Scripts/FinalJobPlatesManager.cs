using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace LapWelding
{
    public class FinalJobPlatesManager : MonoBehaviour
    {
        [SerializeField] XRGrabInteractable grabInteractable;
        [SerializeField] bool isBeingHeld = false;
        [SerializeField] Rigidbody _rb;
        public void ToggleGrab(bool enable = true)
        {
            grabInteractable.enabled = enable;
            if (!enable) OnSelectExit();
        }

        public void OnSelectEnter()
        {
            isBeingHeld = true;
        }
        IEnumerator RigidbodyNullifier()
        {
            while (!isBeingHeld)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                yield return new WaitForEndOfFrame();
            }
        }
        public void OnSelectExit()
        {
            StartCoroutine(RigidbodyNullifier());
            isBeingHeld = false;
        }
    }
}