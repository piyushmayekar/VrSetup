using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TWelding
{
    public class WeldingArea : MonoBehaviour
    {
        public static Action<bool> OnWeldingMachineTipInContact, OnChippingHammerEnter;
        public static Action OnWeldingMachineTipLoseContact;
        [SerializeField] Transform leftT, rightT;
        public Transform LeftT { get => leftT; set => leftT = value; }
        public Transform RightT { get => rightT; set => rightT = value; }
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
            {
                bool isElectrodeAtLeft = Vector3.Distance(LeftT.position, other.transform.position) <
                Vector3.Distance(RightT.position, other.transform.position);
                OnWeldingMachineTipInContact?.Invoke(isElectrodeAtLeft);
            }
            if (other.CompareTag(_Constants.CHIPPING_HAMMER_TAG))
            {
                bool isHittingLeftPoints = Vector3.Distance(LeftT.position, other.transform.position) <
                Vector3.Distance(RightT.position, other.transform.position);
                OnChippingHammerEnter?.Invoke(isHittingLeftPoints);
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG))
            {
                OnWeldingMachineTipLoseContact?.Invoke();
            }
            if (other.CompareTag(_Constants.CHIPPING_HAMMER_TAG))
            {
                // hammer.PlayHitSound();
            }
        }
    }
}