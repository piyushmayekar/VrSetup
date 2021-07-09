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
        [SerializeField] int currentMarkingPointIndex = 0;
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
            int i = (currentMarkingPointIndex);
            Debug.Log(nameof(StartMarkingProcess));
            dotMarkingPoints[currentMarkingPointIndex].StartMarking();
            dotMarkingPoints[currentMarkingPointIndex].OnMarkingDone += OnDotMarkingDone;
        }

        public void OnDotMarkingDone(MarkingPoint markingPoint)
        {
            Debug.Log(nameof(OnDotMarkingDone));
            dotPoints[currentMarkingPointIndex].SetActive(true);
            dotMarkingPoints[currentMarkingPointIndex].ToggleHighlight(false);
            dotMarkingPoints[currentMarkingPointIndex].OnMarkingDone -= OnDotMarkingDone;
            currentMarkingPointIndex++;
            if (currentMarkingPointIndex < dotPoints.Count)
            {
                dotMarkingPoints[currentMarkingPointIndex].StartMarking();
                dotMarkingPoints[currentMarkingPointIndex].OnMarkingDone += OnDotMarkingDone;
            }
            else
            {
                currentMarkingPointIndex = 0;
                lineMarkingPoints.ForEach(point =>
                {
                    point.index = point.transform.GetSiblingIndex();
                    point.OnMarkingDone += OnLineMarkingDone;
                });
                lineMarkingPoints[currentMarkingPointIndex].StartMarking();
            }
        }

        public void OnLineMarkingDone(MarkingPoint point)
        {
            Debug.Log(nameof(OnLineMarkingDone));
            if (point.index <= currentMarkingPointIndex)
            {
                currentMarkingPointIndex++;
                lineRenderer.positionCount = currentMarkingPointIndex;
                lineRenderer.SetPosition(currentMarkingPointIndex - 1, point.transform.localPosition);
                point.ToggleHighlight(false);
                point.OnMarkingDone -= OnLineMarkingDone;
            }
            if (currentMarkingPointIndex >= lineMarkingPoints.Count)
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