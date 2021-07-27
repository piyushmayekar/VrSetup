using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class MechanicalVise : PiyushUtils.SMAW_MechanicalVise
    {
        //[SerializeField] Transform handle, slider;

        //[SerializeField, Tooltip("The transforms of the position holders of according to the job plate types")]
        //List<Transform> jobTransforms;
        //[SerializeField, Tooltip("The transforms of the filing position holders of according to the job plate types")]
        //List<Transform> filingTransforms;
        //[SerializeField] XRSocketInteractor jobSocket;
        //[SerializeField] GameObject jobPlateHighlight, filingCanvas;
        //[SerializeField] JobPlate currentHoldingPlate = null;
        ////[SerializeField] List<GameObject> hacksawAnimators;
        //[SerializeField] float sliderParameter;

        //[SerializeField, Tooltip("The angle limits of the handle with respect to Left World Axis")]
        //Vector2 angleLimits = new Vector2(-125f, 125f);

        //[SerializeField, Tooltip("Slider positions")]
        //Vector3 openPos, closedPos, closedPosFull;
        //[SerializeField] List<Vector3> sliderClosedPositions;
        //[SerializeField] bool isViseOpen = true, shouldSwitchAttachTransforms = true, allowViseOpening = true;

        //public void OnHandleSelectEnter() => StartCoroutine(TrackHandleRotation());
        //public void OnHandleSelectExit() => StopCoroutine(TrackHandleRotation());

        //public bool ShouldSwitchAttachTransforms { get => shouldSwitchAttachTransforms; set => shouldSwitchAttachTransforms = value; }

        ///// <summary>
        ///// Awake is called when the script instance is being loaded.
        ///// </summary>
        //void Awake()
        //{
        //    slider.localPosition = openPos;
        //}

        //internal void OnTaskBegin()
        //{
        //    // jobPlateHighlight.SetActive(true);
        //    // _collider.enabled = true;
        //    jobSocket.socketActive = true;
        //}

        //internal void OnTaskCompleted()
        //{
        //    // jobPlateHighlight.SetActive(false);
        //    // jobSocket.socketActive = false;
        //    // _collider.enabled = false;
        //    TurnOffHacksawAnimations();
        //}

        //IEnumerator TrackHandleRotation()
        //{
        //    while (allowViseOpening)
        //    {
        //        float angle = Vector3.SignedAngle(Vector3.forward, handle.right, Vector3.left);
        //        sliderParameter = Mathf.InverseLerp(angleLimits.x, angleLimits.y, angle);
        //        slider.transform.localPosition = Vector3.Lerp(openPos, closedPos, sliderParameter);
        //        isViseOpen = sliderParameter <= 0.98f;
        //        yield return null;
        //    }

        //}

        //public void OnSocketEnter(SelectEnterEventArgs args)
        //{
        //    if (args.interactable.CompareTag(_Constants.JOB_TAG))
        //    {
        //        JobPlate plate = args.interactable.GetComponent<JobPlate>();
        //        int index = (int)plate.PlateType;
        //        float invokeInterval = 0.1f;
        //        // jobPlateHighlight.SetActive(false);
        //        if (ShouldSwitchAttachTransforms)
        //        {
        //            if (plate.IsCuttingDone && !plate.IsFilingDone)
        //            {
        //                jobSocket.attachTransform = filingTransforms[index];
        //                plate.SetupForFiling();
        //                closedPos = sliderClosedPositions[1];
        //                ToggleFilingCanvas(false);
        //                plate.OnFilingDone += () => { ToggleViseOpening(); };
        //                InvokeRepeating(nameof(KeepCheckingForViseClosing), invokeInterval, invokeInterval);
        //            }
        //            else if (!plate.IsCuttingDone)
        //            {
        //                plate.SetupForCutting();
        //                //hacksawAnimators[index].SetActive(true);
        //                plate.OnGasCuttingDone += TurnOffHacksawAnimations;
        //                if (index >= 0 && index < jobTransforms.Count)
        //                    jobSocket.attachTransform = jobTransforms[index];
        //                if (index <= sliderClosedPositions.Count)
        //                    closedPos = sliderClosedPositions[index];
        //                plate.OnGasCuttingDone += () => {
        //                    ToggleViseOpening();
        //                    ToggleFilingCanvas(); 
        //                };
        //                InvokeRepeating(nameof(KeepCheckingForViseClosing), invokeInterval, invokeInterval);
        //            }
        //            currentHoldingPlate = plate;

        //        }
        //    }
        //}

        //void KeepCheckingForViseClosing()
        //{
        //    if(!isViseOpen)
        //    {
        //        ToggleViseOpening(false);
        //        CancelInvoke(nameof(KeepCheckingForViseClosing));
        //    }
        //}

        //public void OnSocketExit(SelectExitEventArgs args)
        //{
        //    // if (ShouldSwitchAttachTransforms)
        //    //     jobPlateHighlight.SetActive(true);
        //    if (args.interactable.CompareTag(_Constants.JOB_TAG))
        //    {
        //        JobPlate plate = args.interactable.GetComponent<JobPlate>();
        //        plate.OnMechanicalViseSocketExit();
        //    }
        //    currentHoldingPlate = null;
        //    TurnOffHacksawAnimations();
        //}

        //public void TurnOffHacksawAnimations() { }
        //public void ToggleFilingCanvas(bool on = true)
        //{
        //    filingCanvas.SetActive(on);
        //}

        //public void ToggleViseOpening(bool allow=true)
        //{
        //    allowViseOpening = allow;
        //}
    }
}