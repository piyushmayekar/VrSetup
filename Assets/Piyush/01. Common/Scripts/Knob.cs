using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class Knob : MonoBehaviour
    {
        public UnityEvent OnTargetValueReached;

        [Header("Gauge")]
        [SerializeField] public float currentValue = 0f;
        [SerializeField] public float targetValue = 10f;
        [SerializeField] float minValue = 0f, maxValue = 20f;
        [SerializeField] float valueIncreaseSpeed = 1f;
        [SerializeField] bool shouldIncreaseValueOnGrab = true;
        [SerializeField] public bool allowGrab=false;

        [Header("Knob")]
        [SerializeField] float k_minRot;
        [SerializeField] float k_maxRot;
        [SerializeField] Vector3 knobRotAxis;
        [SerializeField] Transform knobT;
        [SerializeField] XRSimpleInteractable interactable;
        [SerializeField] Outline outline;


        [Header("Dial Hand")]
        [SerializeField] bool isDialPresent = true;
        [SerializeField] float d_minRot;
        [SerializeField] float d_maxRot;
        [SerializeField] Vector3 dialRotAxis;
        [SerializeField] Transform dialHandT;

        public float MinValue { get => minValue; }
        public float MaxValue { get => maxValue; }

        void Start()
        {
            isDialPresent = dialHandT != null;
            SetKnobRotation();
            SetDialHandRotation();
            interactable.selectEntered.AddListener(OnKnobGrabStart);
            interactable.selectExited.AddListener(OnKnobGrabEnd);
        }

        public void EnableTurning(float targetValueToSet, bool shouldValueIncrease)
        {
            targetValue = targetValueToSet;
            shouldIncreaseValueOnGrab = shouldValueIncrease;
            if (outline)
                outline.enabled = true;
            allowGrab = true;
        }

        void SetKnobRotation()
        {
            float t = Mathf.InverseLerp(MinValue, MaxValue, currentValue);
            float angle = Mathf.Lerp(k_maxRot, k_minRot, t);
            knobT.localEulerAngles = knobRotAxis * angle;
        }

        void SetDialHandRotation()
        {
            if (!isDialPresent) return;
            float t = Mathf.InverseLerp(MinValue, MaxValue, currentValue);
            float angle = Mathf.Lerp(d_minRot, d_maxRot, t);
            dialHandT.localEulerAngles = dialRotAxis * angle;
        }

        public void OnKnobGrabStart(SelectEnterEventArgs args)
        {
            if (allowGrab)
                StartCoroutine(Handler());
        }

        public void OnKnobGrabEnd(SelectExitEventArgs args)
        {
            StopCoroutine(Handler());
        }

        IEnumerator Handler()
        {
            while (interactable.isSelected && allowGrab)
            {
                float v = shouldIncreaseValueOnGrab ? valueIncreaseSpeed : -valueIncreaseSpeed;
                currentValue += v * Time.deltaTime;
                SetDialHandRotation();
                SetKnobRotation();
                if ((shouldIncreaseValueOnGrab && currentValue>=targetValue) || (!shouldIncreaseValueOnGrab && currentValue <= targetValue))
                {
                    currentValue = targetValue;
                    allowGrab = false;
                    if (outline)
                        outline.enabled = false;
                    OnTargetValueReached?.Invoke();
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}