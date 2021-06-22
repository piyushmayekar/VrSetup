using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace VWelding
{
    public class FinalJobPlatesManager : MonoBehaviour
    {
        public UnityEvent<GameObject> OnSpacerRemoved;
        [SerializeField] Vector3 initPos;
        [SerializeField] Quaternion initRot;
        [SerializeField] XRGrabInteractable grabInteractable;
        void Start()
        {
            initPos = transform.position;
            initRot = transform.rotation;
        }

        public void OnSelectExit()
        {
            transform.SetPositionAndRotation(initPos, initRot);
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.SPACER_TAG))
                OnSpacerRemoved?.Invoke(other.gameObject);
        }

        public void ToggleGrab(bool enable = true) => grabInteractable.enabled = enable;
    }
}