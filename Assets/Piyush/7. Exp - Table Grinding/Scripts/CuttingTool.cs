using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grinding
{
    public class CuttingTool : MonoBehaviour
    {
        public Action<GrindingWheelType> OnGrindingComplete;
        [SerializeField] int grindingDone = 0;
        public Tooltips leftToolTips, rightToolTips;
        void Awake()
        {
            leftToolTips.TurnOnTip();
            rightToolTips.TurnOnTip();
            leftToolTips.RoughGrindingComplete += OnGrindingDone;
            rightToolTips.RoughGrindingComplete += OnGrindingDone;
            leftToolTips.SurfaceGrindingComplete += OnGrindingDone;
            rightToolTips.SurfaceGrindingComplete += OnGrindingDone;
        }

        void OnGrindingDone()
        {
            grindingDone++;
            if (grindingDone == 2)
                OnGrindingComplete?.Invoke(GrindingWheelType.Rough);
            if (grindingDone == 4)
                OnGrindingComplete?.Invoke(GrindingWheelType.Surface);
        }
    }

    [System.Serializable]
    public class Tooltips
    {
        public Action RoughGrindingComplete, SurfaceGrindingComplete;
        public int currentIndex;
        public List<GrindingToolTip> tips;

        public void TurnOnTip()
        {
            if (currentIndex < tips.Count)
            {
                tips[currentIndex].gameObject.SetActive(true);
                tips[currentIndex].OnGrindingComplete += OnGrindingComplete;
            }
            if (currentIndex == tips.Count - 2) RoughGrindingComplete?.Invoke();
            if (currentIndex == tips.Count - 1) SurfaceGrindingComplete?.Invoke();
        }

        public void OnGrindingComplete()
        {
            tips[currentIndex].OnGrindingComplete -= OnGrindingComplete;
            if (currentIndex < tips.Count - 1)
                tips[currentIndex].gameObject.SetActive(false);
            currentIndex++;
            TurnOnTip();
        }
    }
}