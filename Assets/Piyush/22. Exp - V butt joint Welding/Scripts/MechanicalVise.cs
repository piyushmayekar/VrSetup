using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VWelding
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
        //[SerializeField] List<GameObject> hacksawAnimators;
        //[SerializeField] float sliderParameter;

        //[SerializeField, Tooltip("The angle limits of the handle with respect to Left World Axis")]
        //Vector2 angleLimits = new Vector2(-125f, 125f);

        //[SerializeField, Tooltip("Slider positions")]
        //Vector3 openPos, closedPos, closedPosFull;
        //[SerializeField] List<Vector3> sliderClosedPositions;
        //[SerializeField] bool isViseOpen = true, shouldSwitchAttachTransforms = true;

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
        //    while (true)
        //    {
        //        float angle = Vector3.SignedAngle(Vector3.forward, handle.right, Vector3.left);
        //        sliderParameter = Mathf.InverseLerp(angleLimits.x, angleLimits.y, angle);
        //        slider.transform.localPosition = Vector3.Lerp(openPos, closedPos, sliderParameter);
        //        isViseOpen = sliderParameter < 0.9f;
        //        yield return null;
        //    }

        //}

        //public void OnSocketEnter(SelectEnterEventArgs args)
        //{
        //    if (args.interactable.CompareTag(_Constants.JOB_TAG))
        //    {
        //        JobPlate plate = args.interactable.GetComponent<JobPlate>();
        //        int index = (int)plate.PlateType;
        //        // jobPlateHighlight.SetActive(false);
        //        if (ShouldSwitchAttachTransforms)
        //        {
        //            if (plate.IsCuttingDone && !plate.IsFilingDone)
        //            {
        //                jobSocket.attachTransform = filingTransforms[index];
        //                plate.SetupForFiling();
        //                closedPos = sliderClosedPositions[1];
        //                ToggleFilingCanvas(false);
        //            }
        //            else if (!plate.IsCuttingDone)
        //            {
        //                plate.SetupForCutting();
        //                hacksawAnimators[index].SetActive(true);
        //                plate.OnGasCuttingDone += TurnOffHacksawAnimations;
        //                if (index >= 0 && index < jobTransforms.Count)
        //                    jobSocket.attachTransform = jobTransforms[index];
        //                if (index <= sliderClosedPositions.Count)
        //                    closedPos = sliderClosedPositions[index];
        //                plate.OnGasCuttingDone += () => { ToggleFilingCanvas(); };
        //            }
        //            currentHoldingPlate = plate;

        //        }
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

        //public void TurnOffHacksawAnimations() => hacksawAnimators.ForEach(x => x.SetActive(false));
        //public void ToggleFilingCanvas(bool on = true)
        //{
        //    filingCanvas.SetActive(on);
        //}

    }
}