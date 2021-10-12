using System.Collections;
using System.Collections.Generic;
using FlatWelding;
using UnityEngine;
using UnityEngine.Events;

namespace CornerWelding
{
    public class WeldingArea : MonoBehaviour
    {
        public UnityEvent OnWeldingMachineTipEnter, OnWeldingMachineTipExit,
        OnChippingHammerEnter;
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG))
            {
                OnWeldingMachineTipEnter?.Invoke();
                // machine.TipInContact(true);
            }
            if (other.CompareTag(_Constants.CHIPPING_HAMMER_TAG))
            {
                // hammer.PlayHitSound();
                OnChippingHammerEnter?.Invoke();
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG))
                OnWeldingMachineTipExit?.Invoke();
            // machine.TipInContact(false);
        }
    }
}