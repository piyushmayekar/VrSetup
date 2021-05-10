using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class MechanicalVise : MonoBehaviour
    {
        [SerializeField] Transform handle, slider;

        [SerializeField, Tooltip("The transforms of the position holders of both the jobs")]
        List<XRSocketInteractor> jobSockets;

        [SerializeField] GameObject jobPlateHighlight;
        [SerializeField] JobPlate currentHoldingPlate = null;
        [SerializeField] List<GameObject> hacksawAnimators;
        [SerializeField] float sliderParameter;

        [SerializeField, Tooltip("The angle limits of the handle with respect to Left World Axis")]
        Vector2 angleLimits = new Vector2(-125f, 125f);

        [SerializeField, Tooltip("Slider positions")]
        Vector3 openPos, closedPos, closedPosFull, closedPosBreadth, closedPosLength;
        [SerializeField] bool isViseOpen = true;

        public void OnHandleSelectEnter() => StartCoroutine(TrackHandleRotation());
        public void OnHandleSelectExit() => StopCoroutine(TrackHandleRotation());

        [SerializeField] Collider _collider;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            slider.localPosition = openPos;
        }

        internal void OnTaskBegin()
        {
            jobPlateHighlight.SetActive(true);
            _collider.enabled = true;
        }

        internal void OnTaskCompleted()
        {
            jobPlateHighlight.SetActive(false);
            _collider.enabled = false;
            TurnOffHacksawAnimations();
        }

        IEnumerator TrackHandleRotation()
        {
            while (true)
            {
                float angle = Vector3.SignedAngle(Vector3.forward, handle.right, Vector3.left);
                sliderParameter = Mathf.InverseLerp(angleLimits.x, angleLimits.y, angle);
                slider.transform.localPosition = Vector3.Lerp(openPos, closedPos, sliderParameter);
                isViseOpen = sliderParameter < 0.9f;
                yield return null;
            }

        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.JOB_TAG))
            {
                JobPlate plateEntered = other.GetComponent<JobPlate>();
                if (plateEntered)
                {
                    int index = JobPlate.jobPlates.FindIndex(plate => plate == plateEntered);
                    jobSockets[index].socketActive = true;
                    jobSockets[index == 0 ? 1 : 0].socketActive = false;
                }
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.JOB_TAG))
                TurnOffHacksawAnimations();
        }

        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag("Job"))
            {
                JobPlate plate = args.interactable.GetComponent<JobPlate>();
                int index = JobPlate.jobPlates.FindIndex(p => p == plate);
                plate.SetupForCutting();
                jobPlateHighlight.SetActive(false);
                if (!plate.IsCuttingDone)
                {
                    hacksawAnimators[index].SetActive(true);
                    plate.OnHacksawCuttingDone += TurnOffHacksawAnimations;
                }
                currentHoldingPlate = plate;
                if (currentHoldingPlate.PlateType == PlateType.Length) closedPos = closedPosLength;
                if (currentHoldingPlate.PlateType == PlateType.Breadth) closedPos = closedPosBreadth;
            }
        }

        public void OnSocketExit(SelectEnterEventArgs args)
        {
            jobPlateHighlight.SetActive(true);
            currentHoldingPlate = null;
            closedPos = closedPosFull;
        }

        public void TurnOffHacksawAnimations() => hacksawAnimators.ForEach(x => x.SetActive(false));
    }
}