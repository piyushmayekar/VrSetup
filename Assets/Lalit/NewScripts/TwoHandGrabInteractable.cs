using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;

    public enum TwoHandRotationType { None, First, Second };
    public TwoHandRotationType twoHandRotationType;

    public bool snapToSecondHand = true;
    private Quaternion initialRotationOffset;


    private void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position
               - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position
                - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position
               - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }

        return targetRotation;
    }
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor)
        {
            if (snapToSecondHand)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            }
            else
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * initialRotationOffset;
            }

        }
        base.ProcessInteractable(updatePhase);
    }

    private void OnSecondHandRelease(XRBaseInteractor arg0)
    {
        secondInteractor = null;
        // Debug.Log("Second hand release");
    }

    private void OnSecondHandGrab(SelectEnterEventArgs arg0)
    {
        // Debug.Log("Second hand grab");
        secondInteractor = arg0.interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
    }




    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        //  Debug.Log("first grab enter");
        base.OnSelectEntered(args);
        attachInitialRotation = args.interactor.attachTransform.localRotation;

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        //  Debug.Log("first grab exit");
        base.OnSelectExited(args);
        secondInteractor = null;
        args.interactor.attachTransform.localRotation = attachInitialRotation;

    }
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);

        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
