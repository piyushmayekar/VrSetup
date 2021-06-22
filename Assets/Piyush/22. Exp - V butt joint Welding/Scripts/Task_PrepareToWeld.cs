using FlatWelding;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VWelding
{
    public class Task_PrepareToWeld : Task
    {
        [SerializeField] XRSocketInteractor platesUprightPositionSocket;
        [SerializeField] FinalJobPlatesManager finalJobPlates;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            finalJobPlates.ToggleGrab(true);
            platesUprightPositionSocket.gameObject.SetActive(true);
        }

        public void OnPlatesSocketSelectEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                platesUprightPositionSocket.socketActive = false;
                finalJobPlates.ToggleGrab(false);
                args.interactable.transform.position = platesUprightPositionSocket.attachTransform.position;
                args.interactable.transform.rotation = platesUprightPositionSocket.attachTransform.rotation;
                platesUprightPositionSocket.attachTransform.gameObject.SetActive(false);
                OnTaskCompleted();
            }
        }

        public void OnJobPlatesGrabStart()
        {
            if (!platesUprightPositionSocket.socketActive && !IsTaskComplete)
                platesUprightPositionSocket.socketActive = true;
        }
    }
}