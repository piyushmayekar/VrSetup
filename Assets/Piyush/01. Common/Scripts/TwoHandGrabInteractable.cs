using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class TwoHandGrabInteractable : XRGrabInteractable
    {
        public List<XRSimpleInteractable> secondHandGrabPoints;
        public XRBaseInteractor secondInteractor = null;
        public enum TwoHandRotationType { None, First, Second }
        public TwoHandRotationType rotationType;
        public Vector3 rotationOffset;
        Quaternion intialAttachTransformRotation;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            foreach (var grabPoint in secondHandGrabPoints)
            {
                grabPoint.selectEntered.AddListener(new UnityAction<SelectEnterEventArgs>(OnSecondHandGrab));
                grabPoint.selectExited.AddListener(new UnityAction<SelectExitEventArgs>(OnSecondHandRelease));
            }
        }
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            intialAttachTransformRotation = selectingInteractor.attachTransform.localRotation;
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            args.interactor.attachTransform.localRotation = intialAttachTransformRotation;
            secondInteractor = null;
            base.OnSelectExited(args);
        }
        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (secondInteractor && selectingInteractor)
            {
                switch (rotationType)
                {
                    case TwoHandRotationType.None:
                        selectingInteractor.attachTransform.rotation =
                        Quaternion.LookRotation((secondInteractor.transform.position) -
                         selectingInteractor.attachTransform.position) * Quaternion.Euler(rotationOffset);
                        break;

                    case TwoHandRotationType.First:
                        selectingInteractor.attachTransform.rotation =
                        Quaternion.LookRotation((secondInteractor.transform.position) -
                             selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up)
                              * Quaternion.Euler(rotationOffset);
                        break;

                    case TwoHandRotationType.Second:
                        selectingInteractor.attachTransform.rotation =
                        Quaternion.LookRotation((secondInteractor.transform.position + rotationOffset) -
                             selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up)
                             * Quaternion.Euler(rotationOffset);
                        break;
                }
            }
            base.ProcessInteractable(updatePhase);
        }

        public void OnSecondHandGrab(SelectEnterEventArgs args)
        {
            secondInteractor = args.interactor;
        }

        public void OnSecondHandRelease(SelectExitEventArgs args)
        {
            secondInteractor = null;
        }

        public override bool IsSelectableBy(XRBaseInteractor interactor)
        {
            bool isAlreadyGrabbed = selectingInteractor && !selectingInteractor.Equals(interactor);
            return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
        }
    }
}