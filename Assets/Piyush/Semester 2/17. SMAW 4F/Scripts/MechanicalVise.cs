using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2
{
    public class MechanicalVise : MonoBehaviour
    {

        [SerializeField] Transform handle, slider;

        [SerializeField, Tooltip("The transforms of the position holders of according to the job plate types")]
        List<Transform> jobTransforms;
        [SerializeField, Tooltip("The transforms of the filing position holders of according to the job plate types")]
        List<Transform> filingTransforms;
        [SerializeField] XRSocketInteractor jobSocket;
        [SerializeField] GameObject jobPlateHighlight, filingCanvas;
        [SerializeField] JobPlate currentHoldingPlate = null;
        //[SerializeField] List<GameObject> hacksawAnimators;
        [SerializeField] float sliderParameter;

        [SerializeField, Tooltip("The angle limits of the handle with respect to Left World Axis")]
        Vector2 angleLimits = new Vector2(-125f, 125f);

        [SerializeField, Tooltip("Slider positions")]
        Vector3 currClosedPos;
        Vector3 maxOpenPos = new Vector3(0, 0.048f, -0.045f), closedPosFull = new Vector3(0, 0.048f, -0.1855f), closedPosPlate = new Vector3(0, 0.048f, -0.1755f);
        [SerializeField] bool isViseOpen = true, shouldSwitchAttachTransforms = true, allowViseOpening = true, shouldAllowFiling = false;

        [ContextMenu(nameof(TrackHandleRotation))]
        public void OnHandleSelectEnter() => StartCoroutine(TrackHandleRotation());
        public void OnHandleSelectExit() => StopCoroutine(TrackHandleRotation());

        public bool ShouldSwitchAttachTransforms { get => shouldSwitchAttachTransforms; set => shouldSwitchAttachTransforms = value; }
        public bool ShouldAllowFiling { get => shouldAllowFiling; set => shouldAllowFiling = value; }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            currClosedPos = closedPosFull;
            slider.localPosition = maxOpenPos;
        }

        internal void OnTaskBegin()
        {
            jobSocket.socketActive = true;
            ShouldAllowFiling = false;
        }

        internal void OnTaskCompleted()
        {
            allowViseOpening = true;
            TurnOffHacksawAnimations();
        }

        IEnumerator TrackHandleRotation()
        {
            while (allowViseOpening)
            {
                float angle = Vector3.SignedAngle(Vector3.forward, handle.right, Vector3.left);
                sliderParameter = Mathf.InverseLerp(angleLimits.x, angleLimits.y, angle);
                slider.transform.localPosition = Vector3.Lerp(maxOpenPos, currClosedPos, sliderParameter);
                isViseOpen = sliderParameter <= 0.98f;
                yield return null;
            }

        }

        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                JobPlate plate = args.interactable.GetComponent<JobPlate>();
                currClosedPos = closedPosPlate;
                int index = (int)plate.PlateType;
                float invokeInterval = 0.1f;
                // jobPlateHighlight.SetActive(false);
                if (ShouldSwitchAttachTransforms)
                {
                    if (plate.IsCuttingDone && !plate.IsFilingDone) //Filing
                    {
                        jobSocket.attachTransform = filingTransforms[index];
                        //plate.SetupForFiling();
                        ToggleFilingCanvas(false);
                        //plate.OnFilingDone += () => { ToggleViseOpening(); };
                        InvokeRepeating(nameof(KeepCheckingForViseClosing), invokeInterval, invokeInterval);
                    }
                    else if (!plate.IsCuttingDone) //Cutting
                    {
                        //plate.SetupForCutting();
                        //plate.OnGasCuttingDone += TurnOffHacksawAnimations;
                        if (index >= 0 && index < jobTransforms.Count)
                            jobSocket.attachTransform = jobTransforms[index];
                        //plate.OnGasCuttingDone += () =>
                        //{
                        //    ToggleViseOpening();
                        //};
                        InvokeRepeating(nameof(KeepCheckingForViseClosing), invokeInterval, invokeInterval);
                    }
                    currentHoldingPlate = plate;

                }
            }
        }

        /// <summary>
        /// Responsible for not letting the user open the vise after placing a job plate to perform actions on.
        /// </summary>
        void KeepCheckingForViseClosing()
        {
            if (!isViseOpen)
            {
                ToggleViseOpening(false);
                CancelInvoke(nameof(KeepCheckingForViseClosing));
                if (currentHoldingPlate != null)
                {
                    JobPlate plate = currentHoldingPlate;
                    if (plate.IsCuttingDone && !plate.IsFilingDone) //Filing
                    {
                        plate.SetupForFiling();
                        ToggleFilingCanvas(false);
                        plate.OnFilingDone += () => { ToggleViseOpening(); };
                    }
                    else if (!plate.IsCuttingDone) //Cutting
                    {
                        plate.SetupForCutting();
                        plate.OnGasCuttingDone += TurnOffHacksawAnimations;
                        plate.OnGasCuttingDone += () =>
                        {
                            ToggleViseOpening();
                        };
                    }
                }
            }
        }

        public void OnSocketExit(SelectExitEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                JobPlate plate = args.interactable.GetComponent<JobPlate>();
                plate.OnMechanicalViseSocketExit();
            }
            currentHoldingPlate = null;
            currClosedPos = closedPosFull;
            ToggleFilingCanvas(false);
            TurnOffHacksawAnimations();
        }

        public void TurnOffHacksawAnimations() { }
        public void ToggleFilingCanvas(bool on = true)
        {
            filingCanvas.SetActive(on);
        }

        public void ToggleViseOpening(bool allow = true)
        {
            allowViseOpening = allow;
        }

        [ContextMenu("Close vise")]
        public void DebugCloseVise()
        {
            OnHandleSelectEnter();
            sliderParameter = 0.9f;
        }
    }
}