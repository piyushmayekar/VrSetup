using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    public class WeldingArea : MonoBehaviour
    {
        [SerializeField] WeldingMachine machine;
        [SerializeField] ChippingHammer hammer;
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
                machine.TipInContact(true);
                machine.IsElectrodeAtLeft = Vector3.Distance(LeftT.position, machine.transform.position) <
                Vector3.Distance(RightT.position, machine.transform.position);
            }
            if (other.CompareTag(_Constants.CHIPPING_HAMMER_TAG))
            {
                hammer.IsHittingLeftPoints = Vector3.Distance(LeftT.position, other.transform.position) <
                Vector3.Distance(RightT.position, other.transform.position);
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG)) machine.TipInContact(false);
            if (other.CompareTag(_Constants.CHIPPING_HAMMER_TAG)) hammer.PlayHitSound();
        }
    }
}