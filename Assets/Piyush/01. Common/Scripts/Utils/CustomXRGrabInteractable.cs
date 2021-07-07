using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


namespace PiyushUtils
{
    public class CustomXRGrabInteractable : XRGrabInteractable
    {
        [SerializeField] InteractorType grabbedInteractorType = InteractorType.None;
        [SerializeField] Transform leftHandAttachTransform, rightHandAttachTransform;

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            DetermineInteractorType(args.interactor);
            SetAttachTransform();
            base.OnHoverEntered(args);
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);
            grabbedInteractorType = InteractorType.None;
        }
        public void DetermineInteractorType(XRBaseInteractor interactor)
        {
            if (interactor.CompareTag(_Constants.LEFT_HAND_TAG))
                grabbedInteractorType = InteractorType.LeftHand;
            else if (interactor.CompareTag(_Constants.RIGHT_HAND_TAG))
                grabbedInteractorType = InteractorType.RightHand;
            else if (interactor is XRSocketInteractor)
                grabbedInteractorType = InteractorType.Socket;
        }

        public void SetAttachTransform()
        {
            switch (grabbedInteractorType)
            {
                case InteractorType.LeftHand:
                    attachTransform = leftHandAttachTransform;
                    break;
                case InteractorType.RightHand:
                    attachTransform = rightHandAttachTransform;
                    break;
            }
        }

        [System.Serializable]
        public enum InteractorType
        {
            None, LeftHand, RightHand, Socket
        }
    }
}