using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace VWelding
{
    public class MarkingPoint : MonoBehaviour
    {
        public Action<MarkingPoint> OnMarkingDone;
        [SerializeField] MarkingPointType markingType;
        [SerializeField] bool isCenterPunchInside = false;
        [SerializeField] List<GameObject> highlights;
        public int index = 0;
        public bool IsCenterPunchInside { get => isCenterPunchInside; set => isCenterPunchInside = value; }

        public void StartMarking()
        {
            ToggleHighlight();
        }

        public void ToggleHighlight(bool on = true)
        {
            highlights.ForEach(highlight => highlight.SetActive(on));
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.CENTER_PUNCH_DOT_POINT_TAG)
            && markingType == MarkingPointType.CenterPunch)
            {
                IsCenterPunchInside = true;
            }
            else if (other.CompareTag(_Constants.SCRIBER_TIP_TAG)
            && markingType == MarkingPointType.Scriber)
            {
                OnMarkingDone?.Invoke(this);
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.CENTER_PUNCH_DOT_POINT_TAG)
                        && markingType == MarkingPointType.CenterPunch)
            {
                IsCenterPunchInside = false;
            }
        }

        public enum MarkingPointType
        {
            Scriber, CenterPunch
        }
    }
}