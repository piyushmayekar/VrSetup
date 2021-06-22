using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VWelding
{
    public class ScriberMarking : MonoBehaviour
    {
        public Action OnMarkingDone;
        [SerializeField] List<GameObject> dotPoints;
        [SerializeField] List<MarkingPoint> dotMarkingPoints;
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] List<MarkingPoint> lineMarkingPoints;
        [SerializeField] int currentMarkingPointIndex = 0;

        public void StartMarkingProcess()
        {
            dotMarkingPoints[currentMarkingPointIndex].StartMarking();
            dotMarkingPoints[currentMarkingPointIndex].OnMarkingDone += OnDotMarkingDone;
        }

        public void OnDotMarkingDone(MarkingPoint markingPoint)
        {
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
    }
}