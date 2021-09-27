using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class CurrentKnob : MonoBehaviour
    {
        public static event Action OnTargetValueSet;
        [SerializeField] float currentValue = 0f, minValue, targetValue = 100f;
        [SerializeField] float minRot = 0f, maxRot = 180f;
        [SerializeField] TMPro.TextMeshProUGUI displayerText;
        [SerializeField] XRGrabInteractable interactable;
        [SerializeField] Outline outline;

        public float CurrentValue { get => currentValue; set => currentValue = value; }
        public float TargetValue { get => targetValue; set => targetValue = value; }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            SetIndicatorText();
        }

        public void OnKnobGrabStart()
        {
            StartCoroutine(MeterCalculator());
        }

        public void OnKnobGrabEnd()
        {
            StopAllCoroutines();
            if (Mathf.Approximately(currentValue, TargetValue))
            {
                ToggleOutline(false);
                OnTargetValueSet?.Invoke();
            }
        }

        IEnumerator MeterCalculator()
        {
            while (interactable.isSelected)
            {
                var angle = Vector3.Angle(transform.forward, Vector3.up);
                var t = Mathf.InverseLerp(minRot, maxRot, angle);
                currentValue = Mathf.Lerp(minValue, TargetValue, t);
                SetIndicatorText();
                yield return null;
            }
        }

        private void SetIndicatorText()
        {
            displayerText.text = currentValue.ToString("0.0") + "A";
        }

        public void ToggleOutline(bool on=true)
        {
            outline.enabled = on;
        }
    }
}