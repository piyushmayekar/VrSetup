using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class FilingPoint : MonoBehaviour
    {
        [SerializeField] JobPlate job;
        [SerializeField] int cleaningTimes = 5;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FlatFile") && cleaningTimes > 0)
            {
                cleaningTimes--;
                job.OnFilingDoneAtPoint(this);
            }
        }
    }
}
