using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

namespace Grinding
{
    public class WeldedPlates : MonoBehaviour
    {
        public UnityEvent OnScriberMarkingComplete, OnCenterPunchMarkingComplete,
        RoughGrindingComplete, SurfaceGrindingComplete;
        [SerializeField] ScriberMarking scriberMarking;
        [SerializeField] CenterPunchMarking centerPunchMarking;
        [SerializeField] List<GrindingPlate> plates;
        [SerializeField] List<int> grindTracker = new List<int>() { 0, 0 };

        [Header("Grabbing")]
        [SerializeField] PiyushUtils.TwoHandGrabInteractable twoHand;
        [SerializeField] XRSimpleInteractable leftHandInteractable, rightHandInteractable;

        private void Awake()
        {
            twoHand.selectEntered.AddListener(OnFirstHandSelectEnter);
        }

        #region GRABBING
        void OnFirstHandSelectEnter(SelectEnterEventArgs args)
        {
            twoHand.secondHandGrabPoints.ForEach(point =>
            {
                point.selectEntered.RemoveAllListeners();
                point.selectExited.RemoveAllListeners();
            });

            if (twoHand.grabbedInteractorType == CustomXRGrabInteractable.InteractorType.LeftHand) 
                twoHand.secondHandGrabPoints = new List<XRSimpleInteractable>() { rightHandInteractable };
            if (twoHand.grabbedInteractorType == CustomXRGrabInteractable.InteractorType.RightHand) 
                twoHand.secondHandGrabPoints = new List<XRSimpleInteractable>() { leftHandInteractable };

            twoHand.SubscribeToSecondHandGrabPointEvents();
        }
        #endregion

        public void StartScriberMarking()
        {
            scriberMarking.gameObject.SetActive(true);
            scriberMarking.StartMarkingProcess();
            scriberMarking.OnMarkingDone += () =>
            {
                OnScriberMarkingComplete?.Invoke();
            };
        }

        public void StartCenterPunchMarking()
        {
            centerPunchMarking.StartMarkingProcess();
            centerPunchMarking.OnMarkingDone += () =>
            {
                OnCenterPunchMarkingComplete?.Invoke();
            };
        }

        public void StartGrinding(int type)
        {
            plates.ForEach(plate => plate.OnGrindingComplete += OnGrindingComplete);
        }

        public void OnGrindingComplete(GrindingWheelType type)
        {
            grindTracker[(int)type]++;
            if (grindTracker[(int)GrindingWheelType.Rough] >= 2)
            {
                RoughGrindingComplete?.Invoke();
                scriberMarking.gameObject.SetActive(false);
                centerPunchMarking.gameObject.SetActive(false);
            }
            if (grindTracker[(int)GrindingWheelType.Surface] >= 2)
            {
                SurfaceGrindingComplete?.Invoke();
            }
        }

        public void OnPlatesPlacedInMechanicalVise()
        {
            Invoke(nameof(MakeThePlatesNonKinematic), 0.5f);
        }

        private void MakeThePlatesNonKinematic()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            plates.ForEach(plate =>
            {
                Transform plateT = plate.transform.GetChild(0);
                var pos = plateT.localPosition;
                var rot = plateT.localRotation;
                var scale = plateT.localScale;
                Rigidbody rb = plateT.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                plateT.localPosition = pos;
                plateT.localRotation = rot;
                plateT.localScale = scale;
                rb.isKinematic = false;
            });
        }

        public void MakeThePlatesKinematic()
        {
            plates.ForEach(plate =>
                        {
                            Rigidbody rb = plate.transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
                            rb.isKinematic = true;
                        });
        }
    }
}