using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace PiyushUtils
{
    public class CenterPunchMarking : MonoBehaviour
    {
        public UnityAction OnMarkingDone;
        [SerializeField] List<CPMarkingPoint> dotMarkingPoints;
        [SerializeField] int currentMarkingPointIndex = 0;
        [SerializeField] CenterPunch centerPunch;

        public int TotalPointCount => dotMarkingPoints.Count;
        public bool IsMarkingDone => currentMarkingPointIndex >= TotalPointCount;
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
            if (dotMarkingPoints[currentMarkingPointIndex].IsCenterPunchInside)
                OnDotMarkingDone(dotMarkingPoints[currentMarkingPointIndex]);
        }

        public void OnDotMarkingDone(CPMarkingPoint markingPoint)
        {
            dotMarkingPoints[currentMarkingPointIndex].ToggleHighlight(false);
            dotMarkingPoints[currentMarkingPointIndex].visiblePoint.SetActive(true);
            currentMarkingPointIndex++;
            if (currentMarkingPointIndex < TotalPointCount)
            {
                CancelInvoke();
                Invoke(nameof(EnableNextMarkingPoint), 1f);
            }
            else
            {
                CenterPunch.OnHammerHit -= OnHammerHit;
                OnMarkingDone?.Invoke();
            }
        }

        private void EnableNextMarkingPoint()
        {
            if (currentMarkingPointIndex >= 0 && currentMarkingPointIndex < TotalPointCount)
                dotMarkingPoints[currentMarkingPointIndex].StartMarking();
            CancelInvoke();
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

        #region EDITOR METHODS

        [ContextMenu("Fill Marking List")]
        /// <summary>
        /// Editor Method. Don't use anywhere else
        /// </summary>

        public void FillMarkingList()
        {
            dotMarkingPoints = new List<CPMarkingPoint>(GetComponentsInChildren<CPMarkingPoint>());
        }

        [ContextMenu("Perform All Markings")]
        /// <summary>
        /// Editor Method. Don't use anywhere else
        /// </summary>

        public void PerformAllMarkings()
        {
            dotMarkingPoints.ForEach(point => point.visiblePoint.SetActive(true));
        }

        [ContextMenu("Remove All Markings")]
        /// <summary>
        /// Editor Method. Don't use anywhere else
        /// </summary>

        public void RemoveAllMarkings()
        {
            dotMarkingPoints.ForEach(point => point.visiblePoint.SetActive(false));
        }

        #endregion
    }
}