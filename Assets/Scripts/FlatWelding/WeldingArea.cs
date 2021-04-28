using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    public class WeldingArea : MonoBehaviour
    {
        [SerializeField] WeldingMachine machine;
        [SerializeField] Hammer hammer;

        const string WELDING_TAG = "WeldingTip";
        const string CHIPPING_HAMMER_TAG = "ChippingHammer";

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(WELDING_TAG)) machine.TipInContact(true);
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(WELDING_TAG)) machine.TipInContact(false);
            if (other.CompareTag(CHIPPING_HAMMER_TAG)) hammer.PlayHitSound();
        }
    }
}