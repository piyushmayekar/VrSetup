using FlatWelding;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CornerWelding
{
    public class Task_PrepareToWeld : Task
    {
        [SerializeField] CurrentKnob currentKnob;
        [SerializeField] XRSocketInteractor platesUprightPositionSocket;
        [SerializeField] FinalJobPlatesManager finalJobPlates;
        [SerializeField] float knobTargetValue = 110f;
        [SerializeField] bool knobValueSet = false, platesPlaced = false;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            currentKnob.TargetValue = knobTargetValue;
            finalJobPlates.ToggleGrab(true);
            platesUprightPositionSocket.gameObject.SetActive(true);
            CurrentKnob.OnTargetValueSet += OnKnobTargetValueSet;
        }

        private void OnKnobTargetValueSet()
        {
            knobValueSet = true;
            CheckIfTaskIsCompleted();
        }

        public void OnPlatesSocketSelectEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                platesUprightPositionSocket.socketActive = false;
                finalJobPlates.ToggleGrab(false);
                platesPlaced = true;
                args.interactable.transform.position = platesUprightPositionSocket.attachTransform.position;
                args.interactable.transform.rotation = platesUprightPositionSocket.attachTransform.rotation;
                platesUprightPositionSocket.attachTransform.gameObject.SetActive(false);
                CheckIfTaskIsCompleted();
            }
        }

        public void CheckIfTaskIsCompleted()
        {
            if (knobValueSet && platesPlaced)
            {
                CurrentKnob.OnTargetValueSet -= OnKnobTargetValueSet;
                OnTaskCompleted();
            }
        }
    }
}