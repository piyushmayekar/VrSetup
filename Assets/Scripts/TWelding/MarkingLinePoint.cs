using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class MarkingLinePoint : MonoBehaviour
    {
        [SerializeField] JobPlate jobPlate;
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
                if (pointIndex == jobPlate.LineMarkingPoint)
                {
                    GetComponent<Collider>().enabled = false;
                    jobPlate.OnMarkingDone(transform.localPosition);
                }
            }
        }
    }
}