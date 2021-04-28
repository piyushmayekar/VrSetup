using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class BladePoint : MonoBehaviour
    {
        [SerializeField] Collider _collider;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CuttingPoint"))
            {
                other.GetComponent<CuttingPoint>().OnContactWithBladePoint(_collider);
            }
        }
    }
}