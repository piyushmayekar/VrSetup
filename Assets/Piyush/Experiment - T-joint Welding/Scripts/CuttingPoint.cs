using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class CuttingPoint : MonoBehaviour
    {
        [SerializeField] JobPlate job;
        [SerializeField] GameObject indicator;
        [SerializeField] Collider _collider;
        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.HACKSAW_BLADE_TAG))
            {
                _collider.enabled = false;
                indicator.SetActive(false);
                job.CuttingDone();
            }
        }


    }
}