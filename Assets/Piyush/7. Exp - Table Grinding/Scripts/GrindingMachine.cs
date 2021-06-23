using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Grinding
{
    public class GrindingMachine : MonoBehaviour
    {
        public Action OnMachineToggleOn, OnMachineToggleOff;
        [SerializeField] bool isOn = false;
        [SerializeField] float maxRotateSpeed = 5f, currRotateSpeed = 0f, speedDelta = .1f;
        [SerializeField] Vector3 wheelRotationAxis = new Vector3(1, 0, 0);
        [SerializeField] Transform[] wheels;
        [SerializeField] Animator switchAnimator;
        [SerializeField] SoundPlayer soundPlayer;
        Coroutine wheelRotator = null;
        public bool IsOn => currRotateSpeed > 0f;
        public static bool isMachineOn;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            ToggleMachine(isOn);
        }

        public void OnSwitchHoverEnter(HoverEnterEventArgs args)
        {
            ToggleMachine(!isOn);
        }

        public void OnSwitchSelectEnter(SelectEnterEventArgs args)
        {
            ToggleMachine(!isOn);
        }

        public void ToggleMachine(bool on = true)
        {
            isOn = on;
            isMachineOn = isOn;
            switchAnimator?.SetBool("On", isOn);
            if (isOn)
            {
                soundPlayer.PlayClip(soundPlayer.Clips[0], true);
                OnMachineToggleOn?.Invoke();
            }
            else
            {
                soundPlayer.StopPlayingAllSounds();
                OnMachineToggleOff?.Invoke();
            }
            if (isOn && wheelRotator == null)
                wheelRotator = StartCoroutine(WheelRotator());
        }

        IEnumerator WheelRotator()
        {
            while (currRotateSpeed >= 0f)
            {
                if (isOn && currRotateSpeed < maxRotateSpeed)
                    currRotateSpeed += speedDelta;
                if (!isOn && currRotateSpeed > 0f)
                    currRotateSpeed -= speedDelta;
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].Rotate(wheelRotationAxis * currRotateSpeed * Time.deltaTime);
                }
                yield return new WaitForEndOfFrame();
            }
            wheelRotator = null;
        }
    }
}