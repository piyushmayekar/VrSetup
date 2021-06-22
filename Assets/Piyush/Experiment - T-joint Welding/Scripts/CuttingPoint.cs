using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class CuttingPoint : MonoBehaviour
    {
        public Action OnCuttingDone;
        [SerializeField] Collider _collider;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.HACKSAW_BLADE_TAG))
            {
                _collider.enabled = false;
                OnCuttingDone?.Invoke();
            }
        }


    }
}