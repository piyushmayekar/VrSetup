using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2
{
    public class WeldPositionerScrew : MonoBehaviour
    {
        public UnityEvent OnTargetValueReach;

        [SerializeField] float rotationSpeed = 1f, moveSpeed = 1f;
        [SerializeField] Vector3 openPos, closePos;
        [SerializeField] float t = 0f;
        [SerializeField] XRSimpleInteractable headInteractable;
        [SerializeField] Outline outline;

        Vector3 currTargetPos;
        bool isScrewBeingHeld = false, isBeingOpened = false;

        private void Start()
        {
            transform.localPosition = openPos;
        }

        /// <summary>
        /// To setup the open or close of the screw, call this function
        /// </summary>
        /// <param name="open">True if opening, false if closing</param>
        public void TurnTheScrew(bool open = true)
        {
            headInteractable.enabled = true;
            headInteractable.selectEntered.RemoveAllListeners();
            headInteractable.selectExited.RemoveAllListeners();
            headInteractable.selectEntered.AddListener(OnScrewSelectEnter);
            headInteractable.selectExited.AddListener(OnScrewSelectExit);

            currTargetPos = open ? openPos : closePos;
            isBeingOpened = open;

            outline.enabled = true;
        }

        void OnScrewSelectEnter(SelectEnterEventArgs args)
        {
            isScrewBeingHeld = true;
            StartCoroutine(ScrewHandler());
        }

        void OnScrewSelectExit(SelectExitEventArgs args)
        {
            isScrewBeingHeld = false;
            StopAllCoroutines();
        }

        IEnumerator ScrewHandler()
        {
            while (isScrewBeingHeld)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, currTargetPos, moveSpeed * Time.deltaTime);
                var rot = transform.localEulerAngles;
                rot.y += (isBeingOpened ? rotationSpeed : -rotationSpeed) * Time.deltaTime;
                transform.Rotate(transform.up, rotationSpeed * Time.deltaTime, Space.World);

                if(transform.localPosition==currTargetPos)
                {
                    StopAllCoroutines();
                    headInteractable.enabled = false;
                    outline.enabled = false;
                    OnTargetValueReach?.Invoke();
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}