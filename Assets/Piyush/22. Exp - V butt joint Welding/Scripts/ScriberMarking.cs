using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace VWelding
{
    public class ScriberMarking : MonoBehaviour
    {
        public UnityAction OnMarkingDone;
        [SerializeField] List<GameObject> dotPoints;
        [SerializeField] List<MarkingPoint> dotMarkingPoints;
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] List<MarkingPoint> lineMarkingPoints;
        [SerializeField] int currentDotMarkingPointIndex = 0, currentLineMarkingPointIndex = 0;
        [SerializeField] PiyushUtils.Scriber scriber;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            scriber = PiyushUtils.Scriber.Instance;
        }
        public void StartMarkingProcess()
        {
            int i = (currentDotMarkingPointIndex);
            Debug.Log(nameof(StartMarkingProcess) + name);
            dotMarkingPoints[currentDotMarkingPointIndex].StartMarking();
            dotMarkingPoints[currentDotMarkingPointIndex].OnMarkingDone += OnDotMarkingDone;
        }

        public void OnDotMarkingDone(MarkingPoint markingPoint)
        {
            Debug.Log(nameof(OnDotMarkingDone));
            dotPoints[currentDotMarkingPointIndex].SetActive(true);
            dotMarkingPoints[currentDotMarkingPointIndex].ToggleHighlight(false);
            dotMarkingPoints[currentDotMarkingPointIndex].OnMarkingDone -= OnDotMarkingDone;
            currentDotMarkingPointIndex++;
            if (currentDotMarkingPointIndex < dotPoints.Count)
            {
                dotMarkingPoints[currentDotMarkingPointIndex].StartMarking();
                dotMarkingPoints[currentDotMarkingPointIndex].OnMarkingDone += OnDotMarkingDone;
            }
            else
            {
                currentLineMarkingPointIndex = 0;
                lineMarkingPoints.ForEach(point =>
                {
                    point.index = point.transform.GetSiblingIndex();
                    point.OnMarkingDone += OnLineMarkingDone;
                });
                lineMarkingPoints[currentLineMarkingPointIndex].StartMarking();
            }
        }

        public void OnLineMarkingDone(MarkingPoint point)
        {
            Debug.Log(nameof(OnLineMarkingDone));
            if (point.index <= currentLineMarkingPointIndex)
            {
                currentLineMarkingPointIndex++;
                lineRenderer.positionCount = currentLineMarkingPointIndex;
                lineRenderer.SetPosition(currentLineMarkingPointIndex - 1, point.transform.localPosition);
                point.ToggleHighlight(false);
                point.OnMarkingDone -= OnLineMarkingDone;
            }
            else if (currentDotMarkingPointIndex >= dotPoints.Count &&
             currentLineMarkingPointIndex >= lineMarkingPoints.Count)
            {
                OnMarkingDone?.Invoke();
            }
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.SCRIBER_TIP_TAG))
            {
                scriber.OnMarkingAreaEnter();
            }
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.SCRIBER_TIP_TAG))
            {
                scriber.OnMarkingAreaExit();
            }
        }
    }
}