using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Grinding
{
    public class MechanicalVise : MonoBehaviour
    {
        public UnityEvent OnJobPlatesEnterSocket;
        [SerializeField] Transform handle, slider;
        [SerializeField] float sliderParameter;

        [SerializeField, Tooltip("The angle limits of the handle with respect to Left World Axis")]
        Vector2 angleLimits = new Vector2(-125f, 125f);

        [SerializeField, Tooltip("Slider positions")]
        Vector3 openPos, closedPosFull, currClosedPos;
        [SerializeField] List<Vector3> sliderClosedPositions;
        [SerializeField] bool isViseOpen = true;
        [SerializeField] bool isTaskOn = false;

        HandGrinder handGrinder;

        public void OnTaskToggle(bool on) => isTaskOn = on;
        public void OnHandleSelectEnter() => StartCoroutine(TrackHandleRotation());
        public void OnHandleSelectExit() => StopCoroutine(TrackHandleRotation());


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            slider.localPosition = openPos;
            currClosedPos = closedPosFull;
            handGrinder = FindObjectOfType<HandGrinder>();
        }

        IEnumerator TrackHandleRotation()
        {
            while (true)
            {
                float angle = Vector3.SignedAngle(Vector3.forward, handle.right, Vector3.left);
                sliderParameter = Mathf.InverseLerp(angleLimits.x, angleLimits.y, angle);
                slider.transform.localPosition = Vector3.Lerp(openPos, currClosedPos, sliderParameter);
                isViseOpen = sliderParameter < 0.9f;
                yield return null;
            }
        }

        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            JobPlate jobPlate = args.interactable.GetComponent<JobPlate>();
            if (jobPlate && isTaskOn)
            {
                currClosedPos = sliderClosedPositions[0];
                jobPlate.StartGrinding();
                if (isTaskOn)
                {
                    OnJobPlatesEnterSocket?.Invoke();
                }
            }
        }


        public void OnSocketExit(SelectExitEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                currClosedPos = closedPosFull;
            }
        }

    }
}