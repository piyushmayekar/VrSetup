using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VWelding
{
    public class CenterPunchMarking : MonoBehaviour
    {
        public Action OnMarkingDone;
        [SerializeField] List<GameObject> dotPoints;
        [SerializeField] List<MarkingPoint> dotMarkingPoints;
        [SerializeField] int currentMarkingPointIndex = 0;
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
    }
}