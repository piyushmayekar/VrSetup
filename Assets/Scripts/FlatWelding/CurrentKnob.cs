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
        [SerializeField] float currentValue = 0f, initialValue, targetValue = 100f;
        [SerializeField] float minRot = 0f, maxRot = 180f;
        [SerializeField] TMPro.TextMeshProUGUI displayerText;
        [SerializeField] XRGrabInteractable interactable;
        public float CurrentValue { get => currentValue; set => currentValue = value; }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            initialValue = currentValue;
        }

        public void OnKnobGrabStart()
        {
            StartCoroutine(MeterCalculator());
        }

        public void OnKnobGrabEnd()
        {
            StopAllCoroutines();
            if (currentValue == targetValue) OnTargetValueSet?.Invoke();
        }

        IEnumerator MeterCalculator()
        {
            while (interactable.isSelected)
            {
                var angle = Vector3.Angle(transform.forward, Vector3.up);
                var t = Mathf.InverseLerp(minRot, maxRot, angle);
                currentValue = Mathf.Lerp(initialValue, targetValue, t);
                displayerText.text = currentValue.ToString("0.0") + "A";
                yield return null;
            }
        }
    }
}