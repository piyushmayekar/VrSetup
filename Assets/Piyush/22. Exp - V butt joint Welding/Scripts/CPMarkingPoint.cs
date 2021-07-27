using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace PiyushUtils
{
    public class CPMarkingPoint : MonoBehaviour
    {
        public UnityAction<CPMarkingPoint> OnMarkingDone;
        [SerializeField] bool isCenterPunchInside = false;
        [SerializeField] List<GameObject> highlights;
        public GameObject visiblePoint;
        public int index = 0;
        public bool IsCenterPunchInside { get => isCenterPunchInside; set => isCenterPunchInside = value; }

        public void StartMarking()
        {
            ToggleHighlight();
        }

        public void ToggleHighlight(bool on = true)
        {
            string s = highlights.Count + "";
            highlights.ForEach(highlight => highlight.SetActive(on));
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.CENTER_PUNCH_DOT_POINT_TAG))
            {
                IsCenterPunchInside = true;
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.CENTER_PUNCH_DOT_POINT_TAG))
            {
                IsCenterPunchInside = false;
            }
        }
    }
}