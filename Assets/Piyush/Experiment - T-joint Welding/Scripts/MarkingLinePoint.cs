using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class MarkingLinePoint : MonoBehaviour
    {
        public Action<int, Vector3> OnScriberTipEnter;
        [SerializeField] int pointIndex = 0;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            pointIndex = transform.GetSiblingIndex();
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MarkPoint"))
            {
                OnScriberTipEnter?.Invoke(pointIndex, transform.localPosition);
            }
        }
    }
}