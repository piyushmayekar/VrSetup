using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    public class MeasurementPoint : MonoBehaviour
    {
        [SerializeField] MeasurementTask task;

        Collider _collider;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == task.SteelRule)
            {
                _collider.enabled = false;
                task.OnTaskCompleted();
            }
        }
    }
}