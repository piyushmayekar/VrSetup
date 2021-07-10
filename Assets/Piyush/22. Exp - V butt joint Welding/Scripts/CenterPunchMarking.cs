using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace VWelding
{
    public class CenterPunchMarking : MonoBehaviour
    {
        public UnityAction OnMarkingDone;
        [SerializeField] List<GameObject> dotPoints;
        [SerializeField] List<MarkingPoint> dotMarkingPoints;
        [SerializeField] int currentMarkingPointIndex = 0;
        [SerializeField] CenterPunch centerPunch;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            centerPunch = CenterPunch.Instance;

        }
        public void StartMarkingProcess()
        {
            dotMarkingPoints[currentMarkingPointIndex].StartMarking();
            CenterPunch.OnHammerHit += OnHammerHit;
        }

        public void OnHammerHit()
        {
            Debug.Log(nameof(OnHammerHit) + " " + dotMarkingPoints[currentMarkingPointIndex].IsCenterPunchInside);
            if (dotMarkingPoints[currentMarkingPointIndex].IsCenterPunchInside)
                OnDotMarkingDone(dotMarkingPoints[currentMarkingPointIndex]);
        }

        public void OnDotMarkingDone(MarkingPoint markingPoint)
        {
            dotPoints[currentMarkingPointIndex].SetActive(true);
            dotMarkingPoints[currentMarkingPointIndex].ToggleHighlight(false);
            currentMarkingPointIndex++;
            if (currentMarkingPointIndex < dotPoints.Count)
            {
                dotMarkingPoints[currentMarkingPointIndex].StartMarking();
            }
            else
            {
                CenterPunch.OnHammerHit -= OnHammerHit;
                OnMarkingDone?.Invoke();
            }
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.CENTER_PUNCH_DOT_POINT_TAG))
            {
                centerPunch.OnPunchingAreaEnter();
            }
        }
        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.CENTER_PUNCH_DOT_POINT_TAG))
            {
                centerPunch.OnPunchingAreaExit();
            }
        }
    }
}