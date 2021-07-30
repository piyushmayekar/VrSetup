using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PiyushUtils;
using UnityEngine.XR.Interaction.Toolkit;

namespace Grinding
{
    public class CuttingTool : MonoBehaviour
    {
        public Action<GrindingWheelType> OnGrindingComplete;
        [SerializeField] int grindingDone = 0;
        [SerializeField] string toolAnimToPlayOnSecondHandPoint = "Cutting Tool";
        [SerializeField] CustomXRGrabInteractable _XRGrabInteractable;
        [SerializeField] XRSimpleInteractable secondHandInteractable;
        public Tooltips leftToolTips, rightToolTips;
        [SerializeField] XRDirectInteractor mainInteractor;
        
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

        public void OnSecondGrabPointHoverEnter(HoverEnterEventArgs args)
        {
            PiyushUtils.HandPresence handPresence = args.interactor.GetComponentInChildren<PiyushUtils.HandPresence>();
            if (handPresence && _XRGrabInteractable.isSelected && args.interactor != mainInteractor)
            {
                handPresence.OnSecondHandPointHoverEnter(toolAnimToPlayOnSecondHandPoint);
            }
        }

        public void OnSecondGrabPointHoverExit(HoverExitEventArgs args)
        {
            PiyushUtils.HandPresence handPresence = args.interactor.GetComponentInChildren<PiyushUtils.HandPresence>();
            if (handPresence && args.interactor != mainInteractor)
            {
                handPresence.OnSecondHandPointHoverExit();
            }
        }

        public void VisualCheckTask()
        {
            _XRGrabInteractable.selectEntered.AddListener(OnGrabStart);
        }

        public void OnGrabStart(SelectEnterEventArgs args)
        {
            _XRGrabInteractable.selectEntered.RemoveListener(OnGrabStart);
            FindObjectOfType<OneActionTask>().OnActionDone();
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