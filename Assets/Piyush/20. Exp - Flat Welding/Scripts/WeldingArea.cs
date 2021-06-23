using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlatWelding
{
    public class WeldingArea : MonoBehaviour
    {
        /// <summary>
        /// Called when the welding trigger area is entered or exited by the welding tip. 
        /// True when entered, false when exited
        /// </summary>
        public static Action<bool> OnWeldingMachineTipContact;
        public UnityEvent OnChippingHammerEnter;

        #region SINGLETON
        public static WeldingArea Instance { get => instance; }
        private static WeldingArea instance = null;
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }
        #endregion
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG))
                OnWeldingMachineTipContact?.Invoke(true);
            if (other.CompareTag(_Constants.CHIPPING_HAMMER_TAG))
                OnChippingHammerEnter?.Invoke();
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG)) OnWeldingMachineTipContact?.Invoke(false);
        }
    }
}