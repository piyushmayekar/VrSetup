using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class FilingPoint : MonoBehaviour
    {
        public Action<FilingPoint> OnFilingDone;
        [SerializeField] int cleaningTimes = 5;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.FLATFILE_TAG) && cleaningTimes > 0)
            {
                cleaningTimes--;
                if (cleaningTimes <= 0)
                    OnFilingDone?.Invoke(this);
            }
        }
    }
}
