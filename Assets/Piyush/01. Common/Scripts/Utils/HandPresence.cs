using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class HandPresence : MonoBehaviour
    {
        [SerializeField] UnityEvent OnControllerXButtonPress;

        private InputDevice targetDevice;
        public InputDeviceCharacteristics controllerCharacteristics;

        public float triggerValue, gripValue;
        [SerializeField] GameObject handModel;
        [SerializeField] Animator handAnimator;
        [SerializeField] CustomXRGrabInteractable currentGrabInteractable;
        [SerializeField] string toolName = string.Empty;
        [SerializeField] bool isXButtonPressed=false;
        bool wasXButtonPressed = false;

        public readonly static float gripThreshold = 0.9f;
        //Animator clip names
        const string OPEN = "Open", FIST = "Fist";
        //Animator parameters
        const string IS_HAND_OPEN = "IsHandOpen";
        private void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            //Try to get the list of input devices HMD and controllers
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);//Get the deveices with char like left controller

            foreach (var item in devices)
            {
                Debug.Log(item.name + item.characteristics);
            }

            if (devices.Count > 0)
            {
                targetDevice = devices[0];
                // spawnedHandModel = Instantiate(handModelPrefab, transform);
                // handAnimator = spawnedHandModel.GetComponent<Animator>();
            }
        }

        private void Update()
        {

            if (!targetDevice.isValid)
            {
                TryInitialize();
            }
            else
            {
                UpdateHandInputs();
                if (currentGrabInteractable == null && toolName == string.Empty) //Fist anim
                {
                    if (gripValue >= gripThreshold && handAnimator.GetBool(IS_HAND_OPEN))
                    {
                        handAnimator.SetBool(IS_HAND_OPEN, false);
                        handAnimator.Play(FIST);
                    }
                    else if (gripValue < gripThreshold && !handAnimator.GetBool(IS_HAND_OPEN))
                    {
                        handAnimator.SetBool(IS_HAND_OPEN, true);
                    }
                }
            }


        }

        void UpdateHandInputs()
        {
            targetDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
            targetDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isXButtonPressed);
            if(isXButtonPressed && !wasXButtonPressed)
            {
                wasXButtonPressed = true;
                OnControllerXButtonPress?.Invoke();
            }
            else if(!isXButtonPressed && wasXButtonPressed)
            {
                wasXButtonPressed = false;
            }
        }

        //When Grabbing First Grab Point - in case the interactable is a two hand grab interactable
        public void OnHandSelectEnter(SelectEnterEventArgs args)
        {
            currentGrabInteractable = args.interactable as CustomXRGrabInteractable;
            if (currentGrabInteractable)
            {
                toolName = currentGrabInteractable.name;
                handAnimator.SetBool(IS_HAND_OPEN, false);
                if (currentGrabInteractable.ShouldUseDefaultFistAnim)
                    handAnimator.Play(FIST);
                else
                {
                    handAnimator.Play(toolName);
                }
            }
        }
        public void OnHandSelectExit(SelectExitEventArgs args)
        {
            currentGrabInteractable = null;
            toolName = string.Empty;
        }

        //When grabbing the Second Grab point - in case the interactable is a two hand grab interactable
        public void OnHandSelectEnterSecondGrabPoint(TwoHandGrabInteractable grabInteractable)
        {
            toolName = grabInteractable.secondHandGrabAnimationName;
            handAnimator.SetBool(IS_HAND_OPEN, false);
            handAnimator.Play(toolName);
        }

        public void OnHandSelectExitSecondGrabPoint(TwoHandGrabInteractable grabInteractable)
        {
            currentGrabInteractable = null;
            toolName = string.Empty;
        }

        public void OnSecondHandPointHoverEnter(string toolAnimToPlay)
        {
            toolName = toolAnimToPlay;
            handAnimator.SetBool(IS_HAND_OPEN, false);
            handAnimator.Play(toolName);
        }

        public void OnSecondHandPointHoverExit()
        {
            currentGrabInteractable = null;
            toolName = string.Empty;
        }
    }
}